using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CentralErros.Models;
using CentralErros.DTO;
using CentralErros.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Cors;

namespace CentralErros.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ErrorOccurrenceController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly CentralErroContexto _contexto;
        private readonly IErrorOcurrenceService _erroService;
        private readonly ILevelService _levelService;
        private readonly IEnvironmentService _environmentService;
        private readonly IUserService _userService;

        public ErrorOccurrenceController(IMapper mapper, CentralErroContexto contexto,
           IErrorOcurrenceService erroService,
           IUserService userService,
           ILevelService levelService,
           IEnvironmentService environmentService)
        {
            _mapper = mapper;
            _contexto = contexto;
            _erroService = erroService;
            _userService = userService;
            _levelService = levelService;
            _environmentService = environmentService;

        }

        // GET: api/ErrorOccurence
        [EnableCors("AllowSpecificOrigin")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<ErrorOccurrence>> GetAll()
        {
            var erros = _erroService.GetAllErrors();
            if (erros != null)
            {

                return Ok(erros.Select(x => _mapper.Map<ErrorOccurrence>(x)).ToList());
            }
            else
                return NotFound();
        }

        // GET: api/ErrorOccurence/5
        [EnableCors("AllowSpecificOrigin")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ErrorOccurrence> Get(int id)
        {
            var error = _erroService.FindById(id);
       
            if (error != null)
            {
                var retorno = _mapper.Map<ErrorOccurrence>(error);

                return Ok(retorno);
            }

            else
                return NotFound();
        }

        //GET: api/Errors/1/2/0/0
        [EnableCors("AllowSpecificOrigin")]
        [HttpGet("idAmbiente/idOrdenacao/idBusca/textoBuscado")]
        public ActionResult<List<ErrorOccurrence>> GetErrorFilter(int idAmbiente, int idOrdenacao, int idBusca, string textoBuscado)
        {
            var errors = _erroService.FindByFilters(idAmbiente, idOrdenacao, idBusca, textoBuscado);

            if (errors == null)
            {
                object res = null;
                NotFoundObjectResult notfound = new NotFoundObjectResult(res);
                notfound.StatusCode = 404;

                notfound.Value = "Não foi encontrada nenhuma ocorrência referente a essa busca!";
                return NotFound(notfound);
            }

            return Ok(errors.
                        Select(x => _mapper.Map<ErrorOccurrence>(x)).
                        ToList()); ;
        }

        // GET: api/ErrorOccurence/5
        [EnableCors("AllowSpecificOrigin")]
        [HttpGet("getErrorDetails/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ErrorDetails> GetFiledErrors(int id)
        {
            var error = _erroService.FindById(id);

            if (error != null)
            {
                var errorDetails = new ErrorDetails
                {
                    Details = error.Details,
                    UserName = error.UserName,
                    UserToken = error.TokenUser,
                    EventId = error.IdEvent,
                    LevelName = error.LevelName,
                    RegistrationDate = error.RegistrationDate,
                    Origin = error.Origin,
                    Title = error.Title,

                };
                var retorno = _mapper.Map<ErrorDetails>(errorDetails);

                return Ok(retorno);
            }

            else
                return NotFound();
        }

        // GET: api/ErrorOccurence/5
        [EnableCors("AllowSpecificOrigin")]
        [HttpPut("setFiledErrors/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Boolean> SetFiledErrors(int id)
        {
            var error = _erroService.FindById(id);

            if (error != null)
            {
                error.Filed = true;
                var retornError = _erroService.SaveOrUpdate(error);
                var retorno = _mapper.Map<ErrorOccurrenceDTO>(retornError);                

                return Ok(retorno);
            }

            else
                return NotFound();
        }

        // GET: api/ErrorOccurence/5
        [EnableCors("AllowSpecificOrigin")]
        [HttpPut("setUnarchiveErrors/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Boolean> SetUnarchiveErrors(int id)
        {
            var error = _erroService.FindById(id);

            if (error != null)
            {
                error.Filed = false;
                var retornError = _erroService.SaveOrUpdate(error);
                var retorno = _mapper.Map<ErrorOccurrenceDTO>(retornError);

                return Ok(retorno);
            }

            else
                return NotFound();
        }




        // POST: api/ErrorOccurence
        [EnableCors("AllowSpecificOrigin")]
        [HttpPost]
        public ActionResult<ErrorOccurrenceDTO> Post([FromBody] ErrorOccurrenceDTO value)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Montando as fk's
            var user = _userService.FindById(value.UserId);
            var level = _levelService.FindByIdLevel(value.LevelId);
            var env = _environmentService.FindById(value.EnvironmentId);
            string host = Dns.GetHostName();
            string ip = Dns.GetHostAddresses(host)[0].ToString();
            Random random = new Random();
            int randomNumber = random.Next(0, 1000);

            if (user != null && level != null && env != null)
            {
                var errorOcurrence = new ErrorOccurrence()
                {
                    Title = value.Title,
                    RegistrationDate = DateTime.Now,
                    Origin = ip,
                    Filed = value.Filed,
                    Details = value.Details,
                    UserId = user.Id,                    
                    IdEvent = randomNumber,
                    EnvironmentId = env.Id,
                    LevelId = level.IdLevel,
                    TokenUser = user.Token,
                    UserName = user.Name,
                    LevelName = level.LevelName,
                    EnvironmentName = env.Name,
                };

                var registryError = _erroService.SaveOrUpdate(errorOcurrence);
                var retorno = _mapper.Map<ErrorOccurrence>(registryError);
                return Ok(retorno);
            }
            else
            {
                object res = null;
                NotFoundObjectResult notfound = new NotFoundObjectResult(res);
                notfound.StatusCode = 404;

                if (user == null)
                {                    
                    notfound.Value = "O usuário informado não foi encontrado!";
                    return NotFound(notfound);
                }
                if(level == null)
                {
                    notfound.Value = "O Level informado não foi encontrado!";
                    return NotFound(notfound);
                }
                if(env == null)
                {
                    notfound.Value = "O Environment informado não foi encontrado!";
                    return NotFound(notfound);
                }
                return NotFound();
            }

            
            }

        // DELETE: api/ApiWithActions/5
        [EnableCors("AllowSpecificOrigin")]
        [HttpDelete("{id}")]
        public ActionResult<ErrorOccurrenceDTO> Delete(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ErrorOccurrence errorOccurrence = _erroService.FindById(id);

            if (errorOccurrence == null)
                return BadRequest("Erro não existe");
            else
            {
                _contexto.Errors.Remove(errorOccurrence);
                var retornError = _erroService.SaveOrUpdate(errorOccurrence);
                var retorno = _mapper.Map<ErrorOccurrenceDTO>(retornError);
                return Ok(retorno);
            }

        }
    }
}
