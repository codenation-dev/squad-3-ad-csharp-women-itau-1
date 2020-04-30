using CentralErros.Models;
using System.Collections.Generic;
using System.Linq;

namespace CentralErros.Services
{
    public class LevelService : ILevelService
    {
        private CentralErroContexto _context;

        public LevelService(CentralErroContexto contexto)
        {
            _context = contexto;
        }

        public IList<Level> FindAllLevels()
        {
            return _context.Levels.ToList();
        }

        public Level FindByIdLevel(int levelId)
        {
            return _context.Levels.Find(levelId);
        }
        public Level FindByLevelName(string name)
        {
            return _context.Levels.FirstOrDefault(x => x.LevelName == name);
        }

        public Level SaveOrUpdate(Level level)
        {
            var existe = _context.Levels
                    .Where(x => x.IdLevel == level.IdLevel)
                    .FirstOrDefault();

            if (existe == null)
                _context.Levels.Add(level);
            else
            {
                existe.LevelName = level.LevelName;
            }
            _context.SaveChanges();

            return level;
        }
    }
}
