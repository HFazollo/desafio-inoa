using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
