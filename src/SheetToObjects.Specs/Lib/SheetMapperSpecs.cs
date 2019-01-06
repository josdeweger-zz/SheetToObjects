using System;
using System.Linq;
using FluentAssertions;
using SheetToObjects.Lib;
using SheetToObjects.Lib.FluentConfiguration;
using SheetToObjects.Specs.Builders;
using SheetToObjects.Specs.TestModels;
using Xunit;

namespace SheetToObjects.Specs.Lib
{
    public class SheetMapperSpecs
    {
        private readonly Sheet _sheetData;
        private readonly string _stringValue = "foo";
        private readonly double _doubleValue = 42.42D;
        private readonly int _intValue = 42;
        private readonly bool _boolValue = true;
        private readonly DateTime _dateTimeValue = new DateTime(2018, 5, 30);
        private readonly EnumModel _enumValue = EnumModel.Second;
        private readonly decimal _decimalValue = 5.123m;

        public SheetMapperSpecs()
        {
            _sheetData = new SheetBuilder()
                .AddHeaders("Double", "Integer", "Boolean", "Enumeration", "String", "DateTime", "Decimal")
                .AddRow(r => r
                    .AddCell(c => c.WithColumnIndex(0).WithRowIndex(1).WithValue(_doubleValue.ToString()).Build())
                    .AddCell(c => c.WithColumnIndex(1).WithRowIndex(1).WithValue(_intValue.ToString()).Build())
                    .AddCell(c => c.WithColumnIndex(2).WithRowIndex(1).WithValue(_boolValue.ToString()).Build())
                    .AddCell(c => c.WithColumnIndex(3).WithRowIndex(1).WithValue(_enumValue.ToString()).Build())
                    .AddCell(c => c.WithColumnIndex(4).WithRowIndex(1).WithValue(_stringValue.ToString()).Build())
                    .AddCell(c => c.WithColumnIndex(5).WithRowIndex(1).WithValue(_dateTimeValue.ToString("yyyy-MM-dd")).Build())
                    .AddCell(c => c.WithColumnIndex(6).WithRowIndex(1).WithValue(_decimalValue.ToString()).Build())
                    .Build(0))
                .Build();
        }

        [Fact]
        public void GivenModel_WhenNoMappingIsConfigured_ItThrows()
        {
            Action result = () => new SheetMapper()
                .Map<WithoutSheetToObjectConfigModel>(_sheetData);

            result.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void GivenSheet_WhenMappingModelDoubleProperty_ItSetsPropertyOnModel()
        {
            var result = new SheetMapper()
                .AddConfigFor<TestModel>(cfg => cfg
                    .HasHeaders()
                    .MapColumn(column => column.WithHeader("Double").IsRequired().MapTo(t => t.DoubleProperty)))
                .Map<TestModel>(_sheetData);

            result.ParsedModels.Should().HaveCount(1);
            result.ParsedModels.Single().Value.DoubleProperty.Should().Be(_doubleValue);
        }

        [Fact]
        public void GivenSheet_WhenMappingModelIntProperty_ItSetsPropertyOnModel()
        {
            var result = new SheetMapper()
                .AddConfigFor<TestModel>(cfg => cfg
                    .HasHeaders()
                    .MapColumn(column => column.WithHeader("Integer").IsRequired().MapTo(t => t.IntProperty)))
                .Map<TestModel>(_sheetData);

            result.ParsedModels.Should().HaveCount(1);
            result.ParsedModels.Single().Value.IntProperty.Should().Be(_intValue);
        }

        [Fact]
        public void GivenSheet_WhenMappingModelBoolProperty_ItSetsPropertyOnModel()
        {
            var result = new SheetMapper()
                .AddConfigFor<TestModel>(cfg => cfg
                    .HasHeaders()
                    .MapColumn(column => column.WithHeader("Boolean").IsRequired().MapTo(t => t.BoolProperty)))
                .Map<TestModel>(_sheetData);

            result.ParsedModels.Should().HaveCount(1);
            result.ParsedModels.Single().Value.BoolProperty.Should().Be(_boolValue);
        }

        [Fact]
        public void GivenSheet_WhenMappingModelEnumProperty_ItSetsPropertyOnModel()
        {
            var result = new SheetMapper()
                .AddConfigFor<TestModel>(cfg => cfg
                    .HasHeaders()
                    .MapColumn(column => column.WithHeader("Enumeration").IsRequired().MapTo(t => t.EnumProperty)))
                .Map<TestModel>(_sheetData);

            result.ParsedModels.Should().HaveCount(1);
            result.ParsedModels.Single().Value.EnumProperty.Should().Be(_enumValue);
        }

        [Fact]
        public void GivenSheet_WhenMappingModelDateTimeProperty_ItSetsPropertyOnModel()
        {
            var result = new SheetMapper()
                .AddConfigFor<TestModel>(cfg => cfg
                    .HasHeaders()
                    .MapColumn(column => column
                            .WithHeader("DateTime")
                            .UsingFormat("yyyy-MM-dd")
                            .IsRequired()
                            .MapTo(t => t.DateTimeProperty)))
                .Map<TestModel>(_sheetData);

            result.ParsedModels.Should().HaveCount(1);
            result.ParsedModels.Single().Value.DateTimeProperty.Should().Be(_dateTimeValue);
        }

        [Fact]
        public void GivenSheet_WhenMappingModelStringProperty_ItSetsPropertyOnModel()
        {
            var result = new SheetMapper()
                .AddConfigFor<TestModel>(cfg => cfg
                    .HasHeaders()
                    .MapColumn(column => column
                        .WithHeader("String")
                        .MapTo(t => t.StringProperty)))
                .Map<TestModel>(_sheetData);

            result.ParsedModels.Should().HaveCount(1);
            result.ParsedModels.Single().Value.StringProperty.Should().Be(_stringValue);
        }

        [Fact]
        public void GivenSheet_WhenMappingModelStringPropertyWithRegexValidation_ShouldThrowValidationError()
        {
            var columnName = "String";

            var result = new SheetMapper()
                .AddConfigFor<TestModel>(cfg => cfg
                    .HasHeaders()
                    .MapColumn(column => column
                        .WithHeader(columnName)
                        .Matches("some_invalid_pattern")
                        .MapTo(t => t.StringProperty)))
                .Map<TestModel>(_sheetData);

            result.IsFailure.Should().BeTrue();
            result.ParsedModels.Should().HaveCount(0);
            result.ValidationErrors.Single().ColumnName.Should().Be(columnName);
        }

        [Fact]
        public void GivenSheet_WhenMappingRequiredModelPropertyToInvalidType_ItShouldSetDefaultValue()
        {
            var columnName = "String";

            var result = new SheetMapper()
                .AddConfigFor<TestModel>(cfg => cfg
                    .HasHeaders()
                    .MapColumn(column => column
                        .WithHeader(columnName)
                        .IsRequired()
                        .MapTo(t => t.DoubleProperty)))
                .Map<TestModel>(_sheetData);

            result.IsFailure.Should().BeTrue();
            result.ParsedModels.Should().HaveCount(0);
            result.ValidationErrors.Single().ColumnName.Should().Be(columnName);
        }

        [Fact]
        public void GivenSheet_WhenMappingModeltoAPropertyWithBody_ItShouldThrowException()
        {
            Action result = () => new SheetMapper()
                .AddConfigFor<TestModel>(cfg => cfg
                    .HasHeaders()
                    .MapColumn(column => column
                        .WithHeader("String")
                        .MapTo(t => t.PropertyWithBody)))
                .Map<TestModel>(_sheetData);

            result.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void GivenSheetMapper_WhenCreatingTwoConfigs_TheyBothExist()
        {
            var sheetDataModelOne = new SheetBuilder()
                .AddHeaders("ModelOnePropertyOne")
                .AddRow(r => r
                    .AddCell(c => c.WithColumnIndex(0).WithRowIndex(0).WithValue("SomeValue").Build())
                    .Build(1))
                .Build();

            var sheetDataModelTwo = new SheetBuilder()
                .AddHeaders("ModelTwoPropertyOne")
                .AddRow(r => r
                    .AddCell(c => c.WithColumnIndex(0).WithRowIndex(0).WithValue("1").Build())
                    .Build(1))
                .Build();

            var sheetMapper = new SheetMapper()
                .AddConfigFor<ModelOne>(cfg => cfg
                    .HasHeaders()
                    .MapColumn(column => column.WithHeader("ModelOnePropertyOne").IsRequired().MapTo(t => t.ModelOnePropertyOne)))
                .AddConfigFor<ModelTwo>(cfg => cfg
                    .HasHeaders()
                    .MapColumn(column => column.IsRequired().MapTo(t => t.ModelTwoPropertyOne)));

            var resultModelOne = sheetMapper.Map<ModelOne>(sheetDataModelOne);
            var resultModelTwo = sheetMapper.Map<ModelTwo>(sheetDataModelTwo);

            resultModelOne.IsSuccess.Should().BeTrue();
            resultModelOne.ParsedModels.Single().Value.ModelOnePropertyOne.Should().Be("SomeValue");

            resultModelTwo.IsSuccess.Should().BeTrue();
            resultModelTwo.ParsedModels.Single().Value.ModelTwoPropertyOne.Should().Be(1);
        }

        [Fact]
        public void GivenSheet_WhenMappingEmptyNonNullableValueTypeWhichIsNotRequired_ItShouldSetDefaultValue()
        {
            var sheetData = new SheetBuilder()
                .AddHeaders("Age")
                .AddRow(r => r
                    .AddCell(c => c.WithColumnIndex(0).WithRowIndex(1).WithValue("").Build())
                    .Build(1))
                .Build();

            var defaultAge = 8;

            var result = new SheetMapper()
                .AddConfigFor<TestModel>(cfg => cfg
                    .HasHeaders()
                    .MapColumn(column => column
                        .WithHeader("Age")
                        .WithDefaultValue(defaultAge)
                        .MapTo(t => t.IntProperty)))
                .Map<TestModel>(sheetData);

            result.IsSuccess.Should().BeTrue();
            result.ParsedModels.Single().Value.IntProperty.Should().Be(defaultAge);
        }

        [Fact]
        public void GivenSheet_WhenMappingEmptyStringWhichIsNotRequired_ItShouldSetDefaultValue()
        {
            var sheetData = new SheetBuilder()
                .AddHeaders("Label")
                .AddRow(r => r
                    .AddCell(c => c.WithColumnIndex(0).WithRowIndex(1).WithValue("").Build())
                    .Build(1))
                .Build();

            var defaultLabel = "Common";

            var result = new SheetMapper()
                .AddConfigFor<TestModel>(cfg => cfg
                    .HasHeaders()
                    .MapColumn(column => column
                        .WithHeader("Label")
                        .WithDefaultValue(defaultLabel)
                        .MapTo(t => t.StringProperty)))
                .Map<TestModel>(sheetData);

            result.IsSuccess.Should().BeTrue();
            result.ParsedModels.Single().Value.StringProperty.Should().Be(defaultLabel);
        }

        [Fact]
        public void GivenColumnWithUniqueValidation_WhenAllValuesAreUnique_ItIsValid()
        {
            var sheetData = new SheetBuilder()
                .AddHeaders("FirstColumn")
                .AddRow(r => r.AddCell(c => c.WithColumnIndex(0).WithRowIndex(1).WithValue("First Value").Build()).Build(1))
                .AddRow(r => r.AddCell(c => c.WithColumnIndex(0).WithRowIndex(2).WithValue("Second Value").Build()).Build(2))
                .Build();

            var result = new SheetMapper()
                .AddConfigFor<TestModel>(cfg => cfg
                    .HasHeaders()
                    .MapColumn(column => column
                        .WithHeader("FirstColumn")
                        .ShouldHaveUniqueValues()
                        .MapTo(t => t.StringProperty)))
                .Map<TestModel>(sheetData);

            result.IsSuccess.Should().BeTrue();
            result.ParsedModels.Should().HaveCount(2);
        }

        [Fact]
        public void GivenColumnWithUniqueValidation_WhenOneValueExistsTwice_ItIsNotValid()
        {
            var sheetData = new SheetBuilder()
                .AddHeaders("FirstColumn")
                .AddRow(r => r.AddCell(c => c.WithColumnIndex(0).WithRowIndex(1).WithValue("Same Value").Build()).Build(1))
                .AddRow(r => r.AddCell(c => c.WithColumnIndex(0).WithRowIndex(2).WithValue("Same Value").Build()).Build(2))
                .Build();

            var result = new SheetMapper()
                .AddConfigFor<TestModel>(cfg => cfg
                    .HasHeaders()
                    .MapColumn(column => column
                        .WithHeader("FirstColumn")
                        .ShouldHaveUniqueValues()
                        .MapTo(t => t.StringProperty)))
                .Map<TestModel>(sheetData);

            result.IsSuccess.Should().BeFalse();
            result.ParsedModels.Should().HaveCount(2);
        }

        [Fact]
        public void GivenParsingShouldBeStoppedOnFirstEmptyRow_WhenSecondRowIsEmpty_OnlyRowOneIsParsed()
        {
            var sheetData = new SheetBuilder()
                .AddHeaders("FirstColumn")
                .AddRow(r => r.AddCell(c => c.WithColumnIndex(0).WithRowIndex(1).WithValue("Some Value").Build()).Build(1))
                .AddRow(r => r.AddCell(c => c.WithColumnIndex(0).WithRowIndex(2).Build()).Build(2))
                .AddRow(r => r.AddCell(c => c.WithColumnIndex(0).WithRowIndex(3).Build()).Build(3))
                .Build();

            var result = new SheetMapper()
                .AddConfigFor<TestModel>(cfg => cfg
                    .HasHeaders()
                    .MapColumn(column => column
                        .WithHeader("FirstColumn")
                        .MapTo(t => t.StringProperty))
                    .StopParsingAtFirstEmptyRow())
                .Map<TestModel>(sheetData);

            result.IsSuccess.Should().BeTrue();
            result.ParsedModels.Should().HaveCount(1);
        }

        [Fact]
        public void GivenParsingShouldBeStoppedOnFirstEmptyRow_WhenSecondRowIsEmptyAndThirdIsNotEmpty_OnlyRowOneIsParsed()
        {
            var sheetData = new SheetBuilder()
                .AddHeaders("FirstColumn")
                .AddRow(r => r.AddCell(c => c.WithColumnIndex(0).WithRowIndex(1).WithValue("Some Value").Build()).Build(1))
                .AddRow(r => r.AddCell(c => c.WithColumnIndex(0).WithRowIndex(2).Build()).Build(2))
                .AddRow(r => r.AddCell(c => c.WithColumnIndex(0).WithRowIndex(3).WithValue("Foo").Build()).Build(3))
                .Build();

            var result = new SheetMapper()
                .AddConfigFor<TestModel>(cfg => cfg
                    .HasHeaders()
                    .MapColumn(column => column
                        .WithHeader("FirstColumn")
                        .MapTo(t => t.StringProperty))
                    .StopParsingAtFirstEmptyRow())
                .Map<TestModel>(sheetData);

            result.IsSuccess.Should().BeTrue();
            result.ParsedModels.Should().HaveCount(1);
        }

        [Fact]
        public void GivenSheet_WhenCustomBooleanParserIsProvided_ValueIsConvertedToBoolean()
        {
            var sheetData = new SheetBuilder()
                .AddHeaders("Bool")
                .AddRow(r => r.AddCell(c => c.WithColumnIndex(0).WithRowIndex(1).WithValue("1").Build()).Build(1))
                .Build();

            var result = new SheetMapper()
                .AddConfigFor<TestModel>(cfg => cfg
                    .HasHeaders()
                    .MapColumn(column => column
                        .WithHeader("Bool")
                        .IsRequired()
                        .ParseValueUsing(x => x.Equals("1") ? true : false)
                        .MapTo(t => t.BoolProperty)))
                .Map<TestModel>(sheetData);

            result.IsSuccess.Should().BeTrue();
            result.ParsedModels.Should().HaveCount(1);
            result.ParsedModels.First().Value.BoolProperty.Should().BeTrue();
        }

        [Fact]
        public void GivenSheet_WhenCustomEnumParserIsProvided_ValueIsConvertedToEnum()
        {
            var sheetData = new SheetBuilder()
                .AddHeaders("EnumProperty")
                .AddRow(r => r.AddCell(c => c.WithColumnIndex(0).WithRowIndex(1).WithValue("Second").Build()).Build(1))
                .Build();

            var result = new SheetMapper()
                .AddConfigFor<TestModel>(cfg => cfg
                    .HasHeaders()
                    .MapColumn(column => column
                        .WithHeader("EnumProperty")
                        .IsRequired()
                        .ParseValueUsing(x =>
                        {
                            switch (x)
                            {
                                case "First": return EnumModel.First;
                                case "Second": return EnumModel.Second;
                                case "Third": return EnumModel.Third;
                                default: return EnumModel.Default;
                            }
                        })
                        .MapTo(t => t.EnumProperty)))
                .Map<TestModel>(sheetData);

            result.IsSuccess.Should().BeTrue();
            result.ParsedModels.Should().HaveCount(1);
            result.ParsedModels.First().Value.EnumProperty.Should().Be(EnumModel.Second);
        }

        [Fact]
        public void GivenSheet_WhenCustomEnumParserFails_ResultContainsFailure()
        {
            var sheetData = new SheetBuilder()
                .AddHeaders("StringProperty")
                .AddRow(r => r.AddCell(c => c.WithColumnIndex(0).WithRowIndex(1).WithValue("MyValue").Build()).Build(1))
                .Build();

            var result = new SheetMapper()
                .AddConfigFor<TestModel>(cfg => cfg
                    .HasHeaders()
                    .MapColumn(column => column
                        .WithHeader("StringProperty")
                        .ParseValueUsing(x => throw new Exception("An exception occured"))
                        .MapTo(t => t.StringProperty)))
                .Map<TestModel>(sheetData);

            result.IsSuccess.Should().BeFalse();
            result.ValidationErrors.Should().HaveCount(1);
            result.ValidationErrors.First().ErrorMessage.Should().Be("Could not parse value");
        }
    }

    public class ModelOne
    {
        public string ModelOnePropertyOne { get; set; }
    }

    public class ModelTwo
    {
        public int ModelTwoPropertyOne { get; set; }
    }
}