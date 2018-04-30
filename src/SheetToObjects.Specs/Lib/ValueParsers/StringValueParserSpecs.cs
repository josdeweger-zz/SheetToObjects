using FluentAssertions;
using SheetToObjects.Lib.ValueParsers;
using Xunit;

namespace SheetToObjects.Specs.Lib.ValueParsers
{
    public class StringValueParserSpecs
    {
        [Fact]
        public void WhenValueToParseIsNull_ResultHasDefaultValue()
        {
            var result = new StringValueParser().Parse(null);

            result.HasValue.Should().BeTrue();
            result.Value.Should().Be(string.Empty);
        }

        [Fact]
        public void WhenValueToParseIsString_ResultHasValue()
        {
            var value = "My beatiful string";

            var result = new StringValueParser().Parse(value);

            result.HasValue.Should().BeTrue();
            result.Value.Should().Be(value);
        }
    }
}