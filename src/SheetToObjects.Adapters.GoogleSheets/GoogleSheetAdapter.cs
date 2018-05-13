using System;
using System.Collections.Generic;
using System.Linq;
using SheetToObjects.Core;
using SheetToObjects.Lib;

namespace SheetToObjects.Adapters.GoogleSheets
{
    internal class GoogleSheetAdapter : IConvertResponseToSheet<GoogleSheetResponse>
    {
        public Sheet Convert(GoogleSheetResponse googleSheetData)
        {
            if(googleSheetData.IsNull())
                throw new ArgumentException(nameof(googleSheetData));

            if(!googleSheetData.Values.Any())
                return new Sheet(new List<Row>());
            
            var cells = googleSheetData.Values.ToRows();

            return new Sheet(cells);
        }
    }
}
