using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace SheetToObjects.ConsoleApp
{
    internal class Program
    {
        private static void Main()
        {
            //RunGoogleSheetsExampleAsync().GetAwaiter().GetResult();
            RunExcelExample();
            //RunCsvExample();
        }

        private static async Task RunGoogleSheetsExampleAsync()
        {
            await new ServiceCollection()
                .ConfigureServices()
                .BuildServiceProvider()
                .GetService<GoogleSheetsApp>()
                .Run();
        }

        private static void RunExcelExample()
        {
            new ServiceCollection()
                .ConfigureServices()
                .BuildServiceProvider()
                .GetService<ExcelApp>()
                .Run();
        }

        private static void RunCsvExample()
        {
            new ServiceCollection()
                .ConfigureServices()
                .BuildServiceProvider()
                .GetService<CsvApp>()
                .Run();
        }
    }
}
