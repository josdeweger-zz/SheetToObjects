namespace SheetToObjects.Lib
{
    public interface IMapSheetToObjects
    {
        SheetMapper Map(Sheet sheet);

        MappingResult<TModel> To<TModel>() where TModel : new();
    }
}