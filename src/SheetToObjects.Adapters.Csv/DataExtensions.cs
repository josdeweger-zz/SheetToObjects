using System.Collections.Generic;
using System.Linq;
using SheetToObjects.Lib;

namespace SheetToObjects.Adapters.Csv
{
    public static class DataExtensions
    {
        public static List<Row> ToRows(this List<List<string>> sheetData)
        {
            return sheetData.Select((rowData, rowIndex) => new Row(RowDataToCells(rowData, rowIndex))).ToList();
        }

        public static List<Cell> RowDataToCells(this List<string> rowData, int rowIndex)
        {
            return rowData.Select((cellData, columnIndex) => new Cell(columnIndex, rowIndex, cellData)).ToList();
        }
    }
}