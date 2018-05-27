using SheetToObjects.Lib.Configuration.ColumnMappings;


namespace SheetToObjects.Lib.Attributes.MappingType
{
    internal interface IMappingAttribute
    {
        void SetColumnMapping<T>(ColumnMappingBuilder<T> builder);
    }
}
