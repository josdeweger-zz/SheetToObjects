using CSharpFunctionalExtensions;

namespace SheetToObjects.Lib.Validation
{
    public interface IParsingRule
    {
        Result Validate(string value);
    }
}