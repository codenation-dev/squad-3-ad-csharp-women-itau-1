using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CentralErros.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Xunit;

namespace CentralErros.Test.Models
{
    public abstract class ModelBaseTest
    {
        private CentralErroContexto _contexto;

        protected string Model { get; set; }
        protected string Table { get; set; }

        public ModelBaseTest(CentralErroContexto contexto)
        {
            _contexto = contexto;
        }

        private IEntityType GetEntity()
        {
            return _contexto.Model.FindEntityType(Model);
        }

        private IEntityType GetEntity(string tableName)
        {
            return _contexto.Model.GetEntityTypes()
                            .FirstOrDefault(x => GetTableName(x) == tableName);
        }

        private string GetTableName(IEntityType entity)
        {
            var annotation = entity.FindAnnotation("Relational:TableName");
            return annotation?.Value?.ToString();
        }

        private string GetFieldName(IProperty property)
        {
            var annotation = property.FindAnnotation("Relational:ColumnName");
            return annotation?.Value?.ToString();
        }

        private IEnumerable<string> GetFieldName(IEntityType entity)
        {
            var properties = entity.GetProperties();
            return properties?.Select(x => this.GetFieldName(x)).ToList();
        }

        private IEnumerable<string> GetPrimaryKeys(IEntityType entity)
        {
            var key = entity.FindPrimaryKey();
            return key?.Properties.Select(x => this.GetFieldName(x)).ToList();
        }

        protected void ComparePrimaryKeys(params string[] keys)
        {
            var entity = GetEntity();
            Assert.NotNull(entity);

            var currentKeys = GetPrimaryKeys(entity);
            Assert.NotNull(currentKeys);
            Assert.Contains(keys, x => currentKeys.Contains(x));
        }

        private IProperty FindField(IEntityType entity, string fieldName)
        {
            var properties = entity.GetProperties();
            return properties.FirstOrDefault(x => this.GetFieldName(x) == fieldName);
        }

        protected void CompareFields(string fieldName, bool IsNull,
            Type fieldType)
        {
            var entity = GetEntity();
            Assert.NotNull(entity);
            Assert.Contains(fieldName, GetFieldName(entity));

            var property = FindField(entity, fieldName);

            var expected = new
            {
                type = fieldType,
                nulo = IsNull
            }.ToString();

            var actual = new
            {
                type = property.ClrType,
                nulo = property.IsNullable
            }.ToString();

            Assert.Equal(expected, actual);
        }

        protected void AssertTable()
        {
            var entity = GetEntity();
            Assert.NotNull(entity);

            var actual = this.GetTableName(entity);
            Assert.Equal(Table, actual);
        }

        protected void CompareFK(string fieldName, bool IsNull,
            string tabelaRelacionamentoEsperado, params string[] chaveRelacionamentoEsperado)
        {
            var entity = GetEntity();
            Assert.NotNull(entity);

            var relacionamentoEntity = GetEntity(tabelaRelacionamentoEsperado);
            Assert.NotNull(relacionamentoEntity);

            var property = FindField(entity, fieldName);
            Assert.NotNull(property);

            var foreignKey = entity.FindForeignKeys(property)
                                   .FirstOrDefault(x => x.PrincipalEntityType == relacionamentoEntity);
            Assert.NotNull(foreignKey);
            Assert.Equal(IsNull, !foreignKey.IsRequired);

            var currentKeys = foreignKey.PrincipalKey.Properties.Select(x => GetFieldName(x));
            Assert.Contains(chaveRelacionamentoEsperado, x => currentKeys.Contains(x));
        }
    }
}
