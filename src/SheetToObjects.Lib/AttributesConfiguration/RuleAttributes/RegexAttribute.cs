using System;
using SheetToObjects.Lib.Validation;

namespace SheetToObjects.Lib.AttributesConfiguration.RuleAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RegexAttribute : Attribute, IRuleAttribute
    {
        public string Pattern { get; }

        public bool IgnoreCase { get;  }
        
        public RegexAttribute(string pattern, bool ignoreCase)
        {
            Pattern = pattern;
            IgnoreCase = ignoreCase;
        }

        public IRule GetRule()
        {
            return new RegexRule(Pattern, IgnoreCase);
        }
    }
}
