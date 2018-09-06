using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SheetToObjects.Adapters.ProtectedGoogleSheets.Interfaces;
using SheetToObjects.ConsoleApp.Models;
using SheetToObjects.Core;
using SheetToObjects.Lib;

namespace SheetToObjects.ConsoleApp
{
    public class ProtectedGoogleSheetsApp
    {
        private readonly AppSettings _appSettings;
        private readonly IProvideSheet _sheetProvider;
        private readonly IMapSheetToObjects _sheetMapper;

        public ProtectedGoogleSheetsApp(
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
                var sheet = await _sheetProvider.GetAsync("secret.json", 
                                                          "DOCUMENT NAME", 
                                                          "DOCUMENT ID", 
                                                          "SET RANGE HERE"); // 'Job offers'!A:F

                return _sheetMapper.Map<EpicTrackingModel>(sheet);
            });

            Console.WriteLine("===============================================================");
            foreach (var error in result.Item1.ValidationErrors)
            {
                Console.WriteLine($"Column: {error.ColumnName} | Row: {error.RowIndex} | Message: {error.ErrorMessage}");
            }
            Console.WriteLine($"Mapped {result.Item1.ParsedModels.Count} models in {result.Item2.ToString()} " +
                              $"with {result.Item1.ValidationErrors.Count} validation errors");
            Console.WriteLine("===============================================================");
        }

        private static void WriteToConsole(params object[] objects)
        {
            foreach (var obj in objects)
            {
                Console.WriteLine("===============================================================");
                ConsoleWriteJson(obj);
                Console.WriteLine("===============================================================");
            }
        }

        private static void ConsoleWriteJson(object obj)
        {
            Console.WriteLine(JsonConvert.SerializeObject(obj, Formatting.Indented));
        }
    }
}
