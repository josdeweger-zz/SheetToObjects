using System;

namespace SheetToObjects.Lib.Configuration
{
    public class MappingConfigBuilder<TModel>
    {
        private readonly MappingConfig _mappingConfig = new MappingConfig();
        
        /// <summary>
        /// Specify whether the sheet contains headers or not (will skip the first row for data parsing)
        /// </summary>
        public MappingConfigBuilder<TModel> HasHeaders()
        {
            _mappingConfig.HasHeaders = true;
            return this;
        }
        
        /// <summary>
        /// Add columns
        /// </summary>
        public MappingConfigBuilder<TModel> Columns(Func<ColumnsMappingBuilder<TModel>, ColumnsMappingBuilder<TModel>> columnMappingBuilderFunc)
        {
            columnMappingBuilderFunc(new ColumnsMappingBuilder<TModel>(_mappingConfig));
            return this;
        }
        
        public MappingConfig BuildConfig()
        {
            return _mappingConfig;
        }
    }
}