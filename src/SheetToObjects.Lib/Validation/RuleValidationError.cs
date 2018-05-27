namespace SheetToObjects.Lib.Validation
{
    public class RuleValidationError : IValidationError
    {
        private const string ValueRequiredMessage = "This cell is required";

        public string CellValue { get; }
        public string ColumnName { get; }
        public int ColumnIndex { get; }
        public int RowIndex { get; }
        public string ErrorMessage { get; }
        public string PropertyName { get; }

        private RuleValidationError(int columnIndex, int rowIndex, string errorMessage, string columnName, string cellValue, string propertyName)
        {
            ColumnIndex = columnIndex;
            RowIndex = rowIndex;
            ErrorMessage = errorMessage;
            ColumnName = columnName;
            CellValue = cellValue;
            PropertyName = propertyName;
        }
        
        public static IValidationError DoesNotMatchRule(int columnIndex, int rowIndex, string errorMessage, string columnName, string cellValue, string propertyName)
        {
            return new RuleValidationError(columnIndex, rowIndex, errorMessage, columnName, cellValue, propertyName);
        }

        public static IValidationError CellValueRequired(int columnIndex, int rowIndex, string columnName, string propertyName)
        {
            return new RuleValidationError(columnIndex, rowIndex, ValueRequiredMessage, columnName, string.Empty, propertyName);
        }
    }
}