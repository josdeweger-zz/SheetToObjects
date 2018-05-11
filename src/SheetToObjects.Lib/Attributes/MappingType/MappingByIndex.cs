using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SheetToObjects.Lib.Configuration;
using SheetToObjects.Lib.Validation;

namespace SheetToObjects.Lib.Attributes.MappingType
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MappingByIndex : Attribute, IMappingAttribute
    {
        public int ColumnIndex { get; set; }

        public string PropertyName { get; set; }

        public MappingByIndex(int columnIndex, [CallerMemberName]string propertyName = "")
        {
            ColumnIndex = columnIndex;
            PropertyName = propertyName;
        }

        public ColumnMapping GetColumnMapping(List<IRule> rules)
        {
            return new IndexColumnMapping(ColumnIndex, PropertyName, rules);
        }
    }
}
