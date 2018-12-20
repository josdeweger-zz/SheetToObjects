using FluentAssertions;
using SheetToObjects.Lib.Validation;
using Xunit;

namespace SheetToObjects.Specs.Lib.Validation
{
    public class RequiredRuleSpecs
    {
        [Fact]
        public void GivenValidatingRequiredValue_WhenValueIsNotSet_ValidationFails()
        {
            double? value = null;

            var result = new RequiredRule().Validate(value.ToString());

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("Value is required");
        }

        [Fact]
        public void GivenValidatingRequiredValue_WhenValueIsEmpty_ValidationFails()
        {
            var value = string.Empty;

            var result = new RequiredRule().Validate(value);

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("Value is required");
        }

        [Fact]
        public void GivenValidatingRequiredValueWithAllowedWhitespace_WhenValueIsWhiteSpace_ValidationFails()
        {
            var value = " ";

            var result = new RequiredRule().AllowWhiteSpace().Validate(value);

            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void GivenValidatingRequiredValueWithoutAllowedWhitespace_WhenValueIsWhiteSpace_ValidationFails()
        {
            var value = " ";

            var result = new RequiredRule().Validate(value);

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("Value is required");
        }

        [Fact]
        public void GivenValidatingRequiredValue_WhenValueIsSet_ValidationIsSuccessful()
        {
            var value = 42;

            var result = new RequiredRule().Validate(value.ToString());

            result.IsSuccess.Should().BeTrue();
        }
    }
}