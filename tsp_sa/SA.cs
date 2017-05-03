using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace tsp_sa
{
    class SA
    {
        private const double FinalTemperature = 0.1;
        private const double ReductionFactor = 0.8;

        TSP tsp;
        private int maxSwaps;
        private int maxInterations;
        private int[] path;
        private int[] finalPath;


        public SA(TSP temp)
        {
            tsp = temp;
            maxSwaps = temp.CitiesNum * 5;
            maxInterations = temp.CitiesNum * 10;
            path = new int[temp.CitiesNum];
            finalPath = new int[temp.CitiesNum];
        }

        public void Execute()
        {
            double temperature = InitialTemperature();
            Display(path);

            path.CopyTo(finalPath, 0);
            int bestCost = GetCost(finalPath);

            do
            {
                for (int i = 0; i < maxSwaps; i++)
                {
                    int nodeA, nodeB;
                    RandomNodes(out nodeA, out nodeB);
                    double energyDifference = CalcEnergyDifference(nodeA, nodeB);

                    if (Oracle(energyDifference, temperature))
                    {
                        Swap(nodeA, nodeB);
                        if (GetCost(path) < bestCost)
                        {
                            path.CopyTo(finalPath, 0);
                            bestCost = GetCost(finalPath);
                        }
                    }
                }
                temperature = ReductionFactor * temperature;
            } while (temperature - FinalTemperature > 0);

            Display(path);
            Display(finalPath);
        }

        private double CalcEnergyDifference(int nodeA, int nodeB)
        {
            return (tsp.GetDistance(nodeA, nodeB) + tsp.GetDistance(nodeA + 1, nodeB + 1)
              - tsp.GetDistance(nodeA, nodeA + 1) + tsp.GetDistance(nodeB, nodeB + 1));
        }

        private void RandomNodes(out int nodeA, out int nodeB)
        {
            nodeA = new Random().Next(0, tsp.CitiesNum - 1);
            do
            {
                nodeB = new Random().Next(0, tsp.CitiesNum - 1);
            } while ((nodeA - nodeB + tsp.CitiesNum) % tsp.CitiesNum < 2);
        }

        private double InitialTemperature()
        {
            InitialSolution();
            return GetCost(path) / 20;
        }

        private void InitialSolution()
        {
            bool[] visiedNodes = new bool[tsp.CitiesNum];
            for (int i = 0; i < visiedNodes.Length; i++)
            {
                visiedNodes[i] = false;
            }

            int k = 0;
            int node = new Random().Next(0, tsp.CitiesNum);
            while (k < tsp.CitiesNum)
            {
                int j = 0;
                int nodeTrace = node;
                path[k] = node;
                visiedNodes[node] = true;

                int bestCost = Int32.MaxValue;
                while (j < tsp.CitiesNum)
                {
                    if (!visiedNodes[j] && j != node)
                    {
                        if (tsp.GetDistance(node, j) < bestCost)
                        {
                            bestCost = tsp.GetDistance(node, j);
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
            return (energyDifference < 0) || ((new Random().NextDouble() - Math.Exp(-energyDifference / temperature)) < 0);
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
