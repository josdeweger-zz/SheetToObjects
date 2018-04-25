using System.Collections.Generic;

namespace SheetToObjects.Lib
{
    public class Sheet
    {
        public List<Row> Rows { get; }

        public Sheet(List<Row> rows)
        {
            Rows = rows;
        }
    }
}

