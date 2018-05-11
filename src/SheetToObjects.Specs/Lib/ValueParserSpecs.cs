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
            var result = _cellValueParser.ParseValueType<int>(null,0,0,true,"integer");

            result.IsSuccess.Should().BeFalse();
            result.Error.ColumnName.Should().Be("integer");
        }

        [Fact]
        public void GivenParsingEnum_WhenValueCanNotBeParsed_ResultIsNotValid()
        {
            var result = _cellValueParser.ParseValueType<EnumModel>("SomeString", 1, 1,true,"enumColumn");

            result.IsSuccess.Should().BeFalse();
            result.Error.ColumnName.Should().Be($"enumColumn");
        }

        [Fact]
        public void GivenParsingDouble_WhenValueCanBeParsed_ResultIsValid()
        {
            var doubleValue = 3.3D;

            var result = _cellValueParser.ParseValueType<double>(doubleValue.ToString(),1,1,true,"doubleColumn");

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(doubleValue);
        }

        [Fact]
        public void GivenParsingString_WhenValueCanBeParsed_ResultIsValid()
        {
            var stringValue = "MyString";

            var result = _cellValueParser.ParseValueType<string>(stringValue,1,1,true,"StringColumns");

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(stringValue);
        }
    }
}
