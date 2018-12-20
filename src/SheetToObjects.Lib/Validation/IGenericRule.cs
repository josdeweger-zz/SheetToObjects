using CSharpFunctionalExtensions;

namespace SheetToObjects.Lib.Validation
{
    public interface IGenericRule : IRule
    {
        Result<TValue, IValidationError> Validate<TValue>(
            int columnIndex,
            int rowIndex,
            string columnName,
            string propertyName,
            TValue value);
    }
}