using System;
using CentralErros.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace CentralErros.Tests
{
    public class FakeContext
    {
        public DbContextOptions<CentralErroContexto> FakeOptions { get; }
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

            DataFileNames.Add(typeof(User), $"FakeData{Path.DirectorySeparatorChar}users.json");
            DataFileNames.Add(typeof(CentralErros.Models.Environment), $"FakeData{Path.DirectorySeparatorChar}environment.json");
            DataFileNames.Add(typeof(Level), $"FakeData{Path.DirectorySeparatorChar}level.json");
            DataFileNames.Add(typeof(ErrorOccurrence), $"FakeData{Path.DirectorySeparatorChar}errorOcurence.json");

        }

        public void FillWithAll()
        {
            FillWith<User>();
            FillWith<CentralErros.Models.Environment>();
            FillWith<Level>();
            FillWith<ErrorOccurrence>();
        }

        private void FillWith<T>()
        {
            using (var context = new CentralErroContexto(FakeOptions))
            {
                if (context.Set<T>().Count() == 0)
                {
                    foreach (T item in GetFakeData<T>())
                        context.Set<T>().Add(item);
                    context.SaveChanges();
                }
            }
        }

        public List<T> GetFakeData<T>()
        {
            string content = File.ReadAllText(FileName<T>());
            return JsonConvert.DeserializeObject<List<T>>(content);
        }
    }
}
