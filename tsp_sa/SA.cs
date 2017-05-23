using System;
using static System.Console;

namespace tsp_sa
{
    class SA
    {
        private const double FinalTemperature = 0.000001;
        private const double CoolingRate = 0.99;
        private const double TemperaturePercent = 10;

        TSP tsp;
        private int maxSwaps;
        //private int maxIteration;
        private int[] path;
        private int[] finalPath;
        Random rand = new Random();

        public SA(TSP temp)
        {
            tsp = temp;
            maxSwaps = temp.CitiesNum * 100;
            //maxIteration = temp.CitiesNum * 10;
            path = new int[temp.CitiesNum];
            finalPath = new int[temp.CitiesNum];
        }

        public void Execute(double tempPercent = TemperaturePercent, double finalTemp = FinalTemperature, double coolingRate = CoolingRate)
        {
            //WriteLine($"p: {tempPercent}     f:{finalTemp}     r:{coolingRate}");

            double temperature = InitialTemperature(tempPercent);
            int cost = GetCost(path);

            ForegroundColor = ConsoleColor.Red;
            WriteLine("Initial Path:");
            ResetColor();
            Display(path, cost);

            path.CopyTo(finalPath, 0);
            int bestCost = cost;

            int iterationTemp = 0;

            do
            {
                //int iteration = 0;
                for (int i = 0; i < maxSwaps; i++)
                {
                    Random2Pos(out int posA, out int posB);
                    double energyDifference = CalcEngDiffBtwNodesAt(posA, posB);

                    if (Oracle(energyDifference, temperature))
                    {
                        SwapNodesWithin(posA, posB);
                        cost = cost + (int)energyDifference;
                        if (cost < bestCost)
                        {
                            path.CopyTo(finalPath, 0);
                            bestCost = cost;
                        }
                        //iteration++;
                    }
                    //if (iteration > maxIteration) break;
                }
                temperature *= coolingRate;
                iterationTemp++;
            } while (temperature > finalTemp);
#if DEBUG
            Display(path,GetCost(path));
            ForegroundColor = ConsoleColor.Red;
            WriteLine("\nFinal Path:");
            ResetColor();
#else
            WriteLine("\nPath:");
#endif
            Display(finalPath, bestCost);
            WriteLine($"\nLoop Counter: {iterationTemp}");
        }

        private double CalcEngDiffBtwNodesAt(int posA, int posB)
        {
            int neighborPosA = posA < tsp.CitiesNum - 1 ? posA + 1 : 0;
            int neighborPosB = posB < tsp.CitiesNum - 1 ? posB + 1 : 0;
            return ((tsp.GetDistance(path[posA], path[posB]) + tsp.GetDistance(path[neighborPosA], path[neighborPosB]))
              - (tsp.GetDistance(path[posA], path[neighborPosA]) + tsp.GetDistance(path[posB], path[neighborPosB])));
        }

        private void Random2Pos(out int posA, out int posB)
        {
            posA = rand.Next(0, tsp.CitiesNum);
            do
            {
                posB = rand.Next(0, tsp.CitiesNum);
            } while ((posA - posB + tsp.CitiesNum) % tsp.CitiesNum < 2);
        }

        private double InitialTemperature(double p)
        {
            InitialSolution();
            return GetCost(path) * p / 100;
        }

        private void InitialSolution()
        {
            //Initial unvisited nodes list
            bool[] visiedNodes = new bool[tsp.CitiesNum];
            for (int i = 0; i < visiedNodes.Length; i++)
            {
                visiedNodes[i] = false;
            }

            //random start node
            int node = rand.Next(0, tsp.CitiesNum);

            //find the nearest neighbor and add to path
            //loop till full path is completed
            for (int k = 0; k < tsp.CitiesNum; k++)
            {
                path[k] = node;
                visiedNodes[node] = true;
                int bestDistance = Int32.MaxValue;
                for (int j = 0; j < tsp.CitiesNum; j++)
                {
                    if (!visiedNodes[j] && j != node)
                    {
                        int distance = tsp.GetDistance(path[k], j);
                        if (distance < bestDistance)
                        {
                            bestDistance = distance;
                            node = j;
                        }
                    }
                }
            }
        }

        private void SwapNodesWithin(int posA, int posB)
        {
            int tmp;
            if (posA > posB)
            {
                tmp = posA;
                posA = posB;
                posB = tmp;
            }
            while (posB > posA)
            {
                tmp = path[posA + 1];
                path[posA + 1] = path[posB];
                path[posB] = tmp;
                posB--;
                posA++;
            }
        }

        private bool Oracle(double energyDifference, double temperature)
        {
            double probability = Math.Exp(-energyDifference / temperature);
            return (energyDifference < 0) || (rand.NextDouble() < probability);
        }

        public void Display(int[] path)
        {
            for (int i = 0; i < path.Length - 1; i++)
            {
                Write($"{path[i] + 1}, ");
            }
            WriteLine($"{path[path.Length - 1] + 1}");
        }

        public void Display(int[] path, int cost)
        {
            Display(path);
            ForegroundColor = ConsoleColor.Green;
            WriteLine($"Total Cost: {cost} ");
            ResetColor();
        }

        public int GetCost(int[] path)
        {
            int costTotal = 0;
            for (int i = 0; i < path.Length - 1; i++)
            {
                costTotal += tsp.GetDistance(path[i], path[i + 1]);
            }
            costTotal += tsp.GetDistance(path[path.Length - 1], path[0]);
            return costTotal;
        }

        /// <summary>
        ///  Function for debug
        /// </summary>
        /// <returns></returns>
        public int GetBestTour()
        {
            int[] tempPath = tsp.BestTour.ToArray();
            return GetCost(tempPath);
        }

        /// <summary>
        /// Function for debug
        /// </summary>
        public void DisPlayBestTour()
        {
            int[] tempPath = tsp.BestTour.ToArray();
            Display(tempPath);
        }
    }
}
