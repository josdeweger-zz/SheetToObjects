using System;
using SheetToObjects.Lib.Configuration.ColumnMappings;

namespace SheetToObjects.Lib.Attributes.MappingType
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
