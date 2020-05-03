using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using CentralErros.Models;

namespace CentralErros.Tests.Comparers
{
    public class LoginIdComparer : IEqualityComparer<User>
    {
        public bool Equals(User x, User y)
        {
            return x.Email == y.Email && x.Password == y.Password;
        }

        public int GetHashCode(User obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
