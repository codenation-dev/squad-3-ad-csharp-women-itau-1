using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Linq;
using SendGrid;

namespace CentralErros.Models
{
    public class ErrorResponse 
    {
        public int Codigo { get; set; }
        public string Mensagem { get; set; }
        public ErrorResponse InnerException { get; set; }
        public string[] Detalhes { get; set; }

        public static ErrorResponse From(Exception e)
        {
            if (e == null)
            {
                return null;
            }
            return new ErrorResponse
            {
                Codigo = e.HResult,
                Mensagem = e.Message,
                InnerException = ErrorResponse.From(e.InnerException)
            };
        }

        public static object FromModelState(ModelStateDictionary modelState)
        {
            var erros = modelState.Values.SelectMany(x => x.Errors);
            return new ErrorResponse
            {
                Codigo = 100,
                Mensagem = "Houve erro(s) no envio da requisição",
                Detalhes = erros.Select(e => e.ErrorMessage).ToArray()
            };
        }

        public static object FromTokenResponse(TokenResponse tokenResponse)
        {
            return new ErrorResponse()
            {
                Codigo = 401,
                Mensagem = tokenResponse.ErrorDescription
            };
        }

        internal static ErrorResponse FromEmail(SendGrid.Response response)
        {
            return new ErrorResponse
            {
                Codigo = 600,
                Mensagem = $"Não foi possível enviar email, {response.StatusCode}"
            };
        }
    }
}
