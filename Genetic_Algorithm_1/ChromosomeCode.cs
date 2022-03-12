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

        public List<ChromosomeCode> generatePopulationZero()
        {
            List<ChromosomeCode> population = new List<ChromosomeCode>();
            foreach (int value in Enumerable.Range(0, numberOfCodes))
            {
                ChromosomeCode next = generator.generateOne();
                next.fillChromosomeValues();
                while (population.Contains(next))
                {
                    next.fillChromosomeValues();
                }
                population.Add(next);
            }
            return population;
        }


        //funkcja przystosowania
        public ChromosomeCode getTheBest(int numOfCodes) {
            ChromosomeCode theBestOne = null;
            ChromosomeCode next;

            for (int i = 0; i < numOfCodes; i++)
            {

                next = generator.generateOne();
                next.fillChromosomeValues();
                if (theBestOne == null || theBestOne.getTotalCost() > next.getTotalCost()) {
                    theBestOne = next;
                }
            }
            return theBestOne;
        }



        //ratio as AccessViolationException percent value

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


    }

   

    class ChromosomeCode
    {
        public int[,] chromosomeCode;
        public int numberOfMachines;

        public ChromosomeCode(Tuple<int, int> facilitySize, int numOfMach)
        {
            chromosomeCode = new int[facilitySize.Item1, facilitySize.Item2];
            numberOfMachines = numOfMach;

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
            List<int> fillfrom = new List<int>(Enumerable.Range(1, numberOfMachines));
            MatrixUtil<int>.fillMatrix(fillfrom, chromosomeCode, 0); 
        }

        public void print() {

           MatrixUtil<int>.print(Cost.getC());
           MatrixUtil<int>.print(chromosomeCode);
           Console.WriteLine("Total Cost is: " + getTotalCost());
;       }


        public int getTotalCost()
        {

            int totalCost = 0;

            for (int i = 0; i < numberOfMachines; i++)
            {
                for (int j = 0; j < numberOfMachines; j++)
                {


                    totalCost += Cost.getMatrixCellCost(i, j) * getMatrixManhattanDist()[i, j] * Cost.getMatrixCellFlow(i, j);
                }
            }

            return totalCost;
        }

        int[,] getMatrixManhattanDist()
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




        int getMatrixManhattanDist(int machine1, int machine2)
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

    public static class Cost<T> {

        public class Storage {
            String JSONFlowFile;
            String JSONCostFile;

            public Storage(String flow, String cost) {

                this.JSONCostFile = cost;
                this.JSONFlowFile = flow;
            }

            public MatrixCellCost[,] C= setMatrixCost(JSONCostFile);
            public MatrixCellFlow[,] F = setMatrixCost(JSONCostFile);
        }

        

        public static  void  setCost(String FileNameFlow, String FileNameCost) {
            setMatrixCost(FileNameCost);
            setMatrixFlow(FileNameFlow);
        }


        private static int getMatrixSize(List<MatrixCell> list) {

            return Math.Max(list.Max(data => data.dest), list.Max(data => data.source));

        }


        public  MatrixCellCost[,] getC() { return C; }
        public  MatrixCellFlow[,] getF() { return F; }



        //read from file
        private static MatrixCellCost[,] setMatrixCost(String FileName)
        {
            int iterator = 0;
            List<MatrixCellCost>  mtxDataCost = JsonConvert.DeserializeObject<List<MatrixCellCost>>(FileName);
            MatrixCellCost[,] mtx = new MatrixCellCost[getMatrixSize(mtxDataCost), getMatrixSize(mtxDataCost)];
            for (int i = 0; i < getMatrixSize(mtxDataCost); i++) {
                for (int j = 0; j < getMatrixSize(mtxDataCost); j++) {
                    mtx[i,j] = mtxDataCost.ElementAt(iterator++ % 36);
                }
            }
            return mtx;

        }


        //read From file
        private static MatrixCellFlow[,] setMatrixFlow(String FileName)
        {
            int iterator = 0;
            List<MatrixCellFlow>  mtxDataFlow = JsonConvert.DeserializeObject<List<MatrixCellFlow>>(FileName);
            MatrixCellFlow mtx = new MatrixCellFlow[getMatrixSize((MatrixCellCost) mtxDataFlow), getMatrixSize((MatrixCellCost )mtxDataFlow)];
            for(int i = 0; i < getMatrixSize( (MatrixCellCost) mtxDataCost); i++)
            {
                for (int j = 0; j < getMatrixSize((MatrixCellCost) mtxDataCost); j++)
                {
                    F[i, j] = mtxDataFlow.ElementAt(iterator++ % 36);
                }
            }
            return mtx;
        }


        public  int getMatrixCellCost(int source, int dest) {
            if (C[source, dest] != null)
            {
                return C[source, dest].cost;
            }
            else
            {
                return 0;
            }
        }

        public  int getMatrixCellFlow(int source, int dest)
        {
            if (F[source, dest] != null)
            {
                return F[source, dest].flow;
            }

            else
            {
                return 0;
            }
        }


        
    }
}