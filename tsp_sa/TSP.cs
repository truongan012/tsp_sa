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
        ArrayList citiesDistanceMat = new ArrayList();

        public int CitiesNum
        {
            get
            {
                return citiesNum;
            }

        }

        public void GetCitiesInfo()
        {
            try
            {
                using (StreamReader sr = new StreamReader("..\\..\\Lib\\eil8.tsp"))
                {
                    //Get the number of the cities
                    string line;
                    while (!(line = sr.ReadLine()).Equals("NODE_COORD_SECTION"))
                    {
                        if (line.Contains("DIMENSION"))
                        {
                            int index = line.LastIndexOf(" ") + 1;
                            citiesNum = Convert.ToInt32(line.Substring(index, line.Length - index));
                        }
                    }

                    //Get the x, y cordinate of each city
                    double[] xCord = new double[CitiesNum];
                    double[] yCord = new double[CitiesNum];
                    int nodeId = 0;
                    while (!(line = sr.ReadLine()).Equals("EOF"))
                    {
                        string[] temp = line.Split(' ');
                        xCord[nodeId] = Convert.ToDouble(temp[1].Trim());
                        yCord[nodeId] = Convert.ToDouble(temp[2].Trim());
                        nodeId++;
                    }

                    //Calculate the distance between each city
                    for (int i = 0; i < CitiesNum; i++)
                    {
                        for (int j = i + 1; j < CitiesNum; j++)
                        {
                            double xDis = xCord[i] - xCord[j];
                            double yDis = yCord[i] - yCord[j];
                            int distance = (int)(0.5f + Math.Sqrt(xDis * xDis + yDis * yDis));
                            citiesDistanceMat.Add(distance);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                WriteLine(e.Message);
            }
        }

        private int GetIndex(int i, int j)
        {
            if (i > j)
            {
                return CitiesNum * j + i - (j + 1) * j / 2 - j - 1;
            }
            return CitiesNum * i + j - (i + 1) * i / 2 - i - 1;
        }

        public int GetDistance(int i, int j)
        {
            return (int)citiesDistanceMat[GetIndex(i,j)];
        }
    }
}
