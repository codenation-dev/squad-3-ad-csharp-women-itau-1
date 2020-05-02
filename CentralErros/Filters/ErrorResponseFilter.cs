using CentralErros.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;


namespace CentralErros.Filters
{
    public class ErrorResponseFilter : IExceptionFilter
    {
        // quando houver uma exceção não tratada o método é invocado
        public void OnException(ExceptionContext context)
            {
            // qualquer exceção sem tratamento será embrulhada dentro do objeto de response e retornar uma ActionResult
            var errorResponse = ErrorResponse.From(context.Exception);
            context.Result = new ObjectResult(errorResponse) { StatusCode = 500 };
        }
    }
}
