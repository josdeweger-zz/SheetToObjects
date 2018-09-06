using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using SheetToObjects.Adapters.GoogleSheets;
using SheetToObjects.Adapters.GoogleSheets.Shared.Models;
using SheetToObjects.Lib;
using SheetToObjects.Specs.Builders;
using Xunit;

namespace SheetToObjects.Specs.Adapters
{
    public class GoogleSheetProviderSpecs
    {
        private readonly Mock<IGoogleSheetApi> _googleSheetApiMock;
        private readonly Mock<IConvertResponseToSheet<GoogleSheetResponse>> _sheetConverterMock;

        public GoogleSheetProviderSpecs()
        {
            _googleSheetApiMock = new Mock<IGoogleSheetApi>();

            _googleSheetApiMock
                .Setup(g => g.GetSheetAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new GoogleSheetResponse()));

            var sheetData = new SheetBuilder()
                .AddHeaders("header1", "header2")
                .AddRow(r => r
                    .AddCell(c => c.WithValue("FirstRowFirstColumn").Build())
                    .AddCell(c => c.WithValue("FirstRowSecondColumn").Build())
                    .Build(0))
                .AddRow(r => r
                    .AddCell(c => c.WithValue("SecondRowFirstColumn").Build())
                    .AddCell(c => c.WithValue("SecondRowSecondColumn").Build())
                    .Build(1))
                .Build();

            _sheetConverterMock = new Mock<IConvertResponseToSheet<GoogleSheetResponse>>();

            _sheetConverterMock.Setup(s => s.Convert(It.IsAny<GoogleSheetResponse>())).Returns(sheetData);
        }

        [Fact]
        public async void GivenGettingSheet_WhenGettingData_ThenConvertedDataIsReturned()
        {
            var provider = new SheetProvider(_googleSheetApiMock.Object, _sheetConverterMock.Object);

            var sheet = await provider.GetAsync("1", "A1:A2", "someKey");

            sheet.Rows.Count.Should().Be(3);
            sheet.Rows.First().Cells.Count.Should().Be(2);

            //headers
            sheet.Rows.First().Cells.First().Value.Should().Be("header1");
            sheet.Rows.First().Cells.Skip(1).First().Value.Should().Be("header2");

            //first row
            sheet.Rows.Skip(1).First().Cells.First().Value.Should().Be("FirstRowFirstColumn");
            sheet.Rows.Skip(1).First().Cells.Skip(1).First().Value.Should().Be("FirstRowSecondColumn");

            //second row
            sheet.Rows.Skip(2).First().Cells.First().Value.Should().Be("SecondRowFirstColumn");
            sheet.Rows.Skip(2).First().Cells.Skip(1).First().Value.Should().Be("SecondRowSecondColumn");
        }
    }
}
