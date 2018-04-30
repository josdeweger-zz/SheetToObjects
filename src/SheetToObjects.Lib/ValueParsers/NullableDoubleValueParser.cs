using System;
using SheetToObjects.Core;
using SheetToObjects.Lib.Validation;

namespace SheetToObjects.Lib.ValueParsers
{
    public class NullableDoubleValueParser : IParseValueStrategy
    {
        private readonly Result _defaultValue = Result.From(default(double?));

        public Result Parse(object value)
        {
            if (value.IsNull())
                return _defaultValue;

            try
            {
                if (double.TryParse(value.ToString(), out var doubleValue))
                {
                    return Result.From(doubleValue);
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