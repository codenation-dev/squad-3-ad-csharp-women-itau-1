using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using CentralErros.DTO;
using CentralErros.Models;

namespace CentralErros.Test.Comparers
{
    public class ErrorDTOIdComparer : IEqualityComparer<ErrorOccurrenceDTO>
    {
        public bool Equals(ErrorOccurrenceDTO x, ErrorOccurrenceDTO y)
        {
            return x.Username == y.Username;
        }

        public int GetHashCode(ErrorOccurrenceDTO obj)
        {
            return obj.Username.GetHashCode();
        }
    }
}
