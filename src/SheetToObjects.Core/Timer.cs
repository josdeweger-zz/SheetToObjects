using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SheetToObjects.Core
{
    public class Timer
    {
        public static (T, TimeSpan) TimeFunc<T>(Func<T> func)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            var result = func();

            stopwatch.Stop();

            return (result, stopwatch.Elapsed);
        }

        public static async Task<(T, TimeSpan)> TimeFuncAsync<T>(Func<Task<T>> func)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            var result = await func();

            stopwatch.Stop();

            return (result, stopwatch.Elapsed);
        }
    }
}