using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AcompanhaAcoes.Models
{
    internal class ConfiguracoesEmail
    {
        public string SmtpServer { get; set; } = string.Empty;
        public int SmtpPort { get; set; }
        public string EmailRemetente { get; set; } = string.Empty;
        public string SenhaRemetente { get; set; } = string.Empty;
        public string EmailDestinatario { get; set; } = string.Empty;


        // ---- DESSERIALIZA JSON EM GETTERS E SETTERS DE ConfiguracoesEmail ----
        public static ConfiguracoesEmail CarregaConfigEmail()
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
}