using System;
using System.Collections.Generic;
using System.Text;
using SheetToObjects.Lib.Validation;

namespace SheetToObjects.Lib.Configuration.ColumnMappings
{
    public class IndexColumnMapping : ColumnMapping
    {
        public IndexColumnMapping(int columnIndex, string propertyName, List<IRule> rules) : base(propertyName, rules)
        {
            ColumnIndex = columnIndex;
        }
    }
}
