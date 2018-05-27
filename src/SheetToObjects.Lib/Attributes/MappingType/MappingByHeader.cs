using System;
using SheetToObjects.Lib.Configuration.ColumnMappings;

namespace SheetToObjects.Lib.Attributes.MappingType
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MappingByHeader : Attribute, IMappingAttribute
    {
        public string ColumnName { get;  }
        
        public MappingByHeader(string columnName)
        {
            ColumnName = columnName;
        }

        public void SetColumnMapping<T>(ColumnMappingBuilder<T> builder)
        {
            builder.WithHeader(ColumnName);
        }
    }
}
