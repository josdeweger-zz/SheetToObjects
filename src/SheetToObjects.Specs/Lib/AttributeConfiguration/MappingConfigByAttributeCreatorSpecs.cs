﻿using System.Linq;
using FluentAssertions;
using SheetToObjects.Core;
using SheetToObjects.Lib.FluentConfiguration;
using SheetToObjects.Lib.Validation;
using SheetToObjects.Specs.TestModels;
using Xunit;

namespace SheetToObjects.Specs.Lib.AttributeConfiguration
{
    public class MappingConfigByAttributeCreatorSpecs
    {
        [Fact]
        void WhenCreatingFromModelWithoutSheetToObjectConfigAttribute_ItFailsToCreateMappingConfig()
        {
            var result = new MappingConfigByAttributeCreator<WithoutSheetToObjectConfigModel>().CreateMappingConfig();

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        void WhenCreatingFromModelWithAttributes_AttributesAreSetInConfig()
        {
            var result = new MappingConfigByAttributeCreator<AttributeTestModel>().CreateMappingConfig();

            var columnMapping = result.Value.ColumnMappings.Single(c => c.ColumnIndex.Equals(3));

            columnMapping.Should().BeOfType<IndexColumnMapping>();
            columnMapping.Rules.OfType<RequiredRule>().Should().NotBeNull();
            columnMapping.Rules.OfType<RegexRule>().Should().NotBeNull();
        }

        [Fact]
        void WhenCreatingFromModelWithMappingByLetterAttribute_LetterAttributeIsSetInConfig()
        {
            var result = new MappingConfigByAttributeCreator<LetterAttributeTestModel>().CreateMappingConfig();

            result.Value.ColumnMappings.Single().Should().BeOfType<LetterColumnMapping>()
                .Which.ColumnIndex.Should().Be(2);
        }

        [Fact]
        void WhenCreatingFromModelWithMappingByNameAttributeAndRequiredSettingWithWhitespace_NameAttributeIsSetInConfig()
        {
            var columnName = "StringColumn";

            var result = new MappingConfigByAttributeCreator<ColumnNameAttributeTestModel>().CreateMappingConfig();

            result.Value.ColumnMappings.Single().Should().BeOfType<NameColumnMapping>()
                .Which.ColumnName.Should().Be(columnName);

            result.Value.ColumnMappings.Single().ParsingRules.OfType<RequiredRule>().Single().WhiteSpaceAllowed.Should().BeTrue();
        }

        [Fact]
        void WhenCreatingFromModelWithoutAttributes_PropertyIsAutoMapped()
        {
            var result = new MappingConfigByAttributeCreator<AutoMapTestModel>().CreateMappingConfig();

            result.Value.HasHeaders.Should().BeTrue();
            result.Value.ColumnMappings.Should().Contain(c =>
                ((PropertyColumnMapping) c).ColumnName == nameof(AutoMapTestModel.Emailaddress));
        }

        [Fact]
        void WhenCreatingDateTimeWithFormat_FormatIsSet()
        {
            var result = new MappingConfigByAttributeCreator<AttributeTestModel>().CreateMappingConfig();
            
            result.Value.ColumnMappings.Should().Contain(c => c.Format.IsNotNull() && c.Format.Equals("yyyy-MM-dd"));
        }

        [Fact]
        void WhenCreatingDateTimeWithDefaultValue_DefaultValueIsSet()
        {
            var defaultValue = "Default String Value";

            var result = new MappingConfigByAttributeCreator<AttributeTestModel>().CreateMappingConfig();

            var columnMapping = result.Value.ColumnMappings.Single(c => c.ColumnIndex.Equals(3));

            columnMapping.DefaultValue.Should().Be(defaultValue);
        }

        [Fact]
        void WhenPropertyIsRequiredInHeaderRow_RequiredInHeaderRowIsSet()
        {
            var columnName = "RequiredInHeaderProperty";

            var result = new MappingConfigByAttributeCreator<AttributeTestModel>().CreateMappingConfig();

            var columnMapping = result.Value.ColumnMappings.Single(c => c.PropertyName.Equals(columnName)) as IUseHeaderRow;

            columnMapping.IsRequiredInHeaderRow.Should().BeTrue();
        }

        [Fact]
        void WhenPropertyShouldBeUniqueInColumn_UniqueInColumnRuleIsSet()
        {
            var columnName = "UniqueInColumnProperty";

            var result = new MappingConfigByAttributeCreator<AttributeTestModel>().CreateMappingConfig();

            var columnMapping = result.Value.ColumnMappings.Single(c => c.PropertyName.Equals(columnName));

            columnMapping.Rules.OfType<UniqueValuesInColumnRule>().Should().NotBeNull();
        }
    }
}