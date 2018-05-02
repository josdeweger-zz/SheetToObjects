using FluentAssertions;
using SheetToObjects.Lib.Validation;
using Xunit;

namespace SheetToObjects.Specs.Lib.Validation
{
    public class RegexRuleSpecs
    {
        [Fact]
        public void GivenValidatingRegex_WhenValueIsInvalid_ValidationFails()
        {
            var value = "invalidemail@";
            var pattern =
                @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";

            var result = new RegexRule(pattern).Validate(value);

            result.IsValid.Should().BeFalse();
            result.Message.Should().Be($"Value '{value}' does not match pattern '{pattern}'");
        }
    }
}