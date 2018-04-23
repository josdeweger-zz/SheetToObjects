using System.Collections.Generic;
using System.Linq;
using Moq;
using SheetToObjects.Lib;

namespace SheetToObjects.Specs.Builders
{
    public class ColumnLetterGeneratorBuilder
    {
        private readonly Mock<IGenerateColumnLetters> _columnLetterGenerator = new Mock<IGenerateColumnLetters>();

        public ColumnLetterGeneratorBuilder WithColumnLetters(params string[] columnLetters)
        {
            _columnLetterGenerator.Setup(c => c.Generate(It.IsAny<int>())).Returns(columnLetters.ToList());
            return this;
        }

        public Mock<IGenerateColumnLetters> Build()
        {
            return _columnLetterGenerator;
        }
    }
}
