using System;
using CSharpFunctionalExtensions;
using SheetToObjects.Core;
using SheetToObjects.Lib.Extensions;
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

        public Result<object, IValidationError> Map(string value, Type propertyType, int rowIndex, ColumnMapping columnMapping)
        {
            if (string.IsNullOrEmpty(value))
            {
                return HandleEmptyValue(columnMapping.ColumnIndex, rowIndex, columnMapping);
            }

            if (columnMapping.CustomValueParser.IsNotNull())
            {
                try
                {
                    var parsedValue = columnMapping.CustomValueParser(value);
                    return Result.Ok<object, IValidationError>(parsedValue);
                }
                catch (Exception)
                {
                    var parsingValidationError = ParsingValidationError.CouldNotParseValue(
                        columnMapping.ColumnIndex,
                        rowIndex,
                        columnMapping.DisplayName,
                        columnMapping.PropertyName);

                    return Result.Fail<object, IValidationError>(parsingValidationError);
                }
            }

            return _valueParser
                .Parse(propertyType, value, columnMapping.Format)
                .OnValidationSuccess(parsedValue => parsedValue)
                .OnValidationFailure(error => ParsingValidationError.CouldNotParseValue(
                    columnMapping.ColumnIndex,
                    rowIndex,
                    columnMapping.DisplayName,
                    columnMapping.PropertyName));
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