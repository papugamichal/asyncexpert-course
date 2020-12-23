using System;
using System.Reflection;
using BenchmarkDotNet.Running;

namespace Dotnetos.AsyncExpert.Homework.Module01.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            var calc = new FibonacciCalc();
            Console.WriteLine($"Fib recursive: {calc.Recursive(15)}");
            Console.WriteLine($"Fib recursive mem: {calc.RecursiveWithMemoization(15)}");
            Console.WriteLine($"Fib iterative: {calc.Iterative(15)}");
            BenchmarkSwitcher.FromAssembly(Assembly.GetExecutingAssembly()).Run(args);
        }
    }
}
