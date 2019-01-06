using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SheetToObjects.Adapters.GoogleSheets;
using SheetToObjects.ConsoleApp.Models;
using SheetToObjects.Core;
using SheetToObjects.Lib;

namespace SheetToObjects.ConsoleApp
{
    public class GoogleSheetsApp
    {
        private readonly AppSettings _appSettings;
        private readonly IProvideSheet _sheetProvider;
        private readonly IMapSheetToObjects _sheetMapper;

        public GoogleSheetsApp(
            IOptions<AppSettings> appSettings,
            IProvideSheet sheetProvider,
            IMapSheetToObjects sheetMapper)
        {
            _appSettings = appSettings.Value;
            _sheetProvider = sheetProvider;
            _sheetMapper = sheetMapper;
        }

        public async Task Run()
        {
            var result = await Timer.TimeFuncAsync(async () =>
            {
                var sheet = await _sheetProvider.GetAsync(
                    _appSettings.SheetId,
                    "'Herstructurering Filters Data'!A1:H9",
                    _appSettings.ApiKey);

                return _sheetMapper.Map<EpicTracking>(sheet);
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