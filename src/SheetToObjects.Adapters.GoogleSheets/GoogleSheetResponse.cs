using System.Collections.Generic;

namespace SheetToObjects.Adapters.GoogleSheets
{
    internal class GoogleSheetResponse
    {
        public string Range { get; set; }
        public string MajorDimension { get; set; }
        public List<List<string>> Values { get; set; }

        public GoogleSheetResponse()
        {
            Values = new List<List<string>>();
        }
    }
}