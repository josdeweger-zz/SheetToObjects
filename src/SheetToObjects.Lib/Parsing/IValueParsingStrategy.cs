using CSharpFunctionalExtensions;

namespace SheetToObjects.Lib.Parsing
{
    internal interface IValueParsingStrategy
    {
        Result<object, string> Parse(string value);
    }
}