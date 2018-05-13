using System.Threading.Tasks;
using Refit;
using SheetToObjects.Lib;

namespace SheetToObjects.Adapters.GoogleSheets
{
    public class SheetProvider : IProvideSheet
    {
        private readonly IConvertResponseToSheet<GoogleSheetResponse> _googleSheetConverter;

        public SheetProvider(IConvertResponseToSheet<GoogleSheetResponse> googleSheetConverter)
        {
            _googleSheetConverter = googleSheetConverter;
        }

        public async Task<Sheet> GetAsync(string sheetId, string range, string apiKey)
        {
            var sheetDataResponse = await RestService
                .For<IGoogleSheetApi>(apiKey)
                .GetSheetAsync(sheetId, range, apiKey);

            return _googleSheetConverter.Convert(sheetDataResponse);
        }
    }
}