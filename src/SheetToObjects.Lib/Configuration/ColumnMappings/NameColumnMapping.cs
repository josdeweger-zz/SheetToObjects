using System;
using System.Collections.Generic;
using SheetToObjects.Lib.Validation;

namespace SheetToObjects.Lib.Configuration
{
    public class NameColumnMapping : ColumnMapping, IUseHeaderRow
    {
        public string ColumnName { get; }

        public NameColumnMapping(string columnName, string propertyName, List<IRule> rules) : base(propertyName, rules)
        {
            ColumnName = columnName;
            ColumnIndex = -1;
        }

        public void SetColumnIndex(int columnIndex)
        {
            ColumnIndex = columnIndex;
        }
    }
}
