using System;
using Newtonsoft.Json;
using SheetToObjects.Adapters.MicrosoftExcel;
using SheetToObjects.ConsoleApp.Models;
using SheetToObjects.Lib;

namespace SheetToObjects.ConsoleApp
{
    public class ExcelApp
    {
        private readonly IProvideSheet _sheetProvider;
        private readonly IMapSheetTo<ProfileModel> _sheetMapper;

        public ExcelApp(
            IProvideSheet sheetProvider,
            IMapSheetTo<ProfileModel> sheetMapper)
        {
            _sheetProvider = sheetProvider;
            _sheetMapper = sheetMapper;
        }

        public void Run()
        {
            var excelRange = new ExcelRange(new ExcelCell("A", 1), new ExcelCell("I", 5));
            var sheet = _sheetProvider.Get(@"./Files/profiles.xlsx", "profiles", excelRange);

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