using SheetToObjects.Lib.Configuration.ColumnMappings;

namespace SheetToObjects.Lib.Configuration
{
    public interface IMappingConfig
    {
        ColumnMapping For<TModel>();
        ColumnMapping GetColumnMappingByPropertyName(string propertyName);
    }
}