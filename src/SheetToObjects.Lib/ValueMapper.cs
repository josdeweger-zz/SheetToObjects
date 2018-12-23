using System;
using CSharpFunctionalExtensions;
using SheetToObjects.Core;
using SheetToObjects.Lib.FluentConfiguration;
using SheetToObjects.Lib.Parsing;
using SheetToObjects.Lib.Validation;

namespace SheetToObjects.Lib
{
    internal class ValueMapper : IMapValue
    {
        private readonly IProvideParsingStrategy _valueParser;

        public ValueMapper(IProvideParsingStrategy valueParser)
        {
            _valueParser = valueParser;
        }

        public Result<object, IValidationError> Map(
            string value, 
            Type propertyType, 
            int columnIndex, 
            int rowIndex, 
            ColumnMapping columnMapping)
        {
            if (string.IsNullOrEmpty(value))
            {
                return HandleEmptyValue(columnIndex, rowIndex, columnMapping);
            }

            if (columnMapping.CustomParser.IsNotNull())
            {
                try
                {
                    var parsedValue = columnMapping.CustomParser(value);
                    return Result.Ok<object, IValidationError>(parsedValue);
                }
                catch (Exception)
                {
                    var parsingValidationError = ParsingValidationError.CouldNotParseValue(
                        columnIndex,
                        rowIndex,
                        columnMapping.DisplayName,
                        columnMapping.PropertyName);

                    return Result.Fail<object, IValidationError>(parsingValidationError);
                }
            }

            var parsingResult = _valueParser.Parse(propertyType, value, columnMapping.Format);

            if (!parsingResult.IsSuccess)
            {
                var validationError = ParsingValidationError.CouldNotParseValue(
                    columnIndex,
                    rowIndex,
                    columnMapping.DisplayName,
                    columnMapping.PropertyName);

                return Result.Fail<object, IValidationError>(validationError);
            }

            return Result.Ok<object, IValidationError>(parsingResult.Value);
        }

        private static Result<object, IValidationError> HandleEmptyValue(int columnIndex, int rowIndex, ColumnMapping columnMapping)
        {
            if (columnMapping.IsRequired)
            {
                var cellValueRequiredError = RuleValidationError.CellValueRequired(
                    columnIndex,
                    rowIndex,
                    columnMapping.DisplayName,
                    columnMapping.PropertyName);

                return Result.Fail<object, IValidationError>(cellValueRequiredError);
            }

            return Result.Ok<object, IValidationError>(columnMapping.DefaultValue ?? string.Empty);
        }
    }
}