using System;
using System.Globalization;
using CSharpFunctionalExtensions;
using SheetToObjects.Core;

namespace SheetToObjects.Lib.Parsing
{
    internal class EnumValueParser : IValueParsingStrategy
    {
        private readonly Type _type;

        public EnumValueParser(Type type)
        {
            _type = type;
        }

        public Result<object, string> Parse(string value)
        {
            var errorMessage = $"Cannot parse value '{value}' to type '{_type?.Name}'";

            if (_type.IsNull())
                return Result.Fail<object, string>(errorMessage);

            if (!_type.IsEnum)
                return Result.Fail<object, string>(errorMessage);

            if (value.IsNull())
                return Result.Fail<object, string>(errorMessage);

            try
            {
                if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var intValue))
                {
                    if (_type.IsEnumDefined(intValue))
                    {
                        return Result.Ok<object, string>(Enum.ToObject(_type, intValue));
                    }

                    return Result.Fail<object, string>(errorMessage);
                }

                var enumValue = Enum.Parse(_type, value.Replace(" ", string.Empty), ignoreCase: true);
                if (enumValue.IsNotNull())
                {
                    return Result.Ok<object, string>(enumValue);
                }
            }
            catch (Exception)
            {
                return Result.Fail<object, string>(errorMessage);
            }

            return Result.Fail<object, string>(errorMessage);
        }
    }
}