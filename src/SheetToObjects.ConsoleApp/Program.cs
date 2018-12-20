using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace SheetToObjects.ConsoleApp
{
    internal class Program
    {
        private static void Main()
        {
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

            await new ServiceCollection()
                .ConfigureServices()
                .BuildServiceProvider()
                .GetService<GoogleSheetsApp>()
                .Run();
        }

        private static async Task RunProtectedGoogleSheetsExampleAsync()
        {
            Console.WriteLine("===============================================================");
            Console.WriteLine("Running Protected Google Sheets example");

            await new ServiceCollection()
                .ConfigureServices()
                .BuildServiceProvider()
                .GetService<ProtectedGoogleSheetsApp>()
                .Run();
        }

        private static void RunExcelExample()
        {
            Console.WriteLine("===============================================================");
            Console.WriteLine("Running Excel example");

            new ServiceCollection()
                .ConfigureServices()
                .BuildServiceProvider()
                .GetService<ExcelApp>()
                .Run();
        }

        private static void RunCsvWithValidationErrorsExample()
        {
            Console.WriteLine("===============================================================");
            Console.WriteLine("Running CSV with validation errors example");

            new ServiceCollection()
                .ConfigureServices()
                .BuildServiceProvider()
                .GetService<CsvAppWithValidationErrors>()
                .Run();
        }

        private static void RunCsvWithoutValidationErrorsExample()
        {
            Console.WriteLine("===============================================================");
            Console.WriteLine("Running CSV without validation errors example");

            new ServiceCollection()
                .ConfigureServices()
                .BuildServiceProvider()
                .GetService<CsvAppWithoutValidationErrors>()
                .Run();
        }
    }
}
