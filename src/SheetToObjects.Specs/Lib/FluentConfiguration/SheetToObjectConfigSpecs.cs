using System;
using FluentAssertions;
using SheetToObjects.Lib;
using SheetToObjects.Lib.FluentConfiguration;
using SheetToObjects.Specs.Builders;
using SheetToObjects.Specs.TestModels;
using Xunit;

namespace SheetToObjects.Specs.Lib.FluentConfiguration
{
    public class SheetToObjectConfigSpecs
    {
        [Fact]
        public void GivenSheetMap_WhenColumnsAreProperlyConfigured_ModelCanBeMappedToSheet()
        {
            var sheetData = new SheetBuilder()
                .AddHeaders("Weight", "Age", "Married", "Gender", "FirstName", "DateOfBirth")
                .AddRow(r => r
                    .AddCell(c => c.WithColumnIndex(0).WithRowIndex(1).WithValue(80.5.ToString()).Build())
                    .AddCell(c => c.WithColumnIndex(1).WithRowIndex(1).WithValue(49.ToString()).Build())
                    .AddCell(c => c.WithColumnIndex(2).WithRowIndex(1).WithValue(true.ToString()).Build())
                    .AddCell(c => c.WithColumnIndex(3).WithRowIndex(1).WithValue(Gender.M.ToString()).Build())
                    .AddCell(c => c.WithColumnIndex(4).WithRowIndex(1).WithValue("Jan").Build())
                    .AddCell(c => c.WithColumnIndex(5).WithRowIndex(1).WithValue(new DateTime(1970, 1, 15).ToString("yyyy-MM-dd")).Build())
                    .Build(0))
                .Build();

            var result = new SheetMapper()
                .AddSheetToObjectConfig(new TestConfig())
                .Map<Test>(sheetData);

            result.IsSuccess.Should().BeTrue();
        }
    }

    public class TestConfig : SheetToObjectConfig<Test>
    {
        public TestConfig()
        {
            CreateMap(x => x
                .HasHeaders()
                .MapColumn(c => c.WithHeader("Weight").IsRequired().MapTo(m => m.Weight))
                .MapColumn(c => c.WithHeader("Age").IsRequired().MapTo(m => m.Age))
                .MapColumn(c => c.WithHeader("Married").IsRequired().MapTo(m => m.Married))
                .MapColumn(c => c.WithHeader("Gender").IsRequired().MapTo(m => m.Gender))
                .MapColumn(c => c.WithHeader("FirstName").MapTo(m => m.FirstName))
                .MapColumn(c => c.WithHeader("DateOfBirth").IsRequired().UsingFormat("yyyy-MM-dd").MapTo(m => m.DateOfBirth))
            );
        }
    }

    public class Test
    {
        public double Weight { get; set; }
        public int Age { get; set; }
        public bool Married { get; set; }
        public Gender Gender { get; set; }
        public string FirstName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}