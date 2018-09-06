using System.Collections.Generic;
using Google.Apis.Sheets.v4.Data;

namespace SheetToObjects.Specs.Builders
{
    internal class ProtectedGoogleSheetDataResponseBuilder
    {  
        private readonly IList<IList<object>> _values = new List<IList<object>>();

        public ProtectedGoogleSheetDataResponseBuilder WithRow(List<object> row)
        {
            _values.Add(row);
            return this;
        }

        public ValueRange Build()
        {
            return new ValueRange
            {
                Values = _values
            };
        }
    }
}
