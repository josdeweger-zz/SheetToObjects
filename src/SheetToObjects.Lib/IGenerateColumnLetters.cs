using System.Collections.Generic;

namespace SheetToObjects.Lib
{
    public interface IGenerateColumnLetters
    {
        List<string> Generate(int maxNrOfColumns);
    }
}