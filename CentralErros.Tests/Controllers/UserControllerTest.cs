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
    public class UserControllerTest
    {
        [Theory]
        [InlineData(1)]
        public void Should_Be_Ok_When_Get_By_Id (int id)
        {
            var fakes = new FakeContext("UserControllerTest");

            var fakeUserService = fakes.FakeUserService().Object;

            var expected = fakes.Mapper.Map<User>(fakeUserService.FindById(id));
            
            var contexto = new CentralErroContexto(fakes.FakeOptions);

            var controller = new UserController(fakeUserService, 
                fakes.Mapper, contexto);
            var result = controller.Get(id);

            Assert.IsType<OkObjectResult>(result.Result);
            var actual = (result.Result as OkObjectResult).Value as User;

            Assert.NotNull(actual);
            Assert.Equal(expected, actual, new UserIdComparer());
        }

        [Theory]
        [InlineData(1)]
        public void Should_Be_Ok_When_Delete(int id)
        {
            var fakes = new FakeContext("UserControllerTest");

            var fakeUserService = fakes.FakeUserService().Object;

            var expected = fakes.Mapper.Map<User>(fakeUserService.FindById(id));

            var contexto = new CentralErroContexto(fakes.FakeOptions);

            var controller = new UserController(fakeUserService,
                fakes.Mapper, contexto);
            var result = controller.Delete(id);

            Assert.IsType<OkObjectResult>(result.Result);
            var actual = (result.Result as OkObjectResult).Value as User;

            Assert.NotNull(actual);
            Assert.Equal(expected, actual, new UserIdComparer());
        }
    }
}
