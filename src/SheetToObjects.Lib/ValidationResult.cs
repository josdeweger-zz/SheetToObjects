using System.Collections.Generic;
using SheetToObjects.Lib.Validation;

namespace SheetToObjects.Lib
{
    internal class ValidationResult<T>
    {
        public List<T> ValidatedModels { get; }
        public List<IValidationError> ValidationErrors { get; }

        public ValidationResult(List<T> validatedModels, List<IValidationError> validationErrors)
        {
            ValidatedModels = validatedModels;
            ValidationErrors = validationErrors;
        }
    }
}