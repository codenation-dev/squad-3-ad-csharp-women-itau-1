using System.Collections.Generic;
using CentralErros.Models;

namespace CentralErros.Services
{
    public interface IErrorOcurrenceService
    {
        ErrorOccurrence FindById(int id);
        List<ErrorOccurrence> FindErrorsByDetails(string details);
        List<ErrorOccurrence> FindErrorsByOrigin(string origin);
        bool FindFiledOcurrence(ErrorOccurrence error);
        List<ErrorOccurrence> GetAllErrors();
        ErrorOccurrence SaveOrUpdate(ErrorOccurrence error);
    }
}