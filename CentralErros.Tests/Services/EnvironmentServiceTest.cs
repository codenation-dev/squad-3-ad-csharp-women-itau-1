using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using CentralErros.Models;
using CentralErros.Services;
using CentralErros.Tests.Comparers;
using Xunit;

namespace CentralErros.Tests.Services
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
    }
}
