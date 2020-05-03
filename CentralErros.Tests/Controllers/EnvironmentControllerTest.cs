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
    public class EnvironmentControllerTest
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Should_Be_Ok_When_Get_By_Id (int id)
        {
            var fakes = new FakeContext("EnvironmentControllerTest");
            
            var fakeEnvironmentService = fakes.FakeEnvironmentService().Object; 
            
            var expected = fakes.Mapper.Map<CentralErros.Models.Environment>(fakeEnvironmentService.FindById(id));
            
            var contexto = new CentralErroContexto(fakes.FakeOptions);

            var controller = new EnvironmentController(fakeEnvironmentService, 
                fakes.Mapper, contexto);

            var result = controller.Get(id);

            Assert.IsType<OkObjectResult>(result.Result);
            var actual = (result.Result as OkObjectResult).Value as CentralErros.Models.Environment;

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
