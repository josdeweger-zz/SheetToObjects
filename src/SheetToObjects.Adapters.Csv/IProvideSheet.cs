using System.IO;
using SheetToObjects.Lib;

namespace SheetToObjects.Adapters.Csv
{
    public interface IProvideSheet
    {
        Sheet GetFromBase64Encoded(string base64EncodedFile, char delimiter);

        Sheet GetFromPath(string csvPath, char delimiter);

        Sheet GetFromStream(Stream stream, char delimiter);
    }
}
