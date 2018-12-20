using System;
using System.Collections.Generic;

namespace SheetToObjects.Adapters.GoogleSheets.Shared.Models
{
    internal class GoogleSheetResponse
    {
        public string Range { get; set; }
        public string MajorDimension { get; set; }
        public IList<IList<string>> Values { get; set; }

        public GoogleSheetResponse()
        {
            Values = new List<IList<string>>();
        }
    }
}
