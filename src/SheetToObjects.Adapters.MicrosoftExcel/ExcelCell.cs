using System;
using System.Text.RegularExpressions;
using SheetToObjects.Core;

namespace SheetToObjects.Adapters.MicrosoftExcel
{
    public class ExcelCell
    {
        public int ColumnNumber { get; }
        public int RowNumber { get; }

        public ExcelCell(string columnName, int rowNumber)
        {
            if (!Regex.IsMatch(columnName, @"^[a-zA-Z]+$"))
                throw new ArgumentException("Column name can only contain letters");

            if (rowNumber <= 0)
                throw new ArgumentException("Row numbers must be at least 1");

            ColumnNumber = columnName.ConvertExcelColumnNameToIndex() + 1;
            RowNumber = rowNumber;
        }
    }
}