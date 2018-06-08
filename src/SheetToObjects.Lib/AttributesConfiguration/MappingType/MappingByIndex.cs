using System;
using SheetToObjects.Lib.FluentConfiguration;

namespace SheetToObjects.Lib.AttributesConfiguration.MappingType
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MappingByIndex : Attribute, IMappingAttribute
    {
        public int ColumnIndex { get;  }
        
        public MappingByIndex(int columnIndex)
        {
            ColumnIndex = columnIndex;
        }

        public void SetColumnMapping<T>(ColumnMappingBuilder<T> builder)
        {
            builder.WithColumnIndex(ColumnIndex);
        }
    }
}
