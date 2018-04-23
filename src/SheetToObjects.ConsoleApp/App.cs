using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Refit;
using SheetToObjects.Infrastructure.GoogleSheets;
using SheetToObjects.Lib;

namespace SheetToObjects.ConsoleApp
{
    public class App
    {
        private readonly AppSettings _appSettings;
        private readonly IProvideGoogleSheets _googleSheetDataProvider;
        private readonly IConvertResponseToSheet<GoogleSheetResponse> _sheetDataConverter;
        private readonly IMapSheetToObjects _sheetMapper;

        public App(
            IOptions<AppSettings> appSettings, 
            IConvertResponseToSheet<GoogleSheetResponse> sheetDataConverter,
            IMapSheetToObjects sheetMapper)
        {
            _appSettings = appSettings.Value;
            _googleSheetDataProvider = RestService.For<IProvideGoogleSheets>(_appSettings.GoogleSheetsUrl);
            _sheetDataConverter = sheetDataConverter;
            _sheetMapper = sheetMapper;
        }

        public async Task Run()
        {
            const string range = "'Herstructurering Filters Data'!A1:H9";
            
            //get sheet from google docs
            var sheetDataResponse = await _googleSheetDataProvider.GetSheet(_appSettings.SheetId, range, _appSettings.ApiKey);
            
            //convert to generic sheet model
            var sheet = _sheetDataConverter.Convert(sheetDataResponse);

            //create configuration to specify what column value should be mapped to what class property
            var mappingConfig = BuildMappingConfig();

            //do the actual mapping
            var epicTrackingModels = _sheetMapper.Map(sheet).To<EpicTrackingModel>(mappingConfig);

            //write response, sheet and model to console
            WriteToConsole(sheetDataResponse, sheet, epicTrackingModels);
        }

        private static MappingConfig BuildMappingConfig()
        {
            return new MappingConfig()
                .For<EpicTrackingModel>()
                .Column("A").MapTo(m => m.SprintNumber)
                .Column("B").MapTo(m => m.SprintName)
                .Column("C").MapTo(m => m.StoryPointsCompleted)
                .Column("D").MapTo(m => m.TotalCompleted)
                .Column("E").MapTo(m => m.ForecastNormal)
                .Column("F").MapTo(m => m.ForecastHigh)
                .Column("G").MapTo(m => m.ForecastLow)
                .Column("H").MapTo(m => m.Scope)
                .Build();
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