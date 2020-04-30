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
    public class LoginController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly CentralErroContexto _context;

        public LoginController(IUserService userService, IMapper mapper, CentralErroContexto context)
        {
            _userService = userService;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet("{email}, {password}")]
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
                return NotFound();
        }

    }
}