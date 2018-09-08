using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SheetToObjects.ConsoleApp.Models;
using SheetToObjects.Lib;
using SheetToObjects.Lib.FluentConfiguration;

namespace SheetToObjects.ConsoleApp
{
    public static class ServiceConfigurator
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<Adapters.Csv.IProvideSheet, Adapters.Csv.CsvAdapter>();
            serviceCollection.AddTransient<Adapters.GoogleSheets.ISheetsServiceWrapper, Adapters.GoogleSheets.SheetsServiceWrapper>();
            serviceCollection.AddTransient<Adapters.GoogleSheets.ICreateGoogleClientService, Adapters.GoogleSheets.GoogleClientServiceFactory>();
            serviceCollection.AddTransient<Adapters.GoogleSheets.IProvideSheet, Adapters.GoogleSheets.GoogleSheetAdapter>();
            serviceCollection.AddTransient<Adapters.GoogleSheets.IProvideProtectedSheet, Adapters.GoogleSheets.ProtectedGoogleSheetAdapter>();
            serviceCollection.AddTransient<Adapters.MicrosoftExcel.IProvideSheet, Adapters.MicrosoftExcel.SheetProvider>();

            serviceCollection.AddTransient<IMapSheetToObjects>(ctx =>
            {
                var sheetMapper = new SheetMapper();

                sheetMapper
                    .AddConfigFor<EpicTrackingModel>(cfg => cfg
                        .MapColumn(column => column.WithColumnLetter("A").MapTo(m => m.SprintNumber))
                        .MapColumn(column => column.WithColumnLetter("B").MapTo(m => m.SprintName))
                        .MapColumn(column => column.WithColumnLetter("C").MapTo(m => m.StoryPointsCompleted))
                        .MapColumn(column => column.WithColumnLetter("D").MapTo(m => m.TotalCompleted))
                        .MapColumn(column => column.WithColumnLetter("E").MapTo(m => m.ForecastNormal))
                        .MapColumn(column => column.WithColumnLetter("F").MapTo(m => m.ForecastHigh))
                        .MapColumn(column => column.WithColumnLetter("G").MapTo(m => m.ForecastLow))
                        .MapColumn(column => column.WithColumnLetter("H").MapTo(m => m.Scope))
                    )
                    .AddConfigFor<SuperstoreModel>(cfg => cfg
                        .HasHeaders()
                        .MapColumn(column => column.WithHeader("Row ID").ShouldHaveUniqueValues().IsRequired().MapTo(m => m.RowId))
                        .MapColumn(column => column.WithHeader("Order ID").IsRequired().MapTo(m => m.OrderId))
                        .MapColumn(column => column.WithHeader("Order Date").IsRequired().UsingFormat("d-M-yyyy").MapTo(m => m.OrderDate))
                        .MapColumn(column => column.WithHeader("Ship Date").IsRequired().UsingFormat("d-M-yyyy").MapTo(m => m.ShipDate))
                        .MapColumn(column => column.WithHeader("Ship Mode").IsRequired().MapTo(m => m.ShipMode))
                        .MapColumn(column => column.WithHeader("Customer ID").IsRequired().MapTo(m => m.CustomerId))
                        .MapColumn(column => column.WithHeader("Customer Name").IsRequired().MapTo(m => m.CustomerName))
                        .MapColumn(column => column.WithHeader("Segment").IsRequired().MapTo(m => m.Segment))
                        .MapColumn(column => column.WithHeader("Country").IsRequired().MapTo(m => m.Country))
                        .MapColumn(column => column.WithHeader("City").IsRequired().MapTo(m => m.City))
                        .MapColumn(column => column.WithHeader("State").IsRequired().MapTo(m => m.State))
                        .MapColumn(column => column.WithHeader("Postal Code").IsRequired().MapTo(m => m.PostalCode))
                        .MapColumn(column => column.WithHeader("Region").IsRequired().MapTo(m => m.Region))
                        .MapColumn(column => column.WithHeader("Product ID").IsRequired().MapTo(m => m.ProductId))
                        .MapColumn(column => column.WithHeader("Category").IsRequired().MapTo(m => m.Category))
                        .MapColumn(column => column.WithHeader("Product Name").IsRequired().MapTo(m => m.ProductName))
                        .MapColumn(column => column.WithHeader("Sales").IsRequired().MapTo(m => m.Sales))
                        .MapColumn(column => column.WithHeader("Quantity").IsRequired().MapTo(m => m.Quantity))
                        .MapColumn(column => column.WithHeader("Discount").IsRequired().MapTo(m => m.Discount))
                        .MapColumn(column => column.WithHeader("Profit").IsRequired().MapTo(m => m.Profit))
                    );

                return sheetMapper;
            });

            serviceCollection.AddOptions();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("app-settings.json", false)
                .Build();

            serviceCollection.Configure<AppSettings>(configuration.GetSection("Configuration"));

            serviceCollection.AddTransient<CsvApp>();
            serviceCollection.AddTransient<ExcelApp>();
            serviceCollection.AddTransient<GoogleSheetsApp>();
            serviceCollection.AddTransient<ProtectedGoogleSheetsApp>();

            return serviceCollection;
        }
    }
}
