using FluentAssertions;
using SheetToObjects.Lib;
using SheetToObjects.Specs.TestModels;
using Xunit;

namespace SheetToObjects.Specs.Lib
{
    public class ValueParserSpecs
    {
        private readonly ValueParser _valueParser;

        public ValueParserSpecs()
        {
            _valueParser = new ValueParser();
        }

        [Fact]
        public void GivenParsingInt_WhenValueIsNotSet_ResultIsNotValid()
        {
            var result = _valueParser.Parse<int>(null);

            result.IsValid.Should().BeFalse();
            result.Message.Should().Be($"Value of type {typeof(int)} is not set.");
        }

        [Fact]
        public void GivenParsingString_WhenValueIsEmptyString_ResultIsNotValid()
        {
            var result = _valueParser.Parse<string>(string.Empty);

            result.IsValid.Should().BeFalse();
            result.Message.Should().Be($"Value of type {typeof(string)} is empty.");
        }

        [Fact]
        public void GivenParsingEnum_WhenValueCanNotBeParsed_ResultIsNotValid()
        {
            var result = _valueParser.Parse<EnumModel>("SomeString");

            result.IsValid.Should().BeFalse();
            result.Message.Should().Be($"Something went wrong parsing value of type {typeof(EnumModel)}.");
        }

        [Fact]
        public void GivenParsingDouble_WhenValueCanBeParsed_ResultIsValid()
        {
            var doubleValue = 3.3D;

            var result = _valueParser.Parse<double>(doubleValue);

            result.IsValid.Should().BeTrue();
            result.Value.Should().Be(doubleValue);
        }

        [Fact]
        public void GivenParsingString_WhenValueCanBeParsed_ResultIsValid()
        {
            var stringValue = "MyString";

            var result = _valueParser.Parse<string>(stringValue);

            result.IsValid.Should().BeTrue();
            result.Value.Should().Be(stringValue);
        }
    }
}
