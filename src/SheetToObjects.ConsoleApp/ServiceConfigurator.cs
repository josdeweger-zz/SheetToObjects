using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SheetToObjects.Infrastructure.GoogleSheets;
using SheetToObjects.Lib;

namespace SheetToObjects.ConsoleApp
{
    public static class ServiceConfigurator
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IGenerateColumnLetters, ColumnLetterGenerator>();
            serviceCollection.AddTransient<IConvertResponseToSheet<GoogleSheetResponse>, GoogleSheetsConverter>();
            serviceCollection.AddTransient<IMapSheetToObjects, SheetMapper>();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("app-settings.json", false)
                .Build();

            serviceCollection.AddOptions();
            serviceCollection.Configure<AppSettings>(configuration.GetSection("Configuration"));

            serviceCollection.AddTransient<App>();

            return serviceCollection;
        }
    }
}
