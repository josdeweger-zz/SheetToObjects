using System;
using CSharpFunctionalExtensions;
using SheetToObjects.Lib.Validation;

namespace SheetToObjects.Lib
{
    internal interface IMapValue
    {
        Result<object, IValidationError> Map(
            string value,
            Type propertyType,
            int columnIndex,
            int rowIndex,
            string displayName,
            string propertyName,
            string format,
            bool isRequired);
    }
}