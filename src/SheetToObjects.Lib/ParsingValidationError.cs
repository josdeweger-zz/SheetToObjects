namespace SheetToObjects.Lib
{
    public class ParsingValidationError : IValidationError
    {
        private const string CellNotFoundMessage = "Cell not found";
        private const string CouldNotParseValueMessage = "Could not parse value";
        private const string ValueTypeCanNotBeNullMessage = "Value type can not be null";

        public string CellValue { get; }
        public string ColumnName { get; }
        public int ColumnIndex { get; }
        public int RowIndex { get; }
        public string ErrorMessage { get; }
        public string PropertyName { get; }

        private ParsingValidationError(int columnIndex, int rowIndex, string columnName, string propertyName, string cellValue, string errorMessage)
        {
            ColumnIndex = columnIndex;
            RowIndex = rowIndex;
            ColumnName = columnName;
            PropertyName = propertyName;
            CellValue = cellValue;
            ErrorMessage = errorMessage;
        }

        public static IValidationError CellNotFound(int columnIndex, int rowIndex, string columnName, string propertyName)
        {
            return new ParsingValidationError(columnIndex, rowIndex, columnName, propertyName, string.Empty, CellNotFoundMessage);
        }

        public static IValidationError CouldNotParseValue(int columnIndex, int rowIndex, string columnName, string propertyName)
        {
            return new ParsingValidationError(columnIndex, rowIndex, columnName, propertyName, string.Empty, CouldNotParseValueMessage);
        }

        public static IValidationError ValueTypeCanNotBeNull(int columnIndex, int rowIndex, string columnName, string propertyName)
        {
            return new ParsingValidationError(columnIndex, rowIndex, columnName, propertyName, string.Empty, ValueTypeCanNotBeNullMessage);
        }
    }
}