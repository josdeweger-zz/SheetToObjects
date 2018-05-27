using System;
using System.Linq;
using FluentAssertions;
using SheetToObjects.Lib;
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

        public SheetMapperSpecs()
        {
            _sheetData = new SheetBuilder()
                .AddHeaders("Double", "Integer", "Boolean", "Enumeration", "String", "DateTime")
                .AddRow(r => r
                    .AddCell(c => c.WithColumnIndex(0).WithRowIndex(1).WithValue(_doubleValue.ToString()).Build())
                    .AddCell(c => c.WithColumnIndex(1).WithRowIndex(1).WithValue(_intValue.ToString()).Build())
                    .AddCell(c => c.WithColumnIndex(2).WithRowIndex(1).WithValue(_boolValue.ToString()).Build())
                    .AddCell(c => c.WithColumnIndex(3).WithRowIndex(1).WithValue(_enumValue.ToString()).Build())
                    .AddCell(c => c.WithColumnIndex(4).WithRowIndex(1).WithValue(_stringValue.ToString()).Build())
                    .AddCell(c => c.WithColumnIndex(5).WithRowIndex(1).WithValue(_dateTimeValue.ToString("yyyy-MM-dd")).Build())
                    .Build(0))
                .Build();
        }

        [Fact]
        public void GivenModel_WhenNoMappingIsConfigured_ItThrows()
        {
            Action result = () => new SheetMapper()
                .For<WithoutSheetToObjectConfigModel>(cfg => cfg.BuildConfig())
                .Map(_sheetData)
                .To<TestModel>();

            result.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void GivenSheet_WhenMappingModelDoubleProperty_ItSetsPropertyOnModel()
        {
            var result = new SheetMapper()
                .For<TestModel>(cfg => cfg
                    .HasHeaders()
                    .Columns(columns => columns.Add(column => column.WithHeader("Double").MapTo(t => t.DoubleProperty))).BuildConfig())
                .Map(_sheetData)
                .To<TestModel>();

            result.ParsedModels.Should().HaveCount(1);
            result.ParsedModels.Single().DoubleProperty.Should().Be(_doubleValue);
        }

        [Fact]
        public void GivenSheet_WhenMappingModelIntProperty_ItSetsPropertyOnModel()
        {
            var result = new SheetMapper()
                .For<TestModel>(cfg => cfg
                    .HasHeaders()
                    .Columns(columns => columns.Add(column => column.WithHeader("Integer").MapTo(t => t.IntProperty)))
                    .BuildConfig())
                .Map(_sheetData)
                .To<TestModel>();

            result.ParsedModels.Should().HaveCount(1);
            result.ParsedModels.Single().IntProperty.Should().Be(_intValue);
        }

        [Fact]
        public void GivenSheet_WhenMappingModelBoolProperty_ItSetsPropertyOnModel()
        {
            var result = new SheetMapper()
                .For<TestModel>(cfg => cfg
                    .HasHeaders()
                    .Columns(columns => columns.Add(column => column.WithHeader("Boolean").MapTo(t => t.BoolProperty)))
                    .BuildConfig())
                .Map(_sheetData)
                .To<TestModel>();

            result.ParsedModels.Should().HaveCount(1);
            result.ParsedModels.Single().BoolProperty.Should().Be(_boolValue);
        }

        [Fact]
        public void GivenSheet_WhenMappingModelEnumProperty_ItSetsPropertyOnModel()
        {
            var result = new SheetMapper()
                .For<TestModel>(cfg => cfg
                    .HasHeaders()
                    .Columns(columns =>
                        columns.Add(column => column.WithHeader("Enumeration").MapTo(t => t.EnumProperty)))
                    .BuildConfig())
                .Map(_sheetData)
                .To<TestModel>();

            result.ParsedModels.Should().HaveCount(1);
            result.ParsedModels.Single().EnumProperty.Should().Be(_enumValue);
        }

        [Fact]
        public void GivenSheet_WhenMappingModelDateTimeProperty_ItSetsPropertyOnModel()
        {
            var result = new SheetMapper()
                .For<TestModel>(cfg => cfg
                    .HasHeaders()
                    .Columns(columns =>
                        columns.Add(column => column.WithHeader("DateTime").UsingFormat("yyyy-MM-dd").MapTo(t => t.DateTimeProperty)))
                    .BuildConfig())
                .Map(_sheetData)
                .To<TestModel>();

            result.ParsedModels.Should().HaveCount(1);
            result.ParsedModels.Single().DateTimeProperty.Should().Be(_dateTimeValue);
        }

        [Fact]
        public void GivenSheet_WhenMappingModelStringProperty_ItSetsPropertyOnModel()
        {
            var result = new SheetMapper()
                .For<TestModel>(cfg => cfg
                    .HasHeaders()
                    .Columns(columns => columns.Add(column => column.WithHeader("String").MapTo(t => t.StringProperty)))
                    .BuildConfig())
                .Map(_sheetData)
                .To<TestModel>();

            result.ParsedModels.Should().HaveCount(1);
            result.ParsedModels.Single().StringProperty.Should().Be(_stringValue);
        }

        [Fact]
        public void GivenSheet_WhenMappingModelStringPropertyWithRegexValidation_ShouldThrowValidationError()
        {
            var result = new SheetMapper()
                .For<TestModel>(cfg => cfg
                    .HasHeaders()
                    .Columns(columns => columns.Add(column => column.WithHeader("String").Matches("invalid").MapTo(t => t.StringProperty)))
                    .BuildConfig())
                .Map(_sheetData)
                .To<TestModel>();

            result.IsFailure.Should().BeTrue();
            result.ParsedModels.Should().HaveCount(0);
            result.ValidationErrors.Single().ColumnName.Should().Be("string");
        }

        [Fact]
        public void GivenSheet_WhenMappingRequiredModelPropertyToInvalidType_shouldSetDefaultValue()
        {
            var result = new SheetMapper()
                .For<TestModel>(cfg => cfg
                    .HasHeaders()
                    .Columns(columns => columns.Add(column => column.WithHeader("String").IsRequired().MapTo(t => t.DoubleProperty)))
                    .BuildConfig())
                .Map(_sheetData)
                .To<TestModel>();

            result.IsFailure.Should().BeTrue();
            result.ParsedModels.Should().HaveCount(0);
            result.ValidationErrors.Single().ColumnName.Should().Be("string");
        }

        [Fact]
        public void GivenSheet_WhenMappingModeltoAPropertyWithBody_ItShouldThrowException()
        {
            Action result = () => new SheetMapper()
                .For<TestModel>(cfg => cfg
                    .HasHeaders()
                    .Columns(columns =>
                        columns.Add(column => column.WithHeader("String").MapTo(t => t.PropertyWithBody)))
                    .BuildConfig())
                .Map(_sheetData)
                .To<TestModel>();

            result.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void GivenSheet_WhenMappingRequiredPropertyThatIsSet_ItSetsNoValidationError()
        {
            var result = new SheetMapper()
                .For<TestModel>(cfg => cfg
                    .HasHeaders()
                    .Columns(columns => columns
                        .Add(column => column.WithHeader("String").IsRequired().MapTo(t => t.StringProperty)))
                    .BuildConfig())
                .Map(_sheetData)
                .To<TestModel>();

            result.ValidationErrors.Should().BeEmpty();
        }

        [Fact]
        public void GivenASheet_WhenMappingRequiredModelPropertyToInvalidType_ShouldSetDefaultValue()
        {
            var testModelList = new SheetMapper()
                .For<TestModel>(cfg => cfg
                    .HasHeaders()
                    .Columns(columns => columns.Add(column => column.WithHeader("String").IsRequired().MapTo(t => t.DoubleProperty)))
                    .BuildConfig())
                .Map(_sheetData)
                .To<TestModel>();

            testModelList.IsFailure.Should().BeTrue();
            testModelList.ParsedModels.Should().HaveCount(0);
            testModelList.ValidationErrors.Single().ColumnName.Should().Be("string");
        }

        [Fact]
        public void GivenSheet_WhenMappingRequiredPropertyThatIsEmpty_ItSetsValidationError()
        {
            var sheetData = new SheetBuilder()
                .AddHeaders("String")
                .AddRow(r => r
                    .AddCell(c => c.WithColumnIndex(4).WithRowIndex(1).WithValue(string.Empty).Build())
                    .Build(0))
                .Build();

            var result = new SheetMapper()
                .For<TestModel>(cfg => cfg
                    .HasHeaders()
                    .Columns(columns => columns
                        .Add(column => column.WithHeader("String").IsRequired().MapTo(t => t.StringProperty)))
                    .BuildConfig())
                .Map(sheetData)
                .To<TestModel>();

            result.ValidationErrors.Should().HaveCount(1);
        }
    }
}