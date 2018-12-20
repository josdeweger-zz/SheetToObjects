using System.Collections.Generic;
using System.Linq;

namespace SheetToObjects.Lib.Extensions
{
    public static class DataExtensions
    {
        public static List<Row> ToRows(this IList<IList<string>> sheetData)
        {
            return sheetData.Select((rowData, rowIndex) => new Row(RowDataToCells(rowData, rowIndex), rowIndex)).ToList();
        }

        public static List<Row> ToRows(this IList<IList<object>> sheetData)
        {
            return sheetData.Select((rowData, rowIndex) => new Row(RowDataToCells(rowData, rowIndex), rowIndex)).ToList();
        }

        public static List<Cell> RowDataToCells(this IList<string> rowData, int rowIndex)
        {
            return rowData.Select((cellData, columnIndex) => new Cell(columnIndex, rowIndex, cellData)).ToList();
        }

        public static List<Cell> RowDataToCells(this IList<object> rowData, int rowIndex)
        {
            return rowData.Select((cellData, columnIndex) => new Cell(columnIndex, rowIndex, cellData)).ToList();
        }
    }
}
