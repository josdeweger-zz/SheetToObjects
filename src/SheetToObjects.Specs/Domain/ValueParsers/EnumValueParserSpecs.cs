using FluentAssertions;
using SheetToObjects.Core;
using SheetToObjects.Lib.ValueParsers;
using SheetToObjects.Specs.TestModels;
using Xunit;

namespace SheetToObjects.Specs.Domain.ValueParsers
{
    public class EnumValueParserSpecs
    {
        [Fact]
        public void WhenValueToParseIsNull_ResultHasDefaultValue()
        {
            var result = new EnumValueParser(typeof(EnumModel)).Parse(null);

            result.HasValue.Should().BeTrue();
            result.Value.Should().Be(default(double));
        }

        [Fact]
        public void WhenValueToParseIsEnumValue_ResultHasValue()
        {
            var enumValue = "Second";

            var result = new EnumValueParser(typeof(EnumModel)).Parse(enumValue);

            result.HasValue.Should().BeTrue();
            result.Value.Should().Be(EnumModel.Second);
        }

        [Fact]
        public void WhenValueToParseIsNotEnum_ResultHasDefaultValue()
        {
            var invalidEnumValue = "foo";

            var result = new EnumValueParser(typeof(EnumModel)).Parse(invalidEnumValue);

            result.HasValue.Should().BeTrue();
            result.Value.Should().Be(typeof(EnumModel).GetDefault());
        }
    }
}