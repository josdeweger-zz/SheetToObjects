using System;
using CSharpFunctionalExtensions;
using SheetToObjects.Lib.Configuration.ColumnMappings;

namespace SheetToObjects.Lib
{
    internal interface IMapValue
    {
        Result<object, ValidationError> Map(string value, Type propertyType, ColumnMapping columnMapping, int rowIndex);
    }
}