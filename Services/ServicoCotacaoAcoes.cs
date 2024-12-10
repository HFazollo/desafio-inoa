using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AcompanhaAcoes.Services
{
    internal class ServicoCotacaoAcoes
    {
        private readonly HttpClient _httpClient;
        private const string BASE_URL = "https://query1.finance.yahoo.com/v8/finance/chart/";

        public ServicoCotacaoAcoes()
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(10);
        }

        public async Task<decimal> GetPrecoAcaoAsync(string acao)
        {
            try
            {
                acao = SanitizeSymbol(acao);
                string yahooAcao = $"{acao}.SA";

                var request = new HttpRequestMessage(HttpMethod.Get, $"{BASE_URL}{yahooAcao}");
                request.Headers.Add("User-Agent", "Mozilla/5.0");

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return ExtraiPrecoDaResposta(content);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao recuperar o preço da ação: {ex.Message}");
                throw;
            }
        }

        private string SanitizeSymbol(string acao)
        {
            return Regex.Replace(acao, @"[^a-zA-Z0-9]", "");
        }

        private decimal ExtraiPrecoDaResposta(string jsonResponse)
        {
            using var jsonDoc = JsonDocument.Parse(jsonResponse);

            var rootElement = jsonDoc.RootElement;
            var price = rootElement
                .GetProperty("chart")
                .GetProperty("result")[0]
                .GetProperty("meta")
                .GetProperty("regularMarketPrice")
                .GetDecimal();

            return price;
        }
    }
}
