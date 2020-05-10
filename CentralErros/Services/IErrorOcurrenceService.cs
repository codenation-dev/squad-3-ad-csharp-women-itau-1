using System.Collections.Generic;
using CentralErros.Models;

namespace CentralErros.Services
{
    public interface IErrorOcurrenceService
    {
        ErrorOccurrence FindById(int id);        
        IList<ErrorOccurrence> GetAllErrors();
        ErrorOccurrence SaveOrUpdate(ErrorOccurrence error);
        List<ErrorOccurrence> FindByFilters(int ambiente, int campoOrdenacao, int campoBuscado, string textoBuscado);
        IList<ErrorOccurrence> FindFiledErrors();

        void FiledErrors(int id);
    }
}