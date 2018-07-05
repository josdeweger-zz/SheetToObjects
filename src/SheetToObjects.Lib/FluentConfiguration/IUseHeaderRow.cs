namespace SheetToObjects.Lib.FluentConfiguration
{
    internal interface IUseHeaderRow
    {
        string ColumnName { get; }

        bool IsRequiredInHeaderRow { get; }

        void SetColumnIndex(int columnIndex);
    }
}
