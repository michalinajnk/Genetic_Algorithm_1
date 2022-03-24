using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Genetic_Algorithm_1
{

    public static class MatrixUtil<T> {
        

        public static void fillMatrixFromList(List<T> fillFrom, T[,] matrix, T defaultValue)
        {
            var iter = fillFrom.GetEnumerator();
           
    
            for (int i = 0; i < getNumOfRows(matrix); i++ ) 
            {
                for (int j = 0; j < getNumOfCols(matrix); j++)
                {
                    if (iter.MoveNext())
                    {
                        matrix[i, j] = iter.Current;
                    }
                    else {
                        matrix[i, j] = defaultValue;
                    }
                }
            }
           
        }

        public static T[,]  permutateMatrix(int numOfSwapped, T[,] matrix, T defaultVal) {
            List<T> permutation = PermutationUtil<T>.permutate(rewriteToList(matrix), numOfSwapped);
            T[,] mtx = new T[ MatrixUtil<T>.getNumOfRows(matrix), MatrixUtil<T>.getNumOfCols(matrix)];
            fillMatrixFromList(permutation,mtx, defaultVal);
            return mtx;
        }

       

        public static void print(T[,] matrix)
        {
            Console.Write("[ ");
            for (int i = 0; i < getNumOfRows(matrix); i++)
            {
                for (int j = 0; j < getNumOfCols(matrix); j++)
                {
                    Console.Write(matrix[i, j] + " ");

                }
                if (i < getNumOfRows(matrix))
                {
                    Console.WriteLine();
                }
                else { Console.Write(" ] "); }
            }
            
        }


        public static int  getNumOfCols(T[,] matrix) {
            return matrix.GetLength(1);
        }

        public static int  getNumOfRows(T[,] matrix) {
            return matrix.GetLength(0);
        }

        public static int getNumOfElements(T[,] matrix) {
            return getNumOfCols(matrix) * getNumOfRows(matrix);
        }

        public static List<T> rewriteToList(T[,] matrix)
        {
            List<T> rewritten = new List<T>();
            for (int i = 0; i < getNumOfRows(matrix); i++)
            {
                for (int j = 0; j < getNumOfCols(matrix); j++)
                {
                    rewritten.Add(matrix[i, j]);
                }
            }
            return rewritten;
        }

        public static T[] rewriteToArr(T[,] matrix)
        {
           return rewriteToList(matrix).ToArray();
        }

       




        public static bool Equals(T[,] matrix1, T[,] matrix2) {
            List<T> firstArr = rewriteToList(matrix1);
            List<T> secArr = rewriteToList(matrix2);
            return firstArr.Equals(secArr);
                
        }

        internal static void print(MatrixCellCost[,] matrixCellCost)
        {
            throw new NotImplementedException();
        }
    }


    public static  class PermutationUtil<T>
    {
        private const int NUMBER_OF_SWAPPED = 3;


       

        public static  List<T> permutate(List<T> toPermute, int nTOSwap = NUMBER_OF_SWAPPED )
        {
            Random randGenerator = new Random();
            for (int i = 0; i < nTOSwap; i++)
            {
                swap(toPermute, randGenerator.Next(toPermute.Count), randGenerator.Next(toPermute.Count));
            }
            return toPermute;
        }

        public static T[] permutate(T[] toPermute, int nTOSwap = NUMBER_OF_SWAPPED)
        {
            Random randGenerator = new Random();
            for (int i = 0; i < nTOSwap; i++)
            {
                swap(toPermute, randGenerator.Next(toPermute.Length), randGenerator.Next(toPermute.Length));
            }
            return toPermute;
        }


        public static List<T> getSwapped<T>(List<T> list, int indexA, int indexB)
        {
            (list[indexA], list[indexB]) = (list[indexB], list[indexA]);
            return list;
        }

        public static T[] getSwapped(T[] arr, int indexA, int indexB)
        {
            (arr[indexA], arr[indexB]) = (arr[indexB], arr[indexA]);
            return arr;
        }

        public static void swap<T>(List<T> list, int indexA, int indexB)
        {
            (list[indexA], list[indexB]) = (list[indexB], list[indexA]);
        }

        public static void  swap(T[] arr, int indexA, int indexB)
        {
            (arr[indexA], arr[indexB]) = (arr[indexB], arr[indexA]);
           
        }

    }

    public static class MathUtil {
       

        public static float nextFloat(float min, float max)
        {
            System.Random random = new System.Random();
            double val = (random.NextDouble() * (max - min) + min);
            return (float)val;
        }

        public static void printList(List<int> list) {
            Console.WriteLine("");
            foreach (int elem in list) {
                Console.Write(elem +", ");
            }
            Console.WriteLine("" );
        }


      

    }
}
