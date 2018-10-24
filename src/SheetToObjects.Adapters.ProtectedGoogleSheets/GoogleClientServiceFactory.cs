using System.IO;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;

namespace SheetToObjects.Adapters.ProtectedGoogleSheets
{
    public class GoogleClientServiceFactory : ICreateGoogleClientService
    {
        public ISheetsServiceWrapper Create(string authenticationJsonFilePath, string documentName)
        {
            if (!File.Exists(authenticationJsonFilePath))
                throw new FileNotFoundException("File does not exist", authenticationJsonFilePath);

            GoogleCredential credential;
            using (var stream = new FileStream(authenticationJsonFilePath, FileMode.Open, FileAccess.Read))
            {
                var scopes = new[] { SheetsService.Scope.Spreadsheets };
                credential = GoogleCredential.FromStream(stream).CreateScoped(scopes);
            }

            var sheetsService = new SheetsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = documentName,
            });

            return new SheetsServiceWrapper(sheetsService);
        }
    }
}