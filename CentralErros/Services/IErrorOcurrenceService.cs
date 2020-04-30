﻿using System.Collections.Generic;
using CentralErros.Models;

namespace CentralErros.Services
{
    public interface IErrorOcurrenceService
    {
        ErrorOccurrence FindById(int id);        
        List<ErrorOccurrence> GetAllErrors();
        ErrorOccurrence SaveOrUpdate(ErrorOccurrence error);
    }
}