using System.Threading.Tasks;
using Refit;
using SheetToObjects.Lib;
using Sheet = SheetToObjects.Lib.Sheet;

namespace SheetToObjects.Adapters.GoogleSheets
{
    public class GoogleSheetAdapter : IProvideSheet
    {
        private const string GoogleSheetsUrl = "https://sheets.googleapis.com/v4";

        private readonly IGoogleSheetApi _googleSheetApi;
        private readonly IConvertDataToSheet<GoogleSheetResponse> _googleSheetAdapter;

        internal GoogleSheetAdapter(
            IGoogleSheetApi googleSheetApi,
            IConvertDataToSheet<GoogleSheetResponse> googleSheetAdapter)
        {
            _googleSheetApi = googleSheetApi;
            _googleSheetAdapter = googleSheetAdapter;
        }

        public GoogleSheetAdapter() : this(RestService.For<IGoogleSheetApi>(GoogleSheetsUrl), new GoogleSheetToSheetConverter()) { }

        public async Task<Sheet> GetAsync(string sheetId, string range, string apiKey)
        {
            var sheetDataResponse = await _googleSheetApi.GetSheetAsync(sheetId, range, apiKey);

            return _googleSheetAdapter.Convert(sheetDataResponse);
        }
    }
}