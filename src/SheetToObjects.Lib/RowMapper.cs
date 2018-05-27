using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using SheetToObjects.Core;
using SheetToObjects.Lib.Configuration;
using SheetToObjects.Lib.Configuration.ColumnMappings;

namespace SheetToObjects.Lib
{
    internal class RowMapper : IMapRow
    {
        private readonly IMapValue _valueMapper;

        public RowMapper(IMapValue valueMapper)
        {
            _valueMapper = valueMapper;
        }

        public Result<T, List<ValidationError>> Map<T>(Row row, MappingConfig mappingConfig)
            where T : new()
        {
            var rowValidationErrors = new List<ValidationError>();
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
                    var validationError = ValidationError.CellNotFoundError(columnMapping.ColumnIndex, row.RowIndex,
                        columnMapping.DisplayName, property.Name);

                    if (columnMapping.IsRequired)
                        rowValidationErrors.Add(validationError);

                    return;
                }

                rowValidationErrors
                    .AddRange(ValidateValueByColumnMapping(cell.Value.ToString(), columnMapping, row.RowIndex, property.Name)
                        .ToList());

                _valueMapper.Map(cell.Value.ToString(), property.PropertyType, columnMapping, row.RowIndex)
                    .OnSuccess(value => property.SetValue(obj, value))
                    .OnFailure(validationError => { rowValidationErrors.Add(validationError); });
            });

            if (rowValidationErrors.Any())
                return Result.Fail<T, List<ValidationError>>(rowValidationErrors);

            return Result.Ok<T, List<ValidationError>>(obj);
        }

        private IEnumerable<ValidationError> ValidateValueByColumnMapping(string value, ColumnMapping mapping, int rowIndex, string propertyName)
        {
            return mapping.Rules
                .Select(rule => rule.Validate(value))
                .Where(validationResult => validationResult.IsFailure)
                .Select(validationResult =>
                    ValidationError.DoesNotMatchRuleError(
                        mapping.ColumnIndex,
                        rowIndex,
                        validationResult.Error,
                        mapping.DisplayName,
                        value,
                        propertyName));
        }
    }
}