using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CentralErros.Models;
using CentralErros.Services;
using CentralErros.Test.Comparers;
using Xunit;

namespace CentralErros.Test.Services
{
    public class ErrorOccurrenceServiceTest
    {
        private CentralErroContexto _contexto;
        private FakeContext _fakeContext;

        private ErrorOcurrenceService _errorService;

        public ErrorOccurrenceServiceTest()
        {
            _fakeContext = new FakeContext("ErrorTestes");
            _fakeContext.FillWithAll();

            _contexto = new CentralErroContexto(_fakeContext.FakeOptions);
            _errorService = new ErrorOcurrenceService(_contexto);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void Should_Return_Right_Error_When_Find_By_Id(int id)
        {
            var fakeContext = new FakeContext("ErrorById");
            fakeContext.FillWith<ErrorOccurrence>();

            using (var context = new CentralErroContexto(fakeContext.FakeOptions))
            {
                var expected = fakeContext.GetFakeData<ErrorOccurrence>().Find(x => x.Id == id);

                var service = new ErrorOcurrenceService(context);
                var actual = service.FindById(id);

                Assert.Equal(expected, actual, new ErrorOccurrenceIdComparer());
            }
        }

        [Fact]
        public void Should_Add_New_Error_When_Save()
        {
            var fakeContext = new FakeContext("SaveNewError");

            var fakeError = new ErrorOccurrence();
            fakeError.Id = 3;
            fakeError.Title = "title";
            fakeError.RegistrationDate = DateTime.Today;
            fakeError.Origin = "ip";
            fakeError.Filed = false;
            fakeError.Details = "details";
            fakeError.IdEvent = 2;            
            fakeError.EnvironmentId = 2;
            fakeError.LevelId = 2;
            fakeError.Username = "jaque";

            using (var context = new CentralErroContexto(fakeContext.FakeOptions))
            {
                var service = new ErrorOcurrenceService(context);
                var actual = service.SaveOrUpdate(fakeError);

                Assert.NotEqual(0, actual.Id);
            }
        }

        [Fact]
        public void Should_Return_Ok_When_Find_All_Errors()
        {
            var error = _fakeContext.GetFakeData<ErrorOccurrence>()
                .Where(x => x.Filed == false)
                .ToList();

            var service = new ErrorOcurrenceService(_contexto);
            var atual = service.GetAllErrors();

            Assert.Equal(error, atual, new ErrorOccurrenceIdComparer());
        }

        [Fact]
        public void Should_Return_Ok_When_Find_Filed_Errors()
        {
            var error = _fakeContext.GetFakeData<ErrorOccurrence>()
                .Where(x => x.Filed == true)
                .ToList();

            var service = new ErrorOcurrenceService(_contexto);
            var atual = service.FindFiledErrors();

            Assert.Equal(error, atual, new ErrorOccurrenceIdComparer());
        }
    }
}
