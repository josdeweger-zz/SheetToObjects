namespace SheetToObjects.Lib
{
    public interface IValidationError
    {
        string CellValue { get; }
        string ColumnName { get; }
        int ColumnIndex { get; }
        int RowIndex { get; }
        string ErrorMessage { get; }
        string PropertyName { get; }
    }
}