using AutoMapper;
using CentralErros.DTO;
using CentralErros.Models;
using CentralErros.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace CentralErros.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [ApiVersion("1.0")] 
    [Route("api/v{version:apiVersion}/[controller]")]
    //[Authorize]
    public class LevelController : ControllerBase
    {
        private readonly ILevelService _levelService;
        private readonly IMapper _mapper;
        private readonly CentralErroContexto _context;

        public LevelController(ILevelService levelService, IMapper mapper, CentralErroContexto context)
        {
            _levelService = levelService;
            _mapper = mapper;
            _context = context;
        }

        // GET: api/Level/1
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Level> Get(int id)
        {
            var level = _levelService.FindByIdLevel(id);
            if (level != null)
            {
                var retorno = _mapper.Map<Level>(level);
                return Ok(retorno);
            }
            else
            {
                object res = null;
                NotFoundObjectResult notfound = new NotFoundObjectResult(res);
                notfound.StatusCode = 404;

                notfound.Value = "O Level " + id + " não foi encontrado!";
                return NotFound(notfound);
            }
               
        }

        //POST: api/Level/ error, warning or debug 
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<LevelDTO> Post([FromBody]LevelDTO value)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorResponse.FromModelState(ModelState));


            var level = new Level()
            {
                LevelName = value.LevelName

            };

            var resLevel = _levelService.SaveOrUpdate(level);

            var retorno = _mapper.Map<Level>(resLevel);

            return Ok(retorno);
        }

        // GETALL: api/Level/
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<Level>> GetLevels()
        {
            var level = _levelService.FindAllLevels();
            if (level != null)
            {
                
                return Ok(level.Select(x => _mapper.Map<Level>(x)).ToList());
            }
            else
                return NotFound();
        }

        //DELETE: api/Level/ID
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Level> Delete(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            Level level = _levelService.FindByIdLevel(id);
            
            if(level != null)
            {
                _context.Levels.Remove(level);
                var retorno = _levelService.SaveOrUpdate(level);
                return Ok(retorno);
            }
            else
            {
                object res = null;
                NotFoundObjectResult notFound = new NotFoundObjectResult(res);
                notFound.StatusCode = 404;

                notFound.Value = "O Level " + id +" não foi encontrado!";
                return NotFound(notFound);
            }
        }




    }
}
