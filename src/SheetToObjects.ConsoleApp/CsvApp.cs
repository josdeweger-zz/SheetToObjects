using System;
using System.IO;
using Newtonsoft.Json;
using SheetToObjects.Adapters.Csv;
using SheetToObjects.ConsoleApp.Models;
using SheetToObjects.Lib;

namespace SheetToObjects.ConsoleApp
{
    public class CsvApp
    {
        private readonly IProvideSheet _sheetProvider;
        private readonly IMapSheetToObjects _sheetMapper;

        public CsvApp(
            IProvideSheet sheetProvider,
            IMapSheetToObjects sheetMapper)
        {
            _sheetProvider = sheetProvider;
            _sheetMapper = sheetMapper;
        }

        public void Run()
        {
            var fileStream = File.Open(@"./Files/profiles.csv", FileMode.Open);
            var sheet = _sheetProvider.GetFromStream(fileStream, ';');

            var result = _sheetMapper.Map<ProfileModel>(sheet);

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