using AutoMapper;
using CentralErros.DTO;
using CentralErros.Models;
using CentralErros.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
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
