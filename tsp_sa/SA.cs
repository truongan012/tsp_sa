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
        private int maxIteration;
        private int[] path;
        private int[] finalPath;
        Random rand = new Random();

        public SA(TSP temp)
        {
            tsp = temp;
            maxSwaps = temp.CitiesNum * 100;
            maxIteration = temp.CitiesNum * 10;
            path = new int[temp.CitiesNum];
            finalPath = new int[temp.CitiesNum];
        }

        public void Execute(double tempPercent = TemperaturePercent, double finalTemp = FinalTemperature, double coolingRate = CoolingRate)
        {
            //WriteLine($"p: {tempPercent}     f:{finalTemp}     r:{coolingRate}");

            double temperature = InitialTemperature(tempPercent);
            double initialTemp = temperature;
            Display(path);

            path.CopyTo(finalPath, 0);
            int bestCost = GetCost(finalPath);
            int iterationTemp = 0;

            do
            {
                int iteration = 0;
                finalPath.CopyTo(path, 0);
                for (int i = 0; i < maxSwaps; i++)
                {
                    int nodeA, nodeB;
                    RandomNodes(out nodeA, out nodeB);
                    double energyDifference = CalcEnergyDifference(nodeA, nodeB);

                    if (Oracle(energyDifference, temperature))
                    {
                        Swap(nodeA, nodeB);
                        iteration++;
                        int cost = GetCost(path);
                        if (cost < bestCost)
                        {
                            path.CopyTo(finalPath, 0);
                            bestCost = cost;
                        }
                    }
                    if (iteration > maxIteration) break;
                }

                temperature *= coolingRate;

                iterationTemp++;
            } while (temperature > finalTemp);

            Display(path);
            Display(finalPath);
            WriteLine($"Loop: {iterationTemp}");
        }

        private double CalcEnergyDifference(int nodeA, int nodeB)
        {
            int neighborNodeA = nodeA < tsp.CitiesNum - 1 ? nodeA + 1 : 0;
            int neighborNodeB = nodeB < tsp.CitiesNum - 1 ? nodeB + 1 : 0;
            return (tsp.GetDistance(nodeA, nodeB) + tsp.GetDistance(neighborNodeA, neighborNodeB)
              - tsp.GetDistance(nodeA, neighborNodeA) + tsp.GetDistance(nodeB, neighborNodeB));
        }

        private void RandomNodes(out int nodeA, out int nodeB)
        {
            nodeA = rand.Next(0, tsp.CitiesNum);
            do
            {
                nodeB = rand.Next(0, tsp.CitiesNum);
            } while ((nodeA - nodeB + tsp.CitiesNum) % tsp.CitiesNum < 2);
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

        private void Swap(int nodeA, int nodeB)
        {
            int tmp;
            if (nodeA > nodeB)
            {
                tmp = nodeA;
                nodeA = nodeB;
                nodeB = tmp;
            }
            while (nodeB > nodeA)
            {
                tmp = path[nodeA + 1];
                path[nodeA + 1] = path[nodeB];
                path[nodeB] = tmp;
                nodeB--;
                nodeA++;
            }
        }

        private bool Oracle(double energyDifference, double temperature)
        {
            double probability = Math.Exp(-energyDifference / temperature);
            return (energyDifference < 0) || (rand.NextDouble() < probability);
        }

        public void Display(int[] path)
        {
            WriteLine($"Total Cost: {GetCost(path)} ");

            for (int i = 0; i < path.Length; i++)
            {
                if (i == path.Length - 1)
                {
                    WriteLine($"{path[i] + 1} -> {path[0] + 1}");
                }
                else
                {
                    Write($"{path[i] + 1} -> ");
                }
            }
        }

        public int GetCost(int[] path)
        {
            int costTotal = 0;
            for (int i = 0; i < path.Length; i++)
            {
                if (i == path.Length - 1)
                {
                    costTotal += tsp.GetDistance(path[i], path[0]);
                }
                else
                {
                    costTotal += tsp.GetDistance(path[i], path[i + 1]);
                }
            }
            return costTotal;
        }

        public int GetBestTour()
        {
            int[] tempPath = tsp.BestTour.ToArray();
            return GetCost(tempPath);
        }
    }
}
