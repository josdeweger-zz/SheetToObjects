using System;
using SheetToObjects.Core;
using SheetToObjects.Lib.Validation;

namespace SheetToObjects.Lib.ValueParsers
{
    public class BoolValueParser : IParseValueStrategy
    {
        private readonly Result _defaultValue = Result.From(default(bool));

        public Result Parse(object value)
        {
            if (value.IsNull())
                return _defaultValue;

            try
            {
                if (bool.TryParse(value.ToString(), out var boolValue))
                {
                    return Result.From(boolValue);
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