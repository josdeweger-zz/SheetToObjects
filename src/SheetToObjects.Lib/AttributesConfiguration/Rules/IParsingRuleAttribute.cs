using SheetToObjects.Lib.Validation;

namespace SheetToObjects.Lib.AttributesConfiguration.Rules
{
    interface IParsingRuleAttribute
    {
        IParsingRule GetRule();
    }
}
