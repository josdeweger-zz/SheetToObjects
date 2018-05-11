using System;
using SheetToObjects.Lib.Validation;

namespace SheetToObjects.Lib.Attributes.Rules
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IsRequired : Attribute, IRuleAttribute
    {
        public bool AllowWhiteSpace { get; }
        
        public IsRequired()
        {

        }

        public IsRequired(bool allowWhiteSpace)
        {
            AllowWhiteSpace = allowWhiteSpace;
        }

        public IRule GetRule()
        {
            return new RequiredRule(AllowWhiteSpace);
        }
    }
}
