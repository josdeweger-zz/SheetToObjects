using System;
using System.Globalization;
using CSharpFunctionalExtensions;
using SheetToObjects.Core;

namespace SheetToObjects.Lib
{
    internal class ValueParser : IParseValue
    {
        public Result<object, string> Parse(Type type, string value, string format = "")
        {
            if(type.IsNull())
                return Result.Fail<object, string>($"Can not parse value for unspecified type");

            switch (true)
            {
                case var _ when type == typeof(string):
                    return Parse<string>(value);
                case var _ when type == typeof(int) || type == typeof(int?):
                    return Parse<int>(value);
                case var _ when type == typeof(double) || type == typeof(double?):
                    return Parse<double>(value);
                case var _ when type == typeof(float) || type == typeof(float?):
                    return Parse<float>(value);
                case var _ when type == typeof(decimal) || type == typeof(decimal?):
                    return Parse<decimal>(value);
                case var _ when type == typeof(bool) || type == typeof(bool?):
                    return Parse<bool>(value);
                case var _ when type == typeof(DateTime) || type == typeof(DateTime?):
                    return ParseDateTime(value, format);
                case var _ when type.IsEnum:
                    return ParseEnumeration(type, value);
                default:
                    return Result.Fail<object, string>($"Parser for '{type.Name}' not implemented");
            }
        }

        private Result<object, string> Parse<T>(string value)
        {
            var type = typeof(T);

            try
            {
                var parsedValue = Convert.ChangeType(value, type);
                return Result.Ok<object, string>(parsedValue);
            }
            catch (Exception)
            {
                return Result.Fail<object, string>($"Cannot parse value '{value}' to type '{type.Name}'");
            }
        }

        private Result<object, string> ParseDateTime(string value, string format)
        {
            var errorMessage = $"Cannot parse value '{value}' to DateTime using format '{format}'";

            try
            {
                var parsedDateTime = DateTime.ParseExact(value, format, CultureInfo.InvariantCulture);
                return Result.Ok<object, string>(parsedDateTime);
            }
            catch (Exception)
            {
                return Result.Fail<object, string>(errorMessage);
            }
        }

        private Result<object, string> ParseEnumeration(Type type, string value)
        {
            var errorMessage = $"Cannot parse value '{value}' to type '{type?.Name}'";

            if (type.IsNull())
                return Result.Fail<object, string>(errorMessage);

            if (!type.IsEnum)
                return Result.Fail<object, string>(errorMessage);

            if (value.IsNull())
                return Result.Fail<object, string>(errorMessage);

            try
            {
                if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var intValue))
                {
                    if (type.IsEnumDefined(intValue))
                    {
                        return Result.Ok<object, string>(Enum.ToObject(type, intValue));
                    }

                    return Result.Fail<object, string>(errorMessage);
                }

                var enumValue = Enum.Parse(type, value.Replace(" ", string.Empty), ignoreCase: true);
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