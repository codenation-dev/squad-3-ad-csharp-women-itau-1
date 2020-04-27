using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CentralErros.Api.Models;
using CentralErros.DTO;
using CentralErros.Models;
using CentralErros.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<ErrorOccurrenceDTO>> GetAll()
        {
            var error = _erroService.GetAllErrors();

            if (error != null)
            {
                var retorno = _mapper.Map<ErrorOccurrence>(error);

                return Ok(retorno);
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

        // POST: api/ErrorOccurence
        [HttpPost]
        public ActionResult<ErrorOccurrenceDTO> Post([FromBody] ErrorOccurrenceDTO value)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Montando as fk's
            var user = _userService.FindByName(value.UserName);
            var level = _levelService.FindByLevelName(value.LevelName);
            var env = _environmentService.FindByName(value.EnvironmentName);
            var errorOcurrence = new ErrorOccurrence()
            {
                Title = value.Title,
                RegistrationDate = DateTime.Now,
                Origin = value.Origin,
                Filed = value.Filed,
                Details = value.Details,
                TokenUser = user.Token,
                UserId = user.Id,
                EnvironmentName = env.Name,
                EnvironmentId = env.Id,
                LevelName = level.LevelName,
                LevelId = level.IdLevel,
            };

            var registryError = _erroService.SaveOrUpdate(errorOcurrence);
            var retorno = _mapper.Map<ErrorOccurrence>(registryError);
            return Ok(retorno);
        }

        // PUT: api/ErrorOccurence/5
        [HttpPut]
        public ActionResult<ErrorOccurrenceDTO> Put([FromBody] ErrorOccurrenceDTO value)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var error = _mapper.Map<ErrorOccurrence>(value);
            var retorno = _erroService.SaveOrUpdate(error);
            return Ok(_mapper.Map<ErrorOccurrenceDTO>(retorno));
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
