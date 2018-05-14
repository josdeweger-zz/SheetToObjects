using System.Collections.Generic;

namespace SheetToObjects.Adapters.Csv
{
    internal class CsvData
    {
        public List<List<string>> Values { get; set; }

        public CsvData()
        {
            Values = new List<List<string>>();
        }
    }
}