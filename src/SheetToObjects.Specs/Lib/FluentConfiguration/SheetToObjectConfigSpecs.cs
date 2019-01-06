using System;
using System.Globalization;
using FluentAssertions;
using SheetToObjects.Lib;
using SheetToObjects.Lib.FluentConfiguration;
using SheetToObjects.Specs.Builders;
using Xunit;

namespace SheetToObjects.Specs.Lib.FluentConfiguration
{
    public class SheetToObjectConfigSpecs
    {
        [Fact]
        public void GivenSheetMap_WhenColumnsAreProperlyConfigured_ModelCanBeMappedToSheet()
        {
            var sheetData = new SheetBuilder()
                .AddHeaders("Weight")
                .AddRow(r => r
                    .AddCell(c => c.WithColumnIndex(0).WithRowIndex(1).WithValue(80.5.ToString(CultureInfo.InvariantCulture)).Build()).Build(0))
                .Build();

            var result = new SheetMapper()
                .AddSheetToObjectConfig(new TestConfig())
                .Map<Test>(sheetData);

            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void GivenSheetMap_WhenConfigurationIsNotSet_ItThrows()
        {
            var sheetData = new SheetBuilder()
                .AddHeaders("Weight", "Age", "Married", "Gender", "FirstName", "DateOfBirth")
                .AddRow(r => r
                    .AddCell(c => c.WithColumnIndex(0).WithRowIndex(1).WithValue(80.5.ToString()).Build())
                    .Build(0))
                .Build();

            Action result = () =>
            {
                new SheetMapper()
                    .AddSheetToObjectConfig(new EmptyConfig())
                    .Map<Test>(sheetData);
            };

            result.Should().Throw<ArgumentException>();
        }
    }

    public class TestConfig : SheetToObjectConfig
    {
        public TestConfig()
        {
            CreateMap<Test>(x => x
                .HasHeaders()
                .MapColumn(c => c.WithHeader("Weight").IsRequired().MapTo(m => m.Weight))
            );
        }
    }

    public class EmptyConfig : SheetToObjectConfig
    {

    }

    public class Test
    {
        public double Weight { get; set; }
    }
}