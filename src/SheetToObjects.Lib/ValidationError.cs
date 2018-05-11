namespace SheetToObjects.Lib
{
    public class ValidationError
    {
        public int ColumnIndex { get; }
        public int RowIndex { get; }
        public string ErrorMessage { get; }

        public ValidationError(int columnIndex, int rowIndex, string errorMessage)
        {
            ColumnIndex = columnIndex;
            RowIndex = rowIndex;
            ErrorMessage = errorMessage;
        }
    }
}