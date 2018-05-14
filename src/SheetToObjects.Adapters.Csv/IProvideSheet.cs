using System.IO;
using SheetToObjects.Lib;

namespace SheetToObjects.Adapters.Csv
{
    public interface IProvideSheet
    {
        Sheet Get(string csvPath, char delimiter);

        Sheet Get(Stream stream, char delimiter);
    }
}
