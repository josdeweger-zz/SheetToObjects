using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SheetToObjects.Extensions.Microsoft.DependencyInjection;

namespace SheetToObjects.Examples
{
    public static class ServiceConfigurator
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<Adapters.ProtectedGoogleSheets.IProvideSheet, Adapters.ProtectedGoogleSheets.ProtectedGoogleSheetAdapter>();
            serviceCollection.AddTransient<Adapters.Csv.IProvideSheet, Adapters.Csv.CsvAdapter>();
            serviceCollection.AddTransient<Adapters.GoogleSheets.IProvideSheet, Adapters.GoogleSheets.GoogleSheetAdapter>();
            serviceCollection.AddTransient<Adapters.MicrosoftExcel.IProvideSheet, Adapters.MicrosoftExcel.SheetProvider>();

            serviceCollection.AddSheetToObjects(Assembly.GetExecutingAssembly());

            serviceCollection.AddOptions();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("app-settings.json", false)
                .Build();

            serviceCollection.Configure<AppSettings>(configuration.GetSection("Configuration"));

            serviceCollection.AddTransient<ExcelApp>();
            serviceCollection.AddTransient<GoogleSheetsApp>();
            serviceCollection.AddTransient<ProtectedGoogleSheetsApp>();
            serviceCollection.AddTransient<CsvAppWithValidationErrors>();
            serviceCollection.AddTransient<CsvAppWithoutValidationErrors>();

            return serviceCollection;
        }
    }
}
