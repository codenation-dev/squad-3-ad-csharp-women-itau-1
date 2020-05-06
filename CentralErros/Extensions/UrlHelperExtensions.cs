using Microsoft.AspNetCore.Mvc;

namespace CentralErros.Extensions
{
    public static class UrlHelperExtensions
    {
        // extensao IUrlHelper
        public static string ResetPasswordCallbackLink(this IUrlHelper urlHelper, string userId, string code, string scheme)
        {
            return $"{scheme}://localhost:5001/api/v1/Auth/resetPassword?userId={userId}&code={code}";
        }
    }
}
