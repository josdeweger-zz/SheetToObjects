using System;
using System.Linq;
using System.Reflection;
using SheetToObjects.Core;
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

        private void InitByAtributes()
        {
            var objType = typeof(TModel);

            var sheetToConfigAttribute = objType.GetCustomAttributes().OfType<SheetToObjectConfig>().FirstOrDefault();
            if (sheetToConfigAttribute.IsNotNull())
            {
                _mappingConfig.HasHeaders = sheetToConfigAttribute.SheetHasHeaders;
                _mappingConfig.AutoMapProperties = sheetToConfigAttribute.AutoMapProperties;
            }
        }
    }
}