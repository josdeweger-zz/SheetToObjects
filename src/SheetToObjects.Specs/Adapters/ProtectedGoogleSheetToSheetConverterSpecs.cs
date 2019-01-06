using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Google.Apis.Sheets.v4.Data;
using SheetToObjects.Adapters.ProtectedGoogleSheets;
using SheetToObjects.Specs.Builders;
using Xunit;

namespace SheetToObjects.Specs.Adapters
{
    public class ProtectedGoogleSheetToSheetConverterSpecs
    {
        [Fact]
        public void GivenNoResponseData_WhenConvertingToSheet_ArgumentExceptionIsThrown()
        {
            var converter = new ProtectedGoogleSheetToSheetConverter();

            Action result = () => converter.Convert(null);

            result.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void GivenEmptyResponseData_WhenConvertingToSheet_NoCellsAreCreated()
        {
            var responseData = new ValueRange();

            var converter = new ProtectedGoogleSheetToSheetConverter();

            var result = converter.Convert(responseData);

            result.Rows.Should().BeEmpty();
        }

        [Fact]
        public void GivenResponseDataContainsRow_WhenConvertingToSheet_CellsInRowAreCreated()
        {
            var rowZeroColumnAValue = "row";
            var rowZeroColumnBValue = "0";
            var rowZeroColumnCValue = "text";

            var responseData = new ProtectedGoogleSheetDataResponseBuilder()
                .WithRow(new List<object> { rowZeroColumnAValue, rowZeroColumnBValue, rowZeroColumnCValue })
                .Build();

            var converter = new ProtectedGoogleSheetToSheetConverter();

            var result = converter.Convert(responseData);

            result.Rows.Should().NotBeEmpty();
            result.Rows.Single().Cells.Should().Contain(c => c.Value.ToString() == rowZeroColumnAValue);
            result.Rows.Single().Cells.Should().Contain(c => c.Value.ToString() == rowZeroColumnBValue);
            result.Rows.Single().Cells.Should().Contain(c => c.Value.ToString() == rowZeroColumnCValue);
        }

        [Fact]
        public void GivenResponseDataContainsCell_WhenConvertingToSheet_CellShouldHaveCorrectColumnName()
        {
            var responseData = new ProtectedGoogleSheetDataResponseBuilder()
                .WithRow(new List<object> { "myValue" })
                .Build();

            var converter = new ProtectedGoogleSheetToSheetConverter();

            var result = converter.Convert(responseData);

            result.Rows.Single().Cells.Should().NotBeEmpty();
            result.Rows.Single().Cells.Should().Contain(c => c.ColumnIndex == 0);
        }

        [Fact]
        public void GivenResponseDataContainsCell_WhenConvertingToSheet_CellShouldHaveCorrectRowNumber()
        {
            var responseData = new ProtectedGoogleSheetDataResponseBuilder()
                .WithRow(new List<object> { "myValue" })
                .Build();

            var converter = new ProtectedGoogleSheetToSheetConverter();

            var result = converter.Convert(responseData);

            result.Rows.Single().Cells.Should().NotBeEmpty();
            result.Rows.Single().Cells.Should().Contain(c => c.RowIndex == 0);
        }
    }
}
