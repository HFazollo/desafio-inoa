using AcompanhaAcoes.Models;
using AcompanhaAcoes.Services;
using System.Text.Json;

class Program
{
    static async Task Main(string[] args)
    {
        if (args.Length != 3)
        {
            Console.WriteLine("Modo de uso: AcompanhaAcoes.exe <ACAO> <TRESHOLD_VENDA> <THRESHOLD_COMPRA>");
            return;
        }

        var configAcao = new ConfiguracaoAcoes
        {
            Acao = args[0],
            ThresholdVenda = decimal.Parse(args[1]),
            ThresholdCompra = decimal.Parse(args[2])
        };

        ConfiguracoesEmail configEmail;
        try
        {
            configEmail = CarregaConfigEmail();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro de configuração: {ex.Message}");
            return;
        }

        var servicoCotacaoAcoes = new ServicoCotacaoAcoes();
        var servicoEmail = new ServicoEmail(configEmail);

        while (true)
        {
            try
            {
                decimal precoAtual = await servicoCotacaoAcoes.GetPrecoAcaoAsync(configAcao.Acao);
                Console.WriteLine($"{DateTime.Now}: {configAcao.Acao} - Preço Atual: {precoAtual:F2}");

                if (precoAtual >= configAcao.ThresholdVenda)
                {
                    await servicoEmail.EnviaAlertaEmailAsync(
                        configAcao.Acao,
                        precoAtual,
                        "Alerta de Venda"
                    );
                }
                else if (precoAtual <= configAcao.ThresholdCompra)
                {
                    await servicoEmail.EnviaAlertaEmailAsync(
                        configAcao.Acao,
                        precoAtual,
                        "Alerta de Compra"
                    );
                }

                await Task.Delay(TimeSpan.FromSeconds(10));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro no loop de monitoramento: {ex.Message}");
                await Task.Delay(TimeSpan.FromSeconds(50));
            }
        }
    }

    static ConfiguracoesEmail CarregaConfigEmail()
    {
        const string configPath = "appsettings.json";

        if (!File.Exists(configPath))
        {
            throw new FileNotFoundException("Arquivo de configuração não encontrado.", configPath);
        }

        var conteudoJson = File.ReadAllText(configPath);
        var configEmail = JsonSerializer.Deserialize<ConfiguracoesEmail>(conteudoJson);

        if (configEmail == null)
        {
            throw new InvalidOperationException("Configuração de email inválida.");
        }

        return configEmail;
    }
}