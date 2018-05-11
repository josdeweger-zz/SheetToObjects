using System.IO;

namespace SheetToObjects.Adapters.Csv
{
    public interface IProvideCsv
    {
        CsvData Get(string csvPath, char delimiter);

        CsvData Get(Stream stream, char delimiter);
    }
}
