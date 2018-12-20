using SheetToObjects.Lib.Validation;

namespace SheetToObjects.Lib.AttributesConfiguration.RuleAttributes
{
    interface IRuleAttribute
    {
        IRule GetRule();
    }
}
