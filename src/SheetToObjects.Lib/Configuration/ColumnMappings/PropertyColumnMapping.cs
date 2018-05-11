using System;
using System.Collections.Generic;
using SheetToObjects.Lib.Validation;

namespace SheetToObjects.Lib.Configuration
{
    public class PropertyColumnMapping : ColumnMapping, IUseHeaderRow
    {
        public string ColumnName { get; }

        public PropertyColumnMapping(string propertyName, List<IRule> rules) : base(propertyName, rules)
        {
            ColumnName = propertyName;
        }
        public void SetColumnIndex(int columnIndex)
        {
            ColumnIndex = columnIndex;
        }

    }
}
