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
using Microsoft.AspNetCore.Authorization;
using IdentityServer4.Extensions;

namespace CentralErros.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class ErrorOccurrenceController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly CentralErroContexto _contexto;
        private readonly IErrorOcurrenceService _erroService;
        private readonly ILevelService _levelService;
        private readonly IEnvironmentService _environmentService;
        

        public ErrorOccurrenceController(IMapper mapper, CentralErroContexto contexto,
           IErrorOcurrenceService erroService,
           
           ILevelService levelService,
           IEnvironmentService environmentService)
        {
            _mapper = mapper;
            _contexto = contexto;
            _erroService = erroService;            
            _levelService = levelService;
            _environmentService = environmentService;

        }

        // GET: api/ErrorOccurence
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<ErrorOccurrence>> GetAll()
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
        [HttpGet("getErrorDetails/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ErrorDetailsDTO> GetFiledErrors(int id)
        {
            var error = _erroService.FindById(id);
            if (error == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ErrorDetailsDTO>(error));
        }

            // GET: api/ErrorOccurence/5
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

        //GET: api/getFiledErrors
        [HttpGet("getFiledErrors")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ErrorOccurrenceDTO> GetFiled()
        {
            var error = _erroService.FindFiledErrors();
            if (error != null)
            {
                var retorno = _mapper.Map<List<ErrorOccurrenceDTO>>(error);
                return Ok(retorno);
            }
            else
                return NotFound();
        }


        // POST: api/ErrorOccurence
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ErrorOccurrenceDTO> Post([FromBody] ErrorOccurrenceDTO value)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Montando as fk's
            var level = _levelService.FindByIdLevel(value.LevelId);
            var env = _environmentService.FindById(value.EnvironmentId);

            if (level != null && env != null)
            {
                var errorOcurrence = new ErrorOccurrence()
                {
                    Title = value.Title,
                    RegistrationDate = DateTime.Now,
                    Origin = value.Origin,
                    Filed = false,
                    Details = value.Details,                                      
                    IdEvent = value.EventId,
                    EnvironmentId = env.Id,
                    LevelId = level.IdLevel,    
                    Username = value.Username,
                };

                var registryError = _erroService.SaveOrUpdate(errorOcurrence);
                var retorno = _mapper.Map<ErrorOccurrence>(registryError);
                return Ok("Erro cadastrado com sucesso!");
            }
            else
            {
                object res = null;
                NotFoundObjectResult notfound = new NotFoundObjectResult(res);
                notfound.StatusCode = 404;

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
