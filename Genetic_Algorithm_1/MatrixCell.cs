using DotLiquid.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Genetic_Algorithm_1
{

    // MatrixCellCost myDeserializedClass = JsonConvert.DeserializeObject<MatrixCellCost>(myJsonResponse);



    public class MatrixCells<T> where T : MatrixCell
    {

        public List<T> cells { get; set; }

        public MatrixCells(List<T> cells2) {
            cells = new List<T>();
            cells.AddRange(cells2);

        }



        public  int getElemAt(int i, int j, List<MatrixCellFlow> cells)
        {
            IEnumerable<int> elem =
            from cell in cells
            where cell.dest == i && cell.source == j
            select cell.amount;
            if (!elem.Any())
            {
                return 0;
            }

            return elem.First();

        }

        public  int getElemAt(int i, int j, List<MatrixCellCost> cells)
        {
            IEnumerable<int> elem =
            from cell in cells
            where cell.dest == i && cell.source == j
            select cell.cost;
            if (!elem.Any())
            {
                return 0;
            }

            return elem.First();

        }





        public int getMaxCellValue() {
            return Math.Max(getDestValues().Max(), getSourceValues().Max());
        }

       

        public IEnumerable<int> getDestValues() {
            IEnumerable<int> destValues =
                from cell in cells
                select cell.dest;
            return destValues;
        }

        public IEnumerable<int> getSourceValues() {
            IEnumerable<int> sourceValues =
                from cell in cells
                select cell.source;
            return sourceValues;
        }



    }

  


    public class MatrixCell
    {
        public int source { get; set; }
        public int dest { get; set; }
       

    }

    public class MatrixCellCost: MatrixCell
    {
       

        public int cost { get; set; }
        

        public String toString()
        {
            return source + ", " + dest + ", " + cost;
        }
    }


    public class MatrixCellFlow : MatrixCell
    {

       
        public int amount { get; set; }
      

        public String toString() {
            return "{ " + source + ", " + dest + ", " + amount + " }";
        }
    }


}