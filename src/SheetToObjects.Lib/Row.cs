using System.Collections.Generic;
using System.Linq;


namespace SheetToObjects.Lib
{
    public class Row
    {
        public int RowIndex { get; }

        public List<Cell> Cells { get; }

        public Row(List<Cell> cells, int rowIndex)
        {
            Cells = cells;
            RowIndex = rowIndex;
        }

        public Cell GetCellByColumnIndex(int columnIndex)
        {
            return Cells.FirstOrDefault(c =>c.ColumnIndex.Equals(columnIndex));
        }
    }
}