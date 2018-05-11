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
        private readonly string[] _headers = {"Double", "Integer", "Boolean", "Enumeration", "String"};
        private readonly string _stringValue = "foo";
        private readonly double _doubleValue = 42.42D;
        private readonly int _intValue = 42;
        private readonly bool _boolValue = true;
        private readonly EnumModel _enumValue = EnumModel.Second;

        public SheetMapperSpecs()
        {
            _sheetData = new SheetBuilder()
                .AddHeaders(_headers)
                .AddRow(r => r
                    .AddCell(c => c.WithColumnIndex(0).WithRowIndex(1).WithValue(_doubleValue).Build())
                    .AddCell(c => c.WithColumnIndex(1).WithRowIndex(1).WithValue(_intValue).Build())
                    .AddCell(c => c.WithColumnIndex(2).WithRowIndex(1).WithValue(_boolValue).Build())
                    .AddCell(c => c.WithColumnIndex(3).WithRowIndex(1).WithValue(_enumValue).Build())
                    .AddCell(c => c.WithColumnIndex(4).WithRowIndex(1).WithValue(_stringValue).Build())
                    .Build(0))
                .Build(); 
        }

        [Fact]
        public void GivenASheet_WhenMappingModelDoubleProperty_ItSetsPropertyOnModel()
        {
            var testModelList = new SheetMapper()
                .For<TestModel>(cfg => cfg
                    .HasHeaders()
                    .Columns(columns => columns.Add(column => column.WithHeader("Double").MapTo(t => t.DoubleProperty))).Object())
                .Map(_sheetData)
                .To<TestModel>();

            testModelList.ParsedModels.Should().HaveCount(1);
            testModelList.ParsedModels.Single().DoubleProperty.Should().Be(_doubleValue);
        }

        [Fact]
        public void GivenASheet_WhenMappingModelIntProperty_ItSetsPropertyOnModel()
        {
            var testModelList = new SheetMapper()
                .For<TestModel>(cfg => cfg
                    .HasHeaders()
                    .Columns(columns => columns.Add(column => column.WithHeader("Integer").MapTo(t => t.IntProperty))).Object())
                .Map(_sheetData)
                .To<TestModel>();

            testModelList.ParsedModels.Should().HaveCount(1);
            testModelList.ParsedModels.Single().IntProperty.Should().Be(_intValue);
        }

        [Fact]
        public void GivenASheet_WhenMappingModelBoolProperty_ItSetsPropertyOnModel()
        {
            var testModelList = new SheetMapper()
                .For<TestModel>(cfg => cfg
                    .HasHeaders()
                    .Columns(columns => columns.Add(column => column.WithHeader("Boolean").MapTo(t => t.BoolProperty))).Object())
                .Map(_sheetData)
                .To<TestModel>();

            testModelList.ParsedModels.Should().HaveCount(1);
            testModelList.ParsedModels.Single().BoolProperty.Should().Be(_boolValue);
        }

        [Fact]
        public void GivenASheet_WhenMappingModelEnumProperty_ItSetsPropertyOnModel()
        {
            var testModelList = new SheetMapper()
                .For<TestModel>(cfg => cfg
                    .HasHeaders()
                    .Columns(columns =>
                        columns.Add(column => column.WithHeader("Enumeration").MapTo(t => t.EnumProperty))).Object())
                .Map(_sheetData)
                .To<TestModel>();

            testModelList.ParsedModels.Should().HaveCount(1);
            testModelList.ParsedModels.Single().EnumProperty.Should().Be(_enumValue);
        }

        [Fact]
        public void GivenASheet_WhenMappingModelStringProperty_ItSetsPropertyOnModel()
        {
            var testModelList = new SheetMapper()
                .For<TestModel>(cfg => cfg
                    .HasHeaders()
                    .Columns(columns => columns.Add(column => column.WithHeader("String").MapTo(t => t.StringProperty))).Object())
                .Map(_sheetData)
                .To<TestModel>();

            testModelList.ParsedModels.Should().HaveCount(1);
            testModelList.ParsedModels.Single().StringProperty.Should().Be(_stringValue);
        }
    }
}