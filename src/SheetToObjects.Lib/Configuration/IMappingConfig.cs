using SheetToObjects.Lib.Configuration.ColumnMappings;

namespace SheetToObjects.Lib.Configuration
{
    internal interface IMappingConfig
    {
        ColumnMapping For<T>();
        ColumnMapping GetColumnMappingByPropertyName(string propertyName);
    }
}