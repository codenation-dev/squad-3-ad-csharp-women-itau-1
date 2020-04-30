using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CentralErros.Models;
using Microsoft.EntityFrameworkCore;

namespace CentralErros.Services
{
    public class ErrorOcurrenceService : IErrorOcurrenceService
    {
        private CentralErroContexto _context;

        public ErrorOcurrenceService(CentralErroContexto contexto)
        {
            _context = contexto;
        }

        public ErrorOccurrence FindById(int id)
        {
            return _context.Errors.Find(id);
        }

        public List<ErrorOccurrence> GetAllErrors()
        {
            return _context.Errors.ToList();
        }

        public ErrorOccurrence SaveOrUpdate(ErrorOccurrence error)
        {
            if (_context.Environments.Any(e => e.Id == error.EnvironmentId) &&
                _context.Levels.Any(l => l.IdLevel == error.LevelId) &&
                _context.Users.Any(u => u.Id == error.UserId))
            {
                var state = error.Id == 0 ? EntityState.Added : EntityState.Modified;
                _context.Entry(error).State = state;
                _context.SaveChanges();
            }
            return error;
        }

    }
}
