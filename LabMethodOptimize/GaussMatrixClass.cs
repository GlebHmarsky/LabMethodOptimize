using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FractionArifmetic;

namespace GaussMatrixClass
{
    class GaussMatrix
    {
        public uint RowCount;
        public uint ColumCount;
        public Fraction[][] Matrix { get; set; }
        public Fraction[] RightPart { get; set; }
        public List<int> IndexListBasisElements { get; set; }
        public GaussMatrix(uint Row, uint Colum)
        {
            IndexListBasisElements = new List<int>();
            RightPart = new Fraction[Row];

            Matrix = new Fraction[Row][];
            for (int i = 0; i < Row; i++)
                Matrix[i] = new Fraction[Colum];
            RowCount = Row;
            ColumCount = Colum;
        }
        public GaussMatrix(uint Size)
        {
            IndexListBasisElements = new List<int>();
            RightPart = new Fraction[Size];

            Matrix = new Fraction[Size][];
            for (int i = 0; i < Size; i++)
                Matrix[i] = new Fraction[Size];
            RowCount = Size;
            ColumCount = Size;

            for (int i = 0; i < Size; i++)
            {
                IndexListBasisElements.Add(i);
            }
        }

        public int SolveMatrix()
        {
            Fraction MultipleCoeff;
            if (IndexListBasisElements.Count != RowCount /*|| IndexListBasisElements.Count == 0*/)
                return 1; //Ошибка: неправильное количество базисных элементов 
            for (int i = 0; i < RowCount - 1; i++)// После этого цикла имеем матрицу у которой снизу диагонали нули
            {
                if (SortRows(i) > 0) // позволяет вынести ненулевой элемент столбца вверх 
                    return 1; // ругаемся что у нас нулевой столбец
                for (int j = i + 1; j < RowCount; j++)
                {

                    if (Matrix[i][IndexListBasisElements[i]] != 0) //если главный элемент не 0, то производим вычисления
                    {
                        MultipleCoeff = Matrix[j][IndexListBasisElements[i]] / Matrix[i][IndexListBasisElements[i]];
                        for (int k = 0; k < ColumCount; k++)
                        {
                            Matrix[j][k] -= Matrix[i][k] * MultipleCoeff;
                        }
                        RightPart[j] -= RightPart[i] * MultipleCoeff;
                    }
                    //для нулевого главного элемента просто пропускаем данный шаг
                }
            }


            for (int i = 0; i < RowCount; i++)
            {
                if (Matrix[i][IndexListBasisElements[i]] == 0)//Базисный элемент в строке = 0
                {
                    //Зависимые условия или система несовместна
                    //В любом случае выходим и ругаемся на пользователя
                    return 1;
                }
            }

            for (int i = 0; i < RowCount; i++)// Делаем на базисных элементах единицы
            {
                Fraction ReversedElement = 1 / Matrix[i][IndexListBasisElements[i]];
                for (int j = 0; j < ColumCount; j++)
                {
                    Matrix[i][j] *= ReversedElement;
                }
                RightPart[i] *= ReversedElement;
            }


            for (int i = (int)RowCount - 1; i > 0; i--)
            {
                for (int g = 0; g < i; g++)
                {
                    MultipleCoeff = Matrix[g][IndexListBasisElements[i]];
                    for (int col = 0; col < ColumCount; col++)
                    {
                        Matrix[g][col] -= MultipleCoeff * Matrix[i][col];
                    }
                    RightPart[g] -= MultipleCoeff * RightPart[i];
                }
            }
            return 0;
        }

        private int SortRows(int SortIndex)// Метод, который поднимает строку с наибольшим числом выше. (конкртено тут используется для выноса ненулевого элемента вверх)
        {
            if (Matrix[SortIndex][IndexListBasisElements[SortIndex]] != 0)
            {
                return 0;
            }
            int MaxElementIndex = SortIndex;
            for (int i = SortIndex + 1; i < RowCount; i++)
            {
                if (Fraction.Abs(Matrix[i][IndexListBasisElements[SortIndex]]) != 0)
                {
                    MaxElementIndex = i;
                    break;
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
                return 0;
            }
            return 1;
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

        public void GetRidOfNegativeLines()
        {
            for (int i = 0; i < RowCount; i++)
            {                
                if (RightPart[i] < 0)
                {
                    for (int g = 0; g < ColumCount; g++)
                    {
                        if (IndexListBasisElements.Contains(g))
                        {
                            continue;
                        }
                        Matrix[i][g] *= -1;
                    }
                    RightPart[i] *= -1;
                }
            }
        }
    }
}

