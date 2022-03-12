using System;
using System.Collections.Generic;
using System.Text;

namespace Genetic_Algorithm_1
{

    // MatrixCellCost myDeserializedClass = JsonConvert.DeserializeObject<MatrixCellCost>(myJsonResponse);

    public class MatrixCell
    {
      public int source { get; set; }
        public int dest { get; set; }
    }

    public class MatrixCellCost: MatrixCell
    {
        public int cost { get; set; }

    }

    public class MatrixDefaultCellCost : MatrixCellCost {
        public int source = -1;
        public int dest = -1;
        public int cost = -1;


    }

    public class MatrixCellFlow : MatrixCell
    {
        public int flow { get; set; }
    }

    public class MatrixDefaultFlowCost : MatrixCellFlow
    {
        public int source = -1;
        public int dest = -1;
        public int flow = -1;
    }

}