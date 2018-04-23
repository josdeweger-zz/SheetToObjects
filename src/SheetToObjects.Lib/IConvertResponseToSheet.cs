namespace SheetToObjects.Lib
{
    public interface IConvertResponseToSheet<in TResponse>
    {
        Sheet Convert(TResponse sheetData);
    }
}