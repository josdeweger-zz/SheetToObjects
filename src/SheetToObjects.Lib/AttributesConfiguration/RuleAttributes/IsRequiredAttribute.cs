using System;
using SheetToObjects.Lib.Validation;

namespace SheetToObjects.Lib.AttributesConfiguration.RuleAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IsRequiredAttribute : Attribute, IParsingRuleAttribute
    {
        private readonly bool _allowWhiteSpace;

        public IsRequiredAttribute(bool allowWhiteSpace = false)
        {
            _allowWhiteSpace = allowWhiteSpace;
        }

        public IParsingRule GetRule()
        {
            var requireRule = new RequiredRule();

            if(_allowWhiteSpace)
                requireRule.AllowWhiteSpace();

            return requireRule;
        }
    }
}
