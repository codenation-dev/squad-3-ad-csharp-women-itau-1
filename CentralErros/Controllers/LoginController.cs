using AutoMapper;
using IdentityModel.Client;
using CentralErros.DTO;
using CentralErros.Models;
using CentralErros.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using CentralErros.Extensions;
using System.Web;

namespace CentralErros.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly CentralErroContexto _context;

        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppSettings _appSettings;
        private readonly IEmailService _emailServices;

        public LoginController(IUserService userService, IMapper mapper, CentralErroContexto context)
        {
            _userService = userService;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet("{email}/{password}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<User> Get(string email, string password)
        {
            var user = _userService.FindByLogin(email, password);

            if (user != null)
            {
                var retorno = _mapper.Map<User>(user);

                return Ok(retorno);
            }
            else
            {
                object res = null;
                NotFoundObjectResult notfound = new NotFoundObjectResult(res);
                notfound.StatusCode = 404;

                notfound.Value = "Email ou senha inválido!";
                return NotFound(notfound);
            }
        }

        public LoginController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IOptions<AppSettings> appSettings, IEmailService emailServices)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _appSettings = appSettings.Value;
            _emailServices = emailServices;
        }


        [HttpPost("forgotPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO forgotPassword)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(forgotPassword.Email);
            if (user == null)
            {
                return NotFound($"Usuário '{forgotPassword}' não encontrado.");
            }
            else
            {
                var forgotMail = await ForgotMainPassword(user);
                if (forgotMail.Enviado)
                    return Ok();

                return Unauthorized(forgotMail.error);
            }

        }

        // buscar dados através do usuário passado
        [HttpGet("resetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return BadRequest("Não foi possível resetar a senha");
            }

            var resetPassword = new ResetPasswordDTO();
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Usuário ID '{userId}' não encontrado.");
            }
            else
            {
                resetPassword.Code = code;
                resetPassword.Email = user.Email;
                resetPassword.UserId = userId;
                return Ok(resetPassword);
            }
        }

        // envio nova senha
        [HttpPost("resetPasswordConfirm")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPasswordConfirm(ResetPasswordConfirmDTO resetPassword)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(resetPassword.Email);
            if (user == null)
            {
                return NotFound($"Usuário ID não encontrado.");
            }
            else
            {
                // reset senha Identity
                return Ok(await _userManager.ResetPasswordAsync(user, resetPassword.Code, resetPassword.Password));
            }
        }


        private async Task<EmailResponse> ForgotMainPassword(IdentityUser user)
        {
            // gerar JWT para reset de senha
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            // criar link para retorno 
            var callbackUrl = Url.ResetPasswordCallbackLink(user.Id, HttpUtility.UrlEncode(code), Request.Scheme);

            // método de extensão de URL
            return await _emailServices.SendEmailResetPasswordAsync(user.Email, callbackUrl);
        }
    }

}
