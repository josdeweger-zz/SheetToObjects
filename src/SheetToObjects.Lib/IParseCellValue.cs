using System;
using CSharpFunctionalExtensions;

namespace SheetToObjects.Lib
{
    public interface IParseValue
    {
        Result<object, string> ParseValueType<TValue>(string value);
        Result<object, string> ParseEnumeration(string value, Type type);
    }
}