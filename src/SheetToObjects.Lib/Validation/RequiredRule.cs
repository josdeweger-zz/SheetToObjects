using CSharpFunctionalExtensions;
using SheetToObjects.Core;

namespace SheetToObjects.Lib.Validation
{
    public class RequiredRule : IRule
    {
        public bool DoTrimValue { get; }

        public RequiredRule()
        {
        }

        public RequiredRule(bool doTrimValue)
        {
            DoTrimValue = doTrimValue;
        }


        public Result Validate(string value)
        {
            if(value.IsNullOrEmpty())
                return Result.Fail("Value is required");

            if(DoTrimValue && value.IsNullOrWhiteSpace())
                return Result.Fail("Value is required");

            return Result.Ok();
        }
    }
}