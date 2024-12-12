using AcompanhaAcoes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcompanhaAcoesInoa.Services
{
    internal class ValidadorDeInput
    {
        public static void ValidarInputs(string[] args)
        {
            if (args == null)
                throw new ArgumentNullException(nameof(args));

            if (args.Length != 3)
                throw new ArgumentException(
                    "Modo de uso: dotnet run <ACAO> <TRESHOLD_VENDA> <THRESHOLD_COMPRA>",
                    nameof(args)
                );

            if (!decimal.TryParse(args[1], out decimal thresholdVenda) ||
                !decimal.TryParse(args[2], out decimal thresholdCompra))
                throw new ArgumentException("Valores inválidos para os tresholds.");
        }

        public static ConfiguracaoAcoes CreateConfiguracao(string[] args)
        {
            return new ConfiguracaoAcoes
            {
                Acao = args[0],
                ThresholdVenda = decimal.Parse(args[1]),
                ThresholdCompra = decimal.Parse(args[2])
            };
        }
    }
}
