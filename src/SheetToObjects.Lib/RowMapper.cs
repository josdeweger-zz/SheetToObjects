using System.Collections.Generic;
using System.Linq;
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

        public Result<ParsedModelResult<TModel>, List<IValidationError>> Map<TModel>(Row row, MappingConfig mappingConfig)
            where TModel : new()
        {
            var rowIValidationErrors = new List<IValidationError>();
            var obj = new TModel();
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

                _valueMapper.Map(
                        cell.Value.ToString(), 
                        property.PropertyType, 
                        columnMapping.ColumnIndex, 
                        row.RowIndex,
                        columnMapping.DisplayName, 
                        columnMapping.PropertyName, 
                        columnMapping.Format,
                        columnMapping.IsRequired)
                    .OnSuccess(value =>
                    {
                        if (value.ToString().IsNotNullOrEmpty())
                        {
                            property.SetValue(obj, value);
                        }
                    })
                    .OnFailure(validationError => { rowIValidationErrors.Add(validationError); });
            });

            if (rowIValidationErrors.Any())
                return Result.Fail<ParsedModelResult<TModel>, List<IValidationError>>(rowIValidationErrors);

            var parsedModelResult = new ParsedModelResult<TModel>(obj, row.RowIndex);

            return Result.Ok<ParsedModelResult<TModel>, List<IValidationError>>(parsedModelResult);
        }
    }
}