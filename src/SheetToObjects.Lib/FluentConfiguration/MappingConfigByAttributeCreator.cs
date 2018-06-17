using System;
using System.Linq;
using System.Reflection;
using CSharpFunctionalExtensions;
using SheetToObjects.Core;
using SheetToObjects.Lib.AttributesConfiguration;
using SheetToObjects.Lib.AttributesConfiguration.MappingTypeAttributes;
using SheetToObjects.Lib.AttributesConfiguration.RuleAttributes;

namespace SheetToObjects.Lib.FluentConfiguration
{
    internal class MappingConfigByAttributeCreator<T>
    {
        public Result<MappingConfig> CreateMappingConfig()
        {
            var type = typeof(T);

            var sheetToConfigAttribute = type.GetCustomAttributes().OfType<SheetToObjectConfig>().FirstOrDefault();
            if (sheetToConfigAttribute.IsNotNull())
            {
                return Result.Ok(CreateMappingConfigForType(type, sheetToConfigAttribute));
            }

            return Result.Fail<MappingConfig>($"No SheetToObjectConfig attribute found on model of type {type}");
        }

        private MappingConfig CreateMappingConfigForType(Type type, SheetToObjectConfig sheetToConfigAttribute)
        {
            var mappingConfig = new MappingConfig
            {
                HasHeaders = sheetToConfigAttribute.SheetHasHeaders,
                AutoMapProperties = sheetToConfigAttribute.AutoMapProperties
            };

            foreach (var property in type.GetProperties())
            {
                var columnIsMappedByAttribute = false;
                var mappingConfigBuilder = new ColumnMappingBuilder<T>();
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
                    switch (true)
                    {
                        case var _ when attribute is IParsingRuleAttribute:
                            mappingConfigBuilder.AddParsingRule(((IParsingRuleAttribute) attribute).GetRule());
                            break;
                        case var _ when attribute is Format:
                            mappingConfigBuilder.UsingFormat(((Format) attribute).FormatString);
                            break;
                        case var _ when attribute is DefaultValue:
                            mappingConfigBuilder.WithDefaultValue(((DefaultValue) attribute).Value);
                            break;
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