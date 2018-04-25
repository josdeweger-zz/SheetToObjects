using System;
using SheetToObjects.Core;

namespace SheetToObjects.Lib.ValueParsers
{
    public class DoubleValueParser : IParseValueStrategy<double>
    {
        private const double DefaultValue = default(double);

        public double Parse(object value)
        {
            if (value.IsNull())
                return DefaultValue;

            try
            {
                if (double.TryParse(value.ToString(), out var doubleValue))
                {
                    return doubleValue;
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