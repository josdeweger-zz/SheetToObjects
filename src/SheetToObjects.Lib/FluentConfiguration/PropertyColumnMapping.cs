using System;
using System.Collections.Generic;
using SheetToObjects.Lib.Validation;

namespace SheetToObjects.Lib.FluentConfiguration
{
    internal class PropertyColumnMapping : ColumnMapping, IUseHeaderRow
    {
        public string ColumnName { get; }
        public bool IsRequiredInHeaderRow { get; }

        public PropertyColumnMapping(
            string propertyName, 
            string format, 
            List<IParsingRule> parsingRules, 
            List<IRule> rules, 
            object defaultValue, 
            bool isRequiredInHeaderRow,
            Func<string, object> customValueParser) 
            : base(propertyName, format, parsingRules, rules, defaultValue, customValueParser)
        {
            ColumnName = propertyName;
            ColumnIndex = -1;
            IsRequiredInHeaderRow = isRequiredInHeaderRow;
        }
        public void SetColumnIndex(int columnIndex)
        {
            ColumnIndex = columnIndex;
        }

        public override string DisplayName => ColumnName;

    }
}
