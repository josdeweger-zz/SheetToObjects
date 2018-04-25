namespace SheetToObjects.Lib
{
    public interface IParseValueStrategy<out T>
    {
        T Parse(object value);
    }
}