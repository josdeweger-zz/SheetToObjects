using FluentAssertions;
using SheetToObjects.Lib.ValueParsers;
using Xunit;

namespace SheetToObjects.Specs.Lib.ValueParsers
{
    public class DoubleValueParserSpecs
    {
        [Fact]
        public void WhenValueToParseIsNull_ResultHasDefaultValue()
        {
            var result = new DoubleValueParser().Parse(null);

            result.HasValue.Should().BeTrue();
            result.Value.Should().Be(default(double));
        }

        [Fact]
        public void WhenValueToParseIsDouble_ResultHasValue()
        {
            var doubleValue = 5.5D;

            var result = new DoubleValueParser().Parse(doubleValue);

            result.HasValue.Should().BeTrue();
            result.Value.Should().Be(doubleValue);
        }

        [Fact]
        public void WhenValueToParseIsNotDouble_ResultHasDefaultValue()
        {
            var doubleValue = "foo";

            var result = new DoubleValueParser().Parse(doubleValue);

            result.HasValue.Should().BeTrue();
            result.Value.Should().Be(default(double));
        }
    }
}