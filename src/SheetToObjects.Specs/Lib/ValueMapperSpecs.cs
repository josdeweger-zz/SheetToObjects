using System.Collections.Generic;
using CSharpFunctionalExtensions;
using FluentAssertions;
using Moq;
using SheetToObjects.Lib;
using SheetToObjects.Lib.FluentConfiguration;
using SheetToObjects.Lib.Parsing;
using SheetToObjects.Lib.Validation;
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

            var columnMapping = new NameColumnMapping(string.Empty, string.Empty, string.Empty, null, null, null, false, null);

            var result = mapper.Map(
                value: string.Empty, 
                propertyType: typeof(int),
                rowIndex: 0,
                columnMapping: columnMapping);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(string.Empty);
        }

        [Fact]
        public void GivenRequiredEmptyValue_WhenMapping_ValidationFails()
        {
            var parsingStrategyProvider = new Mock<IProvideParsingStrategy>();

            var mapper = new ValueMapper(parsingStrategyProvider.Object);

            var columnMapping = new NameColumnMapping(
                string.Empty, 
                string.Empty, 
                string.Empty, 
                new List<IParsingRule> { new RequiredRule() }, 
                null, null, false, null);

            var result = mapper.Map(
                value: string.Empty, 
                propertyType: typeof(int),
                rowIndex: 0,
                columnMapping: columnMapping);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void GivenValidValue_WhenParsingIsSuccesful_ItSucceeds()
        {
            var value = 50;
            var parsingStrategyProvider = new Mock<IProvideParsingStrategy>();
            parsingStrategyProvider.Setup(v => v.Parse(typeof(int), value.ToString(), string.Empty)).Returns(Result.Ok<object, string>(value));

            var mapper = new ValueMapper(parsingStrategyProvider.Object);

            var columnMapping = new NameColumnMapping(string.Empty, string.Empty, string.Empty, null, null, null, false, null);

            var result = mapper.Map(
                value: value.ToString(),
                propertyType: typeof(int),
                rowIndex: 0,
                columnMapping: columnMapping);

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

            var columnMapping = new NameColumnMapping(string.Empty, string.Empty, string.Empty, null, null, null, false, null);

            var result = mapper.Map(
                value: value,
                propertyType: typeof(int),
                rowIndex: 0,
                columnMapping: columnMapping);

            result.IsFailure.Should().BeTrue();
        }
    }
}