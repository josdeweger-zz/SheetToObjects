using System;
using CSharpFunctionalExtensions;

namespace SheetToObjects.Lib.Parsing
{
    internal interface IProvideParsingStrategy
    {
        Result<object, string> Parse(Type type, string value, string format = "");
    }
}