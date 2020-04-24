using System.Collections.Generic;
using CentralErros.Models;

namespace CentralErros.Services
{
    public interface IErrorOcurrenceService
    {
        bool ErrorExists(int id);
        ErrorOccurrence FindById(int id);
        List<ErrorOccurrence> FindErrorsByDetails(string details);
        List<ErrorOccurrence> FindErrorsByEnvironment(string env);
        List<ErrorOccurrence> FindErrorsByLevel(string level);
        List<ErrorOccurrence> FindErrorsByOrigin(string origin);
        bool FindFiledOcurrence(ErrorOccurrence error);
        List<ErrorOccurrence> GetAllErrors();
        List<ErrorOccurrence> OrderByLevel();
        ErrorOccurrence SaveOrUpdate(ErrorOccurrence error);
    }
}