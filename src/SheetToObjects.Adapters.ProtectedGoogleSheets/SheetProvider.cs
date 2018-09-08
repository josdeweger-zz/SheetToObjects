using System.IO;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using SheetToObjects.Adapters.ProtectedGoogleSheets.Interfaces;
using SheetToObjects.Lib;
using Sheet = SheetToObjects.Lib.Sheet;

namespace SheetToObjects.Adapters.ProtectedGoogleSheets
{
    public class SheetProvider : IProvideSheet
    {
        readonly string[] _scopes = { SheetsService.Scope.Spreadsheets };

        private readonly IConvertResponseToSheet<ValueRange> _googleSheetConverter;

        internal SheetProvider(IConvertResponseToSheet<ValueRange> googleSheetConverter)
        {
            _googleSheetConverter = googleSheetConverter;
        }

        public SheetProvider() : this(new ProtectedGoogleSheetAdapter()) { }

        public async Task<Sheet> GetAsync(string jsonFile, string appName, string sheetId, string range)
        {
            if (!File.Exists(jsonFile)) throw new FileNotFoundException("File does not exist", jsonFile);

            GoogleCredential credential;
            using (var stream = new FileStream(jsonFile, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(_scopes);
            }

            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = appName,
            });

            SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(sheetId, range);
            var sheetDataResponse = await request.ExecuteAsync();

            return _googleSheetConverter.Convert(sheetDataResponse);
        }
    }
}
