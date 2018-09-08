using System.Threading.Tasks;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;

namespace SheetToObjects.Adapters.GoogleSheets
{
    public class SheetsServiceWrapper : ISheetsServiceWrapper
    {
        private readonly SheetsService _sheetsService;

        public SheetsServiceWrapper(SheetsService sheetsService)
        {
            _sheetsService = sheetsService;
        }

        public async Task<ValueRange> Get(string sheetId, string range)
        {
            return await _sheetsService.Spreadsheets.Values.Get(sheetId, range).ExecuteAsync();
        }
    }
}