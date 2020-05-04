using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentralErros.Extensions
{
    public static class UrlHelperExtension
    {
        // extensao IUrlHelper
        public static string ResetPasswordCallbackLink(this IUrlHelper urlHelper, string userId, string code, string scheme)
        {
            return $"{scheme}://localhost:5002/api/v1/CentralErros/resetPassword?userId={userId}&code={code}";
        }
    }
}
