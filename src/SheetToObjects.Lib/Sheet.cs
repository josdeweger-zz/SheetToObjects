using System;
using System.Collections.Generic;
using System.Linq;

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

        public Cell GetCellByColumnLetter(string columnLetter)
        {
            return Cells.FirstOrDefault(c =>
                c.ColumnLetter.Equals(columnLetter, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}

