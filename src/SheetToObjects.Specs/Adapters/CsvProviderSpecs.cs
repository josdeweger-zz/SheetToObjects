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
        public void GivenACsvFileOnDisk_whenLoadingCsvData_ThenCsvDataShouldContainData()
        {
            var provider = new SheetProvider(new CsvAdapter());

            var sheet = provider.Get(@"./test.csv", ';');

            sheet.Rows.Count.Should().Be(3);
            sheet.Rows.First().Cells.Count.Should().Be(2);
        }

        [Fact]
        public void GivenAStream_whenLoadingCsvData_thenCsvDatashouldContainData()
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
                        var provider = new SheetProvider(new CsvAdapter());
                        var csvData = provider.Get(sr.BaseStream, ';');

                        csvData.Rows.Count.Should().Be(3);
                        csvData.Rows.First().Cells.Count.Should().Be(2);
                    }
                }
            }
        }
    }
}
