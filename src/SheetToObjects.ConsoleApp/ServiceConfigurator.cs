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
            serviceCollection.AddTransient<IParseValues, ValueParser>();
            serviceCollection.AddSingleton<IMapSheetToObjects>(ctx =>
            {
                var sheetMapper = new SheetMapper();

                sheetMapper.For<EpicTrackingModel>(cfg => cfg
                    .HasHeaders()
                    .Columns(columns => columns
                        .Add(column => column.Map("A").To(m => m.SprintNumber))
                        .Add(column => column.Map("B").To(m => m.SprintName))
                        .Add(column => column.Map("C").To(m => m.StoryPointsCompleted))
                        .Add(column => column.Map("D").To(m => m.TotalCompleted))
                        .Add(column => column.Map("E").To(m => m.ForecastNormal))
                        .Add(column => column.Map("F").To(m => m.ForecastHigh))
                        .Add(column => column.Map("G").To(m => m.ForecastLow))
                        .Add(column => column.Map("H").To(m => m.Scope))));

                sheetMapper.For<ProfileModel>(cfg => cfg
                    .HasHeaders()
                    .Columns(columns => columns
                        .Add(column => column.Map("A").IsRequired().To(m => m.Email))
                        .Add(column => column.Map("B").IsRequired().To(m => m.Gender))
                        .Add(column => column.Map("C").IsRequired().To(m => m.FirstName))
                        .Add(column => column.Map("D").To(m => m.MiddleName))
                        .Add(column => column.Map("E").IsRequired().To(m => m.LastName))
                        .Add(column => column.Map("F").IsRequired().To(m => m.RelationNumber))
                        .Add(column => column.Map("G").IsRequired().To(m => m.LanguageCode))
                        .Add(column => column.Map("H").IsRequired().To(m => m.Label))
                        .Add(column => column.Map("I").IsRequired().To(m => m.Terms))
                        .Add(column => column.Map("J").IsRequired().To(m => m.ProfileType))
                        .Add(column => column.Map("K").IsRequired().To(m => m.IsVerified))
                        .Add(column => column.Map("L").IsRequired().To(m => m.RegistrationSource))));

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
