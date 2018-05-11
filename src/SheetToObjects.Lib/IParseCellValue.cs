using System;
using CSharpFunctionalExtensions;

namespace SheetToObjects.Lib
{
    public interface IParseValue
    {
        Result<object, ValidationError> ParseValueType<TValue>(string value, int columnIndex, int rowIndex, bool isRequired, string columnName);
        Result<object, ValidationError> ParseEnumeration(string value, int columnIndex, int rowIndex, Type type, string columnName);
    }
}