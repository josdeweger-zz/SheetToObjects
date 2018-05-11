using Microsoft.Extensions.DependencyInjection;
using SheetToObjects.Adapters.Csv;
using SheetToObjects.Adapters.GoogleSheets;
using SheetToObjects.ConsoleApp.Models;
using SheetToObjects.Lib;

namespace SheetToObjects.ConsoleApp
{
    public static class ServiceConfigurator
    {
        public const string EMAIL_REGEX =  @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
        @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";

        public static IServiceCollection ConfigureServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IConvertResponseToSheet<GoogleSheetResponse>, GoogleSheetAdapter>();
            serviceCollection.AddTransient<IProvideCsv, CsvProvider>();
            serviceCollection.AddTransient<IConvertResponseToSheet<CsvData>, CsvAdapter>();
            serviceCollection.AddTransient<IParseCellValue, CellValueParser>();
            serviceCollection.AddSingleton<IMapSheetToObjects>(ctx =>
            {
                var sheetMapper = new SheetMapper();
                /*
                sheetMapper.For<EpicTrackingModel>(cfg => cfg
                    .Columns(columns => columns
                        .Add(column => column.MapLetter("A").To(m => m.SprintNumber))
                        .Add(column => column.MapLetter("B").To(m => m.SprintName))
                        .Add(column => column.MapLetter("C").To(m => m.StoryPointsCompleted))
                        .Add(column => column.MapLetter("D").To(m => m.TotalCompleted))
                        .Add(column => column.MapLetter("E").To(m => m.ForecastNormal))
                        .Add(column => column.MapLetter("F").To(m => m.ForecastHigh))
                        .Add(column => column.MapLetter("G").To(m => m.ForecastLow))
                        .Add(column => column.MapLetter("H").To(m => m.Scope)))); */

                //sheetMapper.For<ProfileModel>(cfg => cfg.HasHeaders().Columns()
                
                
             //   ));


                return sheetMapper;
            });



            serviceCollection.AddOptions();
            

            //serviceCollection.AddTransient<GoogleSheetsApp>();
            serviceCollection.AddTransient<CsvApp>();

            return serviceCollection;
        }
    }
}
