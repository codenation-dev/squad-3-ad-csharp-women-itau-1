using System;
using System.Collections.Generic;
using System.Text;
using CentralErros.Models;
using Xunit;

namespace CentralErros.Test.Models
{
    public class LevelModelTest : ModelBaseTest
    {
        public LevelModelTest() : base(new CentralErroContexto())
        {
            base.Table = "LEVEL";
            base.Model = "CentralErros.Models.Level";
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
        [InlineData("LEVEL", false, typeof(string))]
        public void Should_Have_Fields(string campoNome, bool ehNulo,
            Type campoTipo)
        {
            CompareFields(campoNome, ehNulo, campoTipo);
        }
    }
}
