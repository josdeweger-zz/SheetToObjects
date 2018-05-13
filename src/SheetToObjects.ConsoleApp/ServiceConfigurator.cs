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
            serviceCollection.AddTransient<IConvertResponseToSheet<GoogleSheetResponse>, GoogleSheetAdapter>();
            serviceCollection.AddTransient<IProvideCsv, CsvProvider>();
            serviceCollection.AddTransient<IConvertResponseToSheet<CsvData>, CsvAdapter>();
            serviceCollection.AddTransient<IParseValue, ValueParser>();
            serviceCollection.AddSingleton<IMapSheetToObjects>(ctx =>
            {
                var sheetMapper = new SheetMapper();

                sheetMapper.For<EpicTrackingModel>(cfg => cfg
                    .Columns(columns => columns
                        .Add(column => column.WithColumnLetter("A").MapTo(m => m.SprintNumber))
                        .Add(column => column.WithColumnLetter("B").MapTo(m => m.SprintName))
                        .Add(column => column.WithColumnLetter("C").MapTo(m => m.StoryPointsCompleted))
                        .Add(column => column.WithColumnLetter("D").MapTo(m => m.TotalCompleted))
                        .Add(column => column.WithColumnLetter("E").MapTo(m => m.ForecastNormal))
                        .Add(column => column.WithColumnLetter("F").MapTo(m => m.ForecastHigh))
                        .Add(column => column.WithColumnLetter("G").MapTo(m => m.ForecastLow))
                        .Add(column => column.WithColumnLetter("H").MapTo(m => m.Scope)))
                    .BuildConfig()
                );

                return sheetMapper;
            });
            
            serviceCollection.AddOptions();
            serviceCollection.AddTransient<CsvApp>();

            return serviceCollection;
        }
    }
}
