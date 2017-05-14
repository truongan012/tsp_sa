using System;
using System.Diagnostics;
using System.Threading;
using static System.Console;

namespace tsp_sa
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0 || args[0].Equals("-help") || args[0].Equals("-h"))
            {
                ShowHelp();
            }
            else
            {
                string fileName = args[0];
                double? p = null;
                double? f = null;
                double? r = null;

                for (int i = 1; i < args.Length; i++)
                {
                    if (args[i].Equals("-p")) p = Double.Parse(args[i + 1]);
                    if (args[i].Equals("-f")) f = Double.Parse(args[i + 1]);
                    if (args[i].Equals("-r")) r = Double.Parse(args[i + 1]);
                }

                TSP tsp = new TSP();
                tsp.GetCitiesInfo(fileName);
                RunSA(tsp, p, f, r);
            }
            ReadKey();
        }

        private static void ShowHelp()
        {
            WriteLine("Traveling Salesman Problem solver with Simulated Annealing algorithm");
            WriteLine();
            WriteLine("Syntax:    [FileName] [Options]");
            WriteLine();
            WriteLine("[File Name] \t: File name path, file format as TSPLIB95, egde weigh is EUC_2D");
            WriteLine();
            WriteLine("Options:");
            WriteLine("    -p \t: Percentage of intial solution cost (intial temperature)");
            WriteLine("    -f \t: Final temperature that will stop program");
            WriteLine("    -r \t: Temperature reduction factor");
            WriteLine();
            WriteLine("For Example: eli51.tsp -p 10 -f 0.001 -r 0.99");
        }

        private static void RunSA(TSP tsp, double? p, double? f, double? r, bool test = true)
        {
            SA sa = new SA(tsp);
            Stopwatch stopWatch = new Stopwatch();

            if (test)
            {
                stopWatch.Start();
            }

            if (p == null && f == null && r == null) sa.Execute();
            else if (p == null && f == null && r != null) sa.Execute(coolingRate: r.Value);
            else if (p == null && f != null && r == null) sa.Execute(finalTemp: f.Value);
            else if (p != null && f == null && r == null) sa.Execute(tempPercent: p.Value);
            else if (p != null && f != null && r == null) sa.Execute(tempPercent: p.Value, finalTemp: f.Value);
            else if (p == null && f != null && r != null) sa.Execute(finalTemp: f.Value, coolingRate: r.Value);
            else if (p != null && f == null && r != null) sa.Execute(tempPercent: p.Value, coolingRate: r.Value);
            else sa.Execute(p.Value, f.Value, r.Value);

            if (test)
            {
                stopWatch.Stop();

                TimeSpan ts = stopWatch.Elapsed;
                Console.WriteLine($"RunTime {ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}");
            }
        }
    }
}

