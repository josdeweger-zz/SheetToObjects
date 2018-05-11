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
            InitByAttributes();
            var columnsBuilder = new ColumnsMappingBuilder<TModel>(_mappingConfig);
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

        public MappingConfig Object()
        {
            return _mappingConfig;
        }

        private void InitByAttributes()
        {
            Type objType = typeof(TModel);

            SheetToObjectConfig sheetToConfigAttribute = objType.GetCustomAttributes().OfType<SheetToObjectConfig>().FirstOrDefault();
            if (sheetToConfigAttribute != null)
            {
                _mappingConfig.HasHeaders = sheetToConfigAttribute.SheetHasHeaders;
            }
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
                var mappingConfigBuilder = new ColumnMappingBuilder<TModel>();

                var attributes = property.GetCustomAttributes().ToList();
                
                foreach (var mappingAttribute in attributes.OfType<IMappingAttribute>())
                {
                    mappingAttribute.SetColumnMapping<TModel>(mappingConfigBuilder);
                }
                
                foreach (var attribute in attributes)
                {
                    if (attribute is IRuleAttribute ruleAttribute)
                    {
                        mappingConfigBuilder.AddRule(ruleAttribute.GetRule());
                    }
                }

                _mappingConfig.ColumnMappings.Add(mappingConfigBuilder.MapTo(property));
            }

        }
    }
}