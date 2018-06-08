namespace SheetToObjects.Lib.Validation
{
    public class RuleValidationError : IValidationError
    {
        private const string ValueRequiredMessage = "This cell is required";
        private const string HasToHaveMinimumValueOfMessage = "This cell should have minimum value of {0}";
        private const string CanHaveMaximumValueOfMessage = "This cell can have maximum value of {0}";
        private const string ValueDoesNotMatchRegexMessage = "Value {0} does not match pattern {1}";
        private const string CouldNotValidateValueWithPatternMessage = "Value could not be validated by regex '{0}'";

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

        public static IValidationError CellValueRequired(int columnIndex, int rowIndex, string columnName, string propertyName)
        {
            return new RuleValidationError(columnIndex, rowIndex, ValueRequiredMessage, columnName, string.Empty,
                propertyName);
        }

        public static IValidationError HasToHaveMinimumValueOf<T>(int columnIndex, int rowIndex, string columnName, string propertyName, T minValue)
        {
            return new RuleValidationError(columnIndex, rowIndex, string.Format(HasToHaveMinimumValueOfMessage, minValue),
                columnName, string.Empty, propertyName);
        }

        public static IValidationError CanHaveMaximumValueOf<T>(int columnIndex, int rowIndex, string columnName, string propertyName, T maxValue)
        {
            return new RuleValidationError(columnIndex, rowIndex, string.Format(CanHaveMaximumValueOfMessage, maxValue),
                columnName, string.Empty, propertyName);
        }

        public static IValidationError ValueDoesNotMatchRegex(int columnIndex, int rowIndex, string columnName, string propertyName, string value, string pattern)
        {
            return new RuleValidationError(columnIndex, rowIndex,
                string.Format(ValueDoesNotMatchRegexMessage, value, pattern), columnName, value, propertyName);
        }

        public static IValidationError CouldNotValidateValueWithPattern(int columnIndex, int rowIndex, string columnName, string propertyName, string pattern)
        {
            return new RuleValidationError(columnIndex, rowIndex,
                string.Format(CouldNotValidateValueWithPatternMessage, pattern), columnName, string.Empty,
                propertyName);
        }
    }
}