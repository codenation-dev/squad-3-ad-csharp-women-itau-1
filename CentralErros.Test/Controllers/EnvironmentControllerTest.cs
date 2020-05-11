using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CentralErros.Controllers;
using CentralErros.DTO;
using CentralErros.Models;
using CentralErros.Test;
using CentralErros.Test.Comparers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace CentralErros.Test.Controllers
{
    public class EnvironmentControllerTest
    {
        [Fact]
        public void Should_Be_Ok_When_Find_All()
        {
            var fakes = new FakeContext("EnvironmentControllerTest");
            
            var fakeEnvironmentService = fakes.FakeEnvironmentService().Object; 
            
            var expected = fakes.Mapper.Map<List<CentralErros.Models.Environment>>(fakeEnvironmentService.FindAll());
            
            var contexto = new CentralErroContexto(fakes.FakeOptions);

            var controller = new EnvironmentController(fakeEnvironmentService, 
                fakes.Mapper, contexto);

            var result = controller.FindAll();

            Assert.IsType<OkObjectResult>(result.Result);
            var actual = (result.Result as OkObjectResult).Value as List<CentralErros.Models.Environment>;

            Assert.NotNull(actual);
            Assert.Equal(expected, actual, new EnvironmentIdComparer());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Should_Be_Ok_When_Delete(int id)
        {
            var fakes = new FakeContext("EnvironmentTest");

            var fakeEnvironmentService = fakes.FakeEnvironmentService().Object;

            var expected = fakes.Mapper.Map<CentralErros.Models.Environment>(fakeEnvironmentService.FindById(id));

            var contexto = new CentralErroContexto(fakes.FakeOptions);

            var controller = new EnvironmentController(fakeEnvironmentService,
                fakes.Mapper, contexto);

            var result = controller.Delete(id);

            Assert.IsType<OkObjectResult>(result.Result);
            var actual = (result.Result as OkObjectResult).Value as CentralErros.Models.Environment;

            Assert.NotNull(actual);
            Assert.Equal(expected, actual, new EnvironmentIdComparer());
        }
    }
}
