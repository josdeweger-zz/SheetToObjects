using System.Linq;
using FluentAssertions;
using SheetToObjects.Lib.FluentConfiguration;
using SheetToObjects.Lib.Validation;
using SheetToObjects.Specs.TestModels;
using Xunit;

namespace SheetToObjects.Specs.Lib.FluentConfiguration
{
    public class MappingConfigBuilderSpecs
    {
        [Fact]
        public void GivenCreatingMappingConfiguration_WhenAddingColumnConfig_ColumnConfigIsAdded()
        {
            var result = new MappingConfigBuilder<TestModel>()
                .MapColumn(column => column
                    .WithHeader("FirstName")
                    .MapTo(m => m.StringProperty))
                .Build();

            result.ColumnMappings.OfType<NameColumnMapping>().Should().HaveCount(1);
        }

        [Fact]
        public void GivenCreatingMappingConfiguration_WhenAddingHeader_HeaderIsSetToLower()
        {
            var columnName = "FirstName";

            var result = new MappingConfigBuilder<TestModel>()
                .MapColumn(column => column
                    .WithHeader(columnName)
                    .MapTo(m => m.StringProperty))
                .Build();

            result.ColumnMappings.OfType<NameColumnMapping>().Single().ColumnName.Should().Be(columnName);
        }

        [Fact]
        public void GivenCreatingMappingConfiguration_WhenAddingProperty_PropertyNameIsSet()
        {
            var result = new MappingConfigBuilder<TestModel>()
                .MapColumn(column => column
                    .WithHeader("FirstName")
                    .MapTo(m => m.StringProperty))
                .Build();

            result.ColumnMappings.OfType<NameColumnMapping>().Single().PropertyName.Should().Be("StringProperty");
        }

        [Fact]
        public void GivenCreatingMappingConfiguration_WhenAddingRequiredRule_RuleIsAdded()
        {
            var result = new MappingConfigBuilder<TestModel>()
                .MapColumn(column => column.WithHeader("FirstName")
                    .IsRequired()
                    .MapTo(m => m.StringProperty))
                .Build();

            result.ColumnMappings.OfType<NameColumnMapping>().Single().ParsingRules.Single().Should().BeOfType<RequiredRule>();
        }

        [Fact]
        public void GivenCreatingMappingConfiguration_WhenAddingRegexRule_RuleIsAdded()
        {
            var emailRegex = "^\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$";

            var result = new MappingConfigBuilder<TestModel>()
                .MapColumn(column => column
                    .WithHeader("FirstName")
                    .Matches(emailRegex)
                    .MapTo(m => m.StringProperty))
                .Build();

            result.ColumnMappings.OfType<NameColumnMapping>().Single().Rules.Single().Should().BeOfType<RegexRule>();
        }

        [Fact]
        public void GivenCreatingMappingConfiguration_WhenDataHasHeaders_HeadersAreSetToTrue()
        {
            var emailRegex = "^\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$";

            var result = new MappingConfigBuilder<TestModel>()
                .MapColumn(column => column
                    .WithHeader("FirstName")
                    .Matches(emailRegex)
                    .MapTo(m => m.StringProperty))
                .Build();

            result.ColumnMappings.OfType<NameColumnMapping>().Single().Rules.Single().Should().BeOfType<RegexRule>();
        }
    }
}
