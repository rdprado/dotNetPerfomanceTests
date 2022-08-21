using LinqSelectVsFor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace DotNetPerformanceTests 
{
    class Program 
    {
        private const int listSize = 100000;
        private const int runs = 100;

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

            var test = new LinqVsForsProfiler_ToHashSet();
            test.Run(list, runs);

            Console.ReadLine(); 
        }
    }
}