namespace SheetToObjects.Lib
{
    public class ValidationError
    {
        public string ColumnName { get; set; }

        public int ColumnIndex { get; }
        public int RowIndex { get; }
        public string ErrorMessage { get; }

        public ValidationError(int columnIndex, int rowIndex, string errorMessage, string columnName)
        {
            ColumnIndex = columnIndex;
            RowIndex = rowIndex;
            ErrorMessage = errorMessage;
            ColumnName = columnName;
        }
    }
}