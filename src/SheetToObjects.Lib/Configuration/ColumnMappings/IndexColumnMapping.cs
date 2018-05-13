using System.Collections.Generic;
using SheetToObjects.Lib.Validation;

namespace SheetToObjects.Lib.Configuration.ColumnMappings
{
    internal class IndexColumnMapping : ColumnMapping
    {
        public IndexColumnMapping(int columnIndex, string propertyName, List<IRule> rules) : base(propertyName, rules)
        {
            ColumnIndex = columnIndex;
        }

        public override string DisplayName => $"Column with index {ColumnIndex}";
    }
}
