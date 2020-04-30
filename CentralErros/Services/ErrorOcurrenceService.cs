using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CentralErros.Models;

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

        public List<ErrorOccurrence> FindErrorsByDetails(string details)
        {
            return _context.Errors
                .Where(x => x.Details == details)
                .ToList();
        }

        public List<ErrorOccurrence> FindErrorsByOrigin(string origin)
        {
            return _context.Errors
                .Where(x => x.Origin == origin)
                .ToList();
        }

        public List<ErrorOccurrence> GetAllErrors()
        {
            return _context.Errors.ToList();
        }

        public ErrorOccurrence SaveOrUpdate(ErrorOccurrence error)
        {
            var existe = _context.Errors
                                .Where(x => x.Id == error.Id)
                                .FirstOrDefault();

            if (existe == null)
                _context.Errors.Add(error);
            else
            {
                existe.Id = error.Id;
                existe.Title = error.Title;
                existe.RegistrationDate = error.RegistrationDate;
                existe.Origin = error.Origin;
                existe.Filed = error.Filed;
                existe.Details = error.Details;
                existe.UserId = error.UserId;
                existe.EnvironmentId = error.EnvironmentId;
                existe.LevelId = error.LevelId;
            }

            _context.SaveChanges();

            return error;
        }

        public bool FindFiledOcurrence(ErrorOccurrence error)
        {
            return _context.Errors.Any(x => x.Filed == error.Filed);
        }
    }
}
