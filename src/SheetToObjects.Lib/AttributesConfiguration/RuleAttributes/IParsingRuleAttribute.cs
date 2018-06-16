using SheetToObjects.Lib.Validation;

namespace SheetToObjects.Lib.AttributesConfiguration.RuleAttributes
{
    interface IParsingRuleAttribute
    {
        IParsingRule GetRule();
    }
}
