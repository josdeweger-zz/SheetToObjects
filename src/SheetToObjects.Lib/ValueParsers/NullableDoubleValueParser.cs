using System;
using SheetToObjects.Core;

namespace SheetToObjects.Lib.ValueParsers
{
    public class NullableDoubleValueParser : IParseValueStrategy<double?>
    {
        private readonly double? _defaultValue = default(double?);

        public double? Parse(object value)
        {
            if (value.IsNull())
                return _defaultValue;

            try
            {
                if (double.TryParse(value.ToString(), out var doubleValue))
                {
                    return doubleValue;
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