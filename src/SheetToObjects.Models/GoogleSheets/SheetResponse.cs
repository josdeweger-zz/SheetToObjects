using System.Collections.Generic;

namespace SheetToObjects.Models.GoogleSheets
{
    public class SheetResponse
    {
        public string Range { get; set; }
        public string MajorDimension { get; set; }
        public List<List<string>> Values { get; set; }

        public SheetResponse()
        {
            Values = new List<List<string>>();
        }
    }
}