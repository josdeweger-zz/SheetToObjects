using FluentAssertions;
using SheetToObjects.Lib.ValueParsers;
using Xunit;

namespace SheetToObjects.Specs.Lib.ValueParsers
{
    public class IntValueParserSpecs
    {
        [Fact]
        public void WhenValueToParseIsNull_ResultHasDefaultValue()
        {
            var result = new IntValueParser().Parse(null);

            result.HasValue.Should().BeTrue();
            result.Value.Should().Be(default(int));
        }

        [Fact]
        public void WhenValueToParseIsInt_ResultHasValue()
        {
            var value = "2";

            var result = new IntValueParser().Parse(value);

            result.HasValue.Should().BeTrue();
            result.Value.Should().Be(2);
        }

        [Fact]
        public void WhenValueToParseIsNotInt_ResultHasDefaultValue()
        {
            var value = "foo";

            var result = new IntValueParser().Parse(value);

            result.HasValue.Should().BeTrue();
            result.Value.Should().Be(default(int));
        }
    }
}