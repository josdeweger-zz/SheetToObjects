using SheetToObjects.Lib.Validation;

namespace SheetToObjects.Lib.AttributesConfiguration.Rules
{
    interface IRuleAttribute
    {
        IRule GetRule();
    }
}
