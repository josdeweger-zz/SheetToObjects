using FluentAssertions;
using SheetToObjects.Lib;
using SheetToObjects.Specs.TestModels;
using Xunit;

namespace SheetToObjects.Specs.Lib
{
    public class ValueParserSpecs
    {
        private readonly CellValueParser _cellValueParser;

        public ValueParserSpecs()
        {
            _cellValueParser = new CellValueParser();
        }

        [Fact]
        public void GivenParsingInt_WhenValueIsNotSet_ResultIsNotValid()
        {
            var result = _cellValueParser.ParseValueType<int>(null,0,0,true);

            result.IsSuccess.Should().BeFalse();
            result.Error.ErrorMessage.Should().Be($"Something went wrong parsing value of type {typeof(int)}.");
        }

        [Fact]
        public void GivenParsingEnum_WhenValueCanNotBeParsed_ResultIsNotValid()
        {
            var result = _cellValueParser.ParseValueType<EnumModel>("SomeString", 1, 1,true);

            result.IsSuccess.Should().BeFalse();
            result.Error.ErrorMessage.Should().Be($"Something went wrong parsing value of type {typeof(EnumModel)}.");
        }

        [Fact]
        public void GivenParsingDouble_WhenValueCanBeParsed_ResultIsValid()
        {
            var doubleValue = 3.3D;

            var result = _cellValueParser.ParseValueType<double>(doubleValue.ToString(),1,1,true);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(doubleValue);
        }

        [Fact]
        public void GivenParsingString_WhenValueCanBeParsed_ResultIsValid()
        {
            var stringValue = "MyString";

            var result = _cellValueParser.ParseValueType<string>(stringValue,1,1,true);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(stringValue);
        }
    }
}
