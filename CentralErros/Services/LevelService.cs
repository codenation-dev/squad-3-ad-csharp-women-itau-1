using CentralErros.Api.Models;
using CentralErros.Models;


namespace CentralErros.Services
{
    public class LevelService : ILevelService
    {
        private CentralErroContexto _context;

        public LevelService(CentralErroContexto contexto)
        {
            _context = contexto;
        }
        public Level FindByIdLevel(int levelId)
        {
            return _context.Levels.Find(levelId);
        }
    }
}
