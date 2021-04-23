using FractionArifmetic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GaussMatrixClass;
using System.Windows.Forms;
using LabMethodOptimize;
namespace SimplexSolverClass
{
    class SimplexSolver
    {
        public uint RowCount;
        public uint ColumCount;
        public Fraction[][] Matrix { get; set; }
        public Fraction[] RightPart { get; set; }
        public Fraction[] ObjFuncion { get; set; }
        public Fraction[] InitialObjFuncion { get; set; }
        public List<int> ILBasisEl { get; set; }
        public List<int> ILFreeEl { get; set; }
        public Fraction OFV { get; set; }

        private int iteration;

        public SimplexSolver(uint Row, uint Colum)
        {
            iteration = 0;
            ILBasisEl = new List<int>();
            ILFreeEl = new List<int>();

            Matrix = new Fraction[Row][];
            for (int i = 0; i < Row; i++)
                Matrix[i] = new Fraction[Colum];

            RightPart = new Fraction[Row];
            ObjFuncion = new Fraction[Row];

            OFV = new Fraction(0);

            RowCount = Row;
            ColumCount = Colum;
        }

        public void FillTable(GaussMatrix gaussM, DataGridViewRow initObjFunc)
        {
            ILBasisEl = new List<int>(gaussM.IndexListBasisElements);

            for (int i = 0; i < (ColumCount - 1) + (RowCount - 1); i++)
            {
                if (!ILBasisEl.Contains(i)) ILFreeEl.Add(i);
            }

            for (int i = 0; i < RowCount; i++)
            {
                for (int g = 0; g < ILBasisEl.Count; g++)
                {
                    Matrix[i][ILBasisEl[g]] = gaussM.Matrix[i][ILBasisEl[g]];
                }
            }

            RightPart = gaussM.RightPart;
            CalculateObjectiveFunction(initObjFunc);
            CalculateObjectiveFunctionValue(initObjFunc);
        }

        private void CalculateObjectiveFunction(DataGridViewRow initObjFunc)
        {
            for (int i = 0; i < ColumCount; i++)
            {
                Fraction Result = new Fraction(0);
                for (int g = 0; g < RowCount; g++)
                {
                    Result += -Matrix[g][i] * (new Fraction(initObjFunc.Cells[ILBasisEl[g]].Value.ToString()));
                }

                ObjFuncion[i] = Result + (new Fraction(initObjFunc.Cells[ILFreeEl[i]].Value.ToString()));
            }
        }
        private void CalculateObjectiveFunctionValue(DataGridViewRow initObjFunc)
        {
            Fraction Result = new Fraction(0);
            for (int i = 0; i < RowCount; i++)
            {
                Result += RightPart[i] * (new Fraction(initObjFunc.Cells[ILBasisEl[i]].Value.ToString()));
            }

            OFV = -Result;
        }

    }
}
