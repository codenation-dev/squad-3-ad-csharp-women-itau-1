using System;
using System.Collections.Generic;
using System.Text;
using CentralErros.Models;
using Xunit;

namespace CentralErros.Test.Models
{
    public class EnvironmentModelTest : ModelBaseTest
    {
        public EnvironmentModelTest() : base(new CentralErroContexto())
        {
            base.Table = "ENVIRONMENT";
            base.Model = "CentralErros.Models.Environment";
        }

        [Fact]
        public void Should_Have_Table()
        {
            AssertTable();
        }

        [Fact]
        public void Should_Have_Primary_Key()
        {
            ComparePrimaryKeys("ID");
        }

        [Theory]
        [InlineData("ID", false, typeof(int))]
        [InlineData("NAME", false, typeof(string))]
        public void Should_Have_Fields(string campoNome, bool ehNulo,
            Type campoTipo)
        {
            CompareFields(campoNome, ehNulo, campoTipo);
        }
    }
}
