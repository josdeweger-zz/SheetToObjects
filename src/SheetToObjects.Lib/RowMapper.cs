using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CSharpFunctionalExtensions;
using SheetToObjects.Core;
using SheetToObjects.Lib.FluentConfiguration;
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

        public Result<ParsedModel<T>, List<IValidationError>> Map<T>(Row row, MappingConfig mappingConfig)
            where T : new()
        {
            var rowValidationErrors = new List<IValidationError>();
            var obj = new T();
            var properties = obj.GetType().GetProperties().ToList();

            properties.ForEach(property =>
            {
                rowValidationErrors.AddRange(MapRow(row, mappingConfig, property, obj));
            });

            if (rowValidationErrors.Any())
                return Result.Fail<ParsedModel<T>, List<IValidationError>>(rowValidationErrors);
            
            return Result.Ok<ParsedModel<T>, List<IValidationError>>(new ParsedModel<T>(obj, row.RowIndex));
        }

        private IEnumerable<IValidationError> MapRow<TModel>(
            Row row, 
            MappingConfig mappingConfig, 
            PropertyInfo property, 
            TModel obj) where TModel : new()
        {
            var columnMapping = mappingConfig.GetColumnMappingByPropertyName(property.Name);

            if (columnMapping.IsNull())
                return new List<IValidationError>();

            var cell = row.GetCellByColumnIndex(columnMapping.ColumnIndex);

            if (cell == null)
            {
                return HandleEmptyCell(columnMapping, row.RowIndex, property.Name)
                    .OnValue(error => new List<IValidationError> { error })
                    .OnEmpty(() => new List<IValidationError>())
                    .Unwrap();
            }

            var validationErrors = new List<IValidationError>();

            _valueMapper
                .Map(cell.Value.ToString(), property.PropertyType, row.RowIndex, columnMapping)
                .OnSuccess(value =>
                {
                    if (value.ToString().IsNotNullOrEmpty())
                        property.SetValue(obj, value);
                })
                .OnFailure(validationError => { validationErrors.Add(validationError); });

            return validationErrors;
        }

        private static Maybe<IValidationError> HandleEmptyCell(ColumnMapping columnMapping, int rowIndex, string propertyName)
        {
            if (!columnMapping.IsRequired)
                return Maybe<IValidationError>.None;

            var parsingValidationError = ParsingValidationError.CellNotFound(
                columnMapping.ColumnIndex,
                rowIndex,
                columnMapping.DisplayName,
                propertyName);

            return Maybe<IValidationError>.From(parsingValidationError);
        }
    }
}