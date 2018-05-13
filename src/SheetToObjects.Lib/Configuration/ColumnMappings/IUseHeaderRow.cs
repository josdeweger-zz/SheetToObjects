namespace SheetToObjects.Lib.Configuration.ColumnMappings
{
    internal interface IUseHeaderRow
    {
        string ColumnName { get; }

        void SetColumnIndex(int columnIndex);
    }
}
