using System;
using System.Collections.Generic;
using System.Linq;

namespace SheetToObjects.Lib
{
    public class ColumnLetterGenerator : IGenerateColumnLetters
    {
        private readonly IEnumerable<string> _alphabet = Enumerable.Range('A', 26).Select(x => ((char) x).ToString());

        public List<string> Generate(int maxNrOfColumns)
        {
            var columnLetterList = new List<string>();
            var nrOfAlphabets = (int) Math.Ceiling((double) maxNrOfColumns / 26);
            var nrOfColumns = 0;

            for (var i = 1; i <= nrOfAlphabets; i++)
            {
                var prefixLetter = GetPrefixLetter(i);

                for (var j = 1; j <= 26; j++)
                {
                    if (nrOfColumns >= maxNrOfColumns)
                        return columnLetterList;

                    columnLetterList.Add($"{prefixLetter}{_alphabet.ElementAt(j - 1)}");
                    nrOfColumns++;
                }
            }

            return columnLetterList;
        }

        private string GetPrefixLetter(int i)
        {
            return i == 1
                ? string.Empty
                : _alphabet.ElementAt(i - 2);
        }
    }
}