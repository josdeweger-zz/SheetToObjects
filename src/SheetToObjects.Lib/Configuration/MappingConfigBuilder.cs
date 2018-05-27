using System;

namespace SheetToObjects.Lib.Configuration
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
        /// Add columns
        /// </summary>
        public MappingConfigBuilder<T> Columns(Func<ColumnsMappingBuilder<T>, ColumnsMappingBuilder<T>> columnMappingBuilderFunc)
        {
            columnMappingBuilderFunc(new ColumnsMappingBuilder<T>(_mappingConfig));
            return this;
        }
        
        public MappingConfig BuildConfig()
        {
            return _mappingConfig;
        }
    }
}