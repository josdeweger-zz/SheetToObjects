using System.Threading.Tasks;
using Refit;

namespace SheetToObjects.Infrastructure.GoogleSheets
{
    public interface IProvideGoogleSheets
    {
        [Get("/spreadsheets/{sheetId}/values/{range}")]
        Task<GoogleSheetResponse> GetSheet(string sheetId, string range, [AliasAs("key")] string apiKey);
    }
}
