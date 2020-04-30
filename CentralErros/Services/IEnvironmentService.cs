using CentralErros.Models;
using System.Collections.Generic;

namespace CentralErros.Services
{
    public interface IEnvironmentService
    {
        Environment FindById(int id);
        IList<Environment> FindAll();
        Environment FindByName(string name);
        Environment SaveOrUpdate(Environment env);
    }
}