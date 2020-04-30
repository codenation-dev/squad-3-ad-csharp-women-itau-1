using CentralErros.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace CentralErros.Services
{
    public class EnvironmentService : IEnvironmentService
    {
        private CentralErroContexto _context;

        public EnvironmentService(CentralErroContexto contexto)
        {
            _context = contexto;
        }

        public Environment FindById(int id)
        {
            return _context.Environments.Find(id);
        }
        public Environment FindByName(string name)
        {
            return _context.Environments.FirstOrDefault(x => x.Name == name);
        }

        public IList<Environment> FindAll()
        {
            return _context.Environments.ToList();
        }

        public Environment SaveOrUpdate(Environment env)
        {
            var existe = _context.Environments
                                .Where(x => x.Id == env.Id)
                                .FirstOrDefault();

            if (existe == null)
                _context.Environments.Add(env);
            else
            {
                existe.Name = env.Name;
            }

            _context.SaveChanges();

            return env;
        }
    }
}
