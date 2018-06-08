using System;
using SheetToObjects.Lib.Validation;

namespace SheetToObjects.Lib.AttributesConfiguration.Rules
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IsRequired : Attribute, IParsingRuleAttribute
    {
        private readonly bool _allowWhiteSpace;

        public IsRequired(bool allowWhiteSpace = false)
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
