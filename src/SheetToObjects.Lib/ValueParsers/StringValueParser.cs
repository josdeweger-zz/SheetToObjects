using System;
using SheetToObjects.Core;

namespace SheetToObjects.Lib.ValueParsers
{
    public class StringValueParser : IParseValueStrategy<string>
    {
        private readonly string _defaultValue = string.Empty;

        public string Parse(object value)
        {
            if (value.IsNull())
                return _defaultValue;

            try
            {
                return value.ToString();
            }
            catch (Exception)
            {
                return _defaultValue;
            }
        }
    }
}