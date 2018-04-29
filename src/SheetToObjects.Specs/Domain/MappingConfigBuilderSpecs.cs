using System.Linq;
using FluentAssertions;
using SheetToObjects.Lib.Configuration;
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
                .Build();

            result.ColumnMappings.Should().HaveCount(1);
        }

        [Fact]
        public void GivenCreatingMappingConfiguration_WhenAddingColumnLetter_ColumnLetterIsSet()
        {
            var result = new MappingConfigBuilder()
                .For<TestModel>()
                .Column("A").MapTo(m => m.StringProperty)
                .Build();

            result.ColumnMappings.Single().ColumnLetter.Should().Be("A");
        }

        [Fact]
        public void GivenCreatingMappingConfiguration_WhenAddingProperty_PropertyTypeIsSet()
        {
            var result = new MappingConfigBuilder()
                .For<TestModel>()
                .Column("A").MapTo(m => m.StringProperty)
                .Build();

            result.ColumnMappings.Single().PropertyType.Should().Be<string>();
        }

        [Fact]
        public void GivenCreatingMappingConfiguration_WhenAddingProperty_PropertyNameIsSet()
        {
            var result = new MappingConfigBuilder()
                .For<TestModel>()
                .Column("A").MapTo(m => m.StringProperty)
                .Build();

            result.ColumnMappings.Single().PropertyName.Should().Be("StringProperty");
        }

        [Fact]
        public void GivenCreatingMappingConfiguration_WhenColumnIsNotSetToRequired_ColumnIsNotRequired()
        {
            var result = new MappingConfigBuilder()
                .For<TestModel>()
                .Column("A").MapTo(m => m.StringProperty)
                .Build();

            result.ColumnMappings.Single().Required.Should().BeFalse();
        }

        [Fact]
        public void GivenCreatingMappingConfiguration_WhenColumnIsSetToRequired_ColumnIsRequired()
        {
            var result = new MappingConfigBuilder()
                .For<TestModel>()
                .Column("A").IsRequired().MapTo(m => m.StringProperty)
                .Build();

            result.ColumnMappings.Single().Required.Should().BeTrue();
        }
    }
}
