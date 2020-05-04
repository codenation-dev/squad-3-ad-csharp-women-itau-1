using CentralErros.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentralErros.Services
{
    public interface IEmailService
    {
        Task<EmailResponse> SendEmailBySendGridAsync(string email, string assunto, string mensagem);
    }
}
