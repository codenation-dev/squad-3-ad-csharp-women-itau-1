using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using CentralErros.DTO;
using CentralErros.Extensions;
using CentralErros.Models;
using CentralErros.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CentralErros.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppSettings _appSettings;
        private readonly IEmailServices _emailServices;

        public AuthController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IOptions<AppSettings> appSettings, IEmailServices emailServices)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _appSettings = appSettings.Value;
            _emailServices = emailServices;
        }

        [HttpPost("registerUser")]
        [AllowAnonymous]
        public async Task<ActionResult> Cadastrar(RegisterUserDTO registerUser)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new IdentityUser
            {
                UserName = registerUser.Nome,
                Email = registerUser.Email,
                EmailConfirmed = true
            };

            // cria usuario na base com senha criptografada
            var result = await _userManager.CreateAsync(user, registerUser.Password);

            if (result.Succeeded)
            {
                return Ok(result.Succeeded);
            }
            
            return BadRequest(ErrorResponse.FromIdentity(result.Errors.ToList()));
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginUserDTO loginUser)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, true);

            if (result.Succeeded)
            {
                return Ok(await GerarJwt(loginUser.Email));
            }
            if (result.IsLockedOut)
            {
                return BadRequest(loginUser);
            }

            return NotFound(loginUser);
        }

        [HttpPost("logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        // requisição de redefinição de senha
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
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetPassword = new ResetPasswordDTO();
                resetPassword.Code = code;
                resetPassword.Email = user.Email;
                resetPassword.UserId = user.Id;
                return Ok(resetPassword);  
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

        private async Task<LoginResponseDTO> GerarJwt(string email)
        {
            // buscar usuario na base
            var user = await _userManager.FindByEmailAsync(email);
            // buscar claims
            var claims = await _userManager.GetClaimsAsync(user);
            // buscar regras
            var userRoles = await _userManager.GetRolesAsync(user);

            // add nas claims
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName));

            // add roles
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim("role", userRole));
            }

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            // criar token
            var tokenHandler = new JwtSecurityTokenHandler();

            // chave - app settings
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            // criar token baseado nas info
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Emissor,
                Audience = _appSettings.ValidoEm,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            // gerar código
            var encodedToken = tokenHandler.WriteToken(token);

            // add obj resposta
            var response = new LoginResponseDTO
            {
                AccessToken = encodedToken,
                ExpiresIn = TimeSpan.FromHours(2).TotalSeconds,
                UserToken = new UserTokenDTO
                {
                    Id = user.Id,
                    Email = user.Email,
                    Claims = claims.Select(c => new ClaimDTO { Type = c.Type, Value = c.Value })
                }
            };

            return response;
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
