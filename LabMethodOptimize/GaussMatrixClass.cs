using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FractionArifmetic;

namespace LabMethodOptimize
{
    class GaussMatrix
    {
        public uint RowCount;
        public uint ColumCount;
        public Fraction[][] Matrix { get; set; }
        public Fraction[] RightPart { get; set; }
        public Fraction[] Answer { get; set; }

        public GaussMatrix(uint Row, uint Colum)
        {
            RightPart = new Fraction[Row];
            Answer = new Fraction[Row];
            Matrix = new Fraction[Row][];
            for (int i = 0; i < Row; i++)
                Matrix[i] = new Fraction[Colum];
            RowCount = Row;
            ColumCount = Colum;
        }

        public int SolveMatrix()
        {
            if (RowCount != ColumCount)
                return 1; //нет решения

            for (int i = 0; i < RowCount - 1; i++)// После этого цикла имеем матрицу у которой снизу диагонали нули
            {
                SortRows(i);
                for (int j = i + 1; j < RowCount; j++)
                {
                    if (Matrix[i][i] != 0) //если главный элемент не 0, то производим вычисления
                    {
                        Fraction MultipleCoeff = Matrix[j][i] / Matrix[i][i];
                        for (int k = i; k < ColumCount; k++)
                        {
                            Matrix[j][k] -= Matrix[i][k] * MultipleCoeff;
                        }
                        RightPart[j] -= RightPart[i] * MultipleCoeff;
                    }
                    //для нулевого главного элемента просто пропускаем данный шаг
                }
            }

            if (Matrix[RowCount - 1][ColumCount - 1] == 0 && RightPart[RowCount - 1] != 0)
            {
                //Нет решений
                Console.WriteLine("Нет решений!");
                return 1;
            }
            if (Matrix[RowCount - 1][ColumCount - 1] == 0 && RightPart[RowCount - 1] == 0)
            {
                //бм решений
                Console.WriteLine("Бесконечно много решений!");
                return 1;
            }

            for (int i = 0; i < RowCount; i++)// Делаем на главной диагонали единицы
            {
                Fraction ReversedElement = 1 / Matrix[i][i];
                for (int j = i; j < ColumCount; j++)
                {
                    Matrix[i][j] *= ReversedElement;
                }
                RightPart[i] *= ReversedElement;
            }

            for (int h = 0; h <= (int)(RowCount - 2); h++)// После этого цикла имеем единичную матрицу
            {
                for (int i = (int)(RowCount - 2) - h; i >= 0; i--)
                {
                    Fraction MultipleCoeff = Matrix[i][(int)(ColumCount - 1) - h];
                    Matrix[i][(int)(ColumCount - 1) - h] -= MultipleCoeff;
                    RightPart[i] -= MultipleCoeff * RightPart[RowCount - 1 - h];
                }
            }

            return 0;
        }

        private void SortRows(int SortIndex)// Метод, который поднимает строку с наибольшим числом выше.
        {
            Fraction MaxElement = Matrix[SortIndex][SortIndex];
            int MaxElementIndex = SortIndex;
            for (int i = SortIndex + 1; i < RowCount; i++)
            {
                if (Fraction.Abs(Matrix[i][SortIndex]) > MaxElement)
                {
                    MaxElement = Fraction.Abs(Matrix[i][SortIndex]);
                    MaxElementIndex = i;
                }
            }

            //теперь найден максимальный элемент ставим его на верхнее место
            if (MaxElementIndex > SortIndex)//если это не первый элемент
            {
                Fraction Temp;

                // Меняем местами элементы из правой части матрицы
                Temp = RightPart[MaxElementIndex];
                RightPart[MaxElementIndex] = RightPart[SortIndex];
                RightPart[SortIndex] = Temp;

                for (int i = 0; i < ColumCount; i++)// Меняем местами элементы строчки в которой самый большой элемент
                {                                   // и строчки, номер которой был передан как аргумент.
                    Temp = Matrix[MaxElementIndex][i];
                    Matrix[MaxElementIndex][i] = Matrix[SortIndex][i];
                    Matrix[SortIndex][i] = Temp;
                }
            }
        }

        public override String ToString()
        {
            String S = "";
            for (int i = 0; i < RowCount; i++)
            {
                S += "\r\n";
                for (int j = 0; j < ColumCount; j++)
                {
                    S += Matrix[i][j].ToString() + "\t";
                }

                S += "\t" + RightPart[i].ToString();
            }
            return S;
        }
    }
}

