using System;
using CSharpFunctionalExtensions;
using SheetToObjects.Lib.Configuration.ColumnMappings;
using SheetToObjects.Lib.Validation;

namespace SheetToObjects.Lib
{
    internal interface IMapValue
    {
        Result<object, IValidationError> Map(string value, Type propertyType, ColumnMapping columnMapping, int rowIndex);
    }
}