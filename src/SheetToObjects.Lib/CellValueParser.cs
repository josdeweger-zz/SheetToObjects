using System;
using System.Globalization;
using CSharpFunctionalExtensions;
using SheetToObjects.Core;

namespace SheetToObjects.Lib
{
    public class ValueParser
    {
        public Result<object, string> ParseValueType<TValue>(string value)
        {
            var type = typeof(TValue);
            
            try
            {
                var parsedValue = (TValue) Convert.ChangeType(value, type);
                return Result.Ok<object, string>(parsedValue);
            }
            catch (Exception)
            {
                return Result.Fail<object, string>($"Cannot parse value '{value}' to type '{type.Name}'");
            }
        }

        public Result<object, string> ParseEnumeration(string value, Type type)
        {
            var errorMessage = $"Cannot parse value '{value}' to type '{type?.Name}'";

            if(type.IsNull())
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

                var enumValue = Enum.Parse(type, value.ToString(), ignoreCase: true);
                if (enumValue.IsNotNull())
                {
                    return Result.Ok<object, string>(enumValue);
                }
            }
            catch (Exception) { }

            return Result.Fail<object, string>(errorMessage);
        }
    }
}