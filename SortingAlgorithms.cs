using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Columns;
using BenchmarkDotNet.Order;
using System;
using System.Linq;
using BenchmarkDotNet.Attributes.Jobs;

namespace BenchMarkDotNetExample
{
    [CoreJob]
    [KeepBenchmarkFiles]
    [MinColumn, MaxColumn]
    [OrderProvider(SummaryOrderPolicy.FastestToSlowest)]
    public class SortingAlgorithms
    {
        private int[] _internalArray { get; set; }
        public int[] UnsortedArray
        {
            get
            {
                //return _internalArray;
                return (int[])_internalArray.Clone();
            }
        }

        [Params(100)]
        public int ArraySize { get; set; }

        [Setup]
        public void SetupData()
        {
            _internalArray = GenerateData(ArraySize);
        }

        #region Algorithms

        [Benchmark]
        public int[] BubbleSort()
        {
            return ApplyBubleSort(UnsortedArray);
        }

        [Benchmark]
        public int[] SelectionSort()
        {
            return ApplySelectionSort(UnsortedArray);
        }

        [Benchmark]
        public int[] InsertionSort()
        {
            return ApplyInsertionSort(UnsortedArray);
        }

        [Benchmark]
        public int[] ShellSort()
        {
            return ApplyShellSort(UnsortedArray);
        }

        [Benchmark(Baseline = true)]
        public int[] QuickSort()
        {
            return ApplyQuickSort(UnsortedArray, 0, UnsortedArray.Length - 1);
        }

        #endregion

        #region Auxiliary methods
        private int[] ApplyBubleSort(int[] data)
        {
            for (int j = data.Length - 1; j > 0; j--)
            {
                for (int i = 0; i < j; i++)
                {
                    if (data[i] > data[i + 1])
                        exchange(data, i, i + 1);
                }
            }

            return data;
        }

        private int[] ApplySelectionSort(int[] data)
        {
            for (int i = 0; i < data.Length - 1; i++)
            {
                int k = IntArrayMin(data, i);
                if (i != k)
                    exchange(data, i, k);
            }

            return data;
        }

        private int[] ApplyInsertionSort(int [] data)
        {
            for (int j = 1; j < data.Length; j++)
            {
                for (int i = j; i > 0 && data[i] < data[i - 1]; i--)
                {
                    exchange(data, i, i - 1);
                }
            }

            return data;
        }

        private int[] ApplyShellSort(int[] data)
        {
            int[] intervals = GenerateIntervals(data.Length);

            int i, j, k, m;
            int N = data.Length;

            // The intervals for the shell sort must be sorted, ascending

            for (k = intervals.Length - 1; k >= 0; k--)
            {
                int interval = intervals[k];
                for (m = 0; m < interval; m++)
                {
                    for (j = m + interval; j < N; j += interval)
                    {
                        for (i = j; i >= interval && data[i] < data[i - interval]; i -= interval)
                        {
                            exchange(data, i, i - interval);
                        }
                    }
                }
            }

            return data;
        }

        private int[] ApplyQuickSort(int[] data, int left, int right)
        {
            int i, j;
            int x;

            i = left;
            j = right;

            x = data[(left + right) / 2]; /* find pivot item */
            while (true)
            {
                while (data[i] < x)
                    i++;
                while (x < data[j])
                    j--;
                if (i <= j)
                {
                    exchange(data, i, j);
                    i++;
                    j--;
                }
                if (i > j)
                    break;
            }
            if (left < j)
                ApplyQuickSort(data, left, j);
            if (i < right)
                ApplyQuickSort(data, i, right);

            return data;
        }

        private static int[] GenerateData(int size)
        {
            Random randNum = new Random();

            return
                Enumerable
                .Repeat(0, size)
                .Select(i => randNum.Next(0, size))
                .ToArray();
        }

        private static void exchange(int[] data, int m, int n)
        {
            int temporary;

            temporary = data[m];
            data[m] = data[n];
            data[n] = temporary;
        }

        private static int IntArrayMin(int[] data, int start)
        {
            int minPos = start;
            for (int pos = start + 1; pos < data.Length; pos++)
                if (data[pos] < data[minPos])
                    minPos = pos;
            return minPos;
        }

        private static int[] GenerateIntervals(int n)
        {
            if (n < 2)
            {  // no sorting will be needed
                return new int[0];
            }
            int t = Math.Max(1, (int)Math.Log(n, 3) - 1);
            int[] intervals = new int[t];
            intervals[0] = 1;
            for (int i = 1; i < t; i++)
                intervals[i] = 3 * intervals[i - 1] + 1;
            return intervals;
        }

        #endregion
    }
}
