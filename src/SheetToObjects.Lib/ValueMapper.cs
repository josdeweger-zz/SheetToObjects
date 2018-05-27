using System;
using CSharpFunctionalExtensions;
using SheetToObjects.Core;
using SheetToObjects.Lib.Configuration.ColumnMappings;

namespace SheetToObjects.Lib
{
    internal class ValueMapper : IMapValue
    {
        private readonly IParseValue _valueParser;

        public ValueMapper(IParseValue valueParser)
        {
            _valueParser = valueParser;
        }

        public Result<object, ValidationError> Map(string value, Type propertyType, ColumnMapping columnMapping, int rowIndex)
        {
            if (string.IsNullOrEmpty(value))
            {
                if (columnMapping.IsRequired)
                {
                    var cellValueRequiredError = ValidationError.CellValueRequiredError(
                        columnMapping.ColumnIndex,
                        rowIndex,
                        columnMapping.DisplayName,
                        columnMapping.PropertyName);

                    return Result.Fail<object, ValidationError>(cellValueRequiredError);
                }

                return Result.Ok<object, ValidationError>(propertyType.GetDefault());
            }

            var parsingResult = _valueParser.Parse(propertyType, value, columnMapping.Format);

            if (!parsingResult.IsSuccess)
            {
                var validationError = ValidationError.ParseValueError(
                    columnMapping.ColumnIndex,
                    rowIndex,
                    parsingResult.Error,
                    columnMapping.DisplayName,
                    value,
                    columnMapping.PropertyName);

                return Result.Fail<object, ValidationError>(validationError);
            }

            return Result.Ok<object, ValidationError>(parsingResult.Value);
        }
    }
}