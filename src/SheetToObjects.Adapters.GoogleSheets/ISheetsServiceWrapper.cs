using System.Threading.Tasks;
using Google.Apis.Sheets.v4.Data;

namespace SheetToObjects.Adapters.GoogleSheets
{
    public interface ISheetsServiceWrapper
    {
        Task<ValueRange> Get(string sheetId, string range);
    }
}