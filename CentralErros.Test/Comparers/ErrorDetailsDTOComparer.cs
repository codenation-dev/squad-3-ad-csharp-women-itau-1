using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using CentralErros.DTO;
using CentralErros.Models;

namespace CentralErros.Test.Comparers
{
    public class ErrorDetailsDTOComparer : IEqualityComparer<ErrorDetailsDTO>
    {
        public bool Equals(ErrorDetailsDTO x, ErrorDetailsDTO y)
        {
            return x.Username == y.Username;
        }

        public int GetHashCode(ErrorDetailsDTO obj)
        {
            return obj.Username.GetHashCode();
        }
    }
}
