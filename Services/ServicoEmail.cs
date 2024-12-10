using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AcompanhaAcoes.Models;

namespace AcompanhaAcoes.Services
{
    internal class ServicoEmail
    {
        private readonly ConfiguracoesEmail _configEmail;

        public ServicoEmail(ConfiguracoesEmail configEmail)
        {
            _configEmail = configEmail;
        }

        public async Task EnviaAlertaEmailAsync(string acao, decimal precoAtual, string tipoAlerta)
        {
            try
            {
                using var smtpClient = new SmtpClient(_configEmail.SmtpServer)
                {
                    Port = _configEmail.SmtpPort,
                    Credentials = new NetworkCredential(
                        _configEmail.EmailRemetente,
                        _configEmail.SenhaRemetente
                    ),
                    EnableSsl = true
                };

                var mensagemEmail = new MailMessage
                {
                    From = new MailAddress(_configEmail.EmailRemetente),
                    Subject = $"Alerta Ação: {acao} {tipoAlerta}",
                    Body = $"Ação {acao} disparou um {tipoAlerta}. Preço Atual: {precoAtual:F2}",
                    IsBodyHtml = false
                };
                mensagemEmail.To.Add(_configEmail.EmailDestinatario);

                await smtpClient.SendMailAsync(mensagemEmail);
                Console.WriteLine($"Email de alerta enviado: {acao} - {tipoAlerta}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao enviar o alerta: {ex.Message}");
            }
        }
    }
}
