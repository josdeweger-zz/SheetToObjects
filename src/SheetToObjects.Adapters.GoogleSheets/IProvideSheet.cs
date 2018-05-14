using System.Threading.Tasks;
using SheetToObjects.Lib;

namespace SheetToObjects.Adapters.GoogleSheets
{
    public interface IProvideSheet
    {
        Task<Sheet> GetAsync(string sheetId, string range, string apiKey);
    }
}
