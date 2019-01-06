using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace SheetToObjects.Examples
{
    internal class Program
    {
        private static ServiceProvider _services;

        private static void Main()
        {
            _services = new ServiceCollection().ConfigureServices().BuildServiceProvider();

            RunGoogleSheetsExampleAsync().GetAwaiter().GetResult();
            RunProtectedGoogleSheetsExampleAsync().GetAwaiter().GetResult();
            RunExcelExample();
            RunCsvWithValidationErrorsExample();
            RunCsvWithoutValidationErrorsExample();

            Console.ReadLine();
        }

        private static async Task RunGoogleSheetsExampleAsync()
        {
            Console.WriteLine("===============================================================");
            Console.WriteLine("Running Google Sheets example");

            await _services.GetService<GoogleSheetsApp>().Run();
        }

        private static async Task RunProtectedGoogleSheetsExampleAsync()
        {
            Console.WriteLine("===============================================================");
            Console.WriteLine("Running Protected Google Sheets example");

            await _services.GetService<ProtectedGoogleSheetsApp>().Run();
        }

        private static void RunExcelExample()
        {
            Console.WriteLine("===============================================================");
            Console.WriteLine("Running Excel example");

            _services.GetService<ExcelApp>().Run();
        }

        private static void RunCsvWithValidationErrorsExample()
        {
            Console.WriteLine("===============================================================");
            Console.WriteLine("Running CSV with validation errors example");

            _services.GetService<CsvAppWithValidationErrors>().Run();
        }

        private static void RunCsvWithoutValidationErrorsExample()
        {
            Console.WriteLine("===============================================================");
            Console.WriteLine("Running CSV without validation errors example");

            _services.GetService<CsvAppWithoutValidationErrors>().Run();
        }
    }
}
