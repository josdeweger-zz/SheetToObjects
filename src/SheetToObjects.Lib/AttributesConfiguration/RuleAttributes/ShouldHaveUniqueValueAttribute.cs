using System;
using SheetToObjects.Lib.Validation;

namespace SheetToObjects.Lib.AttributesConfiguration.RuleAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ShouldHaveUniqueValueAttribute : Attribute, IColumnRuleAttribute
    {
        public IColumnRule GetRule()
        {
            return new UniqueValuesInColumnRule();
        }
    }
}
