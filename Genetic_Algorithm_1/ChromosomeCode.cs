using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Complex;
using Newtonsoft.Json;
using Numpy;

namespace Genetic_Algorithm_1
{

    class Population {

        public List<ChromosomeCode> population;
        public ChromosomeCodeGenerator generator;
        public int numberOfCodes;

        public Population(int numberOfCodes, int numOfMachines, Tuple<int, int> facilitySize) {

            generator = new ChromosomeCodeGenerator(facilitySize, numOfMachines);
            this.numberOfCodes = numberOfCodes;
            population = generatePopulationZero();

        }

        public void  print() {
            foreach (ChromosomeCode code in population) {
                code.print();
            }
        }

        public List<ChromosomeCode> generatePopulationZero()
        {
            List<ChromosomeCode> population = new List<ChromosomeCode>();
            ChromosomeCode current = generator.generateOne();
            population.Add(current);
            foreach (int value in Enumerable.Range(1, numberOfCodes))
            {
                ChromosomeCode next = generator.generateNext(current);
                population.Add(next);
            }
            return population;
        }

        


        //funkcja przystosowania
        public ChromosomeCode getTheBest(int numOfCodes, Cost cost) {
            ChromosomeCode theBestOne = null;
            ChromosomeCode next = generator.generateOne();

            for (int i = 0; i < numOfCodes; i++)
            {
                next = generator.generateNext(next);
                if (theBestOne == null || theBestOne.getTotalCost(cost) > next.getTotalCost(cost)) {
                    theBestOne = next;
                }
            }
            return theBestOne;
        }



        //ratio as percent value

        /*
        public ChromosomeCode filterTheBest(double ratio)
        {
            Dictionary<ChromosomeCode, int> theBest = new Dictionary<ChromosomeCode, int>();
            foreach (ChromosomeCode code in population)
            {
                theBest.Add(code, code.getTotalCost());
            }
            var sorted = from entry in theBest orderby entry.Value ascending select entry;
            return sorted.FirstOrDefault;
        }
        
        */

     }





    class ChromosomeCodeGenerator {

        public int numberOfMachines;
        public Tuple<int, int> facilitySize;

        public ChromosomeCodeGenerator(Tuple<int, int> facilitySize, int numOfMach) {
            this.facilitySize = facilitySize;
            this.numberOfMachines = numOfMach;
        }

        public ChromosomeCode generateOne() {
            return new ChromosomeCode(facilitySize, numberOfMachines);
        }

        public ChromosomeCode generateNext(ChromosomeCode prev) {
            return new ChromosomeCode(prev, facilitySize, numberOfMachines);
            
        }




    }

   

    class ChromosomeCode
    {
        public int[,] chromosomeCode;
        public int numberOfMachines;

        public ChromosomeCode(Tuple<int, int> facilitySize, int numOfMach)
        {
            chromosomeCode = new int[facilitySize.Item1, facilitySize.Item2];
            numberOfMachines = numOfMach;
            fillMatrix(numOfMach, 0);

        }

        public void fillMatrix(int end, int defaultValue)
        {
            List<int> fillFrom = Enumerable.Range(0, end).ToList();
            var iter = fillFrom.GetEnumerator();


            for (int i = 0; i < MatrixUtil<int>.getNumOfRows(chromosomeCode); i++)
            {
                for (int j = 0; j < MatrixUtil<int>.getNumOfCols(chromosomeCode); j++)
                {
                    if (iter.MoveNext())
                    {
                        chromosomeCode[i, j] = iter.Current;
                    }
                    else
                    {
                        chromosomeCode[i, j] = defaultValue;
                    }
                }

            }


        }

        public  void permutateMatrix(int numOfSwapped, int defaultVal)
        {
            List<int> permutation = PermutationUtil<int>.permutate(MatrixUtil<int>.rewriteToList(chromosomeCode), numOfSwapped);
            fillMatrix(numberOfMachines-1, defaultVal);
        }

        public ChromosomeCode(ChromosomeCode chromosome, Tuple<int, int> facilitySize, int numOfMach)
        {
            chromosomeCode = MatrixUtil<int>.permutateMatrix(3, chromosome.chromosomeCode, 0);
            this.numberOfMachines = numOfMach;
         

        }


   

      

        public int Compare(ChromosomeCode other) {

            if (other.chromosomeCode == this.chromosomeCode)
            {
                return 1;
            }
            else {
                return -1;
            }

        }

       
        public void fillChromosomeValues()
        {
            List<int> fillfrom = new List<int>(Enumerable.Range(0, numberOfMachines));
            MatrixUtil<int>.fillMatrix(fillfrom, chromosomeCode, 0);
           
        }

        public void print() {

           MatrixUtil<int>.print(chromosomeCode);

;       }


        public int getTotalCost(Cost cost)
        {

            int totalCost = 0;

            for (int i = 0; i < numberOfMachines; i++)
            {
                for (int j = 0; j < numberOfMachines; j++)
                {


                    totalCost += cost.getMatrixCellCost(i, j) * getMatrixManhattanDist()[i, j] * cost.getMatrixCellFlow(i, j);
                }
            }

            return totalCost;
        }

        public int[,] getMatrixManhattanDist()
        {
            int[,] distanceMatrix = new int[numberOfMachines, numberOfMachines];
            for (int i = 0; i < numberOfMachines; i++)
            {
                for (int j = 0; j < numberOfMachines; j++)
                {
                    distanceMatrix[i, j] = getMatrixManhattanDist(i, j);
                }

            }
            return distanceMatrix;
        }




        public int getMatrixManhattanDist(int machine1, int machine2)
        {
            Tuple<int, int> machine1Indices = getCellIndices(machine1);
            Tuple<int, int> machine21Indices = getCellIndices(machine2);

            return Math.Abs(machine1Indices.Item1 - machine21Indices.Item1) + Math.Abs(machine1Indices.Item2 - machine21Indices.Item2);


        }

        Tuple<int, int> getCellIndices(int machine)
        {

            for (int i = 0; i < MatrixUtil<int>.getNumOfRows(chromosomeCode); i++)
            {
                for (int j = 0; j < MatrixUtil<int>.getNumOfCols(chromosomeCode); j++)
                {
                    if (chromosomeCode[i, j] == machine)
                    {
                        return new Tuple<int, int>(i, j);

                    }

                }
            }
            return new Tuple<int, int>(-1, -1);
        }


        
    }

    public class Cost {

        private String JSONCostFile;
        private String JSONFlowFile;

        public int[,] C { get; set; }
        public int[,] F { get; set; }


        public Cost(String JSONCostFile, String JSONFlowFile) { 
            this.JSONCostFile = JSONCostFile;
            this.JSONFlowFile = JSONFlowFile;
            setMatrixCost(JSONCostFile);
            setMatrixFlow(JSONFlowFile);

        }


     



        //read from file
        private  void setMatrixCost(String JSONCostFile)
        {
            if (C == null)
            {

                List<MatrixCellCost> cost = JsonConvert.DeserializeObject<List<MatrixCellCost>>(JSONCostFile);
                MatrixCells<MatrixCellCost> cells = new MatrixCells<MatrixCellCost>(cost);
                
                C = new int[cells.getMaxCellValue()+1, cells.getMaxCellValue()+1];
                for (int i = 0; i < cells.getMaxCellValue()+1; i++)
                {
                    for (int j = 0; j < cells.getMaxCellValue()+1; j++)
                    {
                        C[i, j] = cells.getElemAt(i, j, cells.cells);
                    }

                }

            }

        }

        private  void setMatrixFlow(String JSONFlowFile)
        {
            if (F == null)
            {
                
                
                List<MatrixCellFlow> cost = JsonConvert.DeserializeObject<List<MatrixCellFlow>>(JSONFlowFile);
                MatrixCells<MatrixCellFlow> cells = new MatrixCells<MatrixCellFlow>(cost);
                F = new int [cells.getMaxCellValue()+1, cells.getMaxCellValue()+1];
                for (int i = 0; i < cells.getMaxCellValue()+1; i++)
                {
                    for (int j = 0; j < cells.getMaxCellValue()+1; j++)
                    {
                        F[i, j] = cells.getElemAt(i,j, cells.cells);
                    }
                }

            }

        }


        //read From file
     


        public  int getMatrixCellCost(int source, int dest) {
           
            return C[source, dest];
          
        }

        public int getMatrixCellFlow(int source, int dest)
        {

            return F[source, dest];
        }
          

        
    }
}