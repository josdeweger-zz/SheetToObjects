namespace SheetToObjects.Lib.Configuration
{
    public interface IMappingConfig
    {
        ColumnMapping For<TModel>();
        ColumnMapping GetColumnMappingByPropertyName(string propertyName);
    }
}