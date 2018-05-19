using System;
using System.Linq;
using System.Reflection;
using SheetToObjects.Lib.Attributes.MappingType;
using SheetToObjects.Lib.Attributes.Rules;
using SheetToObjects.Lib.Configuration.ColumnMappings;

namespace SheetToObjects.Lib.Configuration
{
    public class ColumnsMappingBuilder<TModel>
    {
        private readonly MappingConfig _mappingConfig;

        public ColumnsMappingBuilder(MappingConfig mappingConfig)
        {
            _mappingConfig = mappingConfig;
        }

        /// <summary>
        /// Add a column
        /// </summary>
        public ColumnsMappingBuilder<TModel> Add(Func<ColumnMappingBuilder<TModel>, ColumnMapping> columnMappingBuilderFunc)
        {
            var columnMapping = columnMappingBuilderFunc(new ColumnMappingBuilder<TModel>());

            _mappingConfig.ColumnMappings.RemoveAll(c => c.PropertyName == columnMapping.PropertyName);
            _mappingConfig.ColumnMappings.Add(columnMapping);

            return this;
        }
    }
}