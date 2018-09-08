using System;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;
using SheetToObjects.Lib;

namespace SheetToObjects.Adapters.MicrosoftExcel
{
    public class SheetProvider : IProvideSheet
    {
        private readonly IConvertDataToSheet<ExcelData> _excelDataConverter;

        internal SheetProvider(IConvertDataToSheet<ExcelData> excelDataConverter)
        {
            _excelDataConverter = excelDataConverter;
        }

        public SheetProvider() : this(new ExcelAdapter()) { }

        public Sheet GetFromBase64Encoded(string base64EncodedFile, string sheetName, ExcelRange range)
        {
            var fileBytes = Convert.FromBase64String(base64EncodedFile);

            using (var fileStream = new MemoryStream(fileBytes))
            {
                return GetFromStream(fileStream, sheetName, range);
            }
        }

        public Sheet GetFromPath(string excelPath, string sheetName, ExcelRange range)
        {
            using (var fileStream = new FileStream(excelPath, FileMode.Open))
            {
                return GetFromStream(fileStream, sheetName, range);
            }
        }

        public Sheet GetFromStream(Stream fileStream, string sheetName, ExcelRange range)
        {
            using (var excelPackage = new ExcelPackage(fileStream))
            {
                var workBook = excelPackage.Workbook;
                var workSheet = GetSheetFromWorkBook(workBook, sheetName);

                var data = CreateDataForRange(workSheet, range);

                var excelData = new ExcelData { Values = data };

                return _excelDataConverter.Convert(excelData);
            }
        }

        private static List<List<string>> CreateDataForRange(ExcelWorksheet workSheet, ExcelRange range)
        {
            var data = new List<List<string>>();

            for (var rowNumber = range.From.RowNumber; rowNumber <= range.To.RowNumber; rowNumber++)
            {
                var row = new List<string>();

                for (var columnNumber = range.From.ColumnNumber; columnNumber <= range.To.ColumnNumber; columnNumber++)
                {
                    row.Add(workSheet.Cells[rowNumber, columnNumber].Text);
                }

                data.Add(row);
            }

            return data;
        }

        private static ExcelWorksheet GetSheetFromWorkBook(ExcelWorkbook excelWorkbook, string sheetName)
        {
            var normalizedSheetName = sheetName.Replace(" ", "").ToLowerInvariant();

            for (var i = 1; i <= excelWorkbook.Worksheets.Count; i++)
            {
                if (excelWorkbook.Worksheets[i].Name.Replace(" ", "").ToLowerInvariant().Equals(normalizedSheetName))
                {
                    return excelWorkbook.Worksheets[i];
                }
            }

            throw new ArgumentException($"Workbook does not contain Worksheet with name {sheetName}");
        }
    }
}
