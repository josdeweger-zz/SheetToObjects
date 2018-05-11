using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SheetToObjects.Lib.Configuration.ColumnMappings;
using SheetToObjects.Lib.Validation;

namespace SheetToObjects.Lib.Attributes.MappingType
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MappingByColumnName : Attribute, IMappingAttribute
    {
        public string ColumnName { get; set; }

        public string PropertyName { get; set; }

        public MappingByColumnName(string columnName, [CallerMemberName]string propertyName = "")
        {
            ColumnName = columnName;
            PropertyName = propertyName;
        }

        public void SetColumnMapping<TModel>(ColumnMappingBuilder<TModel> builder)
        {
            builder.WithHeader(ColumnName);
        }
    }
}
