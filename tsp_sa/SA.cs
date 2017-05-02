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
        private const double FinalTemperature = 0.000001f;

        TSP tsp;
        private int maxSwaps;
        private int maxInterations;
        private ArrayList path = new ArrayList();
        private int costTotal;


        public SA(TSP temp)
        {
            tsp = temp;
            maxSwaps = temp.CitiesNum * 100;
            maxInterations = temp.CitiesNum * 10;
            costTotal = 0;
        }

        public void Execute()
        {
            double temperature = InitialTemperature();

            //do
            //{
            //    int interation = 0;
            //    for (int i = 0; i < maxSwaps; i++)
            //    {
            //        int nodeA, nodeB;
            //        RandomNodes(out nodeA, out nodeB);
            //        double energyDifference = CalcEnergyDifference(nodeA, nodeB);

            //        if (Oracle(energyDifference, temperature))
            //        {
            //            Swap(nodeA, nodeB);
            //            SaveSolution();
            //            interation++;
            //        }
            //        if (interation < maxInterations)
            //        {
            //            break;
            //        }
            //    }
            //    temperature = ReduceTemperature();
            //} while (temperature < FinalTemperature);

        }

        private double CalcEnergyDifference(int nodeA, int nodeB)
        {
            return (tsp.GetDistance(nodeA, nodeB) + tsp.GetDistance(nodeA + 1, nodeB + 1)
                - tsp.GetDistance(nodeA, nodeA + 1) + tsp.GetDistance(nodeB, nodeB + 1));
        }

        private void RandomNodes(out int nodeA, out int nodeB)
        {
            throw new NotImplementedException();
        }

        private double InitialTemperature()
        {
            return InitialSolution() / 10;
        }

        private int InitialSolution()
        {
            int[] tempPath = new int[tsp.CitiesNum];
            for (int i = 0; i < tsp.CitiesNum; i++)
            {
                tempPath[i] = i;
            }

            new Random().Shuffle(tempPath);

            for (int i = 0; i < tsp.CitiesNum; i++)
            {
                int temp = tempPath[i];
                path.Add(temp);
            }

            for (int i = 0; i < path.Count; i++)
            {
                if (i == path.Count - 1)
                {
                    costTotal += tsp.GetDistance(i, 0);
                }
                else
                {
                    costTotal += tsp.GetDistance(i, i + 1);
                }
            }

            return costTotal;
        }

        private void Swap(int nodeA, int nodeB)
        {
            throw new NotImplementedException();
        }

        private bool Oracle(double energyDifference, double temperature)
        {
            throw new NotImplementedException();
        }

        private double ReduceTemperature()
        {
            throw new NotImplementedException();
        }

        private void SaveSolution()
        {
            throw new NotImplementedException();
        }

        public void Display()
        {
            WriteLine($"Total Cost: {costTotal} ");

            for (int i = 0; i < path.Count; i++)
            {
                if (i == path.Count - 1)
                {
                    WriteLine($"{path.IndexOf(i)} -> {path.IndexOf(0)}");
                }
                else
                {
                    Write($"{path.IndexOf(i)} -> ");
                }
            }
        }
    }
}
