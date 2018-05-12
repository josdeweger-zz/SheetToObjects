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
        public void GivenACsvFileOnDisk_whenLoadingCsvData_thenCsvDatashouldContainData()
        {
            CsvProvider provider = new CsvProvider();

            var csvData = provider.Get(@"./test.csv", ';');

            csvData.Values.Count.Should().Be(3);
            csvData.Values.First().Count.Should().Be(2);
        }

        [Fact]
        public void GivenAStream_whenLoadingCsvData_thenCsvDatashouldContainData()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(memoryStream, Encoding.UTF8, 1024, true))
                {
                    writer.WriteLine("columnn1;column2");
                    writer.WriteLine("one;1");
                    writer.WriteLine("two;2");

                    writer.Flush();
                    memoryStream.Position = 0;
                    using (var sr = new StreamReader(memoryStream, Encoding.UTF8,false, 1024, true))
                    {
                        CsvProvider provider = new CsvProvider();
                        var csvData = provider.Get(sr.BaseStream, ';');

                        csvData.Values.Count.Should().Be(3);
                        csvData.Values.First().Count.Should().Be(2);
                        
                    }

                    
                }
            }
        }
    }
}
