using System;
using CSharpFunctionalExtensions;

namespace SheetToObjects.Lib.Parsing
{
    internal class ObjectValueParser : IValueParsingStrategy
    {
        private readonly Type _type;

        public ObjectValueParser(Type type)
        {
            _type = type;
        }

        public Result<object, string> Parse(string value)
        {
            try
            {
                var parsedValue = Convert.ChangeType(value, _type);
                return Result.Ok<object, string>(parsedValue);
            }
            catch (Exception)
            {
                return Result.Fail<object, string>($"Cannot parse value '{value}' to type '{_type.Name}'");
            }
        }
    }
}