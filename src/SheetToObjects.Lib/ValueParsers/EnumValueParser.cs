using System;
using SheetToObjects.Core;
using SheetToObjects.Lib.Validation;

namespace SheetToObjects.Lib.ValueParsers
{
    public class EnumValueParser : IParseValueStrategy
    {
        private readonly Type _enumType;
        private readonly object _defaultValue;

        public EnumValueParser(Type enumType)
        {
            _enumType = enumType;
            _defaultValue = _enumType.GetDefault();
        }

        public Result Parse(object value)
        {
            if (value.IsNull())
                return Result.From(_defaultValue);

            try
            {
                var enumValue = Enum.Parse(_enumType, value.ToString());
                
                return Result.From(enumValue);
            }
            catch (Exception)
            {
                return Result.From(_defaultValue);
            }
        }
    }
}