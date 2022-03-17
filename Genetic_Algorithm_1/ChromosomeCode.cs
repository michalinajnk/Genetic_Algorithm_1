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



        public Population(int numberOfCodes, int numOfMachines, Tuple<int, int> facilitySize, Cost cost) {

            generator = new ChromosomeCodeGenerator(facilitySize, numOfMachines, cost);
            this.numberOfCodes = numberOfCodes;
            population = generatePopulationZero();
            setForAllFitnessAfter();
            setProbForAll();



        }

        public ChromosomeCode competitionSelection(int numberOfCodes)
        {
            if (this.numberOfCodes==numberOfCodes) { return population.First(); }
            List<ChromosomeCode> shuffledCodes = PermutationUtil<ChromosomeCode>.permutate(population, (int)(numberOfCodes/3) + 1).GetRange(0, numberOfCodes);
            shuffledCodes.Sort((x, y) => x.totalCost.CompareTo(y.totalCost));
            return shuffledCodes.First();

        }

        public ChromosomeCode competitionSelectionRatio(int ratio)
        {
            if (this.numberOfCodes*ratio == 1) { return population.First(); }
            List<ChromosomeCode> shuffledCodes = PermutationUtil<ChromosomeCode>.permutate(population, (int)(numberOfCodes/3) + 1).GetRange(0, (int) (numberOfCodes*ratio));
            shuffledCodes.Sort((x, y) => x.totalCost.CompareTo(y.totalCost));
            return shuffledCodes.First();

        }

        public ChromosomeCode getChromosomeFromRoulleteValue(float roulleteVal) {
            foreach (ChromosomeCode code in population){
                if (roulleteVal > code.rangeOfProb.Item1 &&  roulleteVal < code.rangeOfProb.Item2)
                {
                    return code;
                }
               
            }
            return null;
        }

       

        public ChromosomeCode roulletteSelection() { //im mniejsa wartoś getFitnessValue tym wieksze prawdopodobieństwo wylosowania
            float r = MathUtil.nextFloat(0, 1);
            return getChromosomeFromRoulleteValue(r);
           
        }

        public List<ChromosomeCode> OnePointCrossOverTwoChilds(ChromosomeCode par1, ChromosomeCode par2) {
            List<ChromosomeCode> children = new List<ChromosomeCode>();
           children.Add( OnePointCrossOver( par1,  par2));
           children.Add( OnePointCrossOver(par2, par1));

            return children;

        }





        public ChromosomeCode OnePointCrossOver(ChromosomeCode par1, ChromosomeCode par2)
        {
            List<int> parentCode1 = new List<int>(MatrixUtil<int>.rewriteToList(par1.chromosomeCode));       
            List<int> parentCode2 = new List<int>(MatrixUtil<int>.rewriteToList(par2.chromosomeCode));
         

            List<int> childList = new List<int>();
            Random rand = new Random();

            int crossPoint = rand.Next(1, parentCode1.Count-1);

            Console.WriteLine("Cross point: " + crossPoint);
            MathUtil.printList(parentCode1);
            MathUtil.printList(parentCode2);


            int[] firstPart = new List<int>(parentCode1.GetRange(0, crossPoint)).ToArray();
            int[] secondPart = new List<int>(parentCode2.GetRange(crossPoint, parentCode1.Count - crossPoint)).ToArray();

            childList.AddRange(firstPart);
            childList.AddRange(secondPart);
            
          
    

            return generator.generateFromList(fixChromosomeCode(parentCode1, parentCode2, childList, crossPoint));


        }

        private  List<int> fixChromosomeCode(List<int> par1, List<int> par2, List<int> chromosomeCode, int crossPoint) {

            List<int> child = new List<int>(chromosomeCode);
            var reapetedVals = child.GroupBy(x => x)
              .Where(g => g.Count() > 1)
              .Select(y => y.Key)
              .ToList();
           

            for (int i = crossPoint; i < child.Count; i++) {
               
                if(reapetedVals.Contains(child[i]))
                {
                    int index = par1.IndexOf(child[i]);
                   
                    if (reapetedVals.Contains(par2[index])){
                        child[i] = par2[i];
                    }
                    else {
                        child[i] = par2[index];
                    }
                }
            }
            return child;
        }

       




        public float fitness(int IndexOfChromosomeInPop,int rangeOfIteration)
        {
            //traversing through the chromosomeCode pod indeksem
            int score = 0;
           
            int[,] checkMyFintessVal = population[IndexOfChromosomeInPop].chromosomeCode;
            int[,] theBestChromosomeYet = getTheBestFromRange(rangeOfIteration).chromosomeCode;
            

            for (int i = 0; i < MatrixUtil<int>.getNumOfRows(checkMyFintessVal); i++) {
                for (int j = 0; j < MatrixUtil<int>.getNumOfCols(checkMyFintessVal); j++) {
                    if (checkMyFintessVal[i, j] == theBestChromosomeYet[i, j]) {
                        score +=1;
                    }
                }
            }

            population[IndexOfChromosomeInPop].setWeight(score);
            float res = score /(float) (generator.facilitySize.Item1 * generator.facilitySize.Item2);

            //normalize score to a range [0,1] ;
            return res; 
        }



        public float probabilityOfSelection(int IndexOfChromosomeCode) {
            return population[IndexOfChromosomeCode].fitnessValue/ population.Sum(x => x.fitnessValue);
        }


     

        public void setForAllFitness(int lastIndex)
        {
            for (int i = 0; i < lastIndex; i++)
            {
                population[i].setFitnessValue(fitness(i,lastIndex));
            }

        }

        public void setForAllFitnessAfter()
        {
            for (int i = 0; i < numberOfCodes; i++)
            {
                population[i].setFitnessValue(fitness(i, numberOfCodes));
            }

        }


        public void setProbForAll() {
            var  fromTheWorst = population.OrderByDescending(p => p.totalCost).ToList();
            float sum = 0;
            float startRange = 0;
            float endRange = 0;
            for (int i = 0; i < numberOfCodes; i++)
            {
                startRange = sum;
                sum+=probabilityOfSelection(i); 
                endRange = sum;

                population[i].setProbOfBeingSelected(sum);
                population[i].rangeOfProb = (startRange, endRange);
            }

        }


        public void mutation(ChromosomeCode code) {
           code.rewriteT(MatrixUtil<int>.permutateMatrix(code.weight, code.chromosomeCode, 0));
 

        }



        public void  print() {
            foreach (ChromosomeCode code in population) {
                code.print();
            }
        }

        public List<ChromosomeCode> generatePopulationZero()
        {
            population = new List<ChromosomeCode>();
            ChromosomeCode current = generator.generateOne();
            population.Add(current);
            foreach (int value in Enumerable.Range(1, numberOfCodes))
            {
                ChromosomeCode next = generator.generateNext(current);
                population.Add(next);
               
            }

           
            return population;
        }

        public void  sortPopulationByTotalCost(Cost cost) {

            population.Sort((x, y) => x.totalCost.CompareTo(y.totalCost));

        }


        
        public double getMeanCost() {

            return population.Average(x => x.totalCost);
        }

        public double getFintessValue(ChromosomeCode code) {

            var listDesc=population.OrderByDescending(x => x.totalCost).ToList();
            return listDesc.IndexOf(code)/numberOfCodes;
        }

       

     


        public ChromosomeCode getTheBestFromRange(int numOfCodes)
        {
            ChromosomeCode theBestOne = population[0];
            ChromosomeCode next;

            for (int i = 0; i < numOfCodes; i++)
            {
                next = population[i];
                if (theBestOne.totalCost > next.totalCost)
                {
                    theBestOne = next;
                }
            }
            return theBestOne;
        }



        public ChromosomeCode generateAndGetTheBest(int numOfCodes) {
            ChromosomeCode theBestOne = null;
            ChromosomeCode next = generator.generateOne();

            for (int i = 0; i < numOfCodes; i++)
            {
                next = generator.generateNext(next);
                if (theBestOne == null || theBestOne.totalCost > next.totalCost) {
                    theBestOne = next;
                }
            }
            return theBestOne;
        }



        public List<ChromosomeCode> filterTheBest(double ratio, Cost cost) // ratio as a percent filter value for population (hetting only ratio of all the chromosomeCodes in pop)
        {
            //the list is sorted in ascending order by a total cost
            int lastIndex =Convert.ToInt32(numberOfCodes * ratio);
            return population.GetRange(0, lastIndex);
           
        }
        
        

     }





    class ChromosomeCodeGenerator {

        public int numberOfMachines;
        public Tuple<int, int> facilitySize;
        public Cost cost;

        public ChromosomeCodeGenerator(Tuple<int, int> facilitySize, int numOfMach, Cost cost) {
            this.facilitySize = facilitySize;
            this.numberOfMachines = numOfMach;
            this.cost = cost;
        }

   

        public ChromosomeCode generateOne() {
            ChromosomeCode code = new ChromosomeCode(facilitySize, numberOfMachines);
            code.setTotalCost(cost);
            return code;

        }


       

        public ChromosomeCode generateNext(ChromosomeCode prev) {
            ChromosomeCode code = new ChromosomeCode(prev, numberOfMachines);
            code.setTotalCost(cost);
            return code;

        }


        public ChromosomeCode generateFromList(List<int> list) {
            ChromosomeCode code = new ChromosomeCode(list, facilitySize, numberOfMachines);
            code.setTotalCost(cost);
            return code;
        }



    }



    class ChromosomeCode
    {
        public int[,] chromosomeCode;
        public int numberOfMachines { get; set; }
        public int totalCost;
        public float fitnessValue;
        public bool isFintessValueSet = false;
        public int weight;
        Tuple<int, int> facilitySize { get; set; }

        public (float, float) rangeOfProb { get; set; }
        public float probOfBeingSelected;

        public void setProbOfBeingSelected(float probOfBeingSel) {
            probOfBeingSelected=probOfBeingSel;
        }

        public ChromosomeCode(Tuple<int, int> facilitySize, int numOfMach)
        {
            chromosomeCode = new int[facilitySize.Item1, facilitySize.Item2];
            this.facilitySize = facilitySize;
            numberOfMachines = numOfMach;
            fillMatrix(numOfMach, 0);

        }



        public ChromosomeCode(List<int> list, Tuple<int, int> facilitySize, int numOfMach) {
            chromosomeCode = new int[facilitySize.Item1, facilitySize.Item2];
            numberOfMachines = numOfMach;
            this.facilitySize = facilitySize;
            MatrixUtil<int>.fillMatrix(list, chromosomeCode, 0);

           
     
        }

        public  void rewriteT(int[,] matrix)
        {

            for (int i = 0; i < MatrixUtil<int>.getNumOfRows(matrix); i++)
            {
                for (int j = 0; j <  MatrixUtil<int>.getNumOfRows(matrix); j++)
                {
                    chromosomeCode[i, j] = matrix[i, j];
                }
            }
        }


        //score of how close it is to the target
        public void setFitnessValue(float fitnessVal) {
            fitnessValue =  fitnessVal;
            isFintessValueSet = true;
        }


        


        public void setWeight(int weight) {  
            this.weight=weight;

        }

       
        

        public ChromosomeCode(ChromosomeCode chromosome,  int numOfMach)
        {
            chromosomeCode = MatrixUtil<int>.permutateMatrix(3, chromosome.chromosomeCode, 0);
           
            this.numberOfMachines = numOfMach;


        }

        public void setTotalCost(Cost cost) {
            totalCost = getTotalCost(cost);
        }

        public void fillMatrix(int end, int defaultValue)
        {
            List<int> fillFrom = Enumerable.Range(1, end).ToList();
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