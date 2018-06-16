using System;
using System.Linq;
using FluentAssertions;
using SheetToObjects.Lib;
using SheetToObjects.Specs.Builders;
using SheetToObjects.Specs.TestModels;
using Xunit;

namespace SheetToObjects.Specs.Lib.Validation
{
    public class SheetMapperValidationSpecs
    {
        private readonly Sheet _sheetData;
        private readonly string _stringValue = "foo";
        private readonly double _doubleValue = 42.42D;
        private readonly int _intValue = 42;
        private readonly bool _boolValue = true;
        private readonly DateTime _dateTimeValue = new DateTime(2018, 5, 30);
        private readonly EnumModel _enumValue = EnumModel.Second;
        private readonly decimal _decimalValue = 5.123m;

        public SheetMapperValidationSpecs()
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
        public void GivenSheet_WhenMappingRequiredPropertyThatIsSet_ItSetsNoValidationError()
        {
            var result = new SheetMapper()
                .AddConfigFor<TestModel>(cfg => cfg
                    .HasHeaders()
                    .MapColumn(column => column
                            .WithHeader("String")
                            .IsRequired()
                            .MapTo(t => t.StringProperty)))
                .Map<TestModel>(_sheetData);

            result.ValidationErrors.Should().BeEmpty();
        }

        [Fact]
        public void GivenASheet_WhenMappingRequiredModelPropertyToInvalidType_ShouldSetDefaultValue()
        {
            var columnName = "String";

            var testModelList = new SheetMapper()
                .AddConfigFor<TestModel>(cfg => cfg
                    .HasHeaders()
                    .MapColumn(column => column
                            .WithHeader(columnName)
                            .IsRequired()
                            .MapTo(t => t.DoubleProperty)))
                .Map<TestModel>(_sheetData);

            testModelList.IsFailure.Should().BeTrue();
            testModelList.ParsedModels.Should().HaveCount(0);
            testModelList.ValidationErrors.Single().ColumnName.Should().Be(columnName);
        }

        [Fact]
        public void GivenSheet_WhenMappingRequiredPropertyThatIsEmpty_ItSetsValidationError()
        {
            var columnName = "String";

            var sheetData = new SheetBuilder()
                .AddHeaders(columnName)
                .AddRow(r => r
                    .AddCell(c => c.WithColumnIndex(4).WithRowIndex(1).WithValue(string.Empty).Build())
                    .Build(0))
                .Build();

            var result = new SheetMapper()
                .AddConfigFor<TestModel>(cfg => cfg
                    .HasHeaders()
                    .MapColumn(column => column.WithHeader(columnName).IsRequired().MapTo(t => t.StringProperty)))
                .Map<TestModel>(sheetData);

            result.ValidationErrors.Should().HaveCount(1);
            result.ValidationErrors.Single().PropertyName.Should().Be("StringProperty");
            result.ValidationErrors.Single().CellValue.Should().Be(string.Empty);
            result.ValidationErrors.Single().ColumnIndex.Should().Be(0);
            result.ValidationErrors.Single().RowIndex.Should().Be(0);
            result.ValidationErrors.Single().ColumnName.Should().Be(columnName);
        }

        [Fact]
        public void GivenSheet_WhenMappingIntValueSmallerThanMinValue_ItSetsValidationError()
        {
            var columnName = "Int";

            var sheetData = new SheetBuilder()
                .AddHeaders(columnName)
                .AddRow(r => r
                    .AddCell(c => c.WithColumnIndex(0).WithRowIndex(0).WithValue(5).Build())
                    .Build(0))
                .Build();

            var result = new SheetMapper()
                .AddConfigFor<TestModel>(cfg => cfg
                    .HasHeaders()
                    .MapColumn(column => column
                            .WithHeader(columnName)
                            .WithMinimum(10)
                            .MapTo(t => t.IntProperty)))
                .Map<TestModel>(sheetData);

            result.ValidationErrors.Should().HaveCount(1);
            result.ValidationErrors.Single().PropertyName.Should().Be("IntProperty");
            result.ValidationErrors.Single().CellValue.Should().Be(string.Empty);
            result.ValidationErrors.Single().ColumnName.Should().Be(columnName);
        }

        [Fact]
        public void GivenSheet_WhenMappingDoubleValueBiggerThanMinValue_ItSetsNoValidationError()
        {
            var columnName = "Double";

            var sheetData = new SheetBuilder()
                .AddHeaders(columnName)
                .AddRow(r => r
                    .AddCell(c => c.WithColumnIndex(0).WithRowIndex(0).WithValue(5.0D).Build())
                    .Build(0))
                .Build();

            var result = new SheetMapper()
                .AddConfigFor<TestModel>(cfg => cfg
                    .HasHeaders()
                    .MapColumn(column => column
                            .WithHeader(columnName)
                            .WithMinimum(4D)
                            .MapTo(t => t.DoubleProperty)))
                .Map<TestModel>(sheetData);

            result.ValidationErrors.Should().BeEmpty();
        }

        [Fact]
        public void GivenSheet_WhenMappingDoubleValueSmallerThanMinValue_ItSetsValidationError()
        {
            var columnName = "Double";

            var sheetData = new SheetBuilder()
                .AddHeaders(columnName)
                .AddRow(r => r
                    .AddCell(c => c.WithColumnIndex(0).WithRowIndex(0).WithValue(5.0D).Build())
                    .Build(0))
                .Build();

            var result = new SheetMapper()
                .AddConfigFor<TestModel>(cfg => cfg
                    .HasHeaders()
                    .MapColumn(column => column
                            .WithHeader(columnName)
                            .WithMinimum(10D)
                            .MapTo(t => t.DoubleProperty)))
                .Map<TestModel>(sheetData);

            result.ValidationErrors.Should().HaveCount(1);
            result.ValidationErrors.Single().PropertyName.Should().Be("DoubleProperty");
            result.ValidationErrors.Single().CellValue.Should().Be(string.Empty);
            result.ValidationErrors.Single().ColumnName.Should().Be(columnName);
        }

        [Fact]
        public void GivenSheet_WhenMappingDecimalValueSmallerThanMaxValue_ItSetsNoValidationError()
        {
            var columnName = "Decimal";

            var sheetData = new SheetBuilder()
                .AddHeaders(columnName)
                .AddRow(r => r
                    .AddCell(c => c.WithColumnIndex(0).WithRowIndex(0).WithValue(3.123m).Build())
                    .Build(0))
                .Build();

            var result = new SheetMapper()
                .AddConfigFor<TestModel>(cfg => cfg
                    .HasHeaders()
                    .MapColumn(column => column
                            .WithHeader(columnName)
                            .WithMaximum(3.123m)
                            .MapTo(t => t.DecimalProperty)))
                .Map<TestModel>(sheetData);

            result.ValidationErrors.Should().BeEmpty();
        }

        [Fact]
        public void GivenSheet_WhenMappingDecimalValueBiggerThanMaxValue_ItSetsValidationError()
        {
            var columnName = "Decimal";

            var sheetData = new SheetBuilder()
                .AddHeaders(columnName)
                .AddRow(r => r
                    .AddCell(c => c.WithColumnIndex(0).WithRowIndex(0).WithValue(5.012m).Build())
                    .Build(0))
                .Build();

            var result = new SheetMapper()
                .AddConfigFor<TestModel>(cfg => cfg
                    .HasHeaders()
                    .MapColumn(column => column
                            .WithHeader(columnName)
                            .WithMaximum(5.011m)
                            .MapTo(t => t.DecimalProperty)))
                .Map<TestModel>(sheetData);

            result.ValidationErrors.Should().HaveCount(1);
            result.ValidationErrors.Single().PropertyName.Should().Be("DecimalProperty");
            result.ValidationErrors.Single().CellValue.Should().Be(string.Empty);
            result.ValidationErrors.Single().ColumnName.Should().Be(columnName);
        }
    }
}