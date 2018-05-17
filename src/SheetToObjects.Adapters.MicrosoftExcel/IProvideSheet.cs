using System.IO;
using SheetToObjects.Lib;

namespace SheetToObjects.Adapters.MicrosoftExcel
{
    public interface IProvideSheet
    {
        Sheet Get(string excelPath, string sheetName, ExcelRange range);
        Sheet Get(Stream fileStream, string sheetName, ExcelRange range);
    }
}
