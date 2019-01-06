using System;
using SheetToObjects.Adapters.MicrosoftExcel;
using SheetToObjects.ConsoleApp.Models;
using SheetToObjects.Core;
using SheetToObjects.Lib;

namespace SheetToObjects.ConsoleApp
{
    public class ExcelApp
    {
        private readonly IProvideSheet _sheetProvider;
        private readonly IMapSheetToObjects _sheetMapper;

        public ExcelApp(
            IProvideSheet sheetProvider,
            IMapSheetToObjects sheetMapper)
        {
            _sheetProvider = sheetProvider;
            _sheetMapper = sheetMapper;
        }

        public void Run()
        {
            var result = Timer.TimeFunc(() =>
            {
                var excelRange = new ExcelRange(new ExcelCell("A", 1), new ExcelCell("U", 9995));
                var sheet = _sheetProvider.GetFromPath(@"./Files/Sample - Superstore.xlsx", "orders", excelRange);

                return _sheetMapper.Map<Superstore>(sheet);
            });

            foreach (var error in result.Item1.ValidationErrors)
            {
                Console.WriteLine($"Column: {error.ColumnName} | Row: {error.RowIndex} | Message: {error.ErrorMessage}");
            }
            Console.WriteLine($"Mapped {result.Item1.ParsedModels.Count} models in {result.Item2.ToString()} " +
                $"with {result.Item1.ValidationErrors.Count} validation errors");
            Console.WriteLine("===============================================================");
        }
    }
}