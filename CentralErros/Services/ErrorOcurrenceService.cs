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
        private object _levelService;

        public ErrorOcurrenceService(CentralErroContexto contexto, ILevelService levelService)
        {
            _context = contexto;
            _levelService = levelService;
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

        public List<ErrorOccurrence> FindByFilters(int ambiente, int campoOrdenacao, int campoBuscado, string textoBuscado)
        {
            // Campo buscado
            // 1 - Level
            // 2 - Descrição
            // 3 - Origem

            // Ambiente
            // 1 - Produção
            // 2 - Homologação
            // 3 - Desenvolvimento

            List<ErrorOccurrence> errorsSearchList = new List<ErrorOccurrence>();
            List<ErrorOccurrence> errorsList = new List<ErrorOccurrence>();

            if (textoBuscado != "" && campoBuscado != 0)
            {
                if (campoBuscado == 1)
                    errorsList = _context.Errors.Where(x => x.LevelName == textoBuscado).ToList();
                else if (campoBuscado == 2)
                    errorsList = _context.Errors.Where(x => x.Details == textoBuscado).ToList();
                else if (campoBuscado == 3)
                    errorsSearchList = _context.Errors.Where(x => x.Origin == textoBuscado).ToList();

                errorsSearchList = errorsSearchList.Where(x => x.EnvironmentId == ambiente || ambiente <= 0).ToList();
            }
            else
                errorsSearchList = _context.Errors.Where(x => x.EnvironmentId == ambiente || ambiente <= 0).ToList();

            // Campo ordenação
            // 1 - Level
            // 2 - Frequência

            if (campoOrdenacao == 1)
                errorsSearchList = errorsSearchList.OrderBy(x => x.LevelId).ToList();
            else if (campoOrdenacao == 2)
            {
                List<Occurrences> listOcc = new List<Occurrences>();

                foreach (var item in errorsSearchList)
                {
                    var occ = new Occurrences();

                    occ.ErrorId = item.Id;
                    occ.Quantity = _context.Errors.Where(x => x.Id == item.Id).Count();
                    listOcc.Add(occ);
                }

                listOcc = listOcc.OrderByDescending(x => x.Quantity).ToList();

                foreach (var item in listOcc)
                {
                    errorsList.Add(_context.Errors.Where(x => x.Id == item.ErrorId).FirstOrDefault());
                }

                errorsSearchList = errorsList;
            }

            return errorsSearchList;
        }

        // Classe criada para controlar a quantidade de erros que está sendo apresentado
        public class Occurrences
        {
            public int ErrorId { get; set; }
            public int Quantity { get; set; }
        }

    }
}
