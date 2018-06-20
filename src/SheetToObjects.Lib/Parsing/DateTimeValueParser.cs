using System;
using System.Globalization;
using CSharpFunctionalExtensions;

namespace SheetToObjects.Lib.Parsing
{
    internal class DateTimeValueParser : IValueParsingStrategy
    {
        private readonly string _format;

        public DateTimeValueParser(string format)
        {
            _format = format;
        }

        public Result<object, string> Parse(string value)
        {
            var errorMessage = $"Cannot parse value '{value}' to DateTime using format '{_format}'";

            try
            {
                var parsedDateTime = DateTime.ParseExact(value, _format, CultureInfo.InvariantCulture);
                return Result.Ok<object, string>(parsedDateTime);
            }
            catch (Exception)
            {
                return Result.Fail<object, string>(errorMessage);
            }
        }
    }
}