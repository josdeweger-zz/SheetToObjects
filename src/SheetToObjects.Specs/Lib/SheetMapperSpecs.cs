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
        [Fact]
        public void GivenASheet_WhenMappingModelDoubleProperty_ItSetsPropertyOnModel()
        {
            const double value = 42.42D;

            var sheetData = new SheetBuilder()
                .AddRow(r => r
                    .AddCell(c => c.WithColumnLetter("C").WithRowNumber(1).WithValue(value).Build())
                    .Build())
                .Build();

            var testModelList = new SheetMapper()
                .AddConfigFor<TestModel>(cfg => cfg
                    .WithColumns(columns => columns.Add(column => column.WithLetter("C").MapTo(t => t.DoubleProperty))))
                .Map(sheetData)
                .To<TestModel>();

            testModelList.Should().HaveCount(1);
            testModelList.Single().DoubleProperty.Should().Be(value);
        }

        [Fact]
        public void GivenASheet_WhenMappingModelIntProperty_ItSetsPropertyOnModel()
        {
            const int value = 42;

            var sheetData = new SheetBuilder()
                .AddRow(r => r
                    .AddCell(c => c.WithColumnLetter("B").WithRowNumber(1).WithValue(value).Build())
                    .Build())
                .Build();

            var testModelList = new SheetMapper()
                .AddConfigFor<TestModel>(cfg => cfg
                    .WithColumns(columns => columns.Add(column => column.WithLetter("B").MapTo(t => t.IntProperty))))
                .Map(sheetData)
                .To<TestModel>();

            testModelList.Should().HaveCount(1);
            testModelList.Single().IntProperty.Should().Be(value);
        }

        [Fact]
        public void GivenASheet_WhenMappingModelStringProperty_ItSetsPropertyOnModel()
        {
            const string value = "FirstValue";

            var sheetData = new SheetBuilder()
                .AddRow(r => r
                    .AddCell(c => c.WithColumnLetter("A").WithRowNumber(1).WithValue(value).Build())
                    .Build())
                .Build();

            var testModelList = new SheetMapper()
                .AddConfigFor<TestModel>(cfg => cfg
                    .WithColumns(columns => columns.Add(column => column.WithLetter("A").MapTo(t => t.StringProperty))))
                .Map(sheetData)
                .To<TestModel>();

            testModelList.Should().HaveCount(1);
            testModelList.Single().StringProperty.Should().Be(value);
        }
    }
}