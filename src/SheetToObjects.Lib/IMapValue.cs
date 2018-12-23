using System;
using CSharpFunctionalExtensions;
using SheetToObjects.Lib.FluentConfiguration;
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
            ColumnMapping columnMapping);
    }
}