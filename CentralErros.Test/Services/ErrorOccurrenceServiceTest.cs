using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Core.Internal;
using CentralErros.Models;
using CentralErros.Services;
using CentralErros.Test.Comparers;
using Microsoft.EntityFrameworkCore.Internal;
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

        [Fact]
        public void Should_Return_Ok_When_Filed_Errors()
        {
            var fakeContext = new FakeContext("FileError");

            var fakeError = new ErrorOccurrence();
            fakeError.Id = 1;
            fakeError.Filed = true;

            using (var context = new CentralErroContexto(fakeContext.FakeOptions))
            {
                var service = new ErrorOcurrenceService(context);
                var actual = service.SaveOrUpdate(fakeError);

                Assert.NotEqual(0, actual.Id);
            }
        }

        [Theory]
        //busca pelo Details, pela frequencia de Level
        [InlineData(3, 1, 2, "details")]
        //busca pelo Origin, pela frequencia de Level
        [InlineData(3, 1, 3, "ip")]
        //busca pelo LevelId, ordena pela frequencia do Origin
        [InlineData(3, 1, 1, "error")]
        //campo buscado = 0 e ambiente > 0, pela frequencia de Level
        [InlineData(3, 1, 0, "error")]
        //else, ordena pela frequencia de Level
        [InlineData(-1, 1, 0, "")]
        //busca pelo Details, ordena pela frequencia de Level
        [InlineData(3, 2, 2, "details")]
        //busca pelo Level, ordena pela frequencia de Details
        [InlineData(3, 2, 1, "error")]
        //busca o erro mas sem campo de Ordenação, ordena pela frequencia de Origin
        [InlineData(3, null, 2, "details")]
        //busca o erro mas sem campo Buscado, ordena pela frequencia de Level
        [InlineData(3, 2, null, "details")]
        //busca o erro mas campo de Ordenação = 0, ordena pela frequencia de Origin
        [InlineData(3, 0, 2, "details")]
        //busca o erro mas campo Buscado = 0, ordena pela frequencia de Level
        [InlineData(3, 2, 0, "details")]
        //lista zerada
        [InlineData(4, 1, 2, "details")]
        public void Should_Be_Ok_When_Get_By_Filter(int ambiente, int? campoOrdenacao, int? campoBuscado,
                string textoBuscado)
        {
            var fakeError = new ErrorOccurrence();

            if (textoBuscado == "error")
                fakeError.LevelId = 1;
            else if (textoBuscado == "warn")
                fakeError.LevelId = 2;
            else
                fakeError.LevelId = 3;

            List<ErrorOccurrence> errorsSearchList = new List<ErrorOccurrence>();
            List<ErrorOccurrence> errorsList = new List<ErrorOccurrence>();

            if (textoBuscado != "" && campoBuscado != 0 && campoBuscado != null)
            {
                if (campoBuscado == 1)
                    errorsList = _fakeContext.GetFakeData<ErrorOccurrence>().Where(x => x.LevelId == fakeError.LevelId && x.EnvironmentId == ambiente).ToList();
                else if (campoBuscado == 2)
                    errorsList = _fakeContext.GetFakeData<ErrorOccurrence>().Where(x => x.Details.Contains(textoBuscado) && x.EnvironmentId == ambiente).ToList();
                else if (campoBuscado == 3)
                    errorsList = _fakeContext.GetFakeData<ErrorOccurrence>().Where(x => x.Origin.Contains(textoBuscado) && x.EnvironmentId == ambiente).ToList();
            }
            else if (ambiente > 0)
            {
                errorsList = _fakeContext.GetFakeData<ErrorOccurrence>().Where(x => x.EnvironmentId == ambiente).ToList();
            }
            else
            {
                errorsList = _fakeContext.GetFakeData<ErrorOccurrence>().ToList();
            }
            
            if (errorsList.Count() > 0)
            {

                if (campoOrdenacao == 1 && campoBuscado != 1)
                {
                    errorsSearchList = errorsList.OrderBy(x => x.LevelId).ToList();
                }
                else if (campoOrdenacao == 2)
                {
                    if (campoBuscado != 1)
                    {
                        var ordenacao = errorsList.GroupBy(x => x.LevelId)
                                    .Select(group => new
                                    {
                                        Level = group.Key,
                                        Quantidade = group.Count()
                                    })
                                    .OrderByDescending(x => x.Quantidade)
                                    .ToList();

                        errorsSearchList = errorsList.OrderBy(x => ordenacao.Select(y => y.Level).IndexOf(x.LevelId)).ToList();
                    }
                    else
                    {
                        var ordenacao = errorsList.GroupBy(x => x.Details)
                                                            .Select(group => new
                                                            {
                                                                Details = group.Key,
                                                                Quantidade = group.Count()
                                                            })
                                                            .OrderByDescending(x => x.Quantidade)
                                                            .ToList();

                        errorsSearchList = errorsList.OrderBy(x => ordenacao.Select(y => y.Details).IndexOf(x.Details)).ToList();
                    }
                }
                else
                {
                    var ordenacao = errorsList.GroupBy(x => x.Origin)
                                                            .Select(group => new
                                                            {
                                                                Origin = group.Key,
                                                                Quantidade = group.Count()
                                                            })
                                                            .OrderByDescending(x => x.Quantidade)
                                                            .ToList();

                    errorsSearchList = errorsList.OrderBy(x => ordenacao.Select(y => y.Origin).IndexOf(x.Origin)).ToList();
                }
            }
            else
            {
                errorsSearchList = errorsSearchList.OrderBy(x => x.Origin).ToList();
            }

            errorsSearchList = errorsSearchList.Where(x => x.Filed == false).ToList();

            List<int> errors_int = new List<int>();
            List<string> errors_string = new List<string>();

            if (errorsSearchList.Count() > 0)
            {

                if (campoOrdenacao == 1 && campoBuscado != 1)
                {
                    errors_int = errorsSearchList.Select(x => x.LevelId).ToList();
                }
                else if (campoOrdenacao == 2)
                {
                    if (campoBuscado != 1)
                    {
                        errors_int = errorsSearchList.Select(x => x.LevelId).ToList();
                    }
                    else
                    {
                        errors_string = errorsSearchList.Select(x => x.Details).ToList();
                    }
                }
                else
                {
                    errors_string = errorsSearchList.Select(x => x.Origin).ToList();
                }
            }
            else
            {
                errors_string = errorsSearchList.Select(x => x.Origin).ToList();
            }

            var service = new ErrorOcurrenceService(_contexto);
            var actual = service.FindByFilters(ambiente, campoOrdenacao, campoBuscado, textoBuscado);

            Assert.Equal(errorsSearchList, actual, new ErrorOccurrenceIdComparer());
        }
    }
}
