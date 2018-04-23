namespace SheetToObjects.Lib
{
    public class Cell
    {
        public string ColumnLetter { get; }
        public int RowNumber { get; }
        public object Value { get; }

        public Cell(string columnLetter, int rowNumber, object value)
        {
            ColumnLetter = columnLetter;
            RowNumber = rowNumber;
            Value = value;
        }
    }
}