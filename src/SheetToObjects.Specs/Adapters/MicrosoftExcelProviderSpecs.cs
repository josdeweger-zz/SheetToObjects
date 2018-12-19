using System;
using System.IO;
using System.Linq;
using System.Text;
using FluentAssertions;
using SheetToObjects.Adapters.MicrosoftExcel;
using Xunit;

namespace SheetToObjects.Specs.Adapters
{
    public class MicrosoftExcelProviderSpecs
    {
        private const string TwoColumnsTwoRowsWithHeadersFilePath = @"./TestFiles/two-columns_two-rows_with-header.xlsx";

        [Fact]
        public void GivenInvalidBase64EncodedString_WhenLoadingCsvData_ItThrows()
        {
            var base64EncodedCsv = "some invalid base64encoded string";

            var provider = new SheetProvider(new ExcelAdapter());
            var excelRange = new ExcelRange(new ExcelCell("A", 1), new ExcelCell("A", 2));

            Action result = () => provider.GetFromBase64Encoded(base64EncodedCsv, "my sheet", excelRange);

            result.Should().Throw<FormatException>();
        }

        [Fact]
        public void GivenBase64EncodedString_WhenLoadingExcelData_SheetContainsData()
        {
            var excelFileBytes = File.ReadAllBytes(TwoColumnsTwoRowsWithHeadersFilePath);
            var excelFilebase64Encoded = Convert.ToBase64String(excelFileBytes); ;

            var provider = new SheetProvider(new ExcelAdapter());
            var excelRange = new ExcelRange(new ExcelCell("A", 1), new ExcelCell("B", 3));

            var excelData = provider.GetFromBase64Encoded(excelFilebase64Encoded, "my sheet", excelRange);

            excelData.Rows.Count.Should().Be(3);
            excelData.Rows.First().Cells.Count.Should().Be(2);
        }

        [Fact]
        public void GivenNonExistentPath_WhenLoadingExcelData_ItThrows()
        {
            var provider = new SheetProvider(new ExcelAdapter());
            var excelRange = new ExcelRange(new ExcelCell("A", 1), new ExcelCell("B", 3));

            Action result = () => provider.GetFromPath(@"/some/non/existing/file", "my sheet", excelRange);

            result.Should().Throw<DirectoryNotFoundException>();
        }

        [Fact]
        public void GivenExcelFileOnDisk_WhenLoadingExcelData_SheetContainsData()
        {
            var provider = new SheetProvider(new ExcelAdapter());
            var excelRange = new ExcelRange(new ExcelCell("A", 1), new ExcelCell("B", 3));

            var sheet = provider.GetFromPath(TwoColumnsTwoRowsWithHeadersFilePath, "my sheet", excelRange);

            sheet.Rows.Count.Should().Be(3);
            sheet.Rows.First().Cells.Count.Should().Be(2);
        }

        [Fact]
        public void GivenAStream_WhenLoadingCsvData_SheetContainsData()
        {
            using (var fileStream = new FileStream(TwoColumnsTwoRowsWithHeadersFilePath, FileMode.Open))
            {
                using (var sr = new StreamReader(fileStream, Encoding.UTF8, false, 1024, true))
                {
                    var provider = new SheetProvider(new ExcelAdapter());
                    var excelRange = new ExcelRange(new ExcelCell("A", 1), new ExcelCell("B", 3));

                    var csvData = provider.GetFromStream(sr.BaseStream, "my sheet", excelRange);

                    csvData.Rows.Count.Should().Be(3);
                    csvData.Rows.First().Cells.Count.Should().Be(2);
                }
            }
        }

        [Theory]        
        [InlineData(true, 3)]
        [InlineData(false, 1048576)]

        public void GivenALargeNumberOfRows_WhenLoadingDataWithoutStopping_ReturnsMaxNumberOfRows(bool stopOnEmptyRow, int expectedCount)
        {
            int max_number_of_excel_rows = 1048576;

            using (var fileStream = new FileStream(TwoColumnsTwoRowsWithHeadersFilePath, FileMode.Open))
            {
                using (var sr = new StreamReader(fileStream, Encoding.UTF8, false, 1024, true))
                {
                    var provider = new SheetProvider(new ExcelAdapter());
                    var excelRange = new ExcelRange(new ExcelCell("A", 1), new ExcelCell("B", max_number_of_excel_rows));

                    var csvData = provider.GetFromStream(sr.BaseStream, "my sheet", excelRange, stopOnEmptyRow);

                    csvData.Rows.Count.Should().Be(expectedCount);
                    csvData.Rows.First().Cells.Count.Should().Be(2);
                }
            }
        }
    }
}
