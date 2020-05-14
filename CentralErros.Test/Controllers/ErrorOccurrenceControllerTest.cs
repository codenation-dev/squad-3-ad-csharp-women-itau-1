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
    public class ErrorOccurrenceControllerTest
    {
        [Fact]
        public void Should_Be_Ok_When_Get_All()
        {
            var fakes = new FakeContext("ErrorControllerGetAll");

            var fakeErrorOccurrenceService = fakes.FakeErrorOccurrenceService().Object;
            var fakeLevelService = fakes.FakeLevelService().Object;
            var fakeEnvironmentService = fakes.FakeEnvironmentService().Object;

            var expected = fakes.Mapper.Map<List<ErrorOccurrence>>(fakeErrorOccurrenceService.GetAllErrors());

            var contexto = new CentralErroContexto(fakes.FakeOptions);

            var controller = new ErrorOccurrenceController(fakes.Mapper, contexto,
                fakeErrorOccurrenceService, fakeLevelService,
                fakeEnvironmentService);
            var result = controller.GetAll();

            Assert.IsType<OkObjectResult>(result.Result);
            var actual = (result.Result as OkObjectResult).Value as List<ErrorOccurrence>;

            Assert.NotNull(actual);
            Assert.Equal(expected, actual, new ErrorOccurrenceIdComparer());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void Should_Be_Ok_When_Get_By_Id(int id)
        {
            var fakes = new FakeContext("ErrorControllerId");

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

        [Fact]
        public void Should_Be_Ok_When_Get_Filed()
        {
            var fakes = new FakeContext("ErrorControllerGetFiled");

            var fakeErrorOccurrenceService = fakes.FakeErrorOccurrenceService().Object;
            var fakeLevelService = fakes.FakeLevelService().Object;
            var fakeEnvironmentService = fakes.FakeEnvironmentService().Object;

            var expected = fakes.Mapper.Map<List<ErrorOccurrenceDTO>>(fakeErrorOccurrenceService.FindFiledErrors());

            var contexto = new CentralErroContexto(fakes.FakeOptions);

            var controller = new ErrorOccurrenceController(fakes.Mapper, contexto,
                fakeErrorOccurrenceService, fakeLevelService,
                fakeEnvironmentService);
            var result = controller.GetFiled();

            Assert.IsType<OkObjectResult>(result.Result);
            var actual = (result.Result as OkObjectResult).Value as List<ErrorOccurrenceDTO>;

            Assert.NotNull(actual);
            Assert.Equal(expected, actual, new ErrorDTOIdComparer());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void Should_Be_Ok_When_Delete_By_Id(int id)
        {
            var fakes = new FakeContext("ErrorControllerDelete");

            var fakeErrorOccurrenceService = fakes.FakeErrorOccurrenceService().Object;
            var fakeLevelService = fakes.FakeLevelService().Object;
            var fakeEnvironmentService = fakes.FakeEnvironmentService().Object;

            var expected = fakes.Mapper.Map<ErrorOccurrenceDTO>(fakeErrorOccurrenceService.FindById(id));

            var contexto = new CentralErroContexto(fakes.FakeOptions);

            var controller = new ErrorOccurrenceController(fakes.Mapper, contexto,
                fakeErrorOccurrenceService, fakeLevelService,
                fakeEnvironmentService);
            var result = controller.Delete(id);

            Assert.IsType<OkObjectResult>(result.Result);
            var actual = (result.Result as OkObjectResult).Value as ErrorOccurrenceDTO;

            Assert.NotNull(actual);
            Assert.Equal(expected, actual, new ErrorDTOIdComparer());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void Should_Be_Ok_When_Set_Unarchirve_Error_By_Id(int id)
        {
            var fakes = new FakeContext("ErrorControllerUnarchirve");

            var fakeErrorOccurrenceService = fakes.FakeErrorOccurrenceService().Object;
            var fakeLevelService = fakes.FakeLevelService().Object;
            var fakeEnvironmentService = fakes.FakeEnvironmentService().Object;

            var expected = fakes.Mapper.Map<ErrorOccurrenceDTO>(fakeErrorOccurrenceService.FindById(id));

            var contexto = new CentralErroContexto(fakes.FakeOptions);

            var controller = new ErrorOccurrenceController(fakes.Mapper, contexto,
                fakeErrorOccurrenceService, fakeLevelService,
                fakeEnvironmentService);
            var result = controller.SetUnarchiveErrors(id);

            Assert.IsType<OkObjectResult>(result.Result);
            var actual = (result.Result as OkObjectResult).Value as ErrorOccurrenceDTO;

            Assert.NotNull(actual);
            Assert.Equal(expected, actual, new ErrorDTOIdComparer());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void Should_Be_Ok_When_Set_Filed_Error_By_Id(int id)
        {
            var fakes = new FakeContext("ErrorControllerFile");

            var fakeErrorOccurrenceService = fakes.FakeErrorOccurrenceService().Object;
            var fakeLevelService = fakes.FakeLevelService().Object;
            var fakeEnvironmentService = fakes.FakeEnvironmentService().Object;

            var expected = fakes.Mapper.Map<ErrorOccurrenceDTO>(fakeErrorOccurrenceService.FindById(id));

            var contexto = new CentralErroContexto(fakes.FakeOptions);

            var controller = new ErrorOccurrenceController(fakes.Mapper, contexto,
                fakeErrorOccurrenceService, fakeLevelService,
                fakeEnvironmentService);
            var result = controller.SetFiledErrors(id);

            Assert.IsType<OkObjectResult>(result.Result);
            var actual = (result.Result as OkObjectResult).Value as ErrorOccurrenceDTO;

            Assert.NotNull(actual);
            Assert.Equal(expected, actual, new ErrorDTOIdComparer());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void Should_Be_Ok_When_Get_Filed_Error_By_Id(int id)
        {
            var fakes = new FakeContext("ErrorControllerFiledId");

            var fakeErrorOccurrenceService = fakes.FakeErrorOccurrenceService().Object;
            var fakeLevelService = fakes.FakeLevelService().Object;
            var fakeEnvironmentService = fakes.FakeEnvironmentService().Object;

            var expected = fakes.Mapper.Map<ErrorDetailsDTO>(fakeErrorOccurrenceService.FindById(id));

            var contexto = new CentralErroContexto(fakes.FakeOptions);

            var controller = new ErrorOccurrenceController(fakes.Mapper, contexto,
                fakeErrorOccurrenceService, fakeLevelService,
                fakeEnvironmentService);
            var result = controller.GetFiledErrors(id);

            Assert.IsType<OkObjectResult>(result.Result);
            var actual = (result.Result as OkObjectResult).Value as ErrorDetailsDTO;

            Assert.NotNull(actual);
            Assert.Equal(expected, actual, new ErrorDetailsDTOComparer());
        }


        [Theory]
        //mostra 6 erros
        [InlineData(3, 1, 1, "error")]
        //mostra 1 erro
        [InlineData(1, 1, 0, "ip")]
        //lista sem erro
        [InlineData(4, 1, 2, "details")]
        public void Should_Be_Ok_When_Get_By_Filter(int ambiente, int campoOrdenacao, int campoBuscado,
                string textoBuscado)
        {
            var fakes = new FakeContext("ErrorControllerFilter");

            var fakeErrorOccurrenceService = fakes.FakeErrorOccurrenceService().Object;
            var fakeLevelService = fakes.FakeLevelService().Object;
            var fakeEnvironmentService = fakes.FakeEnvironmentService().Object;

            var expected = fakes.Mapper.Map<List<ErrorOccurrence>>(fakeErrorOccurrenceService.FindByFilters(ambiente, campoOrdenacao, campoBuscado, textoBuscado));

            var contexto = new CentralErroContexto(fakes.FakeOptions);

            var controller = new ErrorOccurrenceController(fakes.Mapper, contexto,
                fakeErrorOccurrenceService, fakeLevelService,
                fakeEnvironmentService);
            var result = controller.GetErrorFilter(ambiente, campoOrdenacao, campoBuscado, textoBuscado);

            Assert.IsType<OkObjectResult>(result.Result);
            var actual = (result.Result as OkObjectResult).Value as List<ErrorOccurrence>;

            Assert.NotNull(actual);
            Assert.Equal(expected, actual, new ErrorOccurrenceIdComparer());
        }
    }
}
