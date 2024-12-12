using AcompanhaAcoes.Models;
using AcompanhaAcoes.Services;

internal class MonitoraAcoes
{
    public async Task MonitorAcaoAsync(ConfiguracaoAcoes configAcao)
    {
        ConfiguracoesEmail configEmail;
        try
        {
            configEmail = ConfiguracoesEmail.CarregaConfigEmail();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro de configuração: {ex.Message}");
            return;
        }

        var servicoCotacaoAcoes = new ServicoCotacaoAcoes();
        var servicoEmail = new ServicoEmail(configEmail);


        // ---- LOOP PRINCIPAL DE MONITORAMENTO ----
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
}