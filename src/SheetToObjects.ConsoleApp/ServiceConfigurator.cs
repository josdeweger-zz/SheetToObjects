using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SheetToObjects.Infrastructure.GoogleSheets;
using SheetToObjects.Lib;
using SheetToObjects.Lib.ValueParsers;

namespace SheetToObjects.ConsoleApp
{
    public static class ServiceConfigurator
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IGenerateColumnLetters, ColumnLetterGenerator>();
            serviceCollection.AddTransient<IConvertResponseToSheet<GoogleSheetResponse>, GoogleSheetsConverter>();
            serviceCollection.AddTransient<IParseValues, ValueParser>();
            serviceCollection.AddSingleton<IMapSheetToObjects>(ctx =>
            {
                return SheetMapper.Create(cfg => cfg
                        .For<EpicTrackingModel>()
                        .Column("A").MapTo(m => m.SprintNumber)
                        .Column("B").MapTo(m => m.SprintName)
                        .Column("C").MapTo(m => m.StoryPointsCompleted)
                        .Column("D").MapTo(m => m.TotalCompleted)
                        .Column("E").MapTo(m => m.ForecastNormal)
                        .Column("F").MapTo(m => m.ForecastHigh)
                        .Column("G").MapTo(m => m.ForecastLow)
                        .Column("H").MapTo(m => m.Scope)
                        .Build());
            });

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
