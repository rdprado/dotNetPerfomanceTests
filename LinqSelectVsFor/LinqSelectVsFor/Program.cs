using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
namespace DotNetPerformanceTests 
{
    class Foo
    { 
        public int account = 1; 
        public string asdfas = "lkdsajn2345fkljsdhnf"; 
        public string c = "lkdsajnf245kljsdhnf"; 
        public string d = "lkdsajnfrfgj56kljsdhnf"; 
        public string e = "lkdsaj2345nf22222kljsdhnf"; 
        public string f = "lkdsajnfkljs235dhnf";
        public int asf = 2; 
        public double asd = 2; 
    } 

    class Program 
    {
        private const int runs = 1000;
        private const int count = 100000;

        static readonly List<long> foreach1Results = new List<long>(count);
        static readonly List<long> foreach2Results = new List<long>(count);
        static readonly List<long> forResults = new List<long>(count);
        static readonly List<long> selectResults = new List<long>(count);

        static void Main(string[] args) 
        {
            Console.WriteLine("Linq select vs foreach vs for!");
            List<Foo> list = new List<Foo>(); 
            for (int i = 0; i < count; ++i) 
            {
                list.Add(new Foo() { account = i });
            }

            for (int i = 0; i < runs; i++)
            {
                foreach1Results.Add(RunForEach(list));
                foreach2Results.Add(ForEach_ListInitSize(list));
                forResults.Add(For(list));
                selectResults.Add(LinqSelect(list));
            }

            float foreach1Res = 0;
            float foreach2Res = 0;
            float forRes = 0;
            float selectRes = 0;
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

            Console.WriteLine($"foreach: {foreach1Res}");
            Console.WriteLine($"foreach with known size: {foreach2Res}");
            Console.WriteLine($"for: {forRes}");
            Console.WriteLine($"select: {selectRes}");

            Console.ReadLine(); 
        }
        private static long RunForEach(List<Foo> list) 
        {
            HashSet<string> hashset = new HashSet<string>();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            foreach (var el in list)
            {
                hashset.Add(el.account.ToString()); 
            }
            sw.Stop();
            return sw.ElapsedMilliseconds;
        }
        private static long ForEach_ListInitSize(List<Foo> list) 
        {
            HashSet<string> hashset = new HashSet<string>(list.Count);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            foreach (var el in list) 
            {
                hashset.Add(el.account.ToString()); 
            } 
            sw.Stop();
            return sw.ElapsedMilliseconds;
        } 
        private static long For(List<Foo> list) 
        {
            HashSet<string> hashset = new HashSet<string>(list.Count);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < list.Count; i++) 
            {
                hashset.Add(list[i].ToString()); 
            } 
            sw.Stop();
            return sw.ElapsedMilliseconds;
        }
        private static long LinqSelect(List<Foo> list)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            list.Select((el) => el.account.ToString()).ToHashSet();
            sw.Stop();
            return sw.ElapsedMilliseconds;
        } 
    }
}