namespace SheetToObjects.Lib.Configuration.ColumnMappings
{
    public interface IUseHeaderRow
    {
        string ColumnName { get; }

        void SetColumnIndex(int columnIndex);
    }
}
