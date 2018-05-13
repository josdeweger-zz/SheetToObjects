using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SheetToObjects.Adapters.Csv
{
    public class CsvProvider : IProvideCsv
    {
        public CsvData Get(string csvPath, char delimiter)
        {
            var data = File.ReadAllLines(csvPath)
                .Select(line => line.Split(delimiter).ToList())
                .ToList();

            return new CsvData { Values = data };
        }

        public CsvData Get(Stream stream, char delimiter)
        {
            var lines = new List<string>();
            using (var reader = new StreamReader(stream))
            {
                while(!reader.EndOfStream)
                {
                    lines.Add(reader.ReadLine());
                }
            }

            var data = lines
                .Select(line => line.Split(delimiter).ToList())
                .ToList();

            return new CsvData { Values = data };
        }
    }
}
