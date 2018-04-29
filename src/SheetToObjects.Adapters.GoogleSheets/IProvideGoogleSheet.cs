using System.Threading.Tasks;
using Refit;

namespace SheetToObjects.Adapters.GoogleSheets
{
    public interface IProvideGoogleSheet
    {
        [Get("/spreadsheets/{sheetId}/values/{range}")]
        Task<GoogleSheetResponse> GetSheet(string sheetId, string range, [AliasAs("key")] string apiKey);
    }
}
