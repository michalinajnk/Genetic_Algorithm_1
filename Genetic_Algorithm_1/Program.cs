using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Numpy;
using System.Threading;





namespace Genetic_Algorithm_1
{
    class Program
    {

     
        static void Main(string[] args)
        {
            string _filePath = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory);


            //----------------------YEASY---------------------------------------------------------------------------------
            
            
            StreamReader fs = new StreamReader(_filePath + "/data/easy/easy_cost.json");
            StreamReader cs = new StreamReader(_filePath + "/data/easy/easy_flow.json");
            String fileCost = fs.ReadToEnd();
            String fileFlow = cs.ReadToEnd();

          
            Cost cost = new Cost(fileCost, fileFlow);
            Population pop = new Population(100, 9, new Tuple<int, int>(3, 3), cost);
            MatrixUtil<int>.print(pop.best.chromosomeCode);

            Console.WriteLine(MatrixUtil<int>.getNumOfRows(pop.best.chromosomeCode));
            Console.WriteLine(MatrixUtil<int>.getNumOfCols(pop.best.chromosomeCode));


            GeneticAlgorithm ga = new GeneticAlgorithm(12,9, new Tuple<int, int>(3, 3),cost, 200, 60, 0.7f, 0.8f);
            ChromosomeCode theBest = ga.run();


            MatrixUtil<int>.print(theBest.chromosomeCode);
            Console.WriteLine("Total cost of the best searched solution: " +theBest.totalCost);
            
            
            //----------------------HARD---------------------------------------------------------------------------------
            /*
            StreamReader fs2 = new StreamReader(_filePath + "/data/hard/hard_cost.json");
            StreamReader cs2 = new StreamReader(_filePath + "/data/hard/hard_flow.json");
            String fileCost2 = fs2.ReadToEnd();
            String fileFlow2 = cs2.ReadToEnd();

            Cost cost2 = new Cost(fileCost2, fileFlow2);

            GeneticAlgorithm ga2 = new GeneticAlgorithm(3, 24, new Tuple<int, int>(6,5), cost2, 10, 10, 0.7f, 0.8f);

            ChromosomeCode theBest2 = ga2.run();

            MatrixUtil<int>.print(theBest2.chromosomeCode);
            Console.WriteLine("Total cost of the best searched solution: " +theBest2.totalCost);

           

            */
            //---------------------FLAT-----------------------------------------------------------------------------------------
            /*
            StreamReader fs3 = new StreamReader(_filePath + "/data/flat/flat_cost.json");
            StreamReader cs3 = new StreamReader(_filePath + "/data/flat/flat_flow.json");
            String fileCost3 = fs3.ReadToEnd();
            String fileFlow3 = cs3.ReadToEnd();


            Cost cost3 = new Cost(fileCost3, fileFlow3);
            GeneticAlgorithm ga3 = new GeneticAlgorithm(12, 12, new Tuple<int, int>(12, 1), cost3, 30, 30, 0.7f, 0.8f);
            ChromosomeCode theBest3 = ga3.run();

            MatrixUtil<int>.print(theBest3.chromosomeCode);
            Console.WriteLine("Total cost of the best searched solution: " +theBest3.totalCost);

            */
            

            /*

            Population populationZero = new Population(25, 9, new Tuple<int, int>(3, 3), cost);
               MatrixUtil<int>.print(populationZero.population[0].chromosomeCode);
               populationZero.population[0].avoidLocalOptimum();
           MatrixUtil<int>.print(populationZero.population[0].chromosomeCode);






 MatrixUtil<int>.print(populationZero.population[0].chromosomeCode);
 MatrixUtil<int>.print(populationZero.population[1].chromosomeCode);
 MatrixUtil<int>.print(populationZero.population[2].chromosomeCode);
 MatrixUtil<int>.print(populationZero.population[3].chromosomeCode);
 MatrixUtil<int>.print(populationZero.population[4].chromosomeCode);
 MatrixUtil<int>.print(populationZero.population[5].chromosomeCode);
 MatrixUtil<int>.print(populationZero.population[6].chromosomeCode);
 MatrixUtil<int>.print(populationZero.population[7].chromosomeCode);
 MatrixUtil<int>.print(populationZero.population[8].chromosomeCode);
 MatrixUtil<int>.print(populationZero.population[9].chromosomeCode);



 List<ChromosomeCode> child = populationZero.OnePointCrossOverTwoChilds(populationZero.population[0], populationZero.population[4]);

 Console.WriteLine("");
 Console.WriteLine("");
 Console.WriteLine("");
 MatrixUtil<int>.print(child[0].chromosomeCode);
 MatrixUtil<int>.print(child[1].chromosomeCode);

 Console.WriteLine("");
 Console.WriteLine("");
 Console.WriteLine("");
 Console.WriteLine(populationZero.population[1].weight);
 MatrixUtil<int>.print(populationZero.population[1].chromosomeCode);

 populationZero.mutation(populationZero.population[1]);

 MatrixUtil<int>.print(populationZero.population[1].chromosomeCode);




 Console.Write("Roullette has choosen a matrix: ");
 MatrixUtil<int>.print((populationZero.roulletteSelection()).chromosomeCode);


 Console.WriteLine(" ");


 ChromosomeCode code = populationZero.population.ElementAt(20);
 Console.WriteLine("Fitness Value: " + code.fitnessValue + ", total Cost: " + code.totalCost);


 // Console.WriteLine("{0:N6}", code.fitnessValue);

 foreach (ChromosomeCode cd in populationZero.population) {
     Console.Write("{0:N6}", cd.probOfBeingSelected);
     Console.Write(" ");
     Console.WriteLine("{0:N6}", cd.totalCost);

 }



 int[, ] tb = code.getMatrixManhattanDist();
 Console.WriteLine(" ");

 // MatrixUtil<int>.print(tb);

 MatrixUtil<int>.print(tb);

 MatrixUtil<int>.print(cost.C);
 Console.WriteLine(" ");
 MatrixUtil<int>.print(cost.F);

 /*
 StreamReader fs2 = new StreamReader(_filePath + "/data/hard/hard_cost.json");
 StreamReader cs2 = new StreamReader(_filePath + "/data/hard/hard_flow.json");
 String fileCost2 = fs2.ReadToEnd();
 String fileFlow2 = cs2.ReadToEnd();

 Cost cost2 = new Cost(fileCost2, fileFlow2);
 Population populationZero2 = new Population(5, 24, new Tuple<int, int>(6, 5), cost2);
 // populationZero.print();

 ChromosomeCode theBest2 = populationZero2.generateAndGetTheBest(5);
 Console.WriteLine(theBest2.getTotalCost(cost2));
 theBest2.print();



 StreamReader fs3 = new StreamReader(_filePath + "/data/flat/flat_cost.json");
 StreamReader cs3 = new StreamReader(_filePath + "/data/flat/flat_flow.json");
 String fileCost3 = fs3.ReadToEnd();
 String fileFlow3 = cs3.ReadToEnd();

 Cost cost3 = new Cost(fileCost3, fileFlow3);
 Population populationZero3 = new Population(5, 12, new Tuple<int, int>(1, 12), cost3);
 // populationZero.print();

 ChromosomeCode theBest3 = populationZero3.generateAndGetTheBest(5);
 Console.WriteLine(theBest3.getTotalCost(cost3));
 theBest3.print();

 */



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








