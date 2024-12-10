using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcompanhaAcoes.Models
{
    internal class ConfiguracaoAcoes
    {
        public string Acao { get; set; } = string.Empty;
        public decimal ThresholdVenda { get; set; }
        public decimal ThresholdCompra { get; set; }
    }
}
