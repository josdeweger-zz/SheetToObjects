using System;
using CSharpFunctionalExtensions;
using SheetToObjects.Core;

namespace SheetToObjects.Lib
{
    public class ValueParser : IParseValues
    {
        public Result<T> Parse<T>(object value, IFormatProvider formatProvider = null)
        {
            var type = typeof(T);

            if (value.IsNull())
                return Result.Fail<T>($"Value of type {type} is not set.");

            if (type == typeof(string) && ((string)value).IsNullOrEmpty())
                return Result.Fail<T>($"Value of type {type} is empty.");

            try
            {
                return Result.Ok((T)Convert.ChangeType(value, type, formatProvider));
            }
            catch (Exception)
            {
                return Result.Fail<T>($"Something went wrong parsing value of type {type}.");
            }
        }

        public Result<object> Parse(object value, Type type)
        {
            try
            {
                var enumValue = Enum.Parse(type, value.ToString(), ignoreCase: true);
                if (enumValue.IsNotNull())
                {
                    return Result.Ok(enumValue);
                }
            }
            catch (Exception)
            {
                return Result.Fail<object>($"Could not parse value to {type}");
            }

            return Result.Fail<object>($"Could not parse value to {type}");
        }
    }
}