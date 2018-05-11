using System;
using System.Collections.Generic;
using SheetToObjects.Lib.Configuration;
using SheetToObjects.Lib.Validation;

namespace SheetToObjects.Lib.Attributes.MappingType
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreMapping : Attribute, IMappingAttribute
    {
        public ColumnMapping GetColumnMapping(List<IRule> rules)
        {
            return null;
        }
    }
}
