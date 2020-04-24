using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CentralErros.Api.Models;
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

        public List<ErrorOccurrence> FindErrorsByLevel(string level)
        {
            return _context.Errors
                .Where(x => x.LevelName == level)
                .ToList();
        }

        public List<ErrorOccurrence> OrderByLevel()
        {
            return _context.Errors
                .OrderBy(x => x.LevelId)
                .ToList();
        }

        public List<ErrorOccurrence> FindErrorsByEnvironment(string env)
        {
            return _context.Errors
                .Where(x => x.EnvironmentName == env)
                .ToList();
        }

        public bool ErrorExists(int id)
        {
            return _context.Errors.Any(x => x.Id == id);
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
                existe.Filed = error.Filed;
                existe.Details = error.Details;
                existe.UserId = error.UserId;
                existe.TokenUser = error.TokenUser;
                existe.EnvironmentId = error.EnvironmentId;
                existe.EnvironmentName = error.EnvironmentName;
                existe.LevelId = error.LevelId;
                existe.LevelName = error.LevelName;
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
