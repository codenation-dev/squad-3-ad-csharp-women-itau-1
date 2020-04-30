using AutoMapper;
using IdentityModel.Client;
using CentralErros.DTO;
using CentralErros.Models;
using CentralErros.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace CentralErros.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly CentralErroContexto _context;

        public UserController(IUserService userService, IMapper mapper, CentralErroContexto context)
        {
            _userService = userService;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<User> Get(int id)
        {
            var user = _userService.FindById(id);

            if (user!= null)
            {
                var retorno = _mapper.Map<User>(user);

                return Ok(retorno);
            }
            else
                return NotFound();
        }

     
        [HttpPost]
        public ActionResult<UserDTO> Post([FromBody]UserDTO value)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorResponse.FromModelState(ModelState));

            var claims = new[]
                {
                new Claim(JwtRegisteredClaimNames.UniqueName, value.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

                };
            var Key = Encoding.ASCII.GetBytes("AppSettings.Secret");
            var credenciais = new SigningCredentials(new SymmetricSecurityKey(Key), SecurityAlgorithms.HmacSha256);
            var exp = DateTime.UtcNow.AddHours(2);
            var emitter = "AppSettings.Emitter";
            var validOn = "AppSettings.ValidOn";

            var token = new JwtSecurityToken(
            issuer: emitter,
            audience: validOn,
            claims: claims,
            signingCredentials: credenciais);

            var Token = new JwtSecurityTokenHandler().WriteToken(token);

            var user = new User()
            {
                Name = value.Name,
                Email = value.Email,
                Password = value.Password,
                Token = Token,
                Expiration = exp
            };

            var returnUser = _userService.Save(user);

            var retorno = _mapper.Map<User>(returnUser);

            return Ok(retorno);
        }

        // POST api/cliente
        [HttpPut]
        public ActionResult<UserDTO> Put([FromBody] UserDTO value)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new User()
            {
                Name = value.Name,
                Email = value.Email,
                Password = value.Password
            };

            var returnUser = _userService.Save(user);

            var retorno = _mapper.Map<UserDTO>(returnUser);

            return Ok(retorno);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<User> Delete(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            User user = _userService.FindById(id);

            if (user != null)
            {
                _context.Users.Remove(user);
                var retorno = _userService.Save(user);

                return Ok(retorno);
            }
            else
                return NotFound();
        }


    }
}