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
            DataFileNames.Add(typeof(CentralErros.Models.Environment), $"FakeData{Path.DirectorySeparatorChar}environment.json");
            DataFileNames.Add(typeof(Level), $"FakeData{Path.DirectorySeparatorChar}level.json");
            DataFileNames.Add(typeof(ErrorOccurrence), $"FakeData{Path.DirectorySeparatorChar}errorOcurrence.json");

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

    }
}
