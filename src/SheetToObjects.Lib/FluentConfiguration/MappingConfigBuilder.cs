using System;

namespace SheetToObjects.Lib.FluentConfiguration
{
    public class MappingConfigBuilder<T>
    {
        private readonly MappingConfig _mappingConfig = new MappingConfig();
        
        /// <summary>
        /// Specify whether the sheet contains headers or not (will skip the first row for data parsing)
        /// </summary>
        public MappingConfigBuilder<T> HasHeaders()
        {
            _mappingConfig.HasHeaders = true;
            return this;
        }

        /// <summary>
        /// Add a column
        /// </summary>
        public MappingConfigBuilder<T> MapColumn(Func<ColumnMappingBuilder<T>, ColumnMapping> columnMappingBuilderFunc)
        {
            var columnMapping = columnMappingBuilderFunc(new ColumnMappingBuilder<T>());

            _mappingConfig.ColumnMappings.RemoveAll(c => c.PropertyName == columnMapping.PropertyName);
            _mappingConfig.ColumnMappings.Add(columnMapping);

            return this;
        }

        public MappingConfig Build()
        {
            return _mappingConfig;
        }
    }
}