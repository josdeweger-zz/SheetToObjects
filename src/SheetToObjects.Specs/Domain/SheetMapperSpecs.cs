using System.Linq;
using FluentAssertions;
using SheetToObjects.Lib;
using SheetToObjects.Lib.ValueParsers;
using SheetToObjects.Specs.Builders;
using SheetToObjects.Specs.TestModels;
using Xunit;

namespace SheetToObjects.Specs.Domain
{
    public class SheetMapperSpecs
    {
        [Fact]
        public void GivenASheet_WhenMappingModelStringProperty_ItSetsPropertyOnModel()
        {
            const string value = "FirstValue";

            var sheetData = new SheetBuilder()
                .AddRow(r => r
                    .AddCell(c => c.WithColumnLetter("A").WithRowNumber(1).WithValue(value).Build())
                    .Build())
                .Build();

            var testModelList = new SheetMapper(new ValueParser())
                .Configure(cfg => cfg
                    .For<TestModel>()
                    .Column("A").MapTo(t => t.StringProperty)
                    .Build())
                .Map(sheetData)
                .To<TestModel>();

            testModelList.Should().HaveCount(1);
            testModelList.Single().StringProperty.Should().Be(value);
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

            var testModelList = new SheetMapper(new ValueParser())
                .Configure(cfg => cfg
                    .For<TestModel>()
                    .Column("B").MapTo(t => t.IntProperty)
                    .Build())
                .Map(sheetData)
                .To<TestModel>();

            testModelList.Should().HaveCount(1);
            testModelList.Single().IntProperty.Should().Be(value);
        }

        [Fact]
        public void GivenASheet_WhenMappingModelDoubleProperty_ItSetsPropertyOnModel()
        {
            const double value = 42.42D;

            var sheetData = new SheetBuilder()
                .AddRow(r => r
                    .AddCell(c => c.WithColumnLetter("C").WithRowNumber(1).WithValue(value).Build())
                    .Build())
                .Build();

            var testModelList = new SheetMapper(new ValueParser())
                .Configure(cfg => cfg
                    .For<TestModel>()
                    .Column("C").MapTo(t => t.DoubleProperty)
                    .Build())
                .Map(sheetData)
                .To<TestModel>();

            testModelList.Should().HaveCount(1);
            testModelList.Single().DoubleProperty.Should().Be(value);
        }
    }
}
