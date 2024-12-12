using AcompanhaAcoes.Models;
using AcompanhaAcoes.Services;
using AcompanhaAcoesInoa.Services;
using System.Text.Json;

class Program
{
    static async Task Main(string[] args)
    {
        ValidadorDeInput.ValidarInputs(args);

        var configAcao = ValidadorDeInput.CreateConfiguracao(args);

        var monitoraAcoes = new MonitoraAcoes();
        await monitoraAcoes.MonitorAcaoAsync(configAcao);
    }
}