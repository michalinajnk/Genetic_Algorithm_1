using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Grpc.Core;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using Newtonsoft.Json;



namespace Genetic_Algorithm_1
{
    class Program
    {
        static void Main(string[] args)
        {
            string _filePath = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory);

            StreamReader fs = new StreamReader(_filePath + "/data/easy/easy_cost.json");
            StreamReader cs = new StreamReader(_filePath + "/data/easy/easy_flow.json");
            String fileCost = fs.ReadToEnd();
            String fileFlow = cs.ReadToEnd();

            Cost cost = new Cost(fileCost, fileFlow);
            Population populationZero = new Population(5, 9, new Tuple<int, int>(3,3));
           // populationZero.print();

            ChromosomeCode theBest = populationZero.getTheBest(2450, cost);
            Console.WriteLine(theBest.getTotalCost(cost));
            theBest.print();
            // int[, ] tb = theBest.getMatrixManhattanDist();
            // Console.WriteLine(" ");

            // MatrixUtil<int>.print(tb);

            // MatrixUtil<int>.print(tb.getMatrixManhattanDist());

            // MatrixUtil<int>.print(cost.C);
            //Console.WriteLine(" ");
            //MatrixUtil<int>.print(cost.F);

            
            StreamReader fs2 = new StreamReader(_filePath + "/data/hard/hard_cost.json");
            StreamReader cs2 = new StreamReader(_filePath + "/data/hard/hard_flow.json");
            String fileCost2 = fs2.ReadToEnd();
            String fileFlow2 = cs2.ReadToEnd();

            Cost cost2 = new Cost(fileCost2, fileFlow2);
            Population populationZero2 = new Population(5, 24, new Tuple<int, int>(6, 5));
            // populationZero.print();

            ChromosomeCode theBest2 = populationZero2.getTheBest(5, cost2);
            Console.WriteLine(theBest2.getTotalCost(cost2));
            theBest2.print();
            

            
            StreamReader fs3 = new StreamReader(_filePath + "/data/flat/flat_cost.json");
            StreamReader cs3 = new StreamReader(_filePath + "/data/flat/flat_flow.json");
            String fileCost3 = fs3.ReadToEnd();
            String fileFlow3 = cs3.ReadToEnd();

            Cost cost3 = new Cost(fileCost3, fileFlow3);
            Population populationZero3 = new Population(5, 12, new Tuple<int, int>(1, 12));
            // populationZero.print();

            ChromosomeCode theBest3 = populationZero3.getTheBest(5, cost3);
            Console.WriteLine(theBest3.getTotalCost(cost3));
            theBest3.print();
            




            /*
            List<int> toPermute = Enumerable.Range(0, 8).ToList();
            print(toPermute);
           

            Console.WriteLine(" first permutation");
            PermutationUtil<int>.permutate(toPermute,3);
            print(toPermute);

            Console.WriteLine(" second permutation");
            PermutationUtil<int>.permutate(toPermute, 4);
            print(toPermute);

            Console.WriteLine(" third permutation");
            PermutationUtil<int>.permutate(toPermute,5);
            print(toPermute);


            int[,] matrix = new int[,] { { 1,2,3}, {4,5,6} , {7, 8, 9} };
            MatrixUtil<int>.print(matrix);


            Console.WriteLine(" ");

            MatrixUtil<int>.permutateMatrix(3, matrix, -1);
            MatrixUtil<int>.print(matrix);
            */




        }

        public static void print(List<int> toprint) {
            toprint.ForEach(elem =>Console.Write(elem));
        }

      


     
        

    }


}



// implementacja wczytywania danych/ładowanie danych 
//fintess method (metoda przystosowania, czy dane rozwiazanie spełnia kryteria)
//metoda losowa (tworzenie generacji)




