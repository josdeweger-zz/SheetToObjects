namespace SheetToObjects.Lib.FluentConfiguration
{
    internal interface IUseHeaderRow
    {
        string ColumnName { get; }

        void SetColumnIndex(int columnIndex);
    }
}
