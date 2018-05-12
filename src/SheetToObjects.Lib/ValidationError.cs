namespace SheetToObjects.Lib
{
    public class ValidationError
    {
        public string CellValue { get; }
        public string ColumnName { get; }
        public int? ColumnIndex { get; }
        public int? RowIndex { get; }
        public string ErrorMessage { get; }
        public string PropertyName { get; }

        public ValidationError(int? columnIndex, int? rowIndex, string errorMessage, string columnName, string cellValue, string propertyName)
        {
            ColumnIndex = columnIndex;
            RowIndex = rowIndex;
            ErrorMessage = errorMessage;
            ColumnName = columnName;
            CellValue = cellValue;
            PropertyName = propertyName;
        }
    }
}