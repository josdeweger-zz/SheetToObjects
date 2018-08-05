using SheetToObjects.Lib.Validation;

namespace SheetToObjects.Lib.AttributesConfiguration.RuleAttributes
{
    interface IColumnRuleAttribute
    {
        IColumnRule GetRule();
    }
}
