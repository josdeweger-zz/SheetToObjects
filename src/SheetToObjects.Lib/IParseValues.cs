using SheetToObjects.Core;

namespace SheetToObjects.Lib
{
    public interface IParseValues
    {
        Result Parse<T>(object value);
    }
}