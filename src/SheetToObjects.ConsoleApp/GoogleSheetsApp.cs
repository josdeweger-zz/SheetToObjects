using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Refit;
using SheetToObjects.Adapters.GoogleSheets;
using SheetToObjects.ConsoleApp.Models;
using SheetToObjects.Lib;

namespace SheetToObjects.ConsoleApp
{
    public class GoogleSheetsApp
    {
        private readonly AppSettings _appSettings;
        private readonly IProvideGoogleSheet _googleSheetDataProvider;
        private readonly IConvertResponseToSheet<GoogleSheetResponse> _sheetDataConverter;
        private readonly IMapSheetToObjects _sheetMapper;

        public GoogleSheetsApp(
            IOptions<AppSettings> appSettings,
            IConvertResponseToSheet<GoogleSheetResponse> sheetDataConverter,
            IMapSheetToObjects sheetMapper)
        {
            _appSettings = appSettings.Value;
            _googleSheetDataProvider = RestService.For<IProvideGoogleSheet>(_appSettings.GoogleSheetsUrl);
            _sheetDataConverter = sheetDataConverter;
            _sheetMapper = sheetMapper;
        }

        public async Task Run()
        {
            await GoogleSheetsExample();
        }

        private async Task GoogleSheetsExample()
        {
            const string range = "'Herstructurering Filters Data'!A1:H9";

            //get sheet from google docs
            var sheetDataResponse = await _googleSheetDataProvider.GetSheet(_appSettings.SheetId, range, _appSettings.ApiKey);

            //convert to generic sheet model
            var sheet = _sheetDataConverter.Convert(sheetDataResponse);

            //do the actual mapping
            var result = _sheetMapper.Map(sheet).To<EpicTrackingModel>();
            
            //write csv data, sheet and model to console
            WriteToConsole(sheetDataResponse, sheet, result.ParsedModels, result.ValidationErrors);
        }

        private static void WriteToConsole(params object[] objects)
        {
            foreach (var obj in objects)
            {
                Console.WriteLine("===============================================================");
                ConsoleWriteJson(obj);
                Console.WriteLine("===============================================================");
            }

            Console.ReadLine();
        }

        private static void ConsoleWriteJson(object obj)
        {
            Console.WriteLine(JsonConvert.SerializeObject(obj, Formatting.Indented));
        }
    }
}