using System.Collections.Generic;
using SheetToObjects.Adapters.Csv;

namespace SheetToObjects.Specs.Builders
{
    public class CsvDataBuilder
    {
        private readonly List<List<string>> _values = new List<List<string>>();

        public CsvDataBuilder WithRow(List<string> row)
        {
            _values.Add(row);
            return this;
        }

        public CsvData Build()
        {
            return new CsvData { Values = _values };
        }
    }
}
