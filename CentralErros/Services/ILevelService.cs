using CentralErros.Models;
using System.Collections.Generic;

namespace CentralErros.Services
{
    public interface ILevelService
    {
        IList<Level> FindAllLevels();
        Level FindByIdLevel(int levelId);
        Level FindByLevelName(string name);
        Level SaveOrUpdate(Level level);
    }
}
