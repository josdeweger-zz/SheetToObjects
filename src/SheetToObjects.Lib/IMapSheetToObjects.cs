namespace SheetToObjects.Lib
{
    public interface IMapSheetToObjects
    {
        SheetMapper Map(Sheet sheet);

        MappingResult<T> To<T>() where T : new();
    }
}