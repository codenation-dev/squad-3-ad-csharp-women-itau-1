using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CentralErros.Controllers;
using CentralErros.DTO;
using CentralErros.Models;
using CentralErros.Tests.Comparers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace CentralErros.Tests.Controllers
{
    public class LevelControllerTest
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Should_Be_Ok_When_Get_By_Id (int id)
        {
            var fakes = new FakeContext("LevelControllerTest");
            
            var fakeLevelService = fakes.FakeLevelService().Object; 
            
            var expected = fakes.Mapper.Map<Level>(fakeLevelService.FindByIdLevel(id));
            
            var contexto = new CentralErroContexto(fakes.FakeOptions);

            var controller = new LevelController(fakeLevelService, 
                fakes.Mapper, contexto);

            var result = controller.Get(id);

            Assert.IsType<OkObjectResult>(result.Result);
            var actual = (result.Result as OkObjectResult).Value as Level;

            Assert.NotNull(actual);
            Assert.Equal(expected, actual, new LevelIdComparer());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Should_Be_Ok_When_Delete(int id)
        {
            var fakes = new FakeContext("LevelTest");

            var fakeLevelService = fakes.FakeLevelService().Object;

            var expected = fakes.Mapper.Map<Level>(fakeLevelService.FindByIdLevel(id));

            var contexto = new CentralErroContexto(fakes.FakeOptions);

            var controller = new LevelController(fakeLevelService,
                fakes.Mapper, contexto);

            var result = controller.Delete(id);

            Assert.IsType<OkObjectResult>(result.Result);
            var actual = (result.Result as OkObjectResult).Value as Level;

            Assert.NotNull(actual);
            Assert.Equal(expected, actual, new LevelIdComparer());
        }
    }
}
