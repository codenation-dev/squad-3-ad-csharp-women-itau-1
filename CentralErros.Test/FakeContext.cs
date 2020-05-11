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
using System.Runtime.CompilerServices;

namespace CentralErros.Test
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
                cfg.CreateMap<ErrorOccurrence, ErrorDetailsDTO>().ReverseMap();
                cfg.CreateMap<Level, LevelDTO>().ReverseMap();
                
            });

            this.Mapper = configuration.CreateMapper();
        }

        public void FillWithAll()
        {
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

            service.Setup(x => x.FindAllLevels())
                .Returns(() => GetFakeData<Level>().ToList());

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

            service.Setup(x => x.GetAllErrors())
                .Returns(() => GetFakeData<ErrorOccurrence>().ToList());

            service.Setup(x => x.FindFiledErrors())
                .Returns(() => GetFakeData<ErrorOccurrence>()
                .Where(x => x.Filed == true)
                .ToList());

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

            service.Setup(x => x.FindAll())
                .Returns(() => GetFakeData<CentralErros.Models.Environment>().ToList());

            service.Setup(x => x.SaveOrUpdate(It.IsAny<CentralErros.Models.Environment>()))
                .Returns((CentralErros.Models.Environment env) =>
                {
                    if (env.Id == 0)
                        env.Id = 999;
                    return env;
                });

            return service;
        }
    }
}
