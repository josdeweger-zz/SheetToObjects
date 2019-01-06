using System;
using System.IO;
using SheetToObjects.ConsoleApp.Models;
using SheetToObjects.Core;
using SheetToObjects.Lib;
using IProvideSheet = SheetToObjects.Adapters.Csv.IProvideSheet;

namespace SheetToObjects.ConsoleApp
{
    public class CsvAppWithoutValidationErrors
    {
        private readonly IProvideSheet _sheetProvider;
        private readonly IMapSheetToObjects _sheetMapper;

        public CsvAppWithoutValidationErrors(
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
                var fileStream = File.Open(@"./Files/profiles-without-validation-errors.csv", FileMode.Open);
                var sheet = _sheetProvider.GetFromStream(fileStream, ';');

                return _sheetMapper.Map<Profile>(sheet);
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