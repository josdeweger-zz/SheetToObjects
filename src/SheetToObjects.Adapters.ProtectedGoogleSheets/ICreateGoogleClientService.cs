namespace SheetToObjects.Adapters.ProtectedGoogleSheets
{
    public interface ICreateGoogleClientService
    {
        ISheetsServiceWrapper Create(string authenticationJsonFilePath, string documentName);
    }
}