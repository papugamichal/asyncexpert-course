using System.Collections.Generic;
using BenchmarkDotNet.Attributes;

namespace Dotnetos.AsyncExpert.Homework.Module01.Benchmark
{
    [DisassemblyDiagnoser(exportCombinedDisassemblyReport: true)]
    [MemoryDiagnoser]
    public class FibonacciCalc
    {
        // HOMEWORK:
        // 1. Write implementations for RecursiveWithMemoization and Iterative solutions
        // 2. Add MemoryDiagnoser to the benchmark
        // 3. Run with release configuration and compare results
        // 4. Open disassembler report and compare machine code
        // 
        // You can use the discussion panel to compare your results with other students

        [Benchmark(Baseline = true)]
        [ArgumentsSource(nameof(Data))]
        public ulong Recursive(ulong n)
        {
            if (n == 1 || n == 2) return 1;
            return Recursive(n - 2) + Recursive(n - 1);
        }

        [Benchmark]
        [ArgumentsSource(nameof(Data))]
        public ulong RecursiveWithMemoization(ulong n)
        {
            if (n == 1) 
                return 1;

            var memory = new ulong[n + 1];

            fib(n);

            return memory[n];

            ulong fib(ulong n)
            {
                if (n == 0)
                {
                    memory[n] = 0;
                    return 0;
                }
                if (n == 1)
                {
                    memory[n] = 1;
                    return 1;
                }

                if (memory[n] == 0)
                {
                    var a = fib(n - 2) + fib(n - 1);
                    memory[n] = a;
                }

                return memory[n];
            }
        }
        
        [Benchmark]
        [ArgumentsSource(nameof(Data))]
        public ulong Iterative(ulong n)
        {
            if (n == 0) return 0;
            if (n == 1) return 1;

            ulong valueBeforePrevious, previous = 0, current = 1;

            for (ulong i = 1; i < n; i++)
            {
                valueBeforePrevious = previous;
                previous = current;
                current = valueBeforePrevious + previous;
            }

            return current;
        }

        public IEnumerable<ulong> Data()
        {
            yield return 15;
            yield return 35;
        }
    }
}
