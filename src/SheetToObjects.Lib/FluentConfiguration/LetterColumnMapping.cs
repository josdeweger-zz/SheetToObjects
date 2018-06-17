using System.Collections.Generic;
using SheetToObjects.Core;
using SheetToObjects.Lib.Validation;

namespace SheetToObjects.Lib.FluentConfiguration
{
    internal class LetterColumnMapping : ColumnMapping
    {
        public string ColumnName { get; }

        public LetterColumnMapping(string columnLetter, string propertyName, string format, List<IParsingRule> parsingRules, List<IRule> rules, object defaultValue) 
            : base(propertyName, format, parsingRules, rules, defaultValue)
        {
            ColumnName = columnLetter;
            ColumnIndex = columnLetter.ConvertExcelColumnNameToIndex();
        }

        public override string DisplayName => ColumnName;
    }
}
