using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SheetToObjects.Adapters.GoogleSheets;
using SheetToObjects.ConsoleApp.Models;
using SheetToObjects.Lib;

namespace SheetToObjects.ConsoleApp
{
    public class GoogleSheetsApp
    {
        private readonly AppSettings _appSettings;
        private readonly IProvideSheet _sheetProvider;
        private readonly IMapSheetTo<EpicTrackingModel> _sheetMapper;

        public GoogleSheetsApp(
            IOptions<AppSettings> appSettings,
            IProvideSheet sheetProvider,
            IMapSheetTo<EpicTrackingModel> sheetMapper)
        {
            _appSettings = appSettings.Value;
            _sheetProvider = sheetProvider;
            _sheetMapper = sheetMapper;
        }

        public async Task Run()
        {
            var sheet = await _sheetProvider.GetAsync(
                _appSettings.SheetId, 
                "'Herstructurering Filters Data'!A1:H9", 
                _appSettings.ApiKey);

            var result = _sheetMapper.Map(sheet);

            WriteToConsole(sheet, result.ParsedModels, result.ValidationErrors);
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