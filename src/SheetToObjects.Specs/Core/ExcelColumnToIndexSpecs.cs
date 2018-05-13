using FluentAssertions;
using SheetToObjects.Core;
using Xunit;

namespace SheetToObjects.Specs.Core
{
    public class ExcelColumnToIndexSpecs
    {
        [Theory]
        [InlineData(0, "A")]
        [InlineData(1, "B")]
        [InlineData(26, "AA")]
        [InlineData(51, "AZ")]
        [InlineData(52, "BA")]
        [InlineData(77, "BZ")]
        [InlineData(78, "CA")]
        [InlineData(103, "CZ")]
        public void WhenConvertingExcelColumnNameToIndex(int columnIndex, string columnLetter)
        {
            var result = columnLetter.ConvertExcelColumnNameToIndex();

            result.Should().Be(columnIndex);
        }
    }
}
