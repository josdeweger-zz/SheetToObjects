using System;
using System.Globalization;
using CSharpFunctionalExtensions;
using SheetToObjects.Core;

namespace SheetToObjects.Lib
{
    public class CellValueParser : IParseValue
    {
        public Result<object, ValidationError> ParseValueType<TValue>(string value, int columnIndex, int rowIndex, bool isRequired)
        {
            var type = typeof(TValue);
            
            try
            {
                var parsedValue = (TValue) Convert.ChangeType(value, type);
                return Result.Ok<object, ValidationError>(parsedValue);
            }
            catch (Exception)
            {
                if (isRequired)
                {
                    return Result.Fail<object, ValidationError>(new ValidationError(columnIndex, rowIndex,
                        $"Something went wrong parsing value of type {type}."));
                }

                return Result.Ok<object, ValidationError>(default(TValue));
            }
        }

        public Result<object, ValidationError> ParseEnumeration(string value, int columnIndex, int rowIndex, Type type)
        {
            if (!type.IsEnum)
                return Result.Fail<object, ValidationError>(new ValidationError(columnIndex, rowIndex,
                    $"Type {type.Name} is not an Enumeration"));

            if (value.IsNull())
                return Result.Fail<object, ValidationError>(new ValidationError(-1, -1,
                    $"Cell or cell value is not set for column index -1 and row index -1"));

            if (value.IsNull())
                return Result.Fail<object, ValidationError>(new ValidationError(columnIndex, rowIndex,
                    $"Cell or cell value is not set for column index {columnIndex} and row index {rowIndex}"));

            try
            {
                if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var intValue))
                {
                    if (type.IsEnumDefined(intValue))
                    {
                        return Result.Ok<object, ValidationError>(intValue);
                    }
                    return Result.Fail<object, ValidationError>(new ValidationError(columnIndex, rowIndex, $"Could not parse value to {type}"));
                }
                else
                {
                    var enumValue = Enum.Parse(type, value.ToString(), ignoreCase: true);
                    if (enumValue.IsNotNull())
                    {
                        return Result.Ok<object, ValidationError>(enumValue);
                    }
                }
            }
            catch (Exception)
            {
                return Result.Fail<object, ValidationError>(new ValidationError(columnIndex, rowIndex, $"Could not parse value to {type}"));
            }

            return Result.Fail<object, ValidationError>(new ValidationError(columnIndex, rowIndex, $"Could not parse value to {type}"));
        }
    }
}