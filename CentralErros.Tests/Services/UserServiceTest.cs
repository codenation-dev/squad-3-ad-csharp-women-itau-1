using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CentralErros.Models;
using CentralErros.Services;
using CentralErros.Tests.Comparers;
using Xunit;

namespace CentralErros.Tests.Services
{
    public class UserServiceTest
    {
        [Theory]
        [InlineData(1)]
        public void Should_Return_Right_User_When_Find_By_Id(int id)
        {
            var fakeContext = new FakeContext("UserById");
            fakeContext.FillWith<User>();

            using (var context = new CentralErroContexto(fakeContext.FakeOptions))
            {
                var expected = fakeContext.GetFakeData<User>().Find(x => x.Id == id);

                var service = new UserService(context);
                var actual = service.FindById(id);

                Assert.Equal(expected, actual, new UserIdComparer());
            }
        }

    }
}
