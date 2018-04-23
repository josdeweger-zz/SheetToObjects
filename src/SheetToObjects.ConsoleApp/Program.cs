using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace SheetToObjects.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            RunAsync().GetAwaiter().GetResult();
        }

        private static async Task RunAsync()
        {
            await new ServiceCollection()
                .ConfigureServices()
                .BuildServiceProvider()
                .GetService<App>()
                .Run();
        }
    }
}
