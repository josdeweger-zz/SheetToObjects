using System;
using CSharpFunctionalExtensions;
using SheetToObjects.Lib.Configuration.ColumnMappings;

namespace SheetToObjects.Lib
{
    internal interface IMapValue
    {
        Result<object, IValidationError> Map(string value, Type propertyType, ColumnMapping columnMapping, int rowIndex);
    }
}