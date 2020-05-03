using System;
using System.Collections.Generic;
using System.Text;
using CentralErros.Controllers;
using CentralErros.Models;
using CentralErros.Tests.Comparers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace CentralErros.Tests.Controllers
{
    public class LoginControllerTest
    {
        [Theory]
        [InlineData("jaquelinelaurenti@gmail.com", "@123456789")]
        public void Should_Be_Ok_When_Get_By_Id(string email, string password)
        {
            var fakes = new FakeContext("LoginControllerTest");

            var fakeUserService = fakes.FakeUserService().Object;

            var expected = fakes.Mapper.Map<User>(fakeUserService.FindByLogin(email, password));

            var contexto = new CentralErroContexto(fakes.FakeOptions);

            var controller = new LoginController(fakeUserService,
                fakes.Mapper, contexto);
            var result = controller.Get(email, password);

            Assert.IsType<OkObjectResult>(result.Result);
            var actual = (result.Result as OkObjectResult).Value as User;

            Assert.NotNull(actual);
            Assert.Equal(expected, actual, new LoginIdComparer());
        }
    }
}
