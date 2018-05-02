using SheetToObjects.Core;
using System;
using System.Text.RegularExpressions;

namespace SheetToObjects.Lib.Validation
{
    public class RegexRule : IRule
    {
        private readonly string _pattern;
        private readonly bool _isCaseSensitive;

        public RegexRule(string pattern, bool isCaseSensitive = false)
        {
            _pattern = pattern;
            _isCaseSensitive = isCaseSensitive;
        }

        public Result Validate(string value)
        {
            if(value.IsNullOrEmpty())
                return Result.Fail("Value is empty");

            try
            {
                if (Regex.IsMatch(value, _pattern, _isCaseSensitive ? RegexOptions.IgnoreCase : RegexOptions.None))
                {
                    return Result.Ok(value);
                }

                return Result.Fail($"Value '{value}' does not match pattern '{_pattern}'");
            }
            catch (Exception)
            {
                return Result.Fail($"Value '{value}' could not be validated by regex '{_pattern}'");
            }
        }
    }
}