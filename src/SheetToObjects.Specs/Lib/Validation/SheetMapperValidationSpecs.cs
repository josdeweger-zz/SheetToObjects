using System;
using System.Linq;
using FluentAssertions;
using SheetToObjects.Lib;
using SheetToObjects.Lib.FluentConfiguration;
using SheetToObjects.Specs.Builders;
using SheetToObjects.Specs.TestModels;
using Xunit;

namespace SheetToObjects.Specs.Lib.Validation
{
    public class SheetMapperValidationSpecs
    {
        [Fact]
        public void GivenASheet_WhenMappingRequiredModelPropertyToInvalidType_ShouldSetDefaultValue()
        {
            var columnName = "String";

            var sheetData = new SheetBuilder()
                .AddHeaders("Double")
                .AddRow(r => r
                    .AddCell(c => c.WithColumnIndex(0).WithRowIndex(1).WithValue(5.3D.ToString()).Build())
                    .Build(0))
                .Build();

            var testModelList = new SheetMapper()
                .AddConfigFor<TestModel>(cfg => cfg
                    .HasHeaders()
                    .MapColumn(column => column
                        .WithHeader(columnName)
                        .IsRequired()
                        .MapTo(t => t.DoubleProperty)))
                .Map<TestModel>(sheetData);

            testModelList.IsFailure.Should().BeTrue();
            testModelList.ParsedModels.Should().HaveCount(0);
            testModelList.ValidationErrors.Single().ColumnName.Should().Be(columnName);
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
        public void GivenSheet_WhenMappingRequiredPropertyThatIsSet_ItSetsNoValidationError()
        {
            var sheetData = new SheetBuilder()
                .AddHeaders("Column1")
                .AddRow(r => r
                    .AddCell(c => c.WithColumnIndex(0).WithRowIndex(0).WithValue("SomeValue").Build())
                    .Build(0))
                .Build();

            var result = new SheetMapper()
                .AddConfigFor<TestModel>(cfg => cfg
                    .HasHeaders()
                    .MapColumn(column => column
                        .WithHeader("Column1")
                        .IsRequired()
                        .MapTo(t => t.StringProperty)))
                .Map<TestModel>(sheetData);

            result.ValidationErrors.Should().BeEmpty();
        }

        [Fact]
        public void GivenValidatingCustomRule_WhenIntIsThreeAndShouldBeBetweenOneAndFive_ItIsValid()
        {
            var sheetData = new SheetBuilder()
                .AddHeaders("Column1")
                .AddRow(r => r.AddCell(c => c.WithColumnIndex(0).WithRowIndex(1).WithValue(3).Build()).Build(1))
                .Build();

            var result = new SheetMapper()
                .AddConfigFor<TestModel>(cfg => cfg
                    .HasHeaders()
                    .MapColumn(column => column
                        .WithHeader("Column1")
                        .WithCustomRule<int>(x => x >= 3 && x <= 5, "Value should be between 3 and 5")
                        .MapTo(t => t.IntProperty)))
                .Map<TestModel>(sheetData);

            result.IsSuccess.Should().BeTrue();
            result.ValidationErrors.Should().BeEmpty();
        }

        [Fact]
        public void GivenValidatingCustomRule_WhenIntIsTwelveAndShouldBeBetweenOneAndFive_ItIsNotValid()
        {
            var sheetData = new SheetBuilder()
                .AddHeaders("Column1")
                .AddRow(r => r.AddCell(c => c.WithColumnIndex(0).WithRowIndex(1).WithValue(4).Build()).Build(1))
                .AddRow(r => r.AddCell(c => c.WithColumnIndex(0).WithRowIndex(2).WithValue(12).Build()).Build(2))
                .Build();

            var result = new SheetMapper()
                .AddConfigFor<TestModel>(cfg => cfg
                    .HasHeaders()
                    .MapColumn(column => column
                        .WithHeader("Column1")
                        .WithCustomRule<int>(x => x >= 3 && x <= 5, "Value should be between 3 and 5")
                        .MapTo(t => t.IntProperty)))
                .Map<TestModel>(sheetData);

            result.IsSuccess.Should().BeFalse();
            result.ValidationErrors.Should().HaveCount(1);
            result.ValidationErrors.Single().RowIndex.Should().Be(2);
            result.ValidationErrors.Single().ColumnIndex.Should().Be(0);
        }
    }
}