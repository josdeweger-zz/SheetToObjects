using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SheetToObjects.Lib.Configuration.ColumnMappings;
using SheetToObjects.Lib.Validation;

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

        public void SetColumnMapping<TModel>(ColumnMappingBuilder<TModel> builder)
        {
            builder.WithColumnIndex(ColumnIndex);
        }
    }
}
