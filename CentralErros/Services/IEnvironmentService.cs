using CentralErros.Api.Models;
using System.Collections.Generic;

namespace CentralErros.Services
{
    public interface IEnvironmentService
    {
        Environment FindById(int id);
        IList<Environment> FindAll();
        Environment SaveOrUpdate(Environment env);
    }
}