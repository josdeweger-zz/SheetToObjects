using System;
using System.Collections.Generic;
using SheetToObjects.Lib.Validation;

namespace SheetToObjects.Lib.FluentConfiguration
{
    internal class IndexColumnMapping : ColumnMapping
    {
        public IndexColumnMapping(
            int columnIndex, 
            string propertyName, 
            string format, 
            List<IParsingRule> parsingRules, 
            List<IRule> rules, 
            object defaultValue,
            Func<string, object> customParser) 
            : base(propertyName, format, parsingRules, rules, defaultValue, customParser)
        {
            ColumnIndex = columnIndex;
        }

        public override string DisplayName => $"Column with index {ColumnIndex}";
    }
}
