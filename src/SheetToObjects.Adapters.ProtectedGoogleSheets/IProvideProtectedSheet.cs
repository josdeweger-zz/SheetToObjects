using System.Threading.Tasks;
using SheetToObjects.Lib;

namespace SheetToObjects.Adapters.ProtectedGoogleSheets
{
    public interface IProvideProtectedSheet
    {
        Task<Sheet> GetAsync(string authenticationJsonFilePath, string documentName, string sheetId, string range);
    }
}