using CentralErros.Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
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
         
            return _context.Users.Find(userId);
        }

        public IList<User> FindByName(string name)
        {
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

        public User RequestTokenSave(User requestUser, string token, DateTime exp)
        {

            var TokenSave = _context.Users.Where(x => x.Email == requestUser.Email && x.Password == requestUser.Password).FirstOrDefault();

            
            if (TokenSave == null)
            {
                requestUser.Token = token;

                _context.Users.Add(requestUser);
                _context.SaveChanges();
            }
            else
            {
                TokenSave.Token = token;
                TokenSave.Expiration = exp;

                _context.SaveChanges();


            }
            return requestUser;

        }
    }
}
