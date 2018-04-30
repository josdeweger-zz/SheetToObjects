using SheetToObjects.Lib.Validation;

namespace SheetToObjects.Lib
{
    public interface IParseValueStrategy
    {
        Result Parse(object value);
    }
}