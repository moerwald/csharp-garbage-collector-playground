using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarbageCollectorPlayground
{
    class Program
    {
        static void Main(string[] args)
        {
            DumpPerformanceCounters();

            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Creating garbage");
            var vt = null as Version;

            foreach (var item in Enumerable.Range(0, 10))
            {
                vt = new Version();
            }

            Console.WriteLine("Garbage created");
            Console.ForegroundColor = oldColor;
            DumpPerformanceCounters();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Fire up garbage collection");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            Console.ForegroundColor = oldColor;
            DumpPerformanceCounters();


            void DumpPerformanceCounters()
            {
                Process currentProc = Process.GetCurrentProcess();
                var bytesInUse = currentProc.PrivateMemorySize64;
                Console.WriteLine("Private Bytes = " + bytesInUse);


                PerformanceCounter ctr1 = new PerformanceCounter("Process", "Private Bytes", Process.GetCurrentProcess().ProcessName);
                PerformanceCounter ctr2 = new PerformanceCounter(".NET CLR Memory", "# Gen 0 Collections", Process.GetCurrentProcess().ProcessName);
                PerformanceCounter ctr3 = new PerformanceCounter(".NET CLR Memory", "# Gen 1 Collections", Process.GetCurrentProcess().ProcessName);
                PerformanceCounter ctr4 = new PerformanceCounter(".NET CLR Memory", "# Gen 2 Collections", Process.GetCurrentProcess().ProcessName);
                PerformanceCounter ctr5 = new PerformanceCounter(".NET CLR Memory", "Gen 0 heap size", Process.GetCurrentProcess().ProcessName);
                //...
                Console.WriteLine("Private Bytes = " + ctr1.NextValue());
                Console.WriteLine("CG 0 generation collections = " + ctr2.NextValue());
                Console.WriteLine("CG 1 generation collections = " + ctr3.NextValue());
                Console.WriteLine("CG 2 generation collections = " + ctr4.NextValue());
                Console.WriteLine("CG 0 generation HEAP Size = " + ctr5.NextValue());

            }
        }
    }
}
