using System.Threading.Tasks;
using Refit;

namespace SheetToObjects.Adapters.GoogleSheets
{
    public interface IGoogleSheetApi
    {
        [Get("/spreadsheets/{sheetId}/values/{range}")]
        Task<GoogleSheetResponse> GetSheetAsync(string sheetId, string range, [AliasAs("key")] string apiKey);
    }
}
