using System.Collections.Generic;
using SheetToObjects.Lib.Validation;

namespace SheetToObjects.Lib.Configuration.ColumnMappings
{
    public abstract class ColumnMapping
    {
        public abstract string DisplayName { get; }

        public bool IsRequired
        {
            get { return Rules.Exists(r => r is RequiredRule); }
        }

        public int? ColumnIndex { get; protected set; }
        public string PropertyName { get; }
        public List<IRule> Rules { get; }

        public ColumnMapping(string propertyName, List<IRule> rules)
        {
            PropertyName = propertyName;
            Rules = rules ?? new List<IRule>();
        }

        public void AddRules(List<IRule> rules)
        {
            Rules.AddRange(rules);
        }
    }
}
