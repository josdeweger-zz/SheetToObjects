using System.Collections.Generic;
using System.Linq;
using SheetToObjects.Lib;

namespace SheetToObjects.Adapters.GoogleSheets.Shared.Extensions
{
    internal static class DataExtensions
    {
        public static List<Row> ToRows(this List<List<string>> sheetData)
        {
            return sheetData.Select((rowData, rowIndex) => new Row(RowDataToCells(rowData, rowIndex), rowIndex)).ToList();
        }

        public static List<Row> ToRows(this IList<IList<object>> sheetData)
        {
            return sheetData.Select((rowData, rowIndex) => new Row(RowDataToCells(rowData, rowIndex), rowIndex)).ToList();
        }

        public static List<Cell> RowDataToCells(this List<string> rowData, int rowIndex)
        {
            return rowData.Select((cellData, columnIndex) => new Cell(columnIndex, rowIndex, cellData)).ToList();
        }

        public static List<Cell> RowDataToCells(this IList<object> rowData, int rowIndex)
        {
            return rowData.Select((cellData, columnIndex) => new Cell(columnIndex, rowIndex, cellData)).ToList();
        }
    }
}
