using System.Threading.Tasks;
using Google.Apis.Sheets.v4.Data;

namespace SheetToObjects.Adapters.ProtectedGoogleSheets
{
    public interface ISheetsServiceWrapper
    {
        Task<ValueRange> Get(string sheetId, string range);
    }
}