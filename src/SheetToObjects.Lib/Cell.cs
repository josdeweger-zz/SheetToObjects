namespace SheetToObjects.Lib
{
    public class Cell
    {
        public int ColumnIndex { get; }
        public int RowIndex { get; }
        public object Value { get; }

        public Cell(int columnIndex, int rowIndex, object value)
        {
            ColumnIndex = columnIndex;
            RowIndex = rowIndex;
            Value = value;
        }
    }
}