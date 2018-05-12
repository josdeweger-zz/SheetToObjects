using System;
using SheetToObjects.Lib.Validation;

namespace SheetToObjects.Lib.Attributes.Rules
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IsRequired : Attribute, IRuleAttribute
    {
        private bool _allowWhiteSpace;

        public IsRequired(bool allowWhiteSpace = false)
        {
            _allowWhiteSpace = allowWhiteSpace;
        }

        public IRule GetRule()
        {
            var requireRule = new RequiredRule();

            if(_allowWhiteSpace)
                requireRule.AllowWhiteSpace();

            return requireRule;
        }
    }
}
