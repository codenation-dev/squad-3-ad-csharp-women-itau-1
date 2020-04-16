using AutoMapper;
using IdentityModel.Client;
using CentralErros.Api.Models;
using CentralErros.DTO;
using CentralErros.Models;
using CentralErros.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace CentralErros.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IMapper _mapper;

        public UsuarioController(IUsuarioService usuarioService, IMapper mapper)
        {
            _usuarioService = usuarioService;
            _mapper = mapper;
        }

        // GET api/cliente/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<UsuarioDTO> Get(int id)
        {
            var usuario = _usuarioService.ProcurarPorId(id);

            if (usuario != null)
            {
                var retorno = _mapper.Map<UsuarioDTO>(usuario);

                return Ok(retorno);
            }
            else
                return NotFound();
        }

        // POST api/cliente
        // binding argumento
        [HttpPost]
        public ActionResult<UsuarioDTO> Post([FromBody]UsuarioDTO value)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorResponse.FromModelState(ModelState));

          
            var usuario = new Usuario()
            {
                Nome = value.Nome,
                Email = value.Email,
                Password = value.Password
            };

            var retornoUsuario = _usuarioService.Salvar(usuario);

            var retorno = _mapper.Map<UsuarioDTO>(retornoUsuario);

            return Ok(retorno);
        }

        // POST api/cliente
        [HttpPut]
        public ActionResult<UsuarioDTO> Put([FromBody] UsuarioDTO value)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);



            var usuario = new Usuario()
            {
                Nome = value.Nome,
                Email = value.Email,
                Password = value.Password
            };

            var retornoUsuario = _usuarioService.Salvar(usuario);

            var retorno = _mapper.Map<UsuarioDTO>(retornoUsuario);

            return Ok(retorno);
        }


        [HttpPost("authToken")]
        [System.Obsolete]
        public async Task<ActionResult<TokenResponse>> AuthToken([FromBody]TokenDTO value)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // async request - await para aguardar retorno
            var disco = await DiscoveryClient.GetAsync("http://localhost:5001");

            // nesta parte, temos um exemplo de requisição com o tipo "password" 
            // esta é a forma mais comum
            var httpClient = new HttpClient();
            var tokenResponse = await httpClient.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "codenation.api_client",
                ClientSecret = "codenation.api_secret",
                UserName = value.UserName,
                Password = value.Password,
                Scope = "codenation"
            });

            // Se não tiver tiver um erro retornar token
            if (!tokenResponse.IsError)
            {
                return Ok(tokenResponse);
            }

            //retorna não autorizado e descrição do erro
            return Unauthorized(ErrorResponse.FromTokenResponse(tokenResponse));
        }
    }
}