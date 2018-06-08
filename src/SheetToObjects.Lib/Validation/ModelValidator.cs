using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using SheetToObjects.Core;
using SheetToObjects.Lib.FluentConfiguration;

namespace SheetToObjects.Lib.Validation
{
    internal class ModelValidator : IValidateModels
    {
        public ValidationResult<TModel> Validate<TModel>(List<TModel> parsedModels, List<ColumnMapping> columnMappings)
            where TModel : new()
        {
            var validatedModels = new List<TModel>();
            var validationErrors = new List<IValidationError>();
            var properties = typeof(TModel).GetProperties();

            foreach (var parsedModel in parsedModels)
            {
                var parsedModelValidationErrors = new List<IValidationError>();

                foreach (var property in properties)
                {
                    var columnMapping = columnMappings.FirstOrDefault(c => c.PropertyName.Equals(property.Name));
                    if (columnMapping.IsNull())
                        continue;

                    var genericRules = GetRulesOfType<IGenericRule>(columnMapping);
                    var comparableRules = GetRulesOfType<IComparableRule>(columnMapping);

                    var propertyValue = property.GetValue(parsedModel);

                    foreach (var genericRule in genericRules)
                    {
                        genericRule.Validate(0, 0, columnMapping.DisplayName, property.Name, propertyValue)
                            .OnFailure(failure =>
                            {
                                parsedModelValidationErrors.Add(failure);
                            });
                    }

                    foreach (var comparableRule in comparableRules)
                    {
                        comparableRule.Validate(0, 0, columnMapping.DisplayName, property.Name, propertyValue)
                            .OnFailure(failure =>
                            {
                                parsedModelValidationErrors.Add(failure);
                            });
                    }
                }

                if (parsedModelValidationErrors.Any())
                    validationErrors.AddRange(parsedModelValidationErrors);
                else
                    validatedModels.Add(parsedModel);
            }

            return new ValidationResult<TModel>(validatedModels, validationErrors);
        }

        private static List<TRule> GetRulesOfType<TRule>(ColumnMapping columnMapping)
            where TRule : class
        {
            return columnMapping.Rules
                .Where(r => r.GetType().GetInterfaces().Contains(typeof(TRule)))
                .Select(r => r as TRule)
                .ToList();
        }
    }
}