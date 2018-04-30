using System;
using System.Collections.Generic;
using SheetToObjects.Lib.Validation;

namespace SheetToObjects.Lib.Configuration
{
    public class ColumnMapping
    {
        public string ColumnLetter { get; }
        public string PropertyName { get; }
        public Type PropertyType { get; }
        public List<IRule> Rules { get; }

        public ColumnMapping(string columnLetter, string propertyName, Type propertyType)
        {
            ColumnLetter = columnLetter;
            PropertyName = propertyName;
            PropertyType = propertyType;
            Rules = new List<IRule>();
        }

        public void AddRules(List<IRule> rules)
        {
            Rules.AddRange(rules);
        }
    }
}