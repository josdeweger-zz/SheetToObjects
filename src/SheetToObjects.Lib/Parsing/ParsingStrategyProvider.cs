using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using SheetToObjects.Core;

namespace SheetToObjects.Lib.Parsing
{
    internal class ParsingStrategyProvider : IProvideParsingStrategy
    {
        private readonly IDictionary<Type, Func<string, string, Result<object, string>>> _parsers;

        public ParsingStrategyProvider()
        {
            _parsers = new Dictionary<Type, Func<string, string, Result<object, string>>>
            {
                {typeof(string), (value, format) => new ObjectValueParser(typeof(string)).Parse(value)},
                {typeof(int), (value, format) => new ObjectValueParser(typeof(int)).Parse(value)},
                {typeof(int?), (value, format) => new ObjectValueParser(typeof(int?)).Parse(value)},
                {typeof(float), (value, format) => new ObjectValueParser(typeof(float)).Parse(value)},
                {typeof(float?), (value, format) => new ObjectValueParser(typeof(float?)).Parse(value)},
                {typeof(double), (value, format) => new ObjectValueParser(typeof(double)).Parse(value)},
                {typeof(double?), (value, format) => new ObjectValueParser(typeof(double?)).Parse(value)},
                {typeof(decimal), (value, format) => new ObjectValueParser(typeof(decimal)).Parse(value)},
                {typeof(decimal?), (value, format) => new ObjectValueParser(typeof(decimal?)).Parse(value)},
                {typeof(bool), (value, format) => new ObjectValueParser(typeof(bool)).Parse(value)},
                {typeof(bool?), (value, format) => new ObjectValueParser(typeof(bool?)).Parse(value)},
                {typeof(DateTime), (value, format) => new DateTimeValueParser(format).Parse(value)},
                {typeof(DateTime?), (value, format) => new DateTimeValueParser(format).Parse(value)}
            };
        }

        public Result<object, string> Parse(Type type, string value, string format = "")
        {
            if (type.IsNull())
                return Result.Fail<object, string>($"Can not parse value for unspecified type");
            
            if (type.IsEnum)
                return new EnumValueParser(type).Parse(value);
            
            if(_parsers.TryGetValue(type, out var parser))
                return parser(value, format);

            return Result.Fail<object, string>($"Parser for '{type.Name}' not implemented");
        }
    }
}