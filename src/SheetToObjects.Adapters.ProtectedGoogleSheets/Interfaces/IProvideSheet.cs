using System.Threading.Tasks;
using SheetToObjects.Lib;

namespace SheetToObjects.Adapters.ProtectedGoogleSheets.Interfaces
{
    public interface IProvideSheet
    {
        Task<Sheet> GetAsync(string jsonFile, string appName, string sheetId, string range);
    }
}
