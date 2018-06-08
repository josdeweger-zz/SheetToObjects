using System.Collections.Generic;
using SheetToObjects.Lib.Validation;

namespace SheetToObjects.Lib.FluentConfiguration
{
    public abstract class ColumnMapping
    {
        public abstract string DisplayName { get; }

        public bool IsRequired
        {
            get { return ParsingRules.Exists(r => r is RequiredRule); }
        }

        public int ColumnIndex { get; protected set; }
        public string PropertyName { get; }
        public string Format { get; }
        public List<IParsingRule> ParsingRules { get; }
        public List<IRule> Rules { get; }

        protected ColumnMapping(string propertyName, string format, List<IParsingRule> parsingRules, List<IRule> rules)
        {
            PropertyName = propertyName;
            Format = format;
            ParsingRules = parsingRules ?? new List<IParsingRule>();
            Rules = rules ?? new List<IRule>();
        }
    }
}
