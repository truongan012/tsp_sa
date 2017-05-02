using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace tsp_sa
{
    class TSP
    {
        int citiesNum;
        ArrayList citiesMat = new ArrayList();

        public void ReadCities()
        {
            try
            {
                using (StreamReader sr = new StreamReader("..\\..\\Lib\\eil8.tsp"))
                {
                    string line;
                    while ((line = sr.ReadLine()) == "EOF")
                    {
                        WriteLine(line);
                    }
                }
            }
            catch (Exception e)
            {
                WriteLine(e.Message);
            }
        }

        public int GetIndex(int i, int j)
        {
            if (i > j)
            {
                return citiesNum * j + i - (j + 1) * j / 2 - j - 1;
            }
            return citiesNum * i + j - (i + 1) * i / 2 - i - 1;
        }
    }
}
