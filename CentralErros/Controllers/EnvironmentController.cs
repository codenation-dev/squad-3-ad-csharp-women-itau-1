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
    public class EnvironmentController : ControllerBase
    {
        private readonly IEnvironmentService _envService;
        private readonly IMapper _mapper;

        public EnvironmentController(IEnvironmentService envService, IMapper mapper)
        {
            _envService = envService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<EnvironmentDTO> Get(int id)
        {
            var env = _envService.FindById(id);

            if (env != null)
            {
                var retorno = _mapper.Map<Environment>(env);

                return Ok(retorno);
            }
            else
                return NotFound();
        }

        [HttpPost]
        public ActionResult<EnvironmentDTO> Post([FromBody]EnvironmentDTO value)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorResponse.FromModelState(ModelState));


            var env = new Environment()
            {
                Id = value.Id,
                Name = value.Name,
                
            };

            var returnEnv = _envService.SaveOrUpdate(env);

            var retorno = _mapper.Map<EnvironmentDTO>(returnEnv);

            return Ok(retorno);
        }


    }
}