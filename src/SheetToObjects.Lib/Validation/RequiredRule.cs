using CSharpFunctionalExtensions;
using SheetToObjects.Core;

namespace SheetToObjects.Lib.Validation
{
    public class RequiredRule : IRule
    {
        private readonly bool _doTrimValue;

        public RequiredRule()
        {
        }

        public RequiredRule(bool doTrimValue)
        {
            _doTrimValue = doTrimValue;
        }


        public Result Validate(string value)
        {
            if(value.IsNullOrEmpty())
                return Result.Fail("Value is required");

            if(_doTrimValue && value.IsNullOrWhiteSpace())
                return Result.Fail("Value is required");

            return Result.Ok();
        }
    }
}