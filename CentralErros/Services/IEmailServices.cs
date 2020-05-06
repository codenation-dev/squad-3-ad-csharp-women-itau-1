using CentralErros.Models;
using System.Threading.Tasks;

namespace CentralErros.Services
{
    public interface IEmailServices
    {
        Task<EmailResponse> SendEmailBySendGridAsync(string email, string assunto, string mensagem);
    }
}