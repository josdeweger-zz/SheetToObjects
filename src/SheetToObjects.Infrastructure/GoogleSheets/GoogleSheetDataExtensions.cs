using System.Collections.Generic;
using System.Linq;
using SheetToObjects.Lib;

namespace SheetToObjects.Infrastructure.GoogleSheets
{
    public static class GoogleSheetDataExtensions
    {
        public static List<Row> ToRows(this List<List<string>> sheetData, List<string> columnLetters)
        {
            return sheetData.Select((rowData, rowIndex) => new Row(RowDataToCells(rowData, rowIndex, columnLetters))).ToList();
        }

        public static List<Cell> RowDataToCells(this List<string> rowData, int rowIndex, List<string> columnLetters)
        {
            return rowData.Select((cellData, columnIndex) => new Cell(columnLetters[columnIndex], rowIndex+1, cellData)).ToList();
        }
    }
}