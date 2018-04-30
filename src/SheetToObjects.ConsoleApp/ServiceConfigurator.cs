using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SheetToObjects.Adapters.Csv;
using SheetToObjects.Adapters.GoogleSheets;
using SheetToObjects.ConsoleApp.Models;
using SheetToObjects.Lib;
using SheetToObjects.Lib.ValueParsers;

namespace SheetToObjects.ConsoleApp
{
    public static class ServiceConfigurator
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IGenerateColumnLetters, ColumnLetterGenerator>();
            serviceCollection.AddTransient<IConvertResponseToSheet<GoogleSheetResponse>, GoogleSheetAdapter>();
            serviceCollection.AddTransient<IProvideCsv, CsvProvider>();
            serviceCollection.AddTransient<IConvertResponseToSheet<CsvData>, CsvAdapter>();
            serviceCollection.AddTransient<IParseValues, ValueParser>();
            serviceCollection.AddSingleton<IMapSheetToObjects>(ctx =>
            {
                var sheetMapper = new SheetMapper();

                sheetMapper.AddConfig(cfg => cfg
                        .For<EpicTrackingModel>()
                        .Column("A").MapTo(m => m.SprintNumber)
                        .Column("B").MapTo(m => m.SprintName)
                        .Column("C").MapTo(m => m.StoryPointsCompleted)
                        .Column("D").MapTo(m => m.TotalCompleted)
                        .Column("E").MapTo(m => m.ForecastNormal)
                        .Column("F").MapTo(m => m.ForecastHigh)
                        .Column("G").MapTo(m => m.ForecastLow)
                        .Column("H").MapTo(m => m.Scope)
                        .Configure());

                sheetMapper.AddConfig(cfg => cfg
                    .For<ProfileModel>()
                    .Column("A").IsRequired().MapTo(m => m.Email)
                    .Column("B").IsRequired().MapTo(m => m.Gender)
                    .Column("C").IsRequired().MapTo(m => m.FirstName)
                    .Column("D").MapTo(m => m.MiddleName)
                    .Column("E").IsRequired().MapTo(m => m.LastName)
                    .Column("F").IsRequired().MapTo(m => m.RelationNumber)
                    .Column("G").IsRequired().MapTo(m => m.LanguageCode)
                    .Column("H").IsRequired().MapTo(m => m.Label)
                    .Column("I").IsRequired().MapTo(m => m.Terms)
                    .Column("J").IsRequired().MapTo(m => m.ProfileType)
                    .Column("K").IsRequired().MapTo(m => m.IsVerified)
                    .Column("L").IsRequired().MapTo(m => m.RegistrationSource)
                    .Configure());

                return sheetMapper;
            });

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("app-settings.json", false)
                .Build();

            serviceCollection.AddOptions();
            serviceCollection.Configure<AppSettings>(configuration.GetSection("Configuration"));

            serviceCollection.AddTransient<GoogleSheetsApp>();
            serviceCollection.AddTransient<CsvApp>();

            return serviceCollection;
        }
    }
}
