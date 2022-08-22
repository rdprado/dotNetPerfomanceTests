using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace JsonDeserializationPerformanceTests
{
    /// <summary>
    /// </summary>
    class JsonSerialzationProfiler_List
    {
        List<long> systemJsonSerializeResults;
        List<long> newtonSoftSerializeResults;
        List<long> systemJsonDeserializeResults;
        List<long> newtonSoftDeserializeResults;

        public void Run(List<Foo> list, int runs)
        {
            // Create lists to store all test runs
            systemJsonSerializeResults = new List<long>(list.Count);
            newtonSoftSerializeResults = new List<long>(list.Count);
            systemJsonDeserializeResults = new List<long>(list.Count);
            newtonSoftDeserializeResults = new List<long>(list.Count);

            // Warmup to ensure JIT compiles code before test
            Warmup(list);

            // Run the test for n times
            RunTest(list, runs);

            // Process results
            ComputeResultAverages(runs, out float res1,
                out float res2,
                out float res3,
                out float res4);

            DisplayResuts(res1, res2, res3, res4);
        }

        private static void Warmup(List<Foo> list)
        {
            RunSerializeSystemJson(list, out var serializedSystem);
            RunSerializeNewtonsoft(list, out var serializedNewtonsoft);
            RunDeserializeSystemJson(serializedSystem);
            RunDeserializeNewtonsoft(serializedNewtonsoft);
        }

        private void RunTest(List<Foo> list, int runs)
        {
            // Always force GC cleanup before a run

            for (int i = 0; i < runs; i++)
            {
                ProfilerTools.CleanUp();
                systemJsonSerializeResults.Add(RunSerializeSystemJson(list, out var serializedSystem));
                ProfilerTools.CleanUp();
                newtonSoftSerializeResults.Add(RunSerializeNewtonsoft(list, out var serializedNewtonSoft));
                ProfilerTools.CleanUp();
                systemJsonDeserializeResults.Add(RunDeserializeSystemJson(serializedSystem));
                ProfilerTools.CleanUp();
                newtonSoftDeserializeResults.Add(RunDeserializeNewtonsoft(serializedNewtonSoft));
            }
        }

        private static long RunSerializeSystemJson(List<Foo> list, out string serialized)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            serialized = System.Text.Json.JsonSerializer.Serialize(list);

            sw.Stop();
            return sw.ElapsedMilliseconds;
        }
        private static long RunSerializeNewtonsoft(List<Foo> list, out string serialized)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            serialized = Newtonsoft.Json.JsonConvert.SerializeObject(list);

            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        private static long RunDeserializeSystemJson(string serialized)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            System.Text.Json.JsonSerializer.Deserialize<List<Foo>>(serialized);

            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        private static long RunDeserializeNewtonsoft(string serialized)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            Newtonsoft.Json.JsonConvert.DeserializeObject<List<Foo>>(serialized);

            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        private void ComputeResultAverages(int runs,
            out float systemJsonSerializeRes,
            out float newtonsoftSerializeRes,
            out float systemJsonDeserializeRes,
            out float newtonsoftDeserializeRes)
        {
            systemJsonSerializeRes = 0;
            newtonsoftSerializeRes = 0;
            systemJsonDeserializeRes = 0;
            newtonsoftDeserializeRes = 0;
            for (int i = 0; i < runs; ++i)
            {
                systemJsonSerializeRes += systemJsonSerializeResults[i];
                newtonsoftSerializeRes += newtonSoftSerializeResults[i];
                systemJsonDeserializeRes += systemJsonDeserializeResults[i];
                newtonsoftDeserializeRes += newtonSoftDeserializeResults[i];
            }
            systemJsonSerializeRes /= runs;
            newtonsoftSerializeRes /= runs;
            systemJsonDeserializeRes /= runs;
            newtonsoftDeserializeRes /= runs;
        }

        private static void DisplayResuts(float systemJsonSerializeRes,
            float newtonsoftSerializeRes,
            float systemJsonDeserializeRes,
            float newtonsoftDeserializeRes)
        {
            Console.WriteLine($"System.Json Serialize: {systemJsonSerializeRes}");
            Console.WriteLine($"Newtonsoft Serialize: {newtonsoftSerializeRes}");
            Console.WriteLine($"System.Json Deserialize: {systemJsonDeserializeRes}");
            Console.WriteLine($"Newtonsoft Deserialize: {newtonsoftDeserializeRes}");
        }
    }
}