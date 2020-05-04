using CentralErros.Models;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentralErros.Services
{
    public class EmailService : IEmailService
    {
        private SendGridOptions _sendGridOptions { get; }

        public EmailService(IOptions<SendGridOptions> sendGridOptions)
        {
            _sendGridOptions = sendGridOptions.Value;
        }

        public async Task<EmailResponse> SendEmailBySendGridAsync(string email, string assunto, string mensagem)
        {
            try
            {
                // buscar SendGrid key
                var client = new SendGridClient(_sendGridOptions.SendGridKey);

                // obj de email
                var msg = new SendGridMessage()
                {
                    From = new EmailAddress(_sendGridOptions.FromEmail, _sendGridOptions.FromFullName),
                    Subject = assunto,
                    PlainTextContent = mensagem,
                    HtmlContent = mensagem
                };

                // email do usuario
                msg.AddTo(new EmailAddress("marceladovallejob@gmail.com", email));

                // envio do email
                var responseSend = await client.SendEmailAsync(msg);

                // obj retorno
                var retorno = new EmailResponse();
                retorno.Enviado = true;

                // verificação de envio
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
