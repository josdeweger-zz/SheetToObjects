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
    }
}
