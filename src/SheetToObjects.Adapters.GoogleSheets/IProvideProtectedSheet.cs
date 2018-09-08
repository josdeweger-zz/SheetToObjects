using System.Threading.Tasks;
using SheetToObjects.Lib;

namespace SheetToObjects.Adapters.GoogleSheets
{
    public interface IProvideProtectedSheet
    {
        Task<Sheet> GetAsync(string authenticationJsonFilePath, string documentName, string sheetId, string range);
    }
}