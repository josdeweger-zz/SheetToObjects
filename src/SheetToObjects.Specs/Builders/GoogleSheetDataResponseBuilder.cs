using System.Collections.Generic;
using SheetToObjects.Adapters.GoogleSheets;

namespace SheetToObjects.Specs.Builders
{
    internal class GoogleSheetDataResponseBuilder
    {
        private readonly List<IList<string>> _values = new List<IList<string>>();

        public GoogleSheetDataResponseBuilder WithRow(IList<string> row)
        {
            _values.Add(row);
            return this;
        }

        public GoogleSheetResponse Build()
        {
            return new GoogleSheetResponse
            {
                Values = _values
            };
        }
    }
}
 