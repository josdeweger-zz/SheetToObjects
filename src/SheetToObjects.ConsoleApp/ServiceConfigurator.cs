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

                sheetMapper.AddConfigFor<EpicTrackingModel>(cfg => cfg
                    .DataHasHeaders()
                    .WithColumns(columns => columns
                        .Add(column => column.WithLetter("A").MapTo(m => m.SprintNumber))
                        .Add(column => column.WithLetter("B").MapTo(m => m.SprintName))
                        .Add(column => column.WithLetter("C").MapTo(m => m.StoryPointsCompleted))
                        .Add(column => column.WithLetter("D").MapTo(m => m.TotalCompleted))
                        .Add(column => column.WithLetter("E").MapTo(m => m.ForecastNormal))
                        .Add(column => column.WithLetter("F").MapTo(m => m.ForecastHigh))
                        .Add(column => column.WithLetter("G").MapTo(m => m.ForecastLow))
                        .Add(column => column.WithLetter("H").MapTo(m => m.Scope))));

                sheetMapper.AddConfigFor<ProfileModel>(cfg => cfg
                    .DataHasHeaders()
                    .WithColumns(columns => columns
                        .Add(column => column.WithLetter("A").IsRequired().MapTo(m => m.Email))
                        .Add(column => column.WithLetter("B").IsRequired().MapTo(m => m.Gender))
                        .Add(column => column.WithLetter("C").IsRequired().MapTo(m => m.FirstName))
                        .Add(column => column.WithLetter("D").MapTo(m => m.MiddleName))
                        .Add(column => column.WithLetter("E").IsRequired().MapTo(m => m.LastName))
                        .Add(column => column.WithLetter("F").IsRequired().MapTo(m => m.RelationNumber))
                        .Add(column => column.WithLetter("G").IsRequired().MapTo(m => m.LanguageCode))
                        .Add(column => column.WithLetter("H").IsRequired().MapTo(m => m.Label))
                        .Add(column => column.WithLetter("I").IsRequired().MapTo(m => m.Terms))
                        .Add(column => column.WithLetter("J").IsRequired().MapTo(m => m.ProfileType))
                        .Add(column => column.WithLetter("K").IsRequired().MapTo(m => m.IsVerified))
                        .Add(column => column.WithLetter("L").IsRequired().MapTo(m => m.RegistrationSource))));

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
