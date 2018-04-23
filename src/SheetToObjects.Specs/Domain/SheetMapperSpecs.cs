using System.Linq;
using FluentAssertions;
using Moq;
using SheetToObjects.Lib;
using SheetToObjects.Specs.Builders;
using SheetToObjects.Specs.TestModels;
using Xunit;

namespace SheetToObjects.Specs.Domain
{
    public class SheetMapperSpecs
    {
        [Fact]
        public void Foo()
        {
            const string firstValue = "FirstValue";
            const int secondValue = 42;
            const double thirdValue = 42.42D;

            var mappingConfig = new MappingConfig()
                .For<TestModel>()
                .Column("A").MapTo(t => t.StringProperty)
                .Column("B").MapTo(t => t.IntProperty)
                .Column("C").MapTo(t => t.DoubleProperty)
                .Build();

            var mappingConfigProviderMock = new Mock<IProvideMappingConfig>();
            mappingConfigProviderMock.Setup(m => m.Get()).Returns(mappingConfig);

            var sheetData = new SheetBuilder()
                .AddRow(r => r
                    .AddCell(c => c.WithColumnLetter("A").WithRowNumber(1).WithValue(firstValue).Build())
                    .AddCell(c => c.WithColumnLetter("B").WithRowNumber(1).WithValue(secondValue).Build())
                    .AddCell(c => c.WithColumnLetter("C").WithRowNumber(1).WithValue(thirdValue).Build())
                    .Build())
                .Build();

            var testModelList = new SheetMapper(mappingConfigProviderMock.Object).Map(sheetData).To<TestModel>();

            testModelList.Should().HaveCount(1);
            testModelList.Single().StringProperty.Should().Be(firstValue);
            testModelList.Single().IntProperty.Should().Be(secondValue);
            testModelList.Single().DoubleProperty.Should().Be(thirdValue);
        }
    }
}
