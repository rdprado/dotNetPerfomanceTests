using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqSelectVsFor
{
    /// <summary>
    /// This test allocates a hashset, proccess a list extracting a specific 
    /// element from it, storing the element in the hashset
    /// It will test if it is faster to do that with a Linq Select or with 
    /// other types of foreach and for
    /// 
    /// It is expected that the foreach running on a previously allocated list
    /// to be faster than the foreach that will allocate each element at a time.
    /// </summary>
    class LinqVsForsProfiler_ToHashSet
    {
        List<long> foreach1Results;
        List<long> foreach2Results;
        List<long> forResults;
        List<long> selectResults;

        public void Run(List<Foo> list, int runs)
        {
            // Create lists to store all test runs
            foreach1Results = new List<long>(list.Count);
            foreach2Results = new List<long>(list.Count);
            forResults = new List<long>(list.Count);
            selectResults = new List<long>(list.Count);

            // Warmup to ensure JIT compiles code before test
            Warmup(list);

            // Run the test for n times
            RunTest(list, runs);

            // Process results
            ComputeResultAverages(runs,
                out float foreach1Res,
                out float foreach2Res,
                out float forRes, 
                out float selectRes);

            DisplayResuts(foreach1Res, foreach2Res, forRes, selectRes);
        }

        private static void Warmup(List<Foo> list)
        {
            RunForEach(list);
            RunForEach_ListInitSize(list);
            RunFor(list);
            RunLinqSelect(list);
        }

        private void RunTest(List<Foo> list, int runs)
        {
            // Always force GC cleanup before a run

            for (int i = 0; i < runs; i++)
            {
                ProfilerTools.CleanUp();
                foreach1Results.Add(RunForEach(list));
                ProfilerTools.CleanUp();
                foreach2Results.Add(RunForEach_ListInitSize(list));
                ProfilerTools.CleanUp();
                forResults.Add(RunFor(list));
                ProfilerTools.CleanUp();
                selectResults.Add(RunLinqSelect(list));
            }
        }

        private static long RunForEach(List<Foo> list)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            HashSet<string> hashset = new HashSet<string>();
            foreach (var el in list)
            {
                hashset.Add(el.account.ToString());
            }

            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        private static long RunForEach_ListInitSize(List<Foo> list)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            HashSet<string> hashset = new HashSet<string>(list.Count);
            foreach (var el in list)
            {
                hashset.Add(el.account.ToString());
            }

            sw.Stop();
            return sw.ElapsedMilliseconds;
        }
        private static long RunFor(List<Foo> list)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            HashSet<string> hashset = new HashSet<string>(list.Count);
            for (int i = 0; i < list.Count; i++)
            {
                hashset.Add(list[i].ToString());
            }

            sw.Stop();
            return sw.ElapsedMilliseconds;
        }
        private static long RunLinqSelect(List<Foo> list)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var hs = list.Select((el) => el.account.ToString()).ToHashSet();

            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        private void ComputeResultAverages(int runs, out float foreach1Res, 
            out float foreach2Res, out float forRes, out float selectRes)
        {
            foreach1Res = 0;
            foreach2Res = 0;
            forRes = 0;
            selectRes = 0;
            for (int i = 0; i < runs; ++i)
            {
                foreach1Res += foreach1Results[i];
                foreach2Res += foreach2Results[i];
                forRes += forResults[i];
                selectRes += selectResults[i];
            }
            foreach1Res /= runs;
            foreach2Res /= runs;
            forRes /= runs;
            selectRes /= runs;
        }

        private static void DisplayResuts(float foreach1Res, float foreach2Res, 
            float forRes, float selectRes)
        {
            Console.WriteLine($"foreach: {foreach1Res}");
            Console.WriteLine($"foreach with known size: {foreach2Res}");
            Console.WriteLine($"for: {forRes}");
            Console.WriteLine($"select: {selectRes}");
        }
    }
}
