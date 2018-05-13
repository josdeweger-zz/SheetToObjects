using System;
using System.Linq;

namespace SheetToObjects.Core
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static bool IsNotNullOrEmpty(this string value)
        {
            return !IsNullOrEmpty(value);
        }

        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static bool IsNotNullOrWhiteSpace(this string value)
        {
            return !IsNullOrWhiteSpace(value);
        }

        /*
         * Converts a Excel Column name to its corresponding index
         * Example 1: 'A' equals 0      (1 * 1) - 1 = 0 
         * Example 2: 'Z' equals 25     (26 * 1) - 1 = 25
         * Example 3: 'AA' equals 26    (26 * 1) + (1 * 1) - 1 = 26
         * Example 4: 'CZ' equals 103   (3 * 26) + (26 * 1) - 1 = 103
         */
        public static int ConvertExcelColumnNameToIndex(this string value)
        {
            return value
                .Select((character, index) => CalculateAlphabetNumber(character) * CalculateMultiplier(value, index))
                .Sum() -1;
        }

        /*
         * Calculate the character number in the alphabet, between 1 and 26
         * Example 1: 'C' equals 3   ('C' - 'A' + 1 equals 67 - 65 + 1 = 3)
         */
        private static int CalculateAlphabetNumber(char character)
        {
            return character - 'A' + 1; 
        }

        /*
         * Calculate the multiplier for the number in the alphabet
         * Example 1: the multiplier 'C' in 'CZ' equals 26 * (2 - 0 - 1) = 26
         * Example 2: the multiplier 'Z' in 'CZ' equals 26 * (2 - 1 - 1) = 1
         */
        private static int CalculateMultiplier(string value, int index)
        {
            return (int)Math.Pow(26, value.Length - index - 1); 
        }
    }
}