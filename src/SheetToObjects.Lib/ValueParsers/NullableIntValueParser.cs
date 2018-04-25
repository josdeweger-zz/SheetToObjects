using System;
using SheetToObjects.Core;

namespace SheetToObjects.Lib.ValueParsers
{
    public class NullableIntValueParser : IParseValueStrategy<int?>
    {
        private readonly int? _defaultValue = default(int?);

        public int? Parse(object value)
        {
            if (value.IsNull())
                return _defaultValue;

            try
            {
                if (int.TryParse(value.ToString(), out var intValue))
                {
                    return intValue;
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