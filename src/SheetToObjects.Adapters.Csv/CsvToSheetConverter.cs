using System;
using System.Collections.Generic;
using System.Linq;
using SheetToObjects.Core;
using SheetToObjects.Lib;

namespace SheetToObjects.Adapters.Csv
{
    internal class CsvToSheetConverter : IConvertDataToSheet<CsvData>
    {
        public Sheet Convert(CsvData csvData)
        {
            if(csvData.IsNull())
                throw new ArgumentException(nameof(csvData));

            if (!csvData.Values.Any())
                return new Sheet(new List<Row>());

            var cells = csvData.Values.ToRows();

            return new Sheet(cells);
        }


    }
}
