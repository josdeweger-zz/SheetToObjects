using SheetToObjects.Core;

namespace SheetToObjects.Lib
{
    public class Cell
    {
        private readonly object _value;

        public int ColumnIndex { get; }
        public int RowIndex { get; }
        public object Value => _value ?? string.Empty;

        public Cell(int columnIndex, int rowIndex, object value)
        {
            ColumnIndex = columnIndex;
            RowIndex = rowIndex;
            _value = value;
        }
    }
}