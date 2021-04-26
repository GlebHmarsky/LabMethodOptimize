using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FractionArifmetic;
using SimplexSolverClass;
using GaussMatrixClass;

namespace LabMethodOptimize
{
    public partial class Form1 : Form
    {
        /*-------------------     GLOBAL VAR      -----------------*/
        GaussMatrix GaussMat;
        uint RowCount, ColumnCount;
        int StartRowForSolutionGrid = 0;
        SimplexSolver SSolver;
        int pivotIndex;

        DataGridViewCellStyle LightCoralStyle = new DataGridViewCellStyle();
        DataGridViewCellStyle AquamarineStyle = new DataGridViewCellStyle();
        

        public Form1()
        {
            InitializeComponent();
            optimizationProblem.SelectedIndex = 0;
            fractionType.SelectedIndex = 0;
            radioButton1.Checked = true;

            LightCoralStyle.BackColor = Color.LightCoral;
            LightCoralStyle.ForeColor = Color.Black;

            AquamarineStyle.BackColor = Color.Aquamarine;
            AquamarineStyle.ForeColor = Color.Black;

            SolutionGridView.RowHeadersWidth = 4; 
        }


        private void numericUpDownColumn_ValueChanged(object sender, EventArgs e)
        {
            if (restrictionTable.Columns.Count < 1)
            {
                this.restrictionTable.Columns.Add(1.ToString(), "C");
            }


            if ((int)numericUpDownColumn.Value + 1 > restrictionTable.Columns.Count)
            {
                for (int i = restrictionTable.Columns.Count; i < (int)numericUpDownColumn.Value + 1; i++)
                {
                    restrictionTable.Columns.Add(i.ToString(), "C" + i);
                    objectiveFunctionTable.Columns.Add(i.ToString(), "a" + i);
                }
            }
            else
            {
                for (int i = restrictionTable.Columns.Count; i > (int)numericUpDownColumn.Value + 1; i--)
                {

                    this.restrictionTable.Columns.RemoveAt(i - 1);
                    objectiveFunctionTable.Columns.RemoveAt(i - 2);


                }
                if (numericUpDownColumn.Value == 0)
                {
                    numericUpDownRow.Value = 0;

                    this.restrictionTable.Columns.RemoveAt(0);
                    //objectiveFunctionTable.Columns.RemoveAt(0);
                    //basicVariablesTable.Columns.RemoveAt(0);
                }
            }

            foreach (DataGridViewColumn column in restrictionTable.Columns)
            {

                column.HeaderText = String.Concat("C",
                    (column.Index + 1).ToString());
                column.Width = 50;
            }
            if (restrictionTable.Columns.Count != 0)
                restrictionTable.Columns[restrictionTable.Columns.Count - 1].HeaderText = "C";


            if (objectiveFunctionTable.Rows.Count == 0 && objectiveFunctionTable.Columns.Count != 0) objectiveFunctionTable.Rows.Add(1);

            restrictionTable.TopLeftHeaderCell.Value = "B/N";

        }
        private void numericUpDownRow_ValueChanged(object sender, EventArgs e)
        {
            if (restrictionTable.Columns.Count == 0)
            {
                numericUpDownRow.Value = 0;
                return;
            }
            if ((int)numericUpDownRow.Value > restrictionTable.Rows.Count)
            {
                this.restrictionTable.Rows.Add((int)numericUpDownRow.Value - restrictionTable.Rows.Count);

                for (int i = basicVariablesTable.Rows.Count; i < (int)numericUpDownRow.Value; i++)
                {
                    basicVariablesTable.Columns.Add(i.ToString(), "");
                }

            }
            else
            {

                for (int i = restrictionTable.Rows.Count; i > (int)numericUpDownRow.Value - 1; i--)
                {
                    restrictionTable.Rows.RemoveAt(i - 1);
                    basicVariablesTable.Columns.RemoveAt(i - 1);
                }

            }

            foreach (DataGridViewRow row in restrictionTable.Rows)
            {

                row.HeaderCell.Value = String.Concat("f",
                    (row.Index + 1).ToString());
            }


            if (basicVariablesTable.Rows.Count == 0 && basicVariablesTable.Columns.Count != 0) basicVariablesTable.Rows.Add(1);
        }




        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var path = openFileDialog.FileName;
                    using (StreamReader sr = new StreamReader(path))
                    {
                        string[] str = sr.ReadLine().Split(' ');
                        // TODO:  Проверки на дурака


                        if (Convert.ToInt32(str[0]) == 0)
                            optimizationProblem.SelectedIndex = 0;
                        else
                            optimizationProblem.SelectedIndex = 1;

                        str = sr.ReadLine().Split(' ');


                        RowCount = (uint)Convert.ToInt32(str[0]);
                        ColumnCount = (uint)Convert.ToInt32(str[1]);
                        GaussMat = new GaussMatrix(RowCount, ColumnCount);

                        numericUpDownColumn.Value = ColumnCount;
                        numericUpDownRow.Value = RowCount;

                        str = sr.ReadLine().Split(' ');
                        for (int g = 0; g < ColumnCount; g++)
                        {
                            objectiveFunctionTable[g, 0].Value = Convert.ToInt32(str[g]);
                        }
                        int i;
                        for (i = 0; i < RowCount; i++)
                        {
                            str = sr.ReadLine().Split(' ');
                            //UNDONE Проверки на дурака 2
                            int g;
                            for (g = 0; g < ColumnCount; g++)
                            {
                                GaussMat.Matrix[i][g] = Convert.ToInt32(str[g]);
                                restrictionTable[g, i].Value = Convert.ToInt32(str[g]);
                            }
                            restrictionTable[g, i].Value = Convert.ToInt32(str[g]);
                            GaussMat.RightPart[i] = Convert.ToInt32(str[g]);
                        }


                    }
                }
                catch (Exception exp)
                {
                    MessageBox.Show($"Security error.\n\nError message: {exp.Message}\n\n" +
                    $"Details:\n\n{exp.StackTrace}");
                }
            }
        }

        private void BeginSolve_Click(object sender, EventArgs e)
        {

            tabControl.SelectTab(tabPage2);


            // TODO Сделать повторное заполнение GaussMat перед всеми операциями. Заполнять из таблицы на форме!
            // TODO Вызывает хороший метод из Simplex Solver и других штучек в соответсвии с выбранным решением задачи.


            GaussMat.IndexListBasisElements.Clear();
            if (SolutionGridView.Rows.Count < 10) SolutionGridView.Rows.Add(100);
            int tmpInteger;
            for (int k = 0; k < basicVariablesTable.Columns.Count; k++)
            {
                tmpInteger = Convert.ToInt32(basicVariablesTable[k, 0].Value) - 1;
                //TODO проверить что получаю корретаные данные а не строку буквенных символов, к примеру.
                if (tmpInteger >= 0 && tmpInteger <= ColumnCount)
                    GaussMat.IndexListBasisElements.Add(tmpInteger);
                else;
                //TODO тогда ошибка, не добавлены все базисные переменные для текущего количества ограничений. 
                //TODO + проверки на дурака чтобы не писали одинаковые перменные

            }

            GaussMat.IndexListBasisElements.Sort();
            if (GaussMat.SolveMatrix() == 1)
            {
                MessageBox.Show("Получил ошибку при решении!");
                return;
            }

            /*SimplexSolver*/
            SSolver = new SimplexSolver(RowCount, ColumnCount - RowCount);
            SSolver.FillTable(GaussMat, objectiveFunctionTable.Rows[0], optimizationProblem.SelectedIndex == 1);

            PrintResultToSoulutionGridView(SSolver);
            FindAndCheckBearingElements();
            pivotIndex = SSolver.FindOptimalBearingElement();
            ColorTheBearingEletemts();
            //TODO Проверить что после подсчёта симплекс таблицы у нас справа нету отрицательных элементов
            //TODO и в случае чего остановить дальнейшую работу приложения по причине некорректного базиса
        }

        private void ColorTheBearingEletemts()
        {
            if (SSolver.bearingEls.Count == 0) return;

            int iRow, iColum;
            int StartRowOfCurTable = StartRowForSolutionGrid - 2 - (int)SSolver.RowCount;
            for (int i = 0; i < SSolver.bearingEls.Count; i++)
            {
                iRow = SSolver.bearingEls[i][0];
                iColum = SSolver.bearingEls[i][1];
                SolutionGridView[iColum + 1, StartRowOfCurTable + iRow].Style = AquamarineStyle;
            }            
            iRow = SSolver.bearingEls[pivotIndex][0];
            iColum = SSolver.bearingEls[pivotIndex][1]; 
             SolutionGridView[iColum + 1, StartRowOfCurTable + iRow].Style = LightCoralStyle;
        }
        private void FindAndCheckBearingElements()
        {
            int returnResult;
            returnResult = SSolver.FindBearingElements();
            //TODO добавить на проверки что всё корренктно завершилось и делать выводы из того сколько элементов нашлось
            //TODO добавить поссле этого метода подстветку элементов на таблице
            if (returnResult > 0)
            {
                /* 
                 * Наверное стоит блокировать кнопку, чтобы пользователь даже  не пытался посчитать таблицу, т.к. нет элементов
                 * Но ведь это состояние уже и ответ и надо подумать что выводить. 
                 * 
                 * TODO Варианта 2:  Либо задача решена ||  Либо есть отрицательные столбцы, но нету элементов для прехода. 
                 * Другими словами - система несовместна.
                 * 
                 * В любом из вариантов мы дальше не шагаем и выходим, но нужно понимать что возвращать пользователю.
                 * 
                 * Думаю, стоит пробежаться по низу и посмотреть, есть ли элементы 
                 */
                ButtonSimplexStep.Enabled = false;

                if (returnResult == 1)
                {
                    // Всё хорошо - выводим ответ

                    StringBuilder answer;
                    answer = new StringBuilder("x* (");

                    for (int i = 0; i < SSolver.RowCount + SSolver.ColumCount; i++)
                    {
                        if (SSolver.ILBasisEl.Contains(i))
                        {
                            answer.Append(SSolver.RightPart[SSolver.ILBasisEl.IndexOf(i)].ToString());
                        }
                        else
                        {
                            answer.Append(0.ToString());
                        }
                        answer.Append(",");
                    }
                    answer.Length--;//Удаляем последний символ.
                    answer.Append(")");

                    answer.Append($"\r\n\nf(x*) = {(-SSolver.OFV).ToString()}");

                    SSTextAnswer.Text = answer.ToString();
                }
                else if (returnResult == 2)
                {
                    // Всё плолохо - система несовместна
                    SSTextAnswer.Text = "Система не совместна\nНет решений.";
                }
            }
        }
        private void ButtonSimplexStep_Click(object sender, EventArgs e)
        {
            SSolver.SimplexStepWithCurrentEl(pivotIndex); //TODO может тоже потребуется ловить возващаемое значение для проверки
            PrintResultToSoulutionGridView(SSolver);

            FindAndCheckBearingElements();
            if((pivotIndex = SSolver.FindOptimalBearingElement()) < 0)
            {
                //Нету элементов и надо выходить
                return;
            }
            ColorTheBearingEletemts();
           
        }

        private int BearingElsIndexOf(int[] arr)
        {
            int index = -1;
            for (int i = 0; i < SSolver.bearingEls.Count; i++)
            {
                if(SSolver.bearingEls[i][0] == arr[0] && SSolver.bearingEls[i][1] == arr[1])
                {
                    index = i;
                    break;
                }
            }
            return index;
        }
        private void SolutionGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int StartRowOfCurTable = StartRowForSolutionGrid - 2 - (int)SSolver.RowCount;
            int[] tmpArr = new int[2] { e.RowIndex - StartRowOfCurTable, e.ColumnIndex - 1};
            bool res = SSolver.bearingEls.Exists(x => (x[0] == tmpArr[0] && x[1] == tmpArr[1]));
            int coolIndex;
            coolIndex = BearingElsIndexOf(tmpArr);
            if (coolIndex >= 0 && coolIndex != pivotIndex)
            {                
                SolutionGridView[SSolver.bearingEls[pivotIndex][1] + 1, SSolver.bearingEls[pivotIndex][0] + StartRowOfCurTable].Style = AquamarineStyle;
                pivotIndex = coolIndex;
                SolutionGridView[SSolver.bearingEls[pivotIndex][1] + 1, SSolver.bearingEls[pivotIndex][0] + StartRowOfCurTable].Style = LightCoralStyle;
            }
            //Иначе Пользователь клацнул не туда, так что ничего не делаем
        }

        private void PrintResultToSoulutionGridView(SimplexSolver SSolver)
        {
            //TODO Сделать проверку что если новая таблица вылетает за границы то перетаскивать скролл ниже 
            //SolutionGridView.FirstDisplayedScrollingRowIndex = StartRowForSolutionGrid;


            int i, g;
            SolutionGridView[0, StartRowForSolutionGrid].Value = "X (" + SSolver.iteration.ToString() + ")";

            for (i = 0; i < SSolver.ColumCount; i++)
            {
                SolutionGridView[i + 1, StartRowForSolutionGrid].Value = "X" + (SSolver.ILFreeEl[i] + 1).ToString();
            }

            StartRowForSolutionGrid++;

            for (i = 0; i < SSolver.RowCount; i++)
            {
                SolutionGridView[0, i + StartRowForSolutionGrid].Value = "X" + (SSolver.ILBasisEl[i] + 1).ToString();
                for (g = 0; g < SSolver.ColumCount; g++)
                {
                    SolutionGridView[g + 1, i + StartRowForSolutionGrid].Value = SSolver.Matrix[i][g].ToString();
                }
                SolutionGridView[g + 1, i + StartRowForSolutionGrid].Value = SSolver.RightPart[i].ToString();
            }


            for (g = 0; g < SSolver.ColumCount; g++)
            {
                SolutionGridView[g + 1, i + StartRowForSolutionGrid].Value = SSolver.ObjFuncion[g].ToString();
            }
            SolutionGridView[g + 1, i + StartRowForSolutionGrid].Value = SSolver.OFV.ToString();

            StartRowForSolutionGrid += i + 2;
            //SolutionGridView[0, StartRowForSolutionGrid].Value = "u here!";
        }
    }


    // TODO:  Не допускать решение когда не прописаны базисные переменные для графического метода.

}
