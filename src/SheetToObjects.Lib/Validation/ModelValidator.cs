﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

            validationErrors.AddRange(ValidatePropertyRules(parsedModels, columnMappings, properties, validatedModels));

            validationErrors.AddRange(ValidateColumnRules(parsedModels, columnMappings, properties));

            return new ValidationResult<ParsedModelResult<TModel>>(validatedModels, validationErrors);
        }

        private static List<IValidationError> ValidatePropertyRules<TModel>(
            List<ParsedModelResult<TModel>> parsedModels, 
            List<ColumnMapping> columnMappings, 
            PropertyInfo[] properties,
            List<ParsedModelResult<TModel>> validatedModels) where TModel : new()
        {
            var validationErrors = new List<IValidationError>();

            foreach (var parsedModelResult in parsedModels)
            {
                var modelValidationErrors = new List<IValidationError>();

                foreach (var property in properties)
                {
                    var columnMapping = columnMappings.FirstOrDefault(c => c.PropertyName.Equals(property.Name));
                    if (columnMapping.IsNull())
                        continue;

                    modelValidationErrors.AddRange(ValidateRules(property, parsedModelResult, columnMapping));
                }

                if (modelValidationErrors.Any())
                    validationErrors.AddRange(modelValidationErrors);
                else
                    validatedModels.Add(parsedModelResult);
            }

            return validationErrors;
        }

        private static List<IValidationError> ValidateColumnRules<TModel>(
            List<ParsedModelResult<TModel>> parsedModels,
            List<ColumnMapping> columnMappings,
            PropertyInfo[] properties) where TModel : new()
        {
            var validationErrors = new List<IValidationError>();

            foreach (var property in properties)
            {
                var columnMapping = columnMappings.FirstOrDefault(c => c.PropertyName.Equals(property.Name));
                if (columnMapping.IsNull())
                    continue;

                var propertyInfo = typeof(TModel).GetProperty(property.Name);

                GetRulesOfType<IColumnRule>(columnMapping)
                    .ForEach(columnRule => columnRule
                        .Validate(
                            columnMapping.ColumnIndex,
                            columnMapping.DisplayName,
                            parsedModels.Select(p => propertyInfo.GetValue(p.ParsedModel)).ToList()
                        )
                        .OnFailure(validationErrors.Add));
            }

            return validationErrors;
        }

        private static List<IValidationError> ValidateRules<TModel>(
            PropertyInfo property, 
            ParsedModelResult<TModel> parsedModelResult, 
            ColumnMapping columnMapping) 
            where TModel : new()
        {
            var modelValidationErrors = new List<IValidationError>();

            var propertyValue = property.GetValue(parsedModelResult.ParsedModel);

            GetRulesOfType<IGenericRule>(columnMapping)
                .ForEach(genericRule => genericRule
                    .Validate(columnMapping.ColumnIndex, parsedModelResult.RowIndex, columnMapping.DisplayName, property.Name,
                        propertyValue)
                    .OnFailure(modelValidationErrors.Add));

            GetRulesOfType<IComparableRule>(columnMapping)
                .ForEach(comparableRule => comparableRule
                    .Validate(columnMapping.ColumnIndex, parsedModelResult.RowIndex, columnMapping.DisplayName, property.Name,
                        propertyValue)
                    .OnFailure(modelValidationErrors.Add));

            GetRulesOfType<ICustomRule>(columnMapping)
                .ForEach(customRule => customRule
                    .Validate(columnMapping.ColumnIndex, parsedModelResult.RowIndex, columnMapping.DisplayName, property.Name,
                        propertyValue)
                    .OnFailure(modelValidationErrors.Add));

            return modelValidationErrors;
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