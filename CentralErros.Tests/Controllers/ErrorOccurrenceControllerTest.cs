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
    public class ErrorOccurrenceControllerTest
    {
        [Theory]
        [InlineData(1)]

        public void Should_Be_Ok_When_Get_By_Id(int id)
        {
            var fakes = new FakeContext("ErrorOccurrenceControllerTest");

            var fakeErrorOccurrenceService = fakes.FakeErrorOccurrenceService().Object;
            var fakeLevelService = fakes.FakeLevelService().Object;
            var fakeEnvironmentService = fakes.FakeEnvironmentService().Object;

            var expected = fakes.Mapper.Map<ErrorOccurrence>(fakeErrorOccurrenceService.FindById(id));

            var contexto = new CentralErroContexto(fakes.FakeOptions);

            var controller = new ErrorOccurrenceController(fakes.Mapper, contexto,
                fakeErrorOccurrenceService, fakeLevelService,
                fakeEnvironmentService);
            var result = controller.Get(id);

            Assert.IsType<OkObjectResult>(result.Result);
            var actual = (result.Result as OkObjectResult).Value as ErrorOccurrence;

            Assert.NotNull(actual);
            Assert.Equal(expected, actual, new ErrorOccurrenceIdComparer());
        }










        
    }
}
