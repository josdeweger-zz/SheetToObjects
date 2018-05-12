using System.Linq;
using FluentAssertions;
using SheetToObjects.Lib.Configuration;
using SheetToObjects.Lib.Configuration.ColumnMappings;
using SheetToObjects.Lib.Validation;
using SheetToObjects.Specs.TestModels;
using Xunit;

namespace SheetToObjects.Specs.Lib
{
    public class MappingConfigBuilderSpecs
    {
        [Fact]
        public void GivenCreatingMappingConfiguration_WhenAddingColumnConfig_ColumnConfigIsAdded()
        {
            var result = new MappingConfigBuilder<TestModel>()
                .Columns(columns => columns
                    .Add(column => column.WithHeader("FirstName").MapTo(m => m.StringProperty))).Object();

            result.ColumnMappings.OfType<NameColumnMapping>().Should().HaveCount(1);
        }

        [Fact]
        public void GivenCreatingMappingConfiguration_WhenAddingHeader_HeaderIsSetToLower()
        {
            var result = new MappingConfigBuilder<TestModel>()
                .Columns(columns => columns
                    .Add(column => column.WithHeader("FirstName").MapTo(m => m.StringProperty))).Object();

            result.ColumnMappings.OfType<NameColumnMapping>().Single().ColumnName.Should().Be("firstname");
        }

        [Fact]
        public void GivenCreatingMappingConfiguration_WhenAddingProperty_PropertyNameIsSet()
        {
            var result = new MappingConfigBuilder<TestModel>()
                .Columns(columns => columns
                    .Add(column => column.WithHeader("FirstName").MapTo(m => m.StringProperty))).Object();

            result.ColumnMappings.OfType<NameColumnMapping>().Single().PropertyName.Should().Be("StringProperty");
        }

        [Fact]
        public void GivenCreatingMappingConfiguration_WhenAddingRequiredRule_RuleIsAdded()
        {
            var result = new MappingConfigBuilder<TestModel>()
                .Columns(columns => columns
                    .Add(column => column.WithHeader("FirstName").IsRequired().MapTo(m => m.StringProperty))).Object();

            result.ColumnMappings.OfType<NameColumnMapping>().Single().Rules.Single().Should().BeOfType<RequiredRule>();
        }

        [Fact]
        public void GivenCreatingMappingConfiguration_WhenAddingRegexRule_RuleIsAdded()
        {
            var emailRegex = "^\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$";

            var result = new MappingConfigBuilder<TestModel>()
                .Columns(columns => columns
                    .Add(column => column.WithHeader("FirstName").Matches(emailRegex).MapTo(m => m.StringProperty))).Object();

            result.ColumnMappings.OfType<NameColumnMapping>().Single().Rules.Single().Should().BeOfType<RegexRule>();
        }

        [Fact]
        public void GivenCreatingMappingConfiguration_WhenDataHasHeaders_HeadersAreSetToTrue()
        {
            var emailRegex = "^\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$";

            var result = new MappingConfigBuilder<TestModel>()
                .Columns(columns => columns
                    .Add(column => column.WithHeader("FirstName").Matches(emailRegex).MapTo(m => m.StringProperty))).Object();

            result.ColumnMappings.OfType<NameColumnMapping>().Single().Rules.Single().Should().BeOfType<RegexRule>();
        }

        [Fact]
        void GivenAModelWithAttributes_AttributesAreSetInConfig()
        {
            var result = new MappingConfigBuilder<AttributeTestModel>().Object();

            result.HasHeaders.Should().BeTrue();
            result.ColumnMappings.Single().Should().BeOfType<IndexColumnMapping>()
                .Which.ColumnIndex.Value.Should().Be(3);

            result.ColumnMappings.Single().Rules.OfType<RequiredRule>().Should().NotBeNull();
            result.ColumnMappings.Single().Rules.OfType<RegexRule>().Should().NotBeNull();
        }

        [Fact]
        void GivenAModelWithMappingByLetterAttribute_LetterAttributeIsSetInConfig()
        {
            var result = new MappingConfigBuilder<LetterAttributeTestModel>().Object();

            result.ColumnMappings.Single().Should().BeOfType<LetterColumnMapping>()
                .Which.ColumnIndex.Value.Should().Be(2);
        }

        [Fact]
        void GivenAModelWithMappingByNameAttributeAndRequiredSettingWithWhitespace_NameAttributeIsSetInConfig()
        {
            var result = new MappingConfigBuilder<ColumnNameAttributeTestModel>().Object();

            result.ColumnMappings.Single().Should().BeOfType<NameColumnMapping>()
                .Which.ColumnName.Should().Be("stringcolumn");

            result.ColumnMappings.Single().Rules.OfType<RequiredRule>().Single().WhiteSpaceAllowed.Should().BeTrue();

        }


    }
}
