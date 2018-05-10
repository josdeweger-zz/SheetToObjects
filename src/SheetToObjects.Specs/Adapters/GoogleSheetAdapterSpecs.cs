using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using SheetToObjects.Adapters.GoogleSheets;
using SheetToObjects.Specs.Builders;
using Xunit;

namespace SheetToObjects.Specs.Adapters
{
    public class GoogleSheetAdapterSpecs
    {
        [Fact]
        public void GivenNoResponseData_WhenConvertingToSheet_ArgumentExceptionIsThrown()
        {
            var converter = new GoogleSheetAdapter();

            Action result = () => converter.Convert(null);

            result.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void GivenEmptyResponseData_WhenConvertingToSheet_NoCellsAreCreated()
        {
            var responseData = new GoogleSheetResponse();

            var converter = new GoogleSheetAdapter();

            var result = converter.Convert(responseData);

            result.Rows.Should().BeEmpty();
        }

        [Fact]
        public void GivenResponseDataContainsRow_WhenConvertingToSheet_CellsInRowAreCreated()
        {
            var rowZeroColumnAValue = "row";
            var rowZeroColumnBValue = "0";
            var rowZeroColumnCValue = "text";

            var responseData = new GoogleSheetDataResponseBuilder()
                .WithRow(new List<string>{ rowZeroColumnAValue, rowZeroColumnBValue, rowZeroColumnCValue })
                .Build();

            var converter = new GoogleSheetAdapter();

            var result = converter.Convert(responseData);

            result.Rows.Should().NotBeEmpty();
            result.Rows.Single().Cells.Should().Contain(c => c.Value.ToString() == rowZeroColumnAValue);
            result.Rows.Single().Cells.Should().Contain(c => c.Value.ToString() == rowZeroColumnBValue);
            result.Rows.Single().Cells.Should().Contain(c => c.Value.ToString() == rowZeroColumnCValue);
        }

        [Fact]
        public void GivenResponseDataContainsCell_WhenConvertingToSheet_CellShouldHaveCorrectColumnName()
        {
            var responseData = new GoogleSheetDataResponseBuilder()
                .WithRow(new List<string> { "myValue" })
                .Build();

            var converter = new GoogleSheetAdapter();

            var result = converter.Convert(responseData);

            result.Rows.Single().Cells.Should().NotBeEmpty();
            result.Rows.Single().Cells.Should().Contain(c => c.ColumnIndex == 0);
        }

        [Fact]
        public void GivenResponseDataContainsCell_WhenConvertingToSheet_CellShouldHaveCorrectRowNumber()
        {
            var responseData = new GoogleSheetDataResponseBuilder()
                .WithRow(new List<string> { "myValue" })
                .Build();

            var converter = new GoogleSheetAdapter();

            var result = converter.Convert(responseData);

            result.Rows.Single().Cells.Should().NotBeEmpty();
            result.Rows.Single().Cells.Should().Contain(c => c.RowIndex == 0);
        }
    }
}
