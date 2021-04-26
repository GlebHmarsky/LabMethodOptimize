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
        public List<int[]> bearingEls { get; }
        public int iteration { get; set; }

        public SimplexSolver(uint Row, uint Colum)
        {
            iteration = 0;
            ILBasisEl = new List<int>();
            ILFreeEl = new List<int>();

            Matrix = new Fraction[Row][];
            for (int i = 0; i < Row; i++)
                Matrix[i] = new Fraction[Colum];

            RightPart = new Fraction[Row];
            ObjFuncion = new Fraction[Colum];

            OFV = new Fraction(0);

            RowCount = Row;
            ColumCount = Colum;

            bearingEls = new List<int[]>();
        }
        /*
         * Заполняет симплекс таблицу из таблицы гаусса 
         * Можно считать это корректной инициилазцией объекта и началом работы с самой симплекс таблицей
         */
        public void FillTable(GaussMatrix gaussM, DataGridViewRow initObjFunc, bool fFindMax)
        {
            ILBasisEl = new List<int>(gaussM.IndexListBasisElements);

            for (int i = 0; i < ColumCount + RowCount; i++)
            {
                if (!ILBasisEl.Contains(i)) ILFreeEl.Add(i);
            }

            for (int i = 0; i < RowCount; i++)
            {
                for (int g = 0; g < ILFreeEl.Count; g++)
                {
                    Matrix[i][g] = gaussM.Matrix[i][ILFreeEl[g]];
                }
            }

            RightPart = gaussM.RightPart;
            CalculateObjectiveFunction(initObjFunc, fFindMax);
            CalculateObjectiveFunctionValue(initObjFunc, fFindMax);
        }

        private void CalculateObjectiveFunction(DataGridViewRow initObjFunc, bool fFindMax)
        {
            for (int i = 0; i < ColumCount; i++)
            {
                Fraction Result = new Fraction(0);
                for (int g = 0; g < RowCount; g++)
                {
                    Result += (-Matrix[g][i]) * (new Fraction(initObjFunc.Cells[ILBasisEl[g]].Value.ToString()));
                }

                if (fFindMax) ObjFuncion[i] = -(Result + (new Fraction(initObjFunc.Cells[ILFreeEl[i]].Value.ToString())));
                else ObjFuncion[i] = Result + (new Fraction(initObjFunc.Cells[ILFreeEl[i]].Value.ToString()));
            }
        }
        private void CalculateObjectiveFunctionValue(DataGridViewRow initObjFunc, bool fFindMax)
        {
            Fraction Result = new Fraction(0);
            for (int i = 0; i < RowCount; i++)
            {
                Result += RightPart[i] * (new Fraction(initObjFunc.Cells[ILBasisEl[i]].Value.ToString()));
            }

            if (fFindMax) OFV = Result;
            else OFV = -Result;
        }

        public int FindBearingElements()
        {
            int indexFreeEl = -1, indexBasisEl = -1;
            bool fHaveNegativeCol = false;
            for (int i = 0; i < ColumCount; i++)
            {
                indexFreeEl = -1;
                indexBasisEl = -1;

                Fraction MinEl = new Fraction(-1);
                if (ObjFuncion[i] < 0) //то пробежаться по столбцу и найти тот самый элемент.
                {
                    indexFreeEl = i; //запоминаем столбец где есть отрицательный элемент снизу
                    //Нужно подобрать первый подходящий минимальный элемент. 
                    fHaveNegativeCol = true;
                    for (int g = 0; g < RowCount; g++)//Ищет первый попавшийся хороший элемент в столбце
                    {
                        if (Matrix[g][i] > 0)
                        {
                            MinEl = (RightPart[g] / Matrix[g][i]);
                            indexBasisEl = g;
                            break;
                        }
                    }

                    for (int g = indexBasisEl + 1; g < RowCount; g++) //Ищет минимальный элемент в столбце. 
                    {

                        if (Matrix[g][i] > 0)
                        {
                            if ((RightPart[g] / Matrix[g][i]) < MinEl)
                            {
                                MinEl = (RightPart[g] / Matrix[g][i]);
                                indexBasisEl = g;
                            }
                        }
                    }

                }
                if (indexBasisEl >= 0) //Нашёлся такой элемент в столбце
                {
                    bearingEls.Add(new int[2] { indexBasisEl, indexFreeEl });
                }



            }//Конец цикла по столбцам
            if (bearingEls.Count == 0)//Список пуст, элементов для шага нету.
            {
                if (fHaveNegativeCol) // Т.е. нашёлся такой столбец где снизу отрицательный элемент, но переход не осуществим. 
                {
                    return 2; //Сигнал о том что задача несовместна.
                }
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// index - переменная отвечающая за номер опорного элемента (слева направо)
        /// </summary>
        /// <param name="indexBearingEl"></param>
        public void SimplexStepWithCurrentEl(int indexBearingEl)
        {
            if (indexBearingEl >= bearingEls.Count)
            {
                MessageBox.Show("Такого опорного элемента не сущетвует");
                return;
            }
            int iRow, iColum;
            iRow = bearingEls[indexBearingEl][0];
            iColum = bearingEls[indexBearingEl][1];

            //TODO сменить индексы элементов, которые собственно и меняются, в List's
            int tmpIndex = ILFreeEl[iColum];
            ILFreeEl[iColum] = ILBasisEl[iRow];
            ILBasisEl[iRow] = tmpIndex;

            Fraction[][] NewMatrix = new Fraction[RowCount][];
            Fraction[] NewRightPart = new Fraction[RowCount];
            Fraction[] NewObjFuncion = new Fraction[ColumCount];
            Fraction NewOFV = new Fraction();

            for (int i = 0; i < RowCount; i++)
                NewMatrix[i] = new Fraction[ColumCount];
            // four steps to succes;
            // 1
            NewMatrix[iRow][iColum] = 1 / Matrix[iRow][iColum];
            // 2
            for (int g = 0; g < ColumCount; g++)
            {
                if (g == iColum) continue;
                NewMatrix[iRow][g] = Matrix[iRow][g] / Matrix[iRow][iColum];
            }
            NewRightPart[iRow] = RightPart[iRow] / Matrix[iRow][iColum];
            // 3
            for (int i = 0; i < RowCount; i++)
            {
                if (i == iRow) continue;
                NewMatrix[i][iColum] = -(Matrix[i][iColum] / Matrix[iRow][iColum]);
            }
            NewObjFuncion[iColum] = -(ObjFuncion[iColum] / Matrix[iRow][iColum]);
            // 4
            Fraction MultiplyEl = new Fraction(0);
            for (int i = 0; i < RowCount; i++)
            {
                if (i == iRow) continue;
                MultiplyEl = Matrix[i][iColum];
                for (int g = 0; g < ColumCount; g++)
                {
                    if (g == iColum) continue;
                    NewMatrix[i][g] = Matrix[i][g] - (NewMatrix[iRow][g] * MultiplyEl);
                }
                NewRightPart[i] = RightPart[i] - (NewRightPart[iRow] * MultiplyEl);
            }

            //Для нижней строки
            MultiplyEl = ObjFuncion[iColum];
            for (int g = 0; g < ColumCount; g++)
            {
                if (g == iColum) continue;
                NewObjFuncion[g] = ObjFuncion[g] - (NewMatrix[iRow][g] * MultiplyEl);
            }
            NewOFV = OFV - (NewRightPart[iRow] * MultiplyEl);

            Matrix = NewMatrix;
            RightPart = NewRightPart;
            ObjFuncion = NewObjFuncion;
            OFV = NewOFV;
            bearingEls.Clear();//Списко опорных элементов изменился для новой таблицы, поэтому его нужно перестраивать.
            iteration++;
        }
        /// <summary>
        /// находим оптимальный вариант (выбираем максимальный элемент по модулю снизу)
        /// </summary>
        /// <returns></returns>
        public int FindOptimalBearingElement()
        {
            if (bearingEls.Count == 0)
            {
                return -1; //Нету элементов для поиска.
            }
            int optimalIndex = 0;
            Fraction MaxEl = Fraction.Abs(ObjFuncion[bearingEls[0][1]]); //Кладём максимальный по модулю элемент в опорном столбце из целевой функции
            for (int i = 1; i < bearingEls.Count; i++)
            {
                if (MaxEl < Fraction.Abs(ObjFuncion[bearingEls[i][1]]))
                {
                    optimalIndex = i;
                    MaxEl = Fraction.Abs(ObjFuncion[bearingEls[i][1]]);
                }
            }
            return optimalIndex;
        }
        public int SimplexStep()
        {
            if (bearingEls.Count == 0)
            {
                return 1; //Нету элементов для симплекс шага.
            }
            SimplexStepWithCurrentEl(FindOptimalBearingElement());

            return 0; //Всё успешно.
        }
    }
}
