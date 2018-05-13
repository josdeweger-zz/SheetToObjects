using System;
using SheetToObjects.Lib.Validation;

namespace SheetToObjects.Lib.Attributes.Rules
{
    [AttributeUsage(AttributeTargets.Property)]
    public class Regex : Attribute, IRuleAttribute
    {
        public string Pattern { get; }

        public bool IgnoreCase { get;  }
        
        public Regex(string pattern, bool ignoreCase)
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
