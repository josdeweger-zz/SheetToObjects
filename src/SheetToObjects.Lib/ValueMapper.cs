using System;
using CSharpFunctionalExtensions;
using SheetToObjects.Core;
using SheetToObjects.Lib.Configuration.ColumnMappings;
using SheetToObjects.Lib.Validation;

namespace SheetToObjects.Lib
{
    internal class ValueMapper : IMapValue
    {
        private readonly IParseValue _valueParser;

        public ValueMapper(IParseValue valueParser)
        {
            _valueParser = valueParser;
        }

        public Result<object, IValidationError> Map(string value, Type propertyType, ColumnMapping columnMapping, int rowIndex)
        {
            if (string.IsNullOrEmpty(value))
            {
                return HandleEmptyValue(propertyType, columnMapping, rowIndex);
            }

            var parsingResult = _valueParser.Parse(propertyType, value, columnMapping.Format);

            if (!parsingResult.IsSuccess)
            {
                var validationError = ParsingValidationError.CouldNotParseValue(
                    columnMapping.ColumnIndex,
                    rowIndex,
                    columnMapping.DisplayName,
                    columnMapping.PropertyName);

                return Result.Fail<object, IValidationError>(validationError);
            }

            return Result.Ok<object, IValidationError>(parsingResult.Value);
        }

        private static Result<object, IValidationError> HandleEmptyValue(Type propertyType, ColumnMapping columnMapping, int rowIndex)
        {
            if (columnMapping.IsRequired)
            {
                var cellValueRequiredError = RuleValidationError.CellValueRequired(
                    columnMapping.ColumnIndex,
                    rowIndex,
                    columnMapping.DisplayName,
                    columnMapping.PropertyName);

                return Result.Fail<object, IValidationError>(cellValueRequiredError);
            }

            return Result.Ok<object, IValidationError>(propertyType.GetDefault() ?? string.Empty);
        }
    }
}