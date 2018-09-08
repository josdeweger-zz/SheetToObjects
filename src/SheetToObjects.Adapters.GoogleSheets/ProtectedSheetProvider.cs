using System.Threading.Tasks;
using Google.Apis.Sheets.v4.Data;
using SheetToObjects.Lib;
using Sheet = SheetToObjects.Lib.Sheet;

namespace SheetToObjects.Adapters.GoogleSheets
{
    public class ProtectedSheetProvider : IProvideProtectedSheet
    {
        private readonly ICreateGoogleClientService _googleClientServiceCreator;
        private readonly IConvertResponseToSheet<ValueRange> _protectedGoogleSheetAdapter;

        internal ProtectedSheetProvider(
            ICreateGoogleClientService googleClientServiceCreator,
            IConvertResponseToSheet<ValueRange> protectedGoogleSheetAdapter)
        {
            _googleClientServiceCreator = googleClientServiceCreator;
            _protectedGoogleSheetAdapter = protectedGoogleSheetAdapter;
        }

        public ProtectedSheetProvider() : this(new GoogleClientServiceFactory(), new ProtectedGoogleSheetAdapter()) { }
        
        public async Task<Sheet> GetAsync(string authenticationJsonFilePath, string documentName, string sheetId, string range)
        {
            var service = _googleClientServiceCreator.Create(authenticationJsonFilePath, documentName);

            var sheetDataResponse = await service.Get(sheetId, range);

            return _protectedGoogleSheetAdapter.Convert(sheetDataResponse);
        }
    }
}