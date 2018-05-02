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
        private readonly double _doubleValue = 42.42D;
        private readonly int _intValue = 42;
        private readonly bool _boolValue = true;
        private readonly EnumModel _enumValue = EnumModel.Second;

        public SheetMapperSpecs()
        {
            _sheetData = new SheetBuilder()
                .AddRow(r => r
                    .AddCell(c => c.WithColumnLetter("A").WithRowNumber(1).WithValue(_doubleValue).Build())
                    .AddCell(c => c.WithColumnLetter("B").WithRowNumber(1).WithValue(_intValue).Build())
                    .AddCell(c => c.WithColumnLetter("C").WithRowNumber(1).WithValue(_boolValue).Build())
                    .AddCell(c => c.WithColumnLetter("D").WithRowNumber(1).WithValue(_enumValue).Build())
                    .Build())
                .Build();
        }

        [Fact]
        public void GivenASheet_WhenMappingModelDoubleProperty_ItSetsPropertyOnModel()
        {
            var testModelList = new SheetMapper()
                .For<TestModel>(cfg => cfg
                    .Columns(columns => columns.Add(column => column.Map("A").To(t => t.DoubleProperty))))
                .Map(_sheetData)
                .To<TestModel>();

            testModelList.Should().HaveCount(1);
            testModelList.Single().DoubleProperty.Should().Be(_doubleValue);
        }

        [Fact]
        public void GivenASheet_WhenMappingModelIntProperty_ItSetsPropertyOnModel()
        {
            var testModelList = new SheetMapper()
                .For<TestModel>(cfg => cfg
                    .Columns(columns => columns.Add(column => column.Map("B").To(t => t.IntProperty))))
                .Map(_sheetData)
                .To<TestModel>();

            testModelList.Should().HaveCount(1);
            testModelList.Single().IntProperty.Should().Be(_intValue);
        }

        [Fact]
        public void GivenASheet_WhenMappingModelBoolProperty_ItSetsPropertyOnModel()
        {
            var testModelList = new SheetMapper()
                .For<TestModel>(cfg => cfg
                    .Columns(columns => columns.Add(column => column.Map("C").To(t => t.BoolProperty))))
                .Map(_sheetData)
                .To<TestModel>();

            testModelList.Should().HaveCount(1);
            testModelList.Single().BoolProperty.Should().Be(_boolValue);
        }

        [Fact]
        public void GivenASheet_WhenMappingModelEnumProperty_ItSetsPropertyOnModel()
        {
            var testModelList = new SheetMapper()
                .For<TestModel>(cfg => cfg
                    .Columns(columns => columns.Add(column => column.Map("D").To(t => t.EnumProperty))))
                .Map(_sheetData)
                .To<TestModel>();

            testModelList.Should().HaveCount(1);
            testModelList.Single().EnumProperty.Should().Be(_enumValue);
        }
    }
}