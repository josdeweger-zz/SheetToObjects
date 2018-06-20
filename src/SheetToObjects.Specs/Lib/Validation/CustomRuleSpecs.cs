using FluentAssertions;
using SheetToObjects.Lib.Validation;
using Xunit;

namespace SheetToObjects.Specs.Lib.Validation
{
    public class CustomRuleSpecs
    {
        [Fact]
        public void GivenValidatingCustomRule_WhenValueIsInvalid_ValidationFails()
        {
            var value = "My String";
            var validationMessage = "The value should match 'Some Other String'";

            var rule = new CustomRule<string>(x => x.Equals("Some Other String"), validationMessage);
            var result = rule.Validate(0, 0, string.Empty, string.Empty, value);
            
            result.IsSuccess.Should().BeFalse();
            result.Error.ErrorMessage.Should().Be(validationMessage);
        }

        [Fact]
        public void GivenValidatingCustomRule_WhenValueIsValid_ValidationIsSuccesful()
        {
            var value = "My String";
            var validationMessage = "The value should match 'Some Other String'";

            var rule = new CustomRule<string>(x => x.Equals(value), validationMessage);
            var result = rule.Validate(0, 0, string.Empty, string.Empty, value);

            result.IsSuccess.Should().BeTrue();
        }
    }
}