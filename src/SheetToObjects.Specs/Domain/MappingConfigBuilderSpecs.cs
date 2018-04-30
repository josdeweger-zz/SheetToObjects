using System.Linq;
using FluentAssertions;
using SheetToObjects.Lib.Configuration;
using SheetToObjects.Lib.Validation;
using SheetToObjects.Specs.TestModels;
using Xunit;

namespace SheetToObjects.Specs.Domain
{
    public class MappingConfigBuilderSpecs
    {
        [Fact]
        public void GivenCreatingMappingConfiguration_WhenAddingColumnConfig_ColumnConfigIsAdded()
        {
            var result = new MappingConfigBuilder()
                .For<TestModel>()
                .Column("A").MapTo(m => m.StringProperty)
                .Configure();

            result.ColumnMappings.Should().HaveCount(1);
        }

        [Fact]
        public void GivenCreatingMappingConfiguration_WhenAddingColumnLetter_ColumnLetterIsSet()
        {
            var result = new MappingConfigBuilder()
                .For<TestModel>()
                .Column("A").MapTo(m => m.StringProperty)
                .Configure();

            result.ColumnMappings.Single().ColumnLetter.Should().Be("A");
        }

        [Fact]
        public void GivenCreatingMappingConfiguration_WhenAddingProperty_PropertyTypeIsSet()
        {
            var result = new MappingConfigBuilder()
                .For<TestModel>()
                .Column("A").MapTo(m => m.StringProperty)
                .Configure();

            result.ColumnMappings.Single().PropertyType.Should().Be<string>();
        }

        [Fact]
        public void GivenCreatingMappingConfiguration_WhenAddingProperty_PropertyNameIsSet()
        {
            var result = new MappingConfigBuilder()
                .For<TestModel>()
                .Column("A").MapTo(m => m.StringProperty)
                .Configure();

            result.ColumnMappings.Single().PropertyName.Should().Be("StringProperty");
        }

        [Fact]
        public void GivenCreatingMappingConfiguration_WhenAddingRequiredRule_RuleIsAdded()
        {
            var result = new MappingConfigBuilder()
                .For<TestModel>()
                .Column("A").IsRequired().MapTo(m => m.StringProperty)
                .Configure();

            result.ColumnMappings.Single().Rules.Single().Should().BeOfType<RequiredRule>();
        }

        [Fact]
        public void GivenCreatingMappingConfiguration_WhenAddingRegexRule_RuleIsAdded()
        {
            var emailRegex = "^\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$";

            var result = new MappingConfigBuilder()
                .For<TestModel>()
                .Column("A").Matches(emailRegex).MapTo(m => m.StringProperty)
                .Configure();

            result.ColumnMappings.Single().Rules.Single().Should().BeOfType<RegexRule>();
        }
    }
}
