using CSharpFunctionalExtensions;
using SheetToObjects.Core;

namespace SheetToObjects.Lib.Validation
{
    internal class RequiredRule : IRule
    {
        public bool WhiteSpaceAllowed { get; private set; }
        
        public RequiredRule AllowWhiteSpace()
        {
            WhiteSpaceAllowed = true;
            return this;
        }

        public Result Validate(string value)
        {
            if(value.IsNullOrEmpty())
                return Result.Fail("Value is required");

            if(!WhiteSpaceAllowed && value.IsNullOrWhiteSpace())
                return Result.Fail("Value is required");

            return Result.Ok();
        }
    }
}