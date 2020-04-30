using IdentityServer4.Models;
using IdentityServer4.Validation;
using CentralErros.Models;
using System.Linq;
using System.Threading.Tasks;
 
namespace CentralErros.Services
{
    public class ValidadorSenhaService : IResourceOwnerPasswordValidator
    {
        private CentralErroContexto _context;

        // utilizar o mesmo banco atual

        public ValidadorSenhaService(CentralErroContexto context)
        {
            _context = context;
        }

        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            // acessar cliente na base
            var user = _context.Users.FirstOrDefault(x => x.Name == context.UserName);

            // verificar a senha
            if (user != null && user.Password.TrimEnd() == context.Password)
            {
                // retornar objeto tipo GrantValidationResult com sub, auth e claims
                context.Result = new GrantValidationResult(
                    subject: user.Id.ToString(),
                    authenticationMethod: "custom", 
                    claims: UserProfileService.GetUserClaims(user)
                );
                return Task.CompletedTask;
            } 
            else 
            {
                context.Result = new GrantValidationResult(
                    TokenRequestErrors.InvalidGrant, "Usuaio ou senha invaidos");

                return Task.FromResult(context.Result);
            }
        }
     
    }
}