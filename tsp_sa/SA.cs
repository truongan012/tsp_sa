using System;
using static System.Console;

namespace tsp_sa
{
    class SA
    {
        private const double FinalTemperature = 0.0001;
        private const double ReductionFactor = 0.9;
        private const double TemperaturePercent = 10;

        TSP tsp;
        private int maxSwaps;
        private int maxInterations;
        private int[] path;
        private int[] finalPath;


        public SA(TSP temp)
        {
            tsp = temp;
            maxSwaps = temp.CitiesNum * 100;
            maxInterations = temp.CitiesNum * 10;
            path = new int[temp.CitiesNum];
            finalPath = new int[temp.CitiesNum];
        }

        public void Execute(double tempPercent = TemperaturePercent, double finalTemp = FinalTemperature, double reduceFactor = ReductionFactor)
        {
            //WriteLine($"p: {tempPercent}     f:{finalTemp}     r:{reduceFactor}");

            double temperature = InitialTemperature(tempPercent);
            Display(path);

            path.CopyTo(finalPath, 0);
            int bestCost = GetCost(finalPath);

            do
            {
                int interation = 0;
                for (int i = 0; i < maxSwaps; i++)
                {
                    int nodeA, nodeB;
                    RandomNodes(out nodeA, out nodeB);
                    double energyDifference = CalcEnergyDifference(nodeA, nodeB);

                    if (Oracle(energyDifference, temperature))
                    {
                        Swap(nodeA, nodeB);
                        interation++;
                        int cost = GetCost(path);
                        if (cost < bestCost)
                        {
                            path.CopyTo(finalPath, 0);
                            bestCost = cost;
                            interation--;
                        }
                    }
                }
                if (interation > maxInterations) break;

                temperature = reduceFactor * temperature;
            } while (temperature > finalTemp);

            Display(path);
            Display(finalPath);
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
            nodeA = new Random().Next(0, tsp.CitiesNum);
            do
            {
                nodeB = new Random().Next(0, tsp.CitiesNum);
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
            int node = new Random().Next(0, tsp.CitiesNum);

            //find the nearest neighbor and add to path
            //loop till full path is completed
            int k = 0;
            while (k < tsp.CitiesNum)
            {
                int j = 0;
                path[k] = node;
                visiedNodes[node] = true;

                int bestDistance = Int32.MaxValue;
                while (j < tsp.CitiesNum)
                {
                    if (!visiedNodes[j] && j != node)
                    {
                        if (tsp.GetDistance(node, j) < bestDistance)
                        {
                            bestDistance = tsp.GetDistance(node, j);
                            node = j;
                        }
                    }
                    j++;
                }
                k++;
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
            double rand = new Random().NextDouble();
            return (energyDifference < 0) || (rand < probability);
        }

        public void Display(int[] path)
        {
            WriteLine($"Total Cost: {GetCost(path)} ");

            for (int i = 0; i < path.Length; i++)
            {
                if (i == path.Length - 1)
                {
                    WriteLine($"{path[i]} -> {path[0]}");
                }
                else
                {
                    Write($"{path[i]} -> ");
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
    }
}
