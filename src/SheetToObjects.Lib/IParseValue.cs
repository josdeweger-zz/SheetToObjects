using System;
using CSharpFunctionalExtensions;

namespace SheetToObjects.Lib
{
    internal interface IParseValue
    {
        Result<object, string> Parse(Type type, string value, string format = "");
    }
}