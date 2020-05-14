using CentralErros.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace CentralErros.Services
{
    public class EmailServices : IEmailServices
    {
        private SendGridOptions _sendGridOptions { get; }

        public EmailServices(IOptions<SendGridOptions> sendGridOptions)
        {
            _sendGridOptions = sendGridOptions.Value;
        }

        public async Task<EmailResponse> SendEmailBySendGridAsync(string email, string assunto, string mensagem)
        {
            try
            {
                var client = new SendGridClient(_sendGridOptions.SendGridKey);

                var msg = new SendGridMessage()
                {
                    From = new EmailAddress(_sendGridOptions.FromEmail, _sendGridOptions.FromFullName),
                    Subject = assunto,
                    PlainTextContent = mensagem,
                    HtmlContent = mensagem
                };

                msg.AddTo(new EmailAddress(email));

                var responseSend = await client.SendEmailAsync(msg);

                var retorno = new EmailResponse
                {
                    Enviado = true
                };

                if (!responseSend.StatusCode.Equals(System.Net.HttpStatusCode.Accepted))
                {
                    retorno.Enviado = false;
                    retorno.error = ErrorResponse.FromEmail(responseSend);
                }

                return retorno;
            }
            catch (Exception ex)
            {
                return new EmailResponse()
                {
                    Enviado = false,
                    error = ErrorResponse.From(ex)
                };
            }
        }
    }
}
