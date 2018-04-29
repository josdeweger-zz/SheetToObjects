using System;
using SheetToObjects.Lib;

namespace SheetToObjects.Adapters.MicrosoftExcel
{
    public class Adapter : IConvertResponseToSheet<ExcelSheet>
    {
        public Sheet Convert(ExcelSheet sheetData)
        {
            throw new NotImplementedException();
        }
    }
}
