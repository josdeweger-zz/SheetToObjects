using System.Collections.Generic;
using System.Linq;
using SheetToObjects.Core;

namespace SheetToObjects.Lib.Validation
{
    public class Result
    {
        public List<string> ValidationMessages { get; set; }

        public object Value { get; private set; }

        public bool IsValid => !ValidationMessages.Any();

        public bool HasValue => Value.IsNotNull();

        public Result()
        {
            ValidationMessages = new List<string>();
        }

        public static Result From<T>(T value)
        {
            return new Result {Value = value};
        }
    }
}