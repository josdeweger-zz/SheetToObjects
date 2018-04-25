using System;

namespace SheetToObjects.Lib.ValueParsers
{
    public class ValueParser : IParseValues
    {
        private readonly StringValueParser _stringValueParser;
        private readonly IntValueParser _intValueParser;
        private readonly NullableIntValueParser _nullableIntValueParser;
        private readonly DoubleValueParser _doubleValueParser;
        private readonly NullableDoubleValueParser _nullableDoubleValueParser;

        public ValueParser()
        {
            _stringValueParser = new StringValueParser();
            _intValueParser = new IntValueParser();
            _nullableIntValueParser = new NullableIntValueParser();
            _doubleValueParser = new DoubleValueParser();
            _nullableDoubleValueParser = new NullableDoubleValueParser();
        }

        public object Parse(Type propertyType, object value)
        {
            switch (true)
            {
                case var _ when propertyType == typeof(string):
                    return _stringValueParser.Parse(value);
                case var _ when propertyType == typeof(int?):
                    return _nullableIntValueParser.Parse(value);
                case var _ when propertyType == typeof(int):
                    return _intValueParser.Parse(value);
                case var _ when propertyType == typeof(double?):
                    return _nullableDoubleValueParser.Parse(value);
                case var _ when propertyType == typeof(double):
                    return _doubleValueParser.Parse(value);
                default:
                    throw new ApplicationException($"No value parser found for type {propertyType}");
            }
        }
    }
}