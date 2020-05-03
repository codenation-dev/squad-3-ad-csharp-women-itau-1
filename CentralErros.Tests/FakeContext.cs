using System;
using CentralErros.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using CentralErros.DTO;
using Moq;
using CentralErros.Services;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;

namespace CentralErros.Tests
{
    public class FakeContext
    {
        public DbContextOptions<CentralErroContexto> FakeOptions { get; }
        public readonly IMapper Mapper;

        private Dictionary<Type, string> DataFileNames { get; } =
            new Dictionary<Type, string>();

        private string FileName<T>()
        {
            return DataFileNames[typeof(T)];
        }

        public FakeContext(string testName)
        {
            FakeOptions = new DbContextOptionsBuilder<CentralErroContexto>()
                .UseInMemoryDatabase(databaseName: $"CentralErrors_{testName}")
                .Options;

            DataFileNames.Add(typeof(User), $"FakeData{Path.DirectorySeparatorChar}user.json");
            DataFileNames.Add(typeof(UserDTO), $"FakeData{Path.DirectorySeparatorChar}user.json");
            DataFileNames.Add(typeof(CentralErros.Models.Environment), $"FakeData{Path.DirectorySeparatorChar}environment.json");
            DataFileNames.Add(typeof(EnvironmentDTO), $"FakeData{Path.DirectorySeparatorChar}environment.json");
            DataFileNames.Add(typeof(Level), $"FakeData{Path.DirectorySeparatorChar}level.json");
            DataFileNames.Add(typeof(LevelDTO), $"FakeData{Path.DirectorySeparatorChar}level.json");
            DataFileNames.Add(typeof(ErrorOccurrence), $"FakeData{Path.DirectorySeparatorChar}errorOcurrence.json");
            DataFileNames.Add(typeof(ErrorOccurrenceDTO), $"FakeData{Path.DirectorySeparatorChar}errorOcurrence.json");

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CentralErros.Models.Environment, EnvironmentDTO>().ReverseMap();
                cfg.CreateMap<ErrorOccurrence, ErrorOccurrenceDTO>().ReverseMap();
                cfg.CreateMap<Level, LevelDTO>().ReverseMap();
                cfg.CreateMap<User, UserDTO>().ReverseMap();
            });

            this.Mapper = configuration.CreateMapper();
        }

        public void FillWithAll()
        {
            FillWith<User>();
            FillWith<CentralErros.Models.Environment>();
            FillWith<Level>();
            FillWith<ErrorOccurrence>();
        }
        public List<T> GetFakeData<T>()
        {
            string content = File.ReadAllText(FileName<T>());
            return JsonConvert.DeserializeObject<List<T>>(content);
        }

        public void FillWith<T>() where T : class
        {
            using (var context = new CentralErroContexto(this.FakeOptions))
            {
                if (context.Set<T>().Count() == 0)
                {
                    foreach (T item in GetFakeData<T>())
                        context.Set<T>().Add(item);
                    context.SaveChanges();
                }
            }
        }

        public Mock<ILevelService> FakeLevelService()
        {
            var service = new Mock<ILevelService>();

            service.Setup(x => x.FindByIdLevel(It.IsAny<int>()))
                .Returns((int id) => GetFakeData<Level>()
                .FirstOrDefault(x => x.IdLevel == id));

            service.Setup(x => x.SaveOrUpdate(It.IsAny<Level>()))
                .Returns((Level level) =>
                {
                    if (level.IdLevel == 0)
                        level.IdLevel = 999;
                    return level;
                });

            return service;
        }

        public Mock<IErrorOcurrenceService> FakeErrorOccurrenceService()
        {
            var service = new Mock<IErrorOcurrenceService>();

            service.Setup(x => x.FindById(It.IsAny<int>()))
                .Returns((int id) => GetFakeData<ErrorOccurrence>()
                .FirstOrDefault(x => x.Id == id));

            service.Setup(x => x.SaveOrUpdate(It.IsAny<ErrorOccurrence>()))
                .Returns((ErrorOccurrence error) =>
                {
                    if (error.Id == 0)
                        error.Id = 999;
                    return error;
                });

            return service;
        }

        public Mock<IEnvironmentService> FakeEnvironmentService()
        {
            var service = new Mock<IEnvironmentService>();

            service.Setup(x => x.FindById(It.IsAny<int>()))
                .Returns((int id) => GetFakeData<CentralErros.Models.Environment>()
                .FirstOrDefault(x => x.Id == id));

            service.Setup(x => x.SaveOrUpdate(It.IsAny<CentralErros.Models.Environment>()))
                .Returns((CentralErros.Models.Environment env) =>
                {
                    if (env.Id == 0)
                        env.Id = 999;
                    return env;
                });

            return service;
        }

        public Mock<IUserService> FakeUserService()
        {
            var service = new Mock<IUserService>();

            service.Setup(x => x.FindById(It.IsAny<int>()))
                .Returns((int id) => GetFakeData<User>()
                .FirstOrDefault(x => x.Id == id));

            service.Setup(x => x.FindByLogin(It.IsAny<string>(), It.IsAny<string>()))
                .Returns((string email, string password) => GetFakeData<User>()
                .FirstOrDefault(x => x.Email == email && x.Password == password));

            service.Setup(x => x.Save(It.IsAny<User>()))
                .Returns((User user) =>
                {
                    if (user.Id == 0)
                        user.Id = 999;
                    return user;
                });

            return service;
        }
    }
}
