using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using FluentAssertions;
using Moq;
using SheetToObjects.Lib;
using SheetToObjects.Lib.Configuration.ColumnMappings;
using SheetToObjects.Lib.Validation;
using Xunit;

namespace SheetToObjects.Specs.Lib
{
    public class ValueMapperSpecs
    {
        [Fact]
        public void GivenNotRequiredEmptyValue_WhenMapping_ItIsSetToDefaultValue()
        {
            var valueParser = new Mock<IParseValue>();

            var mapper = new ValueMapper(valueParser.Object);

            var result = mapper.Map(
                value: string.Empty, 
                propertyType: typeof(int),
                columnMapping: new PropertyColumnMapping("name", string.Empty, new List<IRule>()), 
                rowIndex: 0);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(default(int));
        }

        [Fact]
        public void GivenRequiredEmptyValue_WhenMapping_ValidationFails()
        {
            var valueParser = new Mock<IParseValue>();

            var mapper = new ValueMapper(valueParser.Object);

            var result = mapper.Map(
                value: string.Empty,
                propertyType: typeof(int),
                columnMapping: new PropertyColumnMapping("name", string.Empty, new List<IRule>{ new RequiredRule() }),
                rowIndex: 0);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void GivenValidValue_WhenParsingIsSuccesful_ItSucceeds()
        {
            var value = 50;
            var valueParser = new Mock<IParseValue>();
            valueParser.Setup(v => v.Parse(typeof(int), value.ToString(), string.Empty)).Returns(Result.Ok<object, string>(value));

            var mapper = new ValueMapper(valueParser.Object);

            var result = mapper.Map(
                value: 50.ToString(),
                propertyType: typeof(int),
                columnMapping: new PropertyColumnMapping("name", string.Empty, new List<IRule>()),
                rowIndex: 0);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(value);
        }

        [Fact]
        public void GivenInvalidValue_WhenParsingFails_FailureIsReturned()
        {
            var value = "foo";
            var valueParser = new Mock<IParseValue>();
            valueParser.Setup(v => v.Parse(typeof(int), value.ToString(), string.Empty)).Returns(Result.Fail<object, string>("Parsing failed"));

            var mapper = new ValueMapper(valueParser.Object);

            var result = mapper.Map(
                value: value,
                propertyType: typeof(int),
                columnMapping: new PropertyColumnMapping("name", string.Empty, new List<IRule>()),
                rowIndex: 0);

            result.IsFailure.Should().BeTrue();
        }
    }
}