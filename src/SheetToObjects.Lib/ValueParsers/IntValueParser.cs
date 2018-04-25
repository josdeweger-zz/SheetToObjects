using System;
using SheetToObjects.Core;

namespace SheetToObjects.Lib.ValueParsers
{
    public class IntValueParser : IParseValueStrategy<int>
    {
        private const int DefaultValue = default(int);

        public int Parse(object value)
        {
            if (value.IsNull())
                return DefaultValue;

            try
            {
                if (int.TryParse(value.ToString(), out var intValue))
                {
                    return intValue;
                };
            }
            catch (Exception)
            {
                return DefaultValue;
            }

            return DefaultValue;
        }
    }
}