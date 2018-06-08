using System;
using CSharpFunctionalExtensions;

namespace SheetToObjects.Lib.Validation
{
    public interface IComparableRule : IRule
    {
        Result<object, IValidationError> Validate(
            int columnIndex,
            int rowIndex,
            string columnName,
            string propertyName,
            object value);
    }
}