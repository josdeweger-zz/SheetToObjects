using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using SheetToObjects.Core;
using SheetToObjects.Lib.FluentConfiguration;

namespace SheetToObjects.Lib.Validation
{
    internal class ModelValidator : IValidateModels
    {
        public ValidationResult<ParsedModelResult<TModel>> Validate<TModel>(
            List<ParsedModelResult<TModel>> parsedModels, 
            List<ColumnMapping> columnMappings)
            where TModel : new()
        {
            var validatedModels = new List<ParsedModelResult<TModel>>();
            var validationErrors = new List<IValidationError>();
            var properties = typeof(TModel).GetProperties();

            foreach (var parsedModelResult in parsedModels)
            {
                var modelValidationErrors = new List<IValidationError>();

                foreach (var property in properties)
                {
                    var columnMapping = columnMappings.FirstOrDefault(c => c.PropertyName.Equals(property.Name));
                    if (columnMapping.IsNull())
                        continue;

                    var genericRules = GetRulesOfType<IGenericRule>(columnMapping);
                    var comparableRules = GetRulesOfType<IComparableRule>(columnMapping);
                    var customRules = GetRulesOfType<ICustomRule>(columnMapping);

                    var propertyValue = property.GetValue(parsedModelResult.ParsedModel);

                    foreach (var genericRule in genericRules)
                    {
                        genericRule.Validate(columnMapping.ColumnIndex, parsedModelResult.RowIndex, columnMapping.DisplayName, property.Name, propertyValue)
                            .OnFailure(failure =>
                            {
                                modelValidationErrors.Add(failure);
                            });
                    }

                    foreach (var comparableRule in comparableRules)
                    {
                        comparableRule.Validate(columnMapping.ColumnIndex, parsedModelResult.RowIndex, columnMapping.DisplayName, property.Name, propertyValue)
                            .OnFailure(failure =>
                            {
                                modelValidationErrors.Add(failure);
                            });
                    }

                    foreach (var customRule in customRules)
                    {
                        customRule.Validate(columnMapping.ColumnIndex, parsedModelResult.RowIndex, columnMapping.DisplayName, property.Name, propertyValue)
                            .OnFailure(failure =>
                            {
                                modelValidationErrors.Add(failure);
                            });
                    }
                }

                if (modelValidationErrors.Any())
                    validationErrors.AddRange(modelValidationErrors);
                else
                    validatedModels.Add(parsedModelResult);
            }

            return new ValidationResult<ParsedModelResult<TModel>>(validatedModels, validationErrors);
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