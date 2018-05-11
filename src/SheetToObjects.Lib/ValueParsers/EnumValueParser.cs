using System;
using System.Globalization;
using SheetToObjects.Core;

namespace SheetToObjects.Lib
{
    public class EnumValueParser : IParseValues
    {
        public Result Parse<T>(object value)
        {
            var type = typeof(T);


            if (int.TryParse(value.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var intValue))
            {
                if (Enum.IsDefined(type, intValue))
                {
                    return Result.Ok((T) Enum.ToObject(type, intValue));
                }

                return Result.Fail($"Enumeration value '{value}' not defined fo type {type}.");
            }

            return Result.Ok((T) Enum.Parse(type, value.ToString(), true));
        }
    }
}