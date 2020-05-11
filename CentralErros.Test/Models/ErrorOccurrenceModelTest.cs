using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using CentralErros.Models;
using Xunit;

namespace CentralErros.Test.Models
{
    public class ErrorOccurrenceModelTest : ModelBaseTest
    {
        public ErrorOccurrenceModelTest() : base(new CentralErroContexto())
        {
            base.Table = "ERROR_OCURRENCE";
            base.Model = "CentralErros.Models.ErrorOccurrence";
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
        [InlineData("ENVIRONMENT_ID", false, "ENVIRONMENT", "ID")]
        [InlineData("LEVEL_ID", false, "LEVEL", "ID")]
        public void Should_Have_Foreign_Keys(string campoNome, bool ehNulo,
            string tabelaRelacionamento, string chaveRelacionamento)
        {
            CompareFK(campoNome, ehNulo, tabelaRelacionamento, chaveRelacionamento);
        }

        [Theory]
        [InlineData("ID", false, typeof(int))]
        [InlineData("TITLE", false, typeof(string))]
        [InlineData("REGISTRATION_DATE", false, typeof(DateTime))]
        [InlineData("ORIGIN", false, typeof(string))]
        [InlineData("FILED", false, typeof(bool))]
        [InlineData("DETAILS", false, typeof(string))]
        [InlineData("USERNAME", false, typeof(string))]
        [InlineData("ID_EVENTS", false, typeof(int))]
        [InlineData("ENVIRONMENT_ID", false, typeof(int))]
        [InlineData("LEVEL_ID", false, typeof(int))]
        public void Should_Have_Fields(string campoNome, bool ehNulo,
            Type campoTipo)
        {
            CompareFields(campoNome, ehNulo, campoTipo);
        }
    }
}
