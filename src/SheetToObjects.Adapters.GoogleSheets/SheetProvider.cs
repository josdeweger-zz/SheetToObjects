using System.Threading.Tasks;
using Refit;
using SheetToObjects.Lib;
using Sheet = SheetToObjects.Lib.Sheet;

namespace SheetToObjects.Adapters.GoogleSheets
{
    public class SheetProvider : IProvideSheet
    {
        private const string GoogleSheetsUrl = "https://sheets.googleapis.com/v4";

        private readonly IGoogleSheetApi _googleSheetApi;
        private readonly IConvertResponseToSheet<GoogleSheetResponse> _googleSheetAdapter;

        internal SheetProvider(
            IGoogleSheetApi googleSheetApi,
            IConvertResponseToSheet<GoogleSheetResponse> googleSheetAdapter)
        {
            _googleSheetApi = googleSheetApi;
            _googleSheetAdapter = googleSheetAdapter;
        }

        public SheetProvider() : this(RestService.For<IGoogleSheetApi>(GoogleSheetsUrl), new GoogleSheetAdapter()) { }

        public async Task<Sheet> GetAsync(string sheetId, string range, string apiKey)
        {
            var sheetDataResponse = await _googleSheetApi.GetSheetAsync(sheetId, range, apiKey);

            return _googleSheetAdapter.Convert(sheetDataResponse);
        }
    }
}