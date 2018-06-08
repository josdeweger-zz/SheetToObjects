using System;
using SheetToObjects.Lib.FluentConfiguration;

namespace SheetToObjects.Lib.AttributesConfiguration.MappingType
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MappingByLetter : Attribute, IMappingAttribute
    {
        public string ColumnLetter { get;  }
        
        public MappingByLetter(string columnLetter)
        {
            ColumnLetter = columnLetter;
        }

        public void SetColumnMapping<T>(ColumnMappingBuilder<T> builder)
        {
            builder.WithColumnLetter(ColumnLetter);
        }
    }
}
