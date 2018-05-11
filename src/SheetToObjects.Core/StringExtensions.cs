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

        public static int ConvertExcelColumnNameToIndex(this string value)
        {
            return value.Select((c, i) =>
                ((c - 'A' + 1) * ((int)Math.Pow(26, value.Length - i - 1)))).Sum() -1;
        }
    }
}