using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace tsp_sa
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                TSP tsp = new TSP();
                tsp.GetCitiesInfo(args[0]);

                SA sa = new SA(tsp);
                sa.Execute();

                ReadKey();
            }
        }
    }
}
