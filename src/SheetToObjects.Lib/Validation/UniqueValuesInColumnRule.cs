using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using SheetToObjects.Core;

namespace SheetToObjects.Lib.Validation
{
    internal class UniqueValuesInColumnRule : IColumnRule
    {
        public Result<List<object>, IValidationError> Validate(int columnIndex, string columnName, List<object> allValuesInColumn)
        {
            if (allValuesInColumn.IsNotNull())
            {
                if (allValuesInColumn.GroupBy(x => x).Any(g => g.Count() > 1))
                {
                    var validationError = RuleValidationError.ColumnMustContainUniqueValues(columnIndex, columnName);
                    return Result.Fail<List<object>, IValidationError>(validationError);
                }
            }

            return Result.Ok<List<object>, IValidationError>(allValuesInColumn);
        }
    }
}
