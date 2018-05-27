using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using SheetToObjects.Core;
using SheetToObjects.Lib.Configuration;
using SheetToObjects.Lib.Configuration.ColumnMappings;
using SheetToObjects.Lib.Validation;

namespace SheetToObjects.Lib
{
    internal class RowMapper : IMapRow
    {
        private readonly IMapValue _valueMapper;

        public RowMapper(IMapValue valueMapper)
        {
            _valueMapper = valueMapper;
        }

        public Result<T, List<IValidationError>> Map<T>(Row row, MappingConfig mappingConfig)
            where T : new()
        {
            var rowIValidationErrors = new List<IValidationError>();
            var obj = new T();
            var properties = obj.GetType().GetProperties().ToList();

            properties.ForEach(property =>
            {
                var columnMapping = mappingConfig.GetColumnMappingByPropertyName(property.Name);

                if (columnMapping.IsNull())
                    return;

                var cell = row.GetCellByColumnIndex(columnMapping.ColumnIndex);

                if (cell == null)
                {
                    var parsingValidationError = ParsingValidationError.CellNotFound(columnMapping.ColumnIndex, row.RowIndex,
                        columnMapping.DisplayName, property.Name);

                    if (columnMapping.IsRequired)
                        rowIValidationErrors.Add(parsingValidationError);

                    return;
                }

                rowIValidationErrors
                    .AddRange(ValidateValueByColumnMapping(cell.Value.ToString(), columnMapping, row.RowIndex, property.Name)
                        .ToList());

                _valueMapper.Map(cell.Value.ToString(), property.PropertyType, columnMapping, row.RowIndex)
                    .OnSuccess(value => property.SetValue(obj, value))
                    .OnFailure(validationError => { rowIValidationErrors.Add(validationError); });
            });

            if (rowIValidationErrors.Any())
                return Result.Fail<T, List<IValidationError>>(rowIValidationErrors);

            return Result.Ok<T, List<IValidationError>>(obj);
        }

        private IEnumerable<IValidationError> ValidateValueByColumnMapping(string value, ColumnMapping mapping, int rowIndex, string propertyName)
        {
            return mapping.Rules
                .Select(rule => rule.Validate(value))
                .Where(validationResult => validationResult.IsFailure)
                .Select(validationResult =>
                    RuleValidationError.DoesNotMatchRule(
                        mapping.ColumnIndex,
                        rowIndex,
                        validationResult.Error,
                        mapping.DisplayName,
                        value,
                        propertyName));
        }
    }
}