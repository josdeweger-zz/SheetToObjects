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
            InitByAttributes();
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

        private void InitByAttributes()
        {
            var objType = typeof(TModel);

            foreach (var property in objType.GetProperties())
            {
                var columnIsMappedByAttribute = false;
                var mappingConfigBuilder = new ColumnMappingBuilder<TModel>();
                var attributes = property.GetCustomAttributes().ToList();

                if(attributes.OfType<IgnorePropertyMapping>().Any())
                    continue;
                
                foreach (var mappingAttribute in attributes.OfType<IMappingAttribute>())
                {
                    mappingAttribute.SetColumnMapping(mappingConfigBuilder);
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