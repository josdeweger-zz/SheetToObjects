using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SheetToObjects.Adapters.Csv;
using SheetToObjects.Adapters.GoogleSheets;
using SheetToObjects.ConsoleApp.Models;
using SheetToObjects.Lib;

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
            serviceCollection.AddTransient<IParseCellValue, CellValueParser>();
            serviceCollection.AddSingleton<IMapSheetToObjects>(ctx =>
            {
                var sheetMapper = new SheetMapper();

                sheetMapper.For<EpicTrackingModel>(cfg => cfg
                    .Columns(columns => columns
                        .Add(column => column.WithHeader("Sprint #").MapTo(m => m.SprintNumber))
                        .Add(column => column.WithHeader("Sprint Naam").MapTo(m => m.SprintName))
                        .Add(column => column.WithHeader("Story Points Completed").MapTo(m => m.StoryPointsCompleted))
                        .Add(column => column.WithHeader("Total Completed").MapTo(m => m.TotalCompleted))
                        .Add(column => column.WithHeader("Forecast Normal").MapTo(m => m.ForecastNormal))
                        .Add(column => column.WithHeader("Forecast High").MapTo(m => m.ForecastHigh))
                        .Add(column => column.WithHeader("Forecast Low").MapTo(m => m.ForecastLow))
                        .Add(column => column.WithHeader("Scope").MapTo(m => m.Scope))));

                sheetMapper.For<ProfileModel>(cfg => cfg
                    .Columns(columns => columns
                        .Add(column => column.WithHeader("Email").IsRequired().MapTo(m => m.Email))
                        .Add(column => column.WithHeader("Gender").IsRequired().MapTo(m => m.Gender))
                        .Add(column => column.WithHeader("FirstName").IsRequired().MapTo(m => m.FirstName))
                        .Add(column => column.WithHeader("MiddleName").MapTo(m => m.MiddleName))
                        .Add(column => column.WithHeader("LastName").IsRequired().MapTo(m => m.LastName))
                        .Add(column => column.WithHeader("RelationNumber").IsRequired().MapTo(m => m.RelationNumber))
                        .Add(column => column.WithHeader("Language").IsRequired().MapTo(m => m.LanguageCode))
                        .Add(column => column.WithHeader("Label").IsRequired().MapTo(m => m.Label))
                        .Add(column => column.WithHeader("Terms").IsRequired().MapTo(m => m.Terms))
                        .Add(column => column.WithHeader("ProfileType").IsRequired().MapTo(m => m.ProfileType))
                        .Add(column => column.WithHeader("IsVerified").IsRequired().MapTo(m => m.IsVerified))
                        .Add(column => column.WithHeader("RegistrationSource").IsRequired().MapTo(m => m.RegistrationSource))));

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
