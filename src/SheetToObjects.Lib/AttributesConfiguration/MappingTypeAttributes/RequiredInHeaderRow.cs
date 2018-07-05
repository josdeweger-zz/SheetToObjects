using System;
using SheetToObjects.Lib.FluentConfiguration;

namespace SheetToObjects.Lib.AttributesConfiguration.MappingTypeAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RequiredInHeaderRow : Attribute
    {
        public void SetColumnMapping<T>(ColumnMappingBuilder<T> builder)
        {
            builder.WithRequiredInHeaderRow();
        }
    }
}
