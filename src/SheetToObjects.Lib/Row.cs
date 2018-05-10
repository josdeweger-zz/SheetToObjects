using System.Collections.Generic;
using System.Linq;

namespace SheetToObjects.Lib
{
    public class Row
    {
        public List<Cell> Cells { get; }

        public Row(List<Cell> cells)
        {
            Cells = cells;
        }

        public Cell GetCellByColumnIndex(int columnIndex)
        {
            return Cells.FirstOrDefault(c =>c.ColumnIndex.Equals(columnIndex));
        }
    }
}