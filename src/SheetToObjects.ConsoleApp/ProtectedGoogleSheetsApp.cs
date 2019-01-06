using System;
using System.Threading.Tasks;
using SheetToObjects.Adapters.ProtectedGoogleSheets;
using SheetToObjects.ConsoleApp.Models;
using SheetToObjects.Core;
using SheetToObjects.Lib;

namespace SheetToObjects.ConsoleApp
{
    public class ProtectedGoogleSheetsApp
    {
        private readonly IProvideProtectedSheet _sheetProvider;
        private readonly IMapSheetToObjects _sheetMapper;

        public ProtectedGoogleSheetsApp(
            IProvideProtectedSheet sheetProvider,
            IMapSheetToObjects sheetMapper)
        {
            _sheetProvider = sheetProvider;
            _sheetMapper = sheetMapper;
        }

        public async Task Run()
        {
            var result = await Timer.TimeFuncAsync(async () =>
            {
                var sheet = await _sheetProvider.GetAsync("SheetToObjects-a96682815641.json",
                                                          "SheetToObjects demo",
                                                          "1cxAOIdNlb2UJ8h5ADUyqiolQt7znf-S7AAEKJV8VpJc", 
                                                          "'store'!A1:U9995");

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
