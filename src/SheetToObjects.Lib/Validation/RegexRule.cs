using System;

namespace SheetToObjects.Lib.Validation
{
    public class RegexRule : IRule
    {
        private readonly string _regex;

        public RegexRule(string regex)
        {
            _regex = regex;
        }

        public ValidationResult Validate()
        {
            throw new NotImplementedException();
        }
    }
}