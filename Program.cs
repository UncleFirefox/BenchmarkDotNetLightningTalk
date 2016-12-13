using BenchmarkDotNet.Running;
using System;
using System.Diagnostics;

namespace BenchMarkDotNetExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<SortingAlgorithms>();
            //DoTestManually();
        }

        private static void DoTestManually()
        {
            var sortingAlgorithms = new SortingAlgorithms() { ArraySize = 10000 };
            sortingAlgorithms.SetupData();

            Console.WriteLine("Executing BubbleSort");
            BenchmarkAlgoritm(() => sortingAlgorithms.BubbleSort());

            Console.WriteLine("Executing ShellSort");
            BenchmarkAlgoritm(() => sortingAlgorithms.ShellSort());

            Console.WriteLine("Executing QuickSort");
            BenchmarkAlgoritm(() => sortingAlgorithms.QuickSort());
        }

        private static void BenchmarkAlgoritm(Func<int[]> algorithm)
        {
            var watch = new Stopwatch();

            watch.Start();
            algorithm();
            watch.Stop();

            Console.WriteLine($"Sorted in : {watch.ElapsedMilliseconds*1000.0:F3} us");
        }
    }
}
