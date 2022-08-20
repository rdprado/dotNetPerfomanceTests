using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
namespace DotNetPerformanceTests 
{
    class Foo
    { 
        public int account = 223123; 
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
        static int count = 100000;

        static void Main(string[] args) 
        {
            Stopwatch sw = new Stopwatch(); 
            Console.WriteLine("Linq select vs foreach vs for!");
            List<Foo> list = new List<Foo>(); 
            for (int i = 0; i < count; ++i) 
            {
                list.Add(new Foo()); 
            } 
            ForEach(sw, list); 
            ForEach_ListInitSize(sw, list); 
            For(sw, list); 
            LinqSelect(sw, list); 
            Console.ReadLine(); 
        }
        private static void ForEach(Stopwatch sw, List<Foo> list) 
        {
            HashSet<string> hashset = new HashSet<string>();
            sw.Restart();
            foreach (var el in list)
            {
                hashset.Add(el.account.ToString()); 
            }
            sw.Stop(); 
            Console.WriteLine($"foreach: {sw.ElapsedMilliseconds}"); 
        }
        private static void ForEach_ListInitSize(Stopwatch sw, List<Foo> list) 
        {
            HashSet<string> hashset = new HashSet<string>(list.Count); 
            sw.Restart(); 
            foreach (var el in list) 
            {
                hashset.Add(el.account.ToString()); 
            } 
            sw.Stop(); 
            Console.WriteLine($"foreach with known size: {sw.ElapsedMilliseconds}"); 
        } 
        private static void For(Stopwatch sw, List<Foo> list) 
        {
            HashSet<string> hashset = new HashSet<string>(list.Count); 
            sw.Restart(); 
            for (int i = 0; i < list.Count; i++) 
            {
                hashset.Add(list[i].ToString()); 
            } 
            sw.Stop(); 
            Console.WriteLine($"for: {sw.ElapsedMilliseconds}"); 
        }
        private static void LinqSelect(Stopwatch sw, List<Foo> list)
        {
            sw.Restart(); 
            list.Select((el) => el.account.ToString()).ToHashSet();
            sw.Stop(); 
            Console.WriteLine($"select: {sw.ElapsedMilliseconds}"); 
        } 
    }
}