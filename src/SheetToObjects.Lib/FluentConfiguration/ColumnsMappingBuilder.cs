using System;

namespace SheetToObjects.Lib.FluentConfiguration
{
    public class ColumnsMappingBuilder<T>
    {
        private readonly MappingConfig _mappingConfig;

        public ColumnsMappingBuilder(MappingConfig mappingConfig)
        {
            _mappingConfig = mappingConfig;
        }

        /// <summary>
        /// Add a column
        /// </summary>
        public ColumnsMappingBuilder<T> Add(Func<ColumnMappingBuilder<T>, ColumnMapping> columnMappingBuilderFunc)
        {
            var columnMapping = columnMappingBuilderFunc(new ColumnMappingBuilder<T>());

            _mappingConfig.ColumnMappings.RemoveAll(c => c.PropertyName == columnMapping.PropertyName);
            _mappingConfig.ColumnMappings.Add(columnMapping);

            return this;
        }
    }
}