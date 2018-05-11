namespace SheetToObjects.Lib.Configuration
{
    public interface IUseHeaderRow
    {
        string ColumnName { get; }

        void SetColumnIndex(int columnIndex);
    }
}
