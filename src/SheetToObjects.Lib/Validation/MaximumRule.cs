using System;
using CSharpFunctionalExtensions;

namespace SheetToObjects.Lib.Validation
{
    internal class MaximumRule<TValue> : IComparableRule
        where TValue : IComparable<TValue>
    {
        private readonly TValue _maximumValue;

        public MaximumRule(TValue maximumValue)
        {
            _maximumValue = maximumValue;
        }

        public Result<object, IValidationError> Validate(int columnIndex, int rowIndex, string columnName, string propertyName, object value)
        {
            var castValue = (TValue)value;

            if (castValue.CompareTo(_maximumValue) > 0)
            {
                var validationError = RuleValidationError.CanHaveMaximumValueOf(columnIndex, rowIndex, columnName, propertyName, _maximumValue);
                return Result.Fail<object, IValidationError>(validationError);
            }

            return Result.Ok<object, IValidationError>(_maximumValue);
        }
    }
}
