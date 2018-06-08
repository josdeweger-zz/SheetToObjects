using System;
using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace SheetToObjects.Lib.Validation
{
    internal class RegexRule : IGenericRule
    { 
        private readonly string _pattern;
        private readonly bool _isCaseSensitive;

        public RegexRule(string pattern, bool isCaseSensitive = false)
        {
            _pattern = pattern;
            _isCaseSensitive = isCaseSensitive;
        }

        public Result<TValue, IValidationError> Validate<TValue>(int columnIndex, int rowIndex, string columnName, string propertyName, TValue value)
        {
            try
            {
                var stringValue = value.ToString();

                if (Regex.IsMatch(stringValue, _pattern, _isCaseSensitive ? RegexOptions.IgnoreCase : RegexOptions.None))
                {
                    return Result.Ok<TValue, IValidationError>(value);
                }

                var validationError = RuleValidationError.ValueDoesNotMatchRegex(columnIndex, rowIndex, columnName,
                    propertyName, stringValue, _pattern);

                return Result.Fail<TValue, IValidationError>(validationError);
            }
            catch (Exception)
            {
                var validationError = RuleValidationError.CouldNotValidateValueWithPattern(columnIndex, rowIndex, columnName,
                    propertyName, _pattern);

                return Result.Fail<TValue, IValidationError>(validationError);
            }
        }
    }
}