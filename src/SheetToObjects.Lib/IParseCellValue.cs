using System;
using CSharpFunctionalExtensions;

namespace SheetToObjects.Lib
{
    public interface IParseCellValue
    {
        Result<object, ValidationError> ParseValueType<TValue>(Cell cell);
        Result<object, ValidationError> ParseEnumeration(Cell cell, Type type);
    }
}