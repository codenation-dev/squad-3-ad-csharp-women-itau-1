using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using CentralErros.Models;

namespace CentralErros.Test.Comparers
{
    public class LevelIdComparer : IEqualityComparer<Level>
    {
        public bool Equals(Level x, Level y)
        {
            return x.IdLevel == y.IdLevel;
        }

        public int GetHashCode(Level obj)
        {
            return obj.IdLevel.GetHashCode();
        }
    }
}
