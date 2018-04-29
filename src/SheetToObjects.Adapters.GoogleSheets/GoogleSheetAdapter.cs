using System;
using System.Collections.Generic;
using System.Linq;
using SheetToObjects.Core;
using SheetToObjects.Lib;

namespace SheetToObjects.Adapters.GoogleSheets
{
    public class GoogleSheetAdapter : IConvertResponseToSheet<GoogleSheetResponse>
    {
        private readonly IGenerateColumnLetters _columnLettersGenerator;

        public GoogleSheetAdapter(IGenerateColumnLetters columnLettersGenerator)
        {
            _columnLettersGenerator = columnLettersGenerator;
        }

        public Sheet Convert(GoogleSheetResponse googleSheetData)
        {
            if(googleSheetData.IsNull())
                throw new ArgumentException(nameof(googleSheetData));

            if(!googleSheetData.Values.Any())
                return new Sheet(new List<Row>());

            var maxNrOfColumns = googleSheetData.Values.Select(v => v.Count).OrderByDescending(v => v).First();
            var columnLetters = _columnLettersGenerator.Generate(maxNrOfColumns);
            var cells = googleSheetData.Values.ToRows(columnLetters);

            return new Sheet(cells);
        }

        
    }
}
