using System;
using System.Linq;
using System.Reflection;
using SheetToObjects.Lib.Attributes;
using SheetToObjects.Lib.Attributes.MappingType;
using SheetToObjects.Lib.Attributes.Rules;
using SheetToObjects.Lib.Configuration.ColumnMappings;

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
            Type objType = typeof(TModel);

            SheetToObjectConfig sheetToConfigAttribute = objType.GetCustomAttributes().OfType<SheetToObjectConfig>().FirstOrDefault();
            if (sheetToConfigAttribute != null)
            {
                _mappingConfig.HasHeaders = sheetToConfigAttribute.SheetHasHeaders;
                _mappingConfig.AutoMapProperties = sheetToConfigAttribute.AutoMapProperties;
            }

            new ColumnsMappingBuilder<TModel>(_mappingConfig);
        }
    }

    public class ColumnsMappingBuilder<TModel>
    {
        private readonly MappingConfig _mappingConfig;

        public ColumnsMappingBuilder(MappingConfig mappingConfig)
        {
            _mappingConfig = mappingConfig;
            InitByAttributes();
        }

        public ColumnsMappingBuilder<TModel> Add(Func<ColumnMappingBuilder<TModel>, ColumnMapping> columnMappingBuilderFunc)
        {
            var columnMapping = columnMappingBuilderFunc(new ColumnMappingBuilder<TModel>());

            _mappingConfig.ColumnMappings.RemoveAll(c => c.PropertyName == columnMapping.PropertyName);
            _mappingConfig.ColumnMappings.Add(columnMapping);

            return this;
        }

        private void InitByAttributes()
        {
            Type objType = typeof(TModel);

            foreach (var property in objType.GetProperties())
            {
                bool columnIsMappedByAttribute = false;
                var mappingConfigBuilder = new ColumnMappingBuilder<TModel>();
                var attributes = property.GetCustomAttributes().ToList();

                if(attributes.OfType<IgnorePropertyMapping>().Any())
                    continue;
                
                foreach (var mappingAttribute in attributes.OfType<IMappingAttribute>())
                {
                    mappingAttribute.SetColumnMapping<TModel>(mappingConfigBuilder);
                    columnIsMappedByAttribute = true;
                }
                
                foreach (var attribute in attributes)
                {
                    if (attribute is IRuleAttribute ruleAttribute)
                    {
                        mappingConfigBuilder.AddRule(ruleAttribute.GetRule());
                    }
                }

                if (columnIsMappedByAttribute || _mappingConfig.AutoMapProperties)
                {
                    _mappingConfig.ColumnMappings.Add(mappingConfigBuilder.MapTo(property));
                }
            }

        }
    }
}