using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace SheetToObjects.Lib.Validation
{
    public interface IColumnRule : IRule
    {
        Result<List<object>, IValidationError> Validate(int columnIndex, string columnName, List<object> allValuesInColumn);
    }
}