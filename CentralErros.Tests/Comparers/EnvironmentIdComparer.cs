using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace CentralErros.Tests.Comparers
{
    public class EnvironmentIdComparer : IEqualityComparer<CentralErros.Models.Environment>
    {
        public bool Equals(CentralErros.Models.Environment x, CentralErros.Models.Environment y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(CentralErros.Models.Environment obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
