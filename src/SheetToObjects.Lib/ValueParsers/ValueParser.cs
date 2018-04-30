using System;

namespace SheetToObjects.Lib.ValueParsers
{
    public class ValueParser : IParseValues
    {
        private readonly StringValueParser _stringValueParser = new StringValueParser();
        private readonly IntValueParser _intValueParser = new IntValueParser();
        private readonly NullableIntValueParser _nullableIntValueParser = new NullableIntValueParser();
        private readonly DoubleValueParser _doubleValueParser = new DoubleValueParser();
        private readonly NullableDoubleValueParser _nullableDoubleValueParser = new NullableDoubleValueParser();
        private readonly BoolValueParser _boolValueParser = new BoolValueParser();
        private readonly NullableBoolValueParser _nullableBoolValueParser = new NullableBoolValueParser();

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
                case var _ when propertyType == typeof(bool?):
                    return _nullableBoolValueParser.Parse(value);
                case var _ when propertyType == typeof(bool):
                    return _boolValueParser.Parse(value);
                case var _ when propertyType.IsEnum:
                    return new EnumValueParser(propertyType).Parse(value);
                default:
                    throw new ApplicationException($"No value parser found for type {propertyType}");
            }
        }
    }
}