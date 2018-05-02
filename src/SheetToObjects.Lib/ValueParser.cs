using System;
using SheetToObjects.Core;

namespace SheetToObjects.Lib
{
    public class ValueParser : IParseValues
    {
        public Result Parse<T>(object value)
        {
            var type = typeof(T);

            if (value.IsNull())
                return Result.Fail($"Value of type {type} is not set.");

            if (type == typeof(string) && ((string)value).IsNullOrEmpty())
                return Result.Fail($"Value of type {type} is empty.");

            try
            {
                if (type.IsEnum)
                {
                    return Result.Ok((T)Enum.Parse(type, value.ToString(), true));
                }

                return Result.Ok((T)Convert.ChangeType(value, type));
            }
            catch (Exception)
            {
                return Result.Fail($"Something went wrong parsing value of type {type}.");
            }
        }
    }
}