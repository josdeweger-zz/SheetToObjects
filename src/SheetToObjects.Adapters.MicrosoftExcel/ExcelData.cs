using System.Collections.Generic;

namespace SheetToObjects.Adapters.MicrosoftExcel
{
    internal class ExcelData
    {
        public List<List<string>> Values { get; set; }

        public ExcelData()
        {
            Values = new List<List<string>>();
        }
    }
}