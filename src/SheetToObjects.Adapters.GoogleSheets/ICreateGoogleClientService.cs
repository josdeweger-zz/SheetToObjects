namespace SheetToObjects.Adapters.GoogleSheets
{
    public interface ICreateGoogleClientService
    {
        ISheetsServiceWrapper Create(string authenticationJsonFilePath, string documentName);
    }
}