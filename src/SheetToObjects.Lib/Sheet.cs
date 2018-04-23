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

    public class Row
    {
        public List<Cell> Cells { get; }

        public Row(List<Cell> cells)
        {
            Cells = cells;
        }
    }
}

