using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Google.Apis.Sheets.v4.Data;
using Moq;
using SheetToObjects.Adapters.GoogleSheets;
using SheetToObjects.Lib;
using SheetToObjects.Specs.Builders;
using Xunit;

namespace SheetToObjects.Specs.Adapters
{
    public class GoogleProtectedSheetProviderSpecs
    {
        private readonly Mock<ICreateGoogleClientService> _googleClientServiceCreatorMock;
        private readonly Mock<IConvertResponseToSheet<ValueRange>> _googleSheetConverterMock;

        public GoogleProtectedSheetProviderSpecs()
        {
            var sheetServiceWrapperMock = new Mock<ISheetsServiceWrapper>();
            
            sheetServiceWrapperMock
                .Setup(s => s.Get(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new ValueRange()));

            _googleClientServiceCreatorMock = new Mock<ICreateGoogleClientService>();

            _googleClientServiceCreatorMock
                .Setup(g => g.Create(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(sheetServiceWrapperMock.Object);

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

            _googleSheetConverterMock = new Mock<IConvertResponseToSheet<ValueRange>>();
            _googleSheetConverterMock.Setup(s => s.Convert(It.IsAny<ValueRange>())).Returns(sheetData);
        }

        [Fact]
        public async void GivenGettingSheet_WhenGettingData_ThenConvertedDataIsReturned()
        {
            var provider = new ProtectedSheetProvider(_googleClientServiceCreatorMock.Object, _googleSheetConverterMock.Object);

            var sheet = await provider.GetAsync("my.json", "My Document", "some-guid", "A1:B3");

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
