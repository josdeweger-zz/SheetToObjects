using System.Linq;
using FluentAssertions;
using SheetToObjects.Lib;
using Xunit;

namespace SheetToObjects.Specs.Domain
{
    public class ColumnLetterGeneratorSpecs
    {
        [Fact]
        public void WhenGeneratingFiveColumnLetters_ItReturnsABCDE()
        {
            var result = new ColumnLetterGenerator().Generate(5);

            result.Should().ContainInOrder("A", "B", "C", "D", "E");
        }

        [Fact]
        public void WhenGeneratingTwentySixColumnLetters_ItReturnsTheAlphabet()
        {
            var result = new ColumnLetterGenerator().Generate(26);

            result.Should().ContainInOrder(
                "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", 
                "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z");
        }

        [Theory]
        [InlineData(27, "AA")]
        [InlineData(52, "AZ")]
        [InlineData(53, "BA")]
        [InlineData(78, "BZ")]
        [InlineData(79, "CA")]
        [InlineData(104, "CZ")]
        public void WhenGeneratingColumnLetters_TheLastCharacterMatches(int maxNrOfColumns, string columnLetter)
        {
            var result = new ColumnLetterGenerator().Generate(maxNrOfColumns);

            result.Last().Should().Be(columnLetter);
        }
    }
}
