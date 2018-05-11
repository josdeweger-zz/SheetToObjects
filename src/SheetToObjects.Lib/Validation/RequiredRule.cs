using CSharpFunctionalExtensions;
using SheetToObjects.Core;

namespace SheetToObjects.Lib.Validation
{
    public class RequiredRule : IRule
    {
        public bool AllowWhiteSpace { get; }

        public RequiredRule()
        {
        }

        public RequiredRule(bool allowWhiteSpace)
        {
            AllowWhiteSpace = allowWhiteSpace;
        }


        public Result Validate(string value)
        {
            if(value.IsNullOrEmpty())
                return Result.Fail("Value is required");

            if(!AllowWhiteSpace && value.IsNullOrWhiteSpace())
                return Result.Fail("Value is required");

            return Result.Ok();
        }
    }
}