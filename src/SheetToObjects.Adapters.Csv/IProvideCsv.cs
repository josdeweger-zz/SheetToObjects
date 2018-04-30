namespace SheetToObjects.Adapters.Csv
{
    public interface IProvideCsv
    {
        CsvData Get(string csvPath, char delimiter);
    }
}
