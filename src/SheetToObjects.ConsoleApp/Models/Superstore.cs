using System;

namespace SheetToObjects.ConsoleApp.Models
{
    public class Superstore
    {
        public int RowId { get; set; }
        public string OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ShipDate { get; set; }
        public ShipMode ShipMode { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public Segment Segment { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int PostalCode { get; set; }
        public Region Region { get; set; }
        public string ProductId { get; set; }
        public Category Category { get; set; }
        public string ProductName { get; set; }
        public decimal Sales { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }
        public decimal Profit { get; set; }
    }

    public enum ShipMode
    {
        SameDay,
        FirstClass,
        SecondClass,
        StandardClass
    }

    public enum Segment
    {
        Consumer,
        Corporate,
        HomeOffice
    }

    public enum Region
    {
        Central,
        North,
        East,
        South,
        West
    }

    public enum Category
    {
        Furniture,
        OfficeSupplies,
        Technology
    }
}