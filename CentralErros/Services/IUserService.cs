﻿using CentralErros.Api.Models;
using System.Collections.Generic;

namespace CentralErros.Services
{
    public interface IUserService
    {
        User FindById(int userId);
        IList<User> FindByName(string name);
        User Save(User user);
    }
}