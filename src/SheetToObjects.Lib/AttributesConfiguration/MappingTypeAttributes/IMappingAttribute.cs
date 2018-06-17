using SheetToObjects.Lib.FluentConfiguration;

namespace SheetToObjects.Lib.AttributesConfiguration.MappingTypeAttributes
{
    internal interface IMappingAttribute
    {
        void SetColumnMapping<T>(ColumnMappingBuilder<T> builder);
    }
}
