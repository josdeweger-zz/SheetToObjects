using SheetToObjects.Lib.FluentConfiguration;

namespace SheetToObjects.Lib.AttributesConfiguration.MappingType
{
    internal interface IMappingAttribute
    {
        void SetColumnMapping<T>(ColumnMappingBuilder<T> builder);
    }
}
