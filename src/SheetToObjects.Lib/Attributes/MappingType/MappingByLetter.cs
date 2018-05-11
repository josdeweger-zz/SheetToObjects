using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SheetToObjects.Lib.Configuration;
using SheetToObjects.Lib.Validation;

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

        public ColumnMapping GetColumnMapping(List<IRule> rules)
        {
            return new LetterColumnMapping(ColumnLetter, PropertyName, rules);
        }
    }
}
