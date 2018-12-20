using System.Threading.Tasks;
using Google.Apis.Sheets.v4.Data;
using SheetToObjects.Lib;
using Sheet = SheetToObjects.Lib.Sheet;

namespace SheetToObjects.Adapters.ProtectedGoogleSheets
{
    public class ProtectedGoogleSheetAdapter : IProvideProtectedSheet
    {
        private readonly ICreateGoogleClientService _googleClientServiceCreator;
        private readonly IConvertDataToSheet<ValueRange> _protectedGoogleSheetAdapter;

        internal ProtectedGoogleSheetAdapter(
            ICreateGoogleClientService googleClientServiceCreator,
            IConvertDataToSheet<ValueRange> protectedGoogleSheetAdapter)
        {
            _googleClientServiceCreator = googleClientServiceCreator;
            _protectedGoogleSheetAdapter = protectedGoogleSheetAdapter;
        }

        public ProtectedGoogleSheetAdapter() : this(new GoogleClientServiceFactory(), new ProtectedGoogleSheetToSheetConverter()) { }
        
        public async Task<Sheet> GetAsync(string authenticationJsonFilePath, string documentName, string sheetId, string range)
        {
            var service = _googleClientServiceCreator.Create(authenticationJsonFilePath, documentName);

            var sheetDataResponse = await service.Get(sheetId, range);

            return _protectedGoogleSheetAdapter.Convert(sheetDataResponse);
        }
    }
}