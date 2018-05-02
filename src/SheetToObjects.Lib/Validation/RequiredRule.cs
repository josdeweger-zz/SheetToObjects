using SheetToObjects.Core;

namespace SheetToObjects.Lib.Validation
{
    public class RequiredRule : IRule
    {
        public Result Validate(string value)
        {
            if(value.IsNullOrEmpty())
                return Result.Fail("Value is required");

            return Result.Ok();
        }
    }
}