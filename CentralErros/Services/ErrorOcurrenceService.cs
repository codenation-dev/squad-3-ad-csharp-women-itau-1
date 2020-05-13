using System;
using System.Collections.Generic;
using System.Linq;
using CentralErros.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

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

        public IList<ErrorOccurrence> GetAllErrors()
        {
            return _context.Errors.Where(x => x.Filed == false).ToList();
        }

        public ErrorOccurrence SaveOrUpdate(ErrorOccurrence error)
        {
            if (_context.Environments.Any(e => e.Id == error.EnvironmentId) &&
                _context.Levels.Any(l => l.IdLevel == error.LevelId))
            {
                var state = error.Id == 0 ? EntityState.Added : EntityState.Modified;
                _context.Entry(error).State = state;
                _context.SaveChanges();
            }
            return error;
        }

        public List<ErrorOccurrence> FindByFilters(int ambiente, int? campoOrdenacao, int? campoBuscado, string textoBuscado)
        {
            // Campo buscado
            // 1 - Level
            // 2 - Descrição
            // 3 - Origem

            // Ambiente
            // 1 - Produção
            // 2 - Homologação
            // 3 - Desenvolvimento

            // Campo ordenação
            // 1 - Level
            // 2 - Frequência

            List<ErrorOccurrence> errorsSearchList = new List<ErrorOccurrence>();
            List<ErrorOccurrence> errorsList = new List<ErrorOccurrence>();

            if (textoBuscado != "" && campoBuscado != 0 && campoBuscado != null)
            {
                if (campoBuscado == 1)
                    errorsList = _context.Errors.Where(x => x.Level.LevelName == textoBuscado && x.EnvironmentId == ambiente).ToList();
                else if (campoBuscado == 2)
                    errorsList = _context.Errors.Where(x => x.Details.Contains(textoBuscado) && x.EnvironmentId == ambiente).ToList();
                else if (campoBuscado == 3)
                    errorsList = _context.Errors.Where(x => x.Origin == textoBuscado && x.EnvironmentId == ambiente).ToList();
            }
            else if (ambiente > 0)
            {
                errorsList = _context.Errors.Where(x => x.EnvironmentId == ambiente).ToList();
            }
            else
            {
                errorsList = _context.Errors.ToList();
            }

            if (errorsList.Count() > 0)
            {

                if (campoOrdenacao == 1 && campoBuscado != 1)
                {
                    errorsSearchList = errorsList.OrderBy(x => x.LevelId).ToList();
                }
                else if (campoOrdenacao == 2)
                {
                    if (campoBuscado != 1)
                    {
                        var ordenacao = errorsList.GroupBy(x => x.LevelId)
                                    .Select(group => new
                                    {
                                        Level = group.Key,
                                        Quantidade = group.Count()
                                    })
                                    .OrderByDescending(x => x.Quantidade)
                                    .ToList();

                        errorsSearchList = errorsList.OrderBy(x => ordenacao.Select(y => y.Level).IndexOf(x.LevelId)).ToList();
                    }
                    else
                    {
                        var ordenacao = errorsList.GroupBy(x => x.Details)
                                                            .Select(group => new
                                                            {
                                                                Details = group.Key,
                                                                Quantidade = group.Count()
                                                            })
                                                            .OrderByDescending(x => x.Quantidade)
                                                            .ToList();

                        errorsSearchList = errorsList.OrderBy(x => ordenacao.Select(y => y.Details).IndexOf(x.Details)).ToList();
                    }
                }
                //se não foi informado nenhum campo a ser buscado, eu ordeno a frequencia pela Origin
                else
                {
                    var ordenacao = errorsList.GroupBy(x => x.Origin)
                                                            .Select(group => new
                                                            {
                                                                Origin = group.Key,
                                                                Quantidade = group.Count()
                                                            })
                                                            .OrderByDescending(x => x.Quantidade)
                                                            .ToList();

                    errorsSearchList = errorsList.OrderBy(x => ordenacao.Select(y => y.Origin).IndexOf(x.Origin)).ToList();
                }
            }
            //caso não for informado nenhuma ordenação
            else
            {
                errorsSearchList = errorsSearchList.OrderBy(x => x.Origin).ToList();
            }

            errorsSearchList = errorsSearchList.Where(x => x.Filed == false).ToList();

            return errorsSearchList;
        }

        public void FiledErrors(int id)
        {
            var error = FindById(id);

            if (error != null)
            {
                error.Filed = true;
                _context.SaveChanges();
            }
        }

        public IList<ErrorOccurrence> FindFiledErrors()
        {
            return _context.Errors.Where(x => x.Filed == true).ToList();
        }
    }
}
