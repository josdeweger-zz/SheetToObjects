using System;

namespace SheetToObjects.Lib.Configuration
{
    public class MappingConfigBuilder<TModel>
    {
        private readonly MappingConfig _mappingConfig = new MappingConfig();
        
        public MappingConfigBuilder<TModel> HasHeaders()
        {
            _mappingConfig.DataHasHeaders = true;
            return this;
        }

        public MappingConfig Columns(
            Func<ColumnsMappingBuilder<TModel>, ColumnsMappingBuilder<TModel>> columnMappingBuilderFunc)
        {
            var columnsMappingBuilder = new ColumnsMappingBuilder<TModel>(_mappingConfig);
            columnMappingBuilderFunc(columnsMappingBuilder);
            return _mappingConfig;
        }
    }

    public class ColumnsMappingBuilder<TModel>
    {
        private readonly MappingConfig _mappingConfig;

        public ColumnsMappingBuilder(MappingConfig mappingConfig)
        {
            _mappingConfig = mappingConfig;
        }

        public ColumnsMappingBuilder<TModel> Add(Func<ColumnMappingBuilder<TModel>, ColumnMapping> columnMappingBuilderFunc)
        {
            var columnMappingBuilder = new ColumnMappingBuilder<TModel>();
            var columnMapping = columnMappingBuilderFunc(columnMappingBuilder);
            _mappingConfig.ColumnMappings.Add(columnMapping);
            return this;
        }
    }
}