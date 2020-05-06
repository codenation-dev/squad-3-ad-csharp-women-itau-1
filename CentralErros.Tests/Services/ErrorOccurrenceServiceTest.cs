using System;
using System.Collections.Generic;
using System.Text;
using CentralErros.Models;
using CentralErros.Services;
using CentralErros.Tests.Comparers;
using Xunit;

namespace CentralErros.Tests.Services
{
    public class ErrorOccurrenceServiceTest
    {
        [Theory]
        [InlineData(1)]
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
            fakeError.Id = 2;
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
    }
}
