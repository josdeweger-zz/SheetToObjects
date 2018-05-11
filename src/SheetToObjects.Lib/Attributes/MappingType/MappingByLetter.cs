using System;
using System.Runtime.CompilerServices;
using SheetToObjects.Lib.Configuration.ColumnMappings;

namespace SheetToObjects.Lib.Attributes.MappingType
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MappingByLetter : Attribute, IMappingAttribute
    {
        public string ColumnLetter { get; set; }

        public string PropertyName { get; set; }

        public MappingByLetter(string columnLetter, [CallerMemberName]string propertyName = "")
        {
            ColumnLetter = columnLetter;
            PropertyName = propertyName;
        }

        public void SetColumnMapping<TModel>(ColumnMappingBuilder<TModel> builder)
        {
            builder.WithColumnLetter(ColumnLetter);
        }
    }
}
