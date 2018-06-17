﻿using System.Collections.Generic;
using SheetToObjects.Lib.Validation;

namespace SheetToObjects.Lib.FluentConfiguration
{
    internal class IndexColumnMapping : ColumnMapping
    {
        public IndexColumnMapping(int columnIndex, string propertyName, string format, List<IParsingRule> parsingRules, List<IRule> rules, object defaultValue) 
            : base(propertyName, format, parsingRules, rules, defaultValue)
        {
            ColumnIndex = columnIndex;
        }

        public override string DisplayName => $"Column with index {ColumnIndex}";
    }
}
