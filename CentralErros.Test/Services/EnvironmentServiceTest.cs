using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using CentralErros.Models;
using CentralErros.Services;
using CentralErros.Test.Comparers;
using Xunit;

namespace CentralErros.Test.Services
{
    public class EnvironmentServiceTest
    {
        private CentralErroContexto _contexto;
        private FakeContext _fakeContext;

        private EnvironmentService _envService;

        public EnvironmentServiceTest()
        {
            _fakeContext = new FakeContext("LevelTestes");
            _fakeContext.FillWithAll();

            _contexto = new CentralErroContexto(_fakeContext.FakeOptions);
            _envService = new EnvironmentService(_contexto);
        }

        [Fact]
        public void Should_Add_New_Env_When_Save()
        {
            var fakeEnv = _fakeContext.GetFakeData<CentralErros.Models.Environment>().First();

            var current = new CentralErros.Models.Environment();

            var service = new EnvironmentService(_contexto);
            current = service.SaveOrUpdate(fakeEnv);

            Assert.NotEqual(0, fakeEnv.Id);
        }
        
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Should_Return_Right_User_When_Find_By_Id(int id)
        {
            var environment = _fakeContext.GetFakeData<CentralErros.Models.Environment>().Find(x => x.Id == id);

            var service = new EnvironmentService(_contexto);
            var atual = service.FindById(environment.Id);

            Assert.Equal(environment, atual, new EnvironmentIdComparer());
        }

        [Fact]
        public void Should_Return_Ok_When_Find_All_Env()
        {
            var level = _fakeContext.GetFakeData<CentralErros.Models.Environment>().ToList();

            var service = new EnvironmentService(_contexto);
            var atual = service.FindAll();

            Assert.Equal(level, atual, new EnvironmentIdComparer());
        }
    }
}
