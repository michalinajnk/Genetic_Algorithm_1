﻿using System;
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
            Cost.setCost(fileFlow, fileCost);
            Population populationZero = new Population(5, 9, new Tuple<int, int>(3,3));
            ChromosomeCode theBest = populationZero.getTheBest(5);
            theBest.print();


            /*
            List<int> toPermute = Enumerable.Range(1, 9).ToList();
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



