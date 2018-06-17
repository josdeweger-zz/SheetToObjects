using System;
using CSharpFunctionalExtensions;
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

        public Result<object, IValidationError> Map(
            string value, 
            Type propertyType, 
            int columnIndex, 
            int rowIndex, 
            string displayName, 
            string propertyName, 
            string format,
            bool isRequired,
            object defaultValue)
        {
            if (string.IsNullOrEmpty(value))
            {
                return HandleEmptyValue(isRequired, columnIndex, rowIndex, displayName, propertyName, defaultValue);
            }

            var parsingResult = _valueParser.Parse(propertyType, value, format);

            if (!parsingResult.IsSuccess)
            {
                var validationError = ParsingValidationError.CouldNotParseValue(
                    columnIndex,
                    rowIndex,
                    displayName,
                    propertyName);

                return Result.Fail<object, IValidationError>(validationError);
            }

            return Result.Ok<object, IValidationError>(parsingResult.Value);
        }

        private static Result<object, IValidationError> HandleEmptyValue(bool isRequired, int columnIndex, int rowIndex, string displayName, string propertyName, object defaultValue)
        {
            if (isRequired)
            {
                var cellValueRequiredError = RuleValidationError.CellValueRequired(
                    columnIndex,
                    rowIndex,
                    displayName,
                    propertyName);

                return Result.Fail<object, IValidationError>(cellValueRequiredError);
            }

            return Result.Ok<object, IValidationError>(defaultValue ?? string.Empty);
        }
    }
}