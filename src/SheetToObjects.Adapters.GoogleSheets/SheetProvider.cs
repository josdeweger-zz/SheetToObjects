using System.Threading.Tasks;
using Refit;
using SheetToObjects.Adapters.GoogleSheets.Shared.Models;
using SheetToObjects.Lib;

namespace SheetToObjects.Adapters.GoogleSheets
{
    public class SheetProvider : IProvideSheet
    {
        private const string GoogleSheetsUrl = "https://sheets.googleapis.com/v4";

        private readonly IGoogleSheetApi _googleSheetApi;
        private readonly IConvertResponseToSheet<GoogleSheetResponse> _googleSheetConverter;

        internal SheetProvider(
            IGoogleSheetApi googleSheetApi,
            IConvertResponseToSheet<GoogleSheetResponse> googleSheetConverter)
        {
            _googleSheetApi = googleSheetApi;
            _googleSheetConverter = googleSheetConverter;
        }

        public SheetProvider() : this(RestService.For<IGoogleSheetApi>(GoogleSheetsUrl), new GoogleSheetAdapter()) { }

        public async Task<Sheet> GetAsync(string sheetId, string range, string apiKey)
        {
            var sheetDataResponse = await _googleSheetApi.GetSheetAsync(sheetId, range, apiKey);

            return _googleSheetConverter.Convert(sheetDataResponse);
        }
    }
}