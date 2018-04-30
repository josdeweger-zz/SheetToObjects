using System;
using SheetToObjects.Core;
using SheetToObjects.Lib.Validation;

namespace SheetToObjects.Lib.ValueParsers
{
    public class NullableIntValueParser : IParseValueStrategy
    {
        private readonly Result _defaultValue = Result.From(default(int?));

        public Result Parse(object value)
        {
            if (value.IsNull())
                return _defaultValue;

            try
            {
                if (int.TryParse(value.ToString(), out var intValue))
                {
                    return Result.From(intValue);
                };
            }
            catch (Exception)
            {
                return _defaultValue;
            }

            return _defaultValue;
        }
    }
}