using AutoMapper;
using CentralErros.DTO;
using CentralErros.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;



namespace CentralErros.Controllers
{// Level Names: error warning debug 
    [ApiController]
    [ApiVersion("1.0")] // aqui eu repeti por padrao
    [Route("api/v{version:apiVersion}/[controller]")]// aqui eu repeti por padrao
    [Authorize]
    public class LevelController : ControllerBase
    {
        private readonly ILevelService _levelService;
        private readonly IMapper _mapper;

        public LevelController(ILevelService levelService, IMapper mapper)
        {
            _levelService = levelService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<LevelDTO> Get(int id)
        {
            var level = _levelService.FindByIdLevel(id);
            if (level != null)
            {
                var retorno = _mapper.Map<LevelDTO>(level);
                return Ok(retorno);
            }
            else
                return NotFound();
        }

    }
}
