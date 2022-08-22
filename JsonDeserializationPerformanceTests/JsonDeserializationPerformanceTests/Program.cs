using System;
using System.Collections.Generic;

namespace JsonDeserializationPerformanceTests
{
    class Program
    {
        private const int listSize = 100000;
        private const int runs = 1000;

        static void Main(string[] args)
        {
            Console.WriteLine("Linq select vs foreach vs for!");

            ProfilerTools.PrepareCPU();

            // Fill list with elements

            List<Foo> list = new List<Foo>();
            for (int i = 0; i < listSize; ++i)
            {
                list.Add(new Foo() { account = i });
            }

            // Run test

            var test = new JsonSerialzationProfiler_List();
            test.Run(list, runs);

            Console.ReadLine();
        }
    }
}
