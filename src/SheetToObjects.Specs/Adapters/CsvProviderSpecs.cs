using System;
using System.IO;
using System.Linq;
using System.Text;
using FluentAssertions;
using SheetToObjects.Adapters.Csv;
using Xunit;

namespace SheetToObjects.Specs.Adapters
{
    public class CsvProviderSpecs
    {
        [Fact]
        public void GivenInvalidBase64EncodedString_WhenLoadingCsvData_ItThrows()
        {
            var base64EncodedCsv = "some invalid base64encoded string";

            var provider = new CsvAdapter(new CsvToSheetConverter());
            Action result = () => provider.GetFromBase64Encoded(base64EncodedCsv, ';');

            result.Should().Throw<FormatException>();
        }

        [Fact]
        public void GivenBase64EncodedString_WhenLoadingCsvData_SheetContainsData()
        {
            var base64EncodedCsv = "Y29sdW1uMTtjb2x1bW5zMg0Kb25lOzENCnR3bzsy";

            var provider = new CsvAdapter(new CsvToSheetConverter());
            var csvData = provider.GetFromBase64Encoded(base64EncodedCsv, ';');

            csvData.Rows.Count.Should().Be(3);
            csvData.Rows.First().Cells.Count.Should().Be(2);
        }

        [Fact]
        public void GivenNonExistentPath_WhenLoadingCsvData_ItThrows()
        {
            var provider = new CsvAdapter(new CsvToSheetConverter());

            Action result = () => provider.GetFromPath(@"/some/non/existing/file", ';');

            result.Should().Throw<DirectoryNotFoundException>();
        }

        [Fact]
        public void GivenCsvFileOnDisk_WhenLoadingCsvData_SheetContainsData()
        {
            var provider = new CsvAdapter(new CsvToSheetConverter());

            var sheet = provider.GetFromPath(@"./TestFiles/test.csv", ';');

            sheet.Rows.Count.Should().Be(3);
            sheet.Rows.First().Cells.Count.Should().Be(2);
        }

        [Fact]
        public void GivenAStream_WhenLoadingCsvData_SheetContainsData()
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var writer = new StreamWriter(memoryStream, Encoding.UTF8, 1024, true))
                {
                    writer.WriteLine("columnn1;column2");
                    writer.WriteLine("one;1");
                    writer.WriteLine("two;2");

                    writer.Flush();
                    memoryStream.Position = 0;

                    using (var sr = new StreamReader(memoryStream, Encoding.UTF8,false, 1024, true))
                    {
                        var provider = new CsvAdapter(new CsvToSheetConverter());
                        var csvData = provider.GetFromStream(sr.BaseStream, ';');

                        csvData.Rows.Count.Should().Be(3);
                        csvData.Rows.First().Cells.Count.Should().Be(2);
                    }
                }
            }
        }
    }
}
