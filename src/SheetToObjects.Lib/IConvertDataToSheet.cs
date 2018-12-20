namespace SheetToObjects.Lib
{
    public interface IConvertDataToSheet<in TData>
    {
        Sheet Convert(TData sheetData);
    }
}