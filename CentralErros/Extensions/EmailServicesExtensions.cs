using CentralErros.Models;
using CentralErros.Services;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace CentralErros.Extensions
{
    public static class EmailServicesExtensions
    {
        // extensao IEmailServices
        public static Task<EmailResponse> SendEmailResetPasswordAsync(this IEmailServices emailServices, string email, string link)
        {
            return emailServices.SendEmailBySendGridAsync(email, "Reset Password",
                $"Por favor para resetar sua senha clique nesse link: <a href='{HtmlEncoder.Default.Encode(link)}'>link</a>");
        }
    }
}
