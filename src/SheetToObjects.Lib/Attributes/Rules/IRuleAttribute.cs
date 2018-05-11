using SheetToObjects.Lib.Validation;

namespace SheetToObjects.Lib.Attributes.Rules
{
    interface IRuleAttribute
    {
        IRule GetRule();
    }
}
