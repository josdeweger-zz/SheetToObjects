using System.Threading.Tasks;
using SheetToObjects.Lib;

namespace SheetToObjects.Adapters.ProtectedGoogleSheets
{
    public interface IProvideSheet
    {
        Task<Sheet> GetAsync(string authenticationJsonFilePath, string documentName, string sheetId, string range);
    }
}