using System;
using System.Linq;
using System.Reflection;
using CSharpFunctionalExtensions;
using SheetToObjects.Core;
using SheetToObjects.Lib.Attributes;
using SheetToObjects.Lib.Attributes.MappingType;
using SheetToObjects.Lib.Attributes.Rules;
using SheetToObjects.Lib.Configuration;
using SheetToObjects.Lib.Configuration.ColumnMappings;

namespace SheetToObjects.Lib
{
    internal class MappingConfigByAttributeCreator<TModel>
    {
        public Result<MappingConfig> CreateMappingConfigByAttributes()
        {
            var type = typeof(TModel);

            var sheetToConfigAttribute = type.GetCustomAttributes().OfType<SheetToObjectConfig>().FirstOrDefault();
            if (sheetToConfigAttribute.IsNotNull())
            {
                return Result.Ok(InitByAttributes(type, sheetToConfigAttribute));
            }

            return Result.Fail<MappingConfig>($"No SheetToObjectConfig attribute found on model of type {type}");
        }

        private MappingConfig InitByAttributes(Type type, SheetToObjectConfig sheetToConfigAttribute)
        {
            var mappingConfig = new MappingConfig
            {
                HasHeaders = sheetToConfigAttribute.SheetHasHeaders,
                AutoMapProperties = sheetToConfigAttribute.AutoMapProperties
            };

            foreach (var property in type.GetProperties())
            {
                var columnIsMappedByAttribute = false;
                var mappingConfigBuilder = new ColumnMappingBuilder<TModel>();
                var attributes = property.GetCustomAttributes().ToList();

                if (attributes.OfType<IgnorePropertyMapping>().Any())
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

                    if (attribute is Format formatAttribute)
                    {
                        mappingConfigBuilder.UsingFormat(formatAttribute.FormatString);
                    }
                }

                if (columnIsMappedByAttribute || mappingConfig.AutoMapProperties)
                {
                    mappingConfig.ColumnMappings.Add(mappingConfigBuilder.MapTo(property));
                }
            }

            return mappingConfig;
        }
    }
}