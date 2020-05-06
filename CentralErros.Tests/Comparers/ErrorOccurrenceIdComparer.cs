using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using CentralErros.Models;

namespace CentralErros.Tests.Comparers
{
    public class ErrorOccurrenceIdComparer : IEqualityComparer<ErrorOccurrence>
    {
        public bool Equals(ErrorOccurrence x, ErrorOccurrence y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(ErrorOccurrence obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
