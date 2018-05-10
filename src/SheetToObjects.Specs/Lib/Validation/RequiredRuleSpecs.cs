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
            string value = string.Empty;

            var result = new RequiredRule().Validate(value);

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("Value is required");
        }

        [Fact]
        public void GivenValidatingRequiredValue_WhenValueIsSet_ValidationIsSuccessful()
        {
            int value = 42;

            var result = new RequiredRule().Validate(value.ToString());

            result.IsSuccess.Should().BeTrue();
        }
    }
}