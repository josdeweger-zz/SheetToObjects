using System;
using System.Collections.Generic;
using System.Linq;
using SheetToObjects.Core;
using SheetToObjects.Lib;
using SheetToObjects.Lib.Extensions;
using Sheet = SheetToObjects.Lib.Sheet;

namespace SheetToObjects.Adapters.GoogleSheets
{
    internal class GoogleSheetToSheetConverter : IConvertDataToSheet<GoogleSheetResponse>
    {
        public Sheet Convert(GoogleSheetResponse googleSheetData)
        {
            if (googleSheetData.IsNull())
                throw new ArgumentException(nameof(googleSheetData));

            if (!googleSheetData.Values.Any())
                return new Sheet(new List<Row>());

            var cells = googleSheetData.Values.ToRows();

            return new Sheet(cells);
        }
    }
}
