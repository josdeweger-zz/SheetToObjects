using FluentAssertions;
using SheetToObjects.Lib;
using SheetToObjects.Specs.TestModels;
using Xunit;

namespace SheetToObjects.Specs.Lib
{
    public class ValueParserSpecs
    {
        private readonly ValueParser _cellValueParser;

        public ValueParserSpecs()
        {
            _cellValueParser = new ValueParser();
        }

        [Fact]
        public void GivenParsingInt_WhenValueIsNotSet_ResultIsNotValid()
        {
            var result = _cellValueParser.ParseValueType<int>(null);

            result.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public void GivenParsingEnum_WhenValueCanNotBeParsed_ResultIsNotValid()
        {
            var result = _cellValueParser.ParseValueType<EnumModel>("SomeString");

            result.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public void GivenParsingDouble_WhenValueCanBeParsed_ResultIsValid()
        {
            var doubleValue = 3.3D;

            var result = _cellValueParser.ParseValueType<double>(doubleValue.ToString());

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(doubleValue);
        }

        [Fact]
        public void GivenParsingString_WhenValueCanBeParsed_ResultIsValid()
        {
            var stringValue = "MyString";

            var result = _cellValueParser.ParseValueType<string>(stringValue);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(stringValue);
        }

        [Fact]
        public void GivenParsingString_WhenValueCanBeParsedToEnum_ResultIsValid()
        {
            var value = "Second";

            var result = _cellValueParser.ParseEnumeration(value, typeof(EnumModel));

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(EnumModel.Second);
        }

        [Fact]
        public void GivenParsingStringNumber_WhenValueCanBeParsedToEnum_ResultIsValid()
        {
            var value = "2";

            var result = _cellValueParser.ParseEnumeration(value, typeof(EnumModel));

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(EnumModel.Second);
        }

        [Fact]
        public void GivenParsingStringNumber_WhenValueDoesNotExistInEnum_ResultIsInValid()
        {
            var value = "12";

            var result = _cellValueParser.ParseEnumeration(value, typeof(EnumModel));

            result.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public void GivenParsingString_WhenValueCantBeParsedToEnum_ResultIsInvalid()
        {
            var value = "notexisting";

            var result = _cellValueParser.ParseEnumeration(value, typeof(EnumModel));

            result.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public void GivenParsingString_WhenValueIsEmpty_ResultIsInvalid()
        {
            string value = null;

            var result = _cellValueParser.ParseEnumeration(value, typeof(EnumModel));

            result.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public void GivenParsingString_WhenTypeIsNull_ResultIsInvalid()
        {
            var value = "second";
            var result = _cellValueParser.ParseEnumeration(value, null);

            result.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public void GivenParsingString_WhenTypeIsNoEnum_ResultIsInvalid()
        {
            var value = "second";
            var result = _cellValueParser.ParseEnumeration(value, typeof(double));

            result.IsSuccess.Should().BeFalse();
        }


    }
}
