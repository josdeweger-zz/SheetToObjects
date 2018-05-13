using System;
using System.Linq;
using System.Reflection;
using SheetToObjects.Lib.Attributes;

namespace SheetToObjects.Lib.Configuration
{
    public class MappingConfigBuilder<TModel>
    {
        private readonly MappingConfig _mappingConfig = new MappingConfig();

        public MappingConfigBuilder()
        {
            InitByAtributes();
        }


        public MappingConfigBuilder<TModel> HasHeaders()
        {
            _mappingConfig.HasHeaders = true;
            return this;
        }
        
        public MappingConfigBuilder<TModel> Columns(Func<ColumnsMappingBuilder<TModel>, ColumnsMappingBuilder<TModel>> columnMappingBuilderFunc)
        {
            columnMappingBuilderFunc(new ColumnsMappingBuilder<TModel>(_mappingConfig));
            return this;
        }
        
        public MappingConfig BuildConfig()
        {
            return _mappingConfig;
        }

        private void InitByAtributes()
        {
            var objType = typeof(TModel);

            var sheetToConfigAttribute = objType.GetCustomAttributes().OfType<SheetToObjectConfig>().FirstOrDefault();
            if (sheetToConfigAttribute != null)
            {
                _mappingConfig.HasHeaders = sheetToConfigAttribute.SheetHasHeaders;
                _mappingConfig.AutoMapProperties = sheetToConfigAttribute.AutoMapProperties;
            }

            new ColumnsMappingBuilder<TModel>(_mappingConfig);
        }
    }
}