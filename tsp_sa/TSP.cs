using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using static System.Console;

namespace tsp_sa
{
    class TSP
    {
        int citiesNum;
        ArrayList citiesDistanceMat = new ArrayList();
        List<int> bestTour = new List<int>();

        public int CitiesNum
        {
            get
            {
                return citiesNum;
            }

        }

        public List<int> BestTour
        {
            get
            {
                return bestTour;
            }
        }

        public void GetCitiesInfo(string args)
        {
            try
            {
                using (StreamReader sr = new StreamReader(args))
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
                    while ((line = sr.ReadLine()) != null && !line.Equals("EOF"))
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
            int index = GetIndex(i, j);
            return (int)citiesDistanceMat[index];
        }

        public void GetBestTourInfo(string fileName)
        {
            try
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    string line;
                    while (!(line = sr.ReadLine()).Equals("TOUR_SECTION")) ;
                    while (!(line = sr.ReadLine()).Equals("-1"))
                    {
                        int value = Convert.ToInt32(line.Trim()) - 1;
                        bestTour.Add(value);
                    }
                }
            }
            catch (Exception e)
            {
                WriteLine(e.Message);
            }
        }
    }
}
