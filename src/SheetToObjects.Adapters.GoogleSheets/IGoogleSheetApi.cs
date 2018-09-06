using System.Threading.Tasks;
using Refit;
using SheetToObjects.Adapters.GoogleSheets.Shared.Models;

namespace SheetToObjects.Adapters.GoogleSheets
{
    internal interface IGoogleSheetApi
    {
        [Get("/spreadsheets/{sheetId}/values/{range}")]
        Task<GoogleSheetResponse> GetSheetAsync(string sheetId, string range, [AliasAs("key")] string apiKey);
    }
}
