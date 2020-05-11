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
    public class LevelServiceTest
    {
        private CentralErroContexto _contexto;
        private FakeContext _fakeContext;

        private LevelService _levelService;

        public LevelServiceTest()
        {
            _fakeContext = new FakeContext("LevelTestes");
            _fakeContext.FillWithAll();

            _contexto = new CentralErroContexto(_fakeContext.FakeOptions);
            _levelService = new LevelService(_contexto);
        }

        [Fact]
        public void Should_Add_New_Level_When_Save()
        {
            var fakeLevel = _fakeContext.GetFakeData<Level>().First();

            var current = new Level();

            var service = new LevelService(_contexto);
            current = service.SaveOrUpdate(fakeLevel);

            Assert.NotEqual(0, fakeLevel.IdLevel);
        }
        
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Should_Return_Right_Level_When_Find_By_Id(int id)
        {
            var level = _fakeContext.GetFakeData<Level>().Find(x => x.IdLevel == id);

            var service = new LevelService(_contexto);
            var atual = service.FindByIdLevel(level.IdLevel);

            Assert.Equal(level, atual, new LevelIdComparer());
        }

        [Fact]
        public void Should_Return_Ok_When_Find_All_Levels()
        {
            var level = _fakeContext.GetFakeData<Level>().ToList();

            var service = new LevelService(_contexto);
            var atual = service.FindAllLevels();

            Assert.Equal(level, atual, new LevelIdComparer());
        }
    }
}
