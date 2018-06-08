using System;
using CSharpFunctionalExtensions;

namespace SheetToObjects.Lib.Validation
{
    internal class MinimumRule<TValue> : IComparableRule
        where TValue : IComparable<TValue>
    {
        private readonly TValue _minimumValue;

        public MinimumRule(TValue minimumValue)
        {
            _minimumValue = minimumValue;
        }

        public Result<object, IValidationError> Validate(int columnIndex, int rowIndex, string columnName, string propertyName, object value)
        {
            var castValue = (TValue)value;

            if (castValue.CompareTo(_minimumValue) <= 0)
            {
                var validationError = RuleValidationError.HasToHaveMinimumValueOf(columnIndex, rowIndex, columnName, propertyName, _minimumValue);
                return Result.Fail<object, IValidationError>(validationError);
            }

            return Result.Ok<object, IValidationError>(_minimumValue);
        }
    }
}
