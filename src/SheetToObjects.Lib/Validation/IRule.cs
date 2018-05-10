using CSharpFunctionalExtensions;

namespace SheetToObjects.Lib.Validation
{
    public interface IRule
    {
        Result Validate(string value);
    }
}