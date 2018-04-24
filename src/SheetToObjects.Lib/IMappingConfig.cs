namespace SheetToObjects.Lib
{
    public interface IMappingConfig
    {
        ColumnMapping For<TModel>();
        ColumnMapping GetColumnMappingByPropertyName(string propertyName);
    }
}