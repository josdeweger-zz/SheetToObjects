using SheetToObjects.Lib.Configuration.ColumnMappings;


namespace SheetToObjects.Lib.Attributes.MappingType
{
    interface IMappingAttribute
    {
        void SetColumnMapping<TModel>(ColumnMappingBuilder<TModel> builder);
    }
}
