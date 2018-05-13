using SheetToObjects.Lib.Configuration.ColumnMappings;

namespace SheetToObjects.Lib.Configuration
{
    internal interface IMappingConfig
    {
        ColumnMapping For<TModel>();
        ColumnMapping GetColumnMappingByPropertyName(string propertyName);
    }
}