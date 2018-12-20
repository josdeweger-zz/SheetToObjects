using CSharpFunctionalExtensions;
using FluentAssertions;
using Moq;
using SheetToObjects.Lib;
using SheetToObjects.Lib.Parsing;
using Xunit;

namespace SheetToObjects.Specs.Lib
{
    public class ValueMapperSpecs
    {
        [Fact]
        public void GivenNotRequiredEmptyValue_WhenMapping_ItIsSetToDefaultValue()
        {
            var parsingStrategyProvider = new Mock<IProvideParsingStrategy>();

            var mapper = new ValueMapper(parsingStrategyProvider.Object);

            var result = mapper.Map(
                value: string.Empty, 
                propertyType: typeof(int),
                columnIndex: 0,
                rowIndex: 0,
                displayName: string.Empty,
                propertyName: string.Empty,
                format: string.Empty,
                isRequired: false,
                defaultValue: null);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(string.Empty);
        }

        [Fact]
        public void GivenRequiredEmptyValue_WhenMapping_ValidationFails()
        {
            var parsingStrategyProvider = new Mock<IProvideParsingStrategy>();

            var mapper = new ValueMapper(parsingStrategyProvider.Object);

            var result = mapper.Map(
                value: string.Empty,
                propertyType: typeof(int),
                columnIndex: 0,
                rowIndex: 0,
                displayName: string.Empty,
                propertyName: string.Empty,
                format: string.Empty,
                isRequired: true,
                defaultValue: null);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void GivenValidValue_WhenParsingIsSuccesful_ItSucceeds()
        {
            var value = 50;
            var parsingStrategyProvider = new Mock<IProvideParsingStrategy>();
            parsingStrategyProvider.Setup(v => v.Parse(typeof(int), value.ToString(), string.Empty)).Returns(Result.Ok<object, string>(value));

            var mapper = new ValueMapper(parsingStrategyProvider.Object);

            var result = mapper.Map(
                value: value.ToString(),
                propertyType: typeof(int),
                columnIndex: 0,
                rowIndex: 0,
                displayName: string.Empty,
                propertyName: string.Empty,
                format: string.Empty,
                isRequired: false,
                defaultValue: null);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(value);
        }

        [Fact]
        public void GivenInvalidValue_WhenParsingFails_FailureIsReturned()
        {
            var value = "foo";
            var parsingStrategyProvider = new Mock<IProvideParsingStrategy>();
            parsingStrategyProvider.Setup(v => v.Parse(typeof(int), value.ToString(), string.Empty)).Returns(Result.Fail<object, string>("Parsing failed"));

            var mapper = new ValueMapper(parsingStrategyProvider.Object);

            var result = mapper.Map(
                value: value,
                propertyType: typeof(int),
                columnIndex: 0,
                rowIndex: 0,
                displayName: string.Empty,
                propertyName: string.Empty,
                format: string.Empty,
                isRequired: false,
                defaultValue: null);

            result.IsFailure.Should().BeTrue();
        }
    }
}