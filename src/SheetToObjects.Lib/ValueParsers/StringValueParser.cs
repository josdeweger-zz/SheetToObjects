using System;
using SheetToObjects.Core;
using SheetToObjects.Lib.Validation;

namespace SheetToObjects.Lib.ValueParsers
{
    public class StringValueParser : IParseValueStrategy
    {
        private readonly Result _defaultValue = Result.From(string.Empty);

        public Result Parse(object value)
        {
            if (value.IsNull())
                return _defaultValue;

            try
            {
                return Result.From(value.ToString());
            }
            catch (Exception)
            {
                return _defaultValue;
            }
        }
    }
}