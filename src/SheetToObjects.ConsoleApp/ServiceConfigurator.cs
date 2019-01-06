using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SheetToObjects.Adapters.ProtectedGoogleSheets;
using SheetToObjects.ConsoleApp.SheetToObjectConfigs;
using SheetToObjects.Lib;

namespace SheetToObjects.ConsoleApp
{
    public static class ServiceConfigurator
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<Adapters.Csv.IProvideSheet, Adapters.Csv.CsvAdapter>();
            serviceCollection.AddTransient<ISheetsServiceWrapper, SheetsServiceWrapper>();
            serviceCollection.AddTransient<ICreateGoogleClientService, GoogleClientServiceFactory>();
            serviceCollection.AddTransient<IProvideProtectedSheet, ProtectedGoogleSheetAdapter>();
            serviceCollection.AddTransient<Adapters.GoogleSheets.IProvideSheet, Adapters.GoogleSheets.GoogleSheetAdapter>();
            serviceCollection.AddTransient<Adapters.MicrosoftExcel.IProvideSheet, Adapters.MicrosoftExcel.SheetProvider>();

            serviceCollection.AddTransient<IMapSheetToObjects>(ctx => new SheetMapper()
                .AddSheetToObjectConfig(new EpicTrackingConfig())
                .AddSheetToObjectConfig(new SuperstoreConfig()));

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
