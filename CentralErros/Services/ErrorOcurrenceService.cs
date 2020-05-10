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

            // Campo ordenação
            // 1 - Level
            // 2 - Frequência

            List<ErrorOccurrence> errorsSearchList = new List<ErrorOccurrence>();
            List<ErrorOccurrence> errorsList = new List<ErrorOccurrence>();

            if (textoBuscado != "" && campoBuscado != 0)
            {
                if (campoBuscado == 1)
                    errorsList = _context.Errors.Where(x => x.Level.LevelName == textoBuscado).Where(x => x.Filed == false).ToList();
                else if (campoBuscado == 2)
                    errorsList = _context.Errors.Where(x => x.Details.Contains(textoBuscado)).Where(x => x.Filed == false).ToList();
                else if (campoBuscado == 3)
                    errorsList = _context.Errors.Where(x => x.Origin == textoBuscado).Where(x => x.Filed == false).ToList();

                errorsSearchList = errorsList.Where(x => x.EnvironmentId == ambiente).Where(x => x.Filed == false).ToList();
            }
            else if (ambiente > 0)
            {
                errorsSearchList = _context.Errors.Where(x => x.EnvironmentId == ambiente).Where(x => x.Filed == false).ToList();
            }
            else
            {
                errorsSearchList = _context.Errors.ToList();
            }


            // Só entra na ordenção se houver registros no errorSearchList
            if (errorsSearchList.Count() > 0)
            {

                // Ordenação por LEVEL
                if (campoOrdenacao == 1)
                {
                    errorsSearchList = errorsSearchList.Where(x => x.Filed == false).OrderBy(x => x.LevelId).ToList();
                }
                // Ordenação por FREQUÊNCIA
                else if (campoOrdenacao == 2)
                {
                    // Se foi preenchido o campo a ser bucado eu verifico a frequencia com base no que foi informado:
                    // Campo buscado
                    // 1 - Level
                    // 2 - Descrição
                    // 3 - Origem

                    if (campoBuscado > 0)
                    {
                        if (campoBuscado == 1)
                        {
                            var ordenacao = errorsList.Where(x => x.Filed == false).GroupBy(x => x.LevelId)
                                        .Select(group => new
                                        {
                                            Level = group.Key,
                                            Quantidade = group.Count()
                                        })
                                        .OrderByDescending(x => x.Quantidade)
                                        .ToList();

                            // aplicar ordenação definida na lista desejada                                    
                            return errorsList.Where(x => x.Filed == false).OrderBy(x => ordenacao.Select(y => y.Level).IndexOf(x.LevelId)).ToList();
                        }

                        if (campoBuscado == 2)
                        {
                            var ordenacao = errorsList.Where(x => x.Filed == false).GroupBy(x => x.Details)
                                        .Select(group => new
                                        {
                                            Details = group.Key,
                                            Quantidade = group.Count()
                                        })
                                        .OrderByDescending(x => x.Quantidade)
                                        .ToList();

                            // aplicar ordenação definida na lista desejada                                    
                            return errorsList.OrderBy(x => ordenacao.Select(y => y.Details).IndexOf(x.Details)).Where(x => x.Filed == false).ToList();
                        }

                        if (campoBuscado == 3)
                        {
                            var ordenacao = errorsList.Where(x => x.Filed == false).GroupBy(x => x.Origin)
                                        .Select(group => new
                                        {
                                            Origin = group.Key,
                                            Quantidade = group.Count()
                                        })
                                        .OrderByDescending(x => x.Quantidade)
                                        .ToList();

                            // aplicar ordenação definida na lista desejada                                    
                            return errorsList.OrderBy(x => ordenacao.Select(y => y.Origin).IndexOf(x.Origin)).ToList();
                        }
                    }
                    // se não foi informado nenhum campo a ser buscado eu orderno a frequencia pelo Environment
                    else
                    {
                        var ordenacao = errorsList.Where(x => x.Filed == false).GroupBy(x => x.EnvironmentId)
                                        .Select(group => new
                                        {
                                            Environment = group.Key,
                                            Quantidade = group.Count()
                                        })
                                        .OrderByDescending(x => x.Quantidade)
                                        .ToList();

                        // aplicar ordenação definida na lista desejada                                    
                        return errorsList.OrderBy(x => ordenacao.Select(y => y.Environment).IndexOf(x.EnvironmentId)).ToList();
                    }


                }
                // Caso não for informado nenhuma ordenação eu ordeno pelo Environment
                else
                {
                    errorsSearchList = errorsSearchList.Where(x => x.Filed == false).OrderBy(x => x.EnvironmentId).ToList();
                }

            }

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
