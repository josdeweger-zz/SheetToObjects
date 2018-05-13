namespace SheetToObjects.Lib
{
    public class ValidationError
    {
        private const string CellNotFoundMessage = "Cell not found";
        public string CellValue { get; }
        public string ColumnName { get; }
        public int ColumnIndex { get; }
        public int RowIndex { get; }
        public string ErrorMessage { get; }
        public string PropertyName { get; }

        private ValidationError(int columnIndex, int rowIndex, string errorMessage, string columnName, string cellValue, string propertyName)
        {
            ColumnIndex = columnIndex;
            RowIndex = rowIndex;
            ErrorMessage = errorMessage;
            ColumnName = columnName;
            CellValue = cellValue;
            PropertyName = propertyName;
        }

        public static ValidationError CellNotFoundError(int columnIndex, int rowIndex, string columnName, string propertyName)
        {
            return new ValidationError(columnIndex, rowIndex, CellNotFoundMessage, columnName, string.Empty, propertyName);
        }

        public static ValidationError ParseValueError(int columnIndex, int rowIndex, string errorMessage, string columnName, string cellValue, string propertyName)
        {
            return new ValidationError(columnIndex, rowIndex, errorMessage, columnName, cellValue, propertyName);
        }

        public static ValidationError DoesNotMatchRuleError(int columnIndex, int rowIndex, string errorMessage, string columnName, string cellValue, string propertyName)
        {
            return new ValidationError(columnIndex, rowIndex, errorMessage, columnName, cellValue, propertyName);
        }
    }
}