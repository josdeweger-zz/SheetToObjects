using SheetToObjects.ConsoleApp.Models;
using SheetToObjects.Lib.FluentConfiguration;

namespace SheetToObjects.ConsoleApp.SheetToObjectConfigs
{
    public class SuperstoreConfig : SheetToObjectConfig<Superstore>
    {
        public SuperstoreConfig()
        {
            CreateMap(x => x
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
                .MapColumn(column => column.WithHeader("Profit").IsRequired().MapTo(m => m.Profit)));
        }
    }
}
