using System;
using System.Collections.Generic;
using System.Linq;
using SheetToObjects.Core;
using SheetToObjects.Lib;

namespace SheetToObjects.Adapters.Csv
{
    public class CsvAdapter : IConvertResponseToSheet<CsvData>
    {
        private readonly IGenerateColumnLetters _columnLettersGenerator;

        public CsvAdapter(IGenerateColumnLetters columnLettersGenerator)
        {
            _columnLettersGenerator = columnLettersGenerator;
        }

        public Sheet Convert(CsvData csvData)
        {
            if(csvData.IsNull())
                throw new ArgumentException(nameof(csvData));

            if (!csvData.Values.Any())
                return new Sheet(new List<Row>());

            var maxNrOfColumns = csvData.Values.Select(v => v.Count).OrderByDescending(v => v).First();
            var columnLetters = _columnLettersGenerator.Generate(maxNrOfColumns);
            var cells = csvData.Values.ToRows(columnLetters);

            return new Sheet(cells);
        }


    }
}
