using CentralErros.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace CentralErros.Services
{
    public class UserService : IUserService
    {
        private CentralErroContexto _context;

        public UserService(CentralErroContexto contexto)
        {
            _context = contexto;
        }

        public User FindById(int userId)
        {
            //utilzar metodo Find
            return _context.Users.Find(userId);
        }

        public IList<User> FindByName(string name)
        {
            //utilizar método Where
            return _context.Users.Where(x => x.Name == name).ToList();
        }

        public User Save(User user)
        {
            var existe = _context.Users
                                .Where(x => x.Id == user.Id)
                                .FirstOrDefault();

            if (existe == null)
                _context.Users.Add(user);
            else
            {
                existe.Name = user.Name;
            }

            _context.SaveChanges();

            return user;
        }
    }
}
