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
using System.Text.RegularExpressions;

namespace LabMethodOptimize
{

    public partial class Form1 : Form
    {
        /*-------------------     GLOBAL VAR      -----------------*/
        //GaussMatrix GaussMat;
        uint RowCount, ColumCount;
        int StartRowForSSolutionGrid = 0;
        SimplexSolver SSolver;
        int pivotIndex;

        DataGridViewCellStyle LightCoralStyle = new DataGridViewCellStyle();
        DataGridViewCellStyle AquamarineStyle = new DataGridViewCellStyle();


        public Form1()
        {
            this.Text = "Simplex Solver";
            InitializeComponent();
            optimizationProblem.SelectedIndex = 0;
            fractionType.SelectedIndex = 0;
            RBSimplexMethod.Checked = true;

            LightCoralStyle.BackColor = Color.LightCoral;
            LightCoralStyle.ForeColor = Color.Black;

            AquamarineStyle.BackColor = Color.Aquamarine;
            AquamarineStyle.ForeColor = Color.Black;

            objectiveFunctionTable.RowHeadersWidth = 50;
            restrictionTable.RowHeadersWidth = 50;
            basicVariablesTable.RowHeadersVisible = false;
            SSolutionTable.RowHeadersVisible = false;
            ABSolverTable.RowHeadersVisible = false;
            //TODO Добавить сюда инициализацию таблицы решения и её многих столбцов.
            for (int g = 0; g < 18; g++)
            {
                SSolutionTable.Columns.Add(g.ToString(), "");
                SSolutionTable.Columns[g].Width = 60;
            }
            for (int g = 0; g < 34; g++)
            {
                ABSolverTable.Columns.Add(g.ToString(), "");
                ABSolverTable.Columns[g].Width = 60;
            }

        }
        // План на день ;)
        //  TODO необходимо пофиксить окраску и выбор элементов для каждого метода по отдельности
        // TODO  и разумеется доделать по нормальному иск. базис
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
                column.Width = 60;
            }
            if (restrictionTable.Columns.Count != 0)
                restrictionTable.Columns[restrictionTable.Columns.Count - 1].HeaderText = "C";

            foreach (DataGridViewColumn column in objectiveFunctionTable.Columns)
            {
                column.Width = 60;
            }
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

                for (int i = basicVariablesTable.Columns.Count; i < (int)numericUpDownRow.Value; i++)
                {
                    basicVariablesTable.Columns.Add(i.ToString(), "");
                }
            }
            else
            {
                for (int i = restrictionTable.Rows.Count; i > (int)numericUpDownRow.Value; i--)
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

            foreach (DataGridViewColumn column in basicVariablesTable.Columns)
            {
                column.Width = 35;
            }
            if (basicVariablesTable.Rows.Count == 0 && basicVariablesTable.Columns.Count != 0) basicVariablesTable.Rows.Add(1);
        }



        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {

                tabControl.SelectTab(tabPage1);
                //TODO При открытии файла очищать список базисных переменных
                if (basicVariablesTable.Rows.Count != 0)
                {
                    foreach (DataGridViewCell cell in basicVariablesTable.Rows[0].Cells)
                    {
                        cell.Value = null;
                    }
                }
                SSTextAnswer.Text = "";

                try
                {
                    var path = openFileDialog.FileName;
                    using (StreamReader sr = new StreamReader(path))
                    {
                        string[] str = sr.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (str.Length != 1)
                        {
                            throw new DataReadException("Ошибка в указании задачи оптимизации");
                        }

                        if (Convert.ToInt32(str[0]) == 0)
                            optimizationProblem.SelectedIndex = 0;
                        else
                            optimizationProblem.SelectedIndex = 1;

                        str = sr.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (str.Length != 2)
                        {
                            throw new DataReadException("Ошибка в указании количества ограничений/переменных");
                        }

                        RowCount = (uint)Convert.ToInt32(str[0]);
                        ColumCount = (uint)Convert.ToInt32(str[1]);
                        if (RowCount > ColumCount || RowCount == 0 || ColumCount == 0)
                        {
                            throw new DataReadException("Ошибка в указании количества ограничений/переменных");
                        }
                        if (RowCount > 16 || ColumCount > 16)
                        {
                            throw new DataReadException("Ошибка в указании количества ограничений/переменных\n\nМаскимальный размер 16x16");
                        }



                        numericUpDownColumn.Value = ColumCount;
                        numericUpDownRow.Value = RowCount;

                        str = sr.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (str.Length != ColumCount) //TODO ВОзможно тут надо будет добавить +1 т.к. не ясно может ли у целевой функции быть константа
                        {
                            throw new DataReadException("Ошибка в указании количества переменных целевой функции\n" +
                                $"Их: {str.Length}\nДолжно быть: {ColumCount}");
                        }
                        for (int g = 0; g < ColumCount; g++)
                        {
                            objectiveFunctionTable[g, 0].Value = Convert.ToInt32(str[g]);
                        }

                        int i;
                        for (i = 0; i < RowCount; i++)
                        {
                            str = sr.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            if (str.Length != ColumCount + 1) //+1 от того что записывается и число справа
                            {
                                throw new DataReadException($"Ошибка в указании количества коэффициентов в {i + 1} строке ограничений\n" +
                                    $"Их: {str.Length}\nДолжно быть: {ColumCount + 1}");
                            }

                            int g;
                            for (g = 0; g < ColumCount; g++)
                            {
                                restrictionTable[g, i].Value = Convert.ToInt32(str[g]);
                            }
                            restrictionTable[g, i].Value = Convert.ToInt32(str[g]);
                        }
                    }
                }
                catch (DataReadException exp)
                {
                    MessageBox.Show($"Data Read error.\n\nError message: Плохой формат ввода данных\n{exp.Message}\n\n" +
                    "\nС параметрами ввода вы можете ознакомится вj вкладке \"About\"");
                }
                catch (FormatException exp)
                {
                    MessageBox.Show($"Format error.\n\nError message: {exp.Message}\n\n" +
                    $"Details:\n\n{exp.StackTrace}");
                }
                catch (Exception exp)
                {
                    MessageBox.Show($"Security error.\n\nError message: {exp.Message}\n\n" +
                    $"Details:\n\n{exp.StackTrace}");
                }
            }
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                StreamWriter st;
                if ((st = new StreamWriter(Path.GetFullPath(saveFileDialog.FileName))) != null)
                {
                    st.Write(optimizationProblem.SelectedIndex.ToString());
                    st.Write("\r\n" + numericUpDownRow.Value.ToString() + " " + numericUpDownColumn.Value.ToString());
                    st.Write("\r\n");
                    for (int g = 0; g < ColumCount; g++)
                    {
                        st.Write(objectiveFunctionTable[g, 0].Value.ToString() + " ");
                    }
                    for (int i = 0; i < RowCount; i++)
                    {
                        st.Write("\r\n");
                        int g;
                        for (g = 0; g < ColumCount; g++)
                        {
                            st.Write(restrictionTable[g, i].Value.ToString() + " ");
                        }
                        st.Write(restrictionTable[g, i].Value.ToString()); // Правый элемент так же записываем.
                    }
                    st.Close();
                }
            }
        }

        private int SetIndexListBasisElementsFromTable(GaussMatrix GaussMat)
        {
            try
            {
                int tmpInteger;
                for (int k = 0; k < basicVariablesTable.Columns.Count; k++)
                {
                    tmpInteger = Convert.ToInt32(basicVariablesTable[k, 0].Value) - 1;
                    if (GaussMat.IndexListBasisElements.Contains(tmpInteger))
                    {
                        //Тогда элементы плохо написаны и следует заблокировать кнопку старта решения 
                        BeginSolve.Enabled = false;
                        throw new DataReadException("Элемент базиса повторяется");
                    }

                    if (tmpInteger >= 0 && tmpInteger <= ColumCount)
                    {
                        GaussMat.IndexListBasisElements.Add(tmpInteger);
                    }
                    else
                    {
                        BeginSolve.Enabled = false;
                        throw new DataReadException("Элемент начального базиса выходит за границы количества переменных задачи или не вовсе не задан");
                    }
                }
            }
            catch (DataReadException exp)
            {
                MessageBox.Show($"Data Read error.\n\nError message: Плохой формат ввода данных\n{exp.Message}\n\n");
                return 1;
            }
            catch (FractionException exp)
            {
                MessageBox.Show($"Format Fraction error.\n\nError message: {exp.Message}\n\n");
                return 1;
            }
            catch (FormatException exp)
            {
                MessageBox.Show($"Error message: {exp.Message}\n\n" /*+ $"Details:\n\n{exp.StackTrace}"*/, "Format error");
                return 1;
            }
            catch (Exception exp)
            {
                MessageBox.Show($"Security error.\n\nError message: {exp.Message}\n\n" +
                $"Details:\n\n{exp.StackTrace}");
                return 1;
            }
            return 0;
        }
        private GaussMatrix FillTheGaussMatFromTable(GaussMatrix GaussMat)
        {
            try
            {
                for (int i = 0; i < RowCount; i++)
                {
                    int g;
                    for (g = 0; g < ColumCount; g++)
                    {
                        GaussMat.Matrix[i][g] = new Fraction(restrictionTable[g, i].Value.ToString());
                    }
                    GaussMat.RightPart[i] = new Fraction(restrictionTable[g, i].Value.ToString());
                }
            }
            catch (DataReadException exp)
            {
                MessageBox.Show($"Data Read error.\n\nError message: Плохой формат ввода данных\n{exp.Message}\n\n");
                return null;
            }
            catch (FractionException exp)
            {
                MessageBox.Show($"Format Fraction error.\n\nError message: {exp.Message}\n\n");
                return null;
            }
            catch (FormatException exp)
            {
                MessageBox.Show($"Error message: {exp.Message}\n\n" /*+ $"Details:\n\n{exp.StackTrace}"*/, "Format error");
                return null;
            }
            catch (Exception exp)
            {
                MessageBox.Show($"Security error.\n\nError message: {exp.Message}\n\n" +
                $"Details:\n\n{exp.StackTrace}");
                return null;
            }

            return GaussMat;
        }
        private void BeginSolve_Click(object sender, EventArgs e)
        {
            // TODO Вызывает хороший метод из Simplex Solver и других штучек в соответсвии с выбранным решением задачи.

            //TODO сделать проверку, что если число ограничений больше переменных, то бросать дело и требовать испраления
            //Однако у нас же в графическом методе бывает куча ограничений, как быть с ними?
            /*
             * Наверное нужно полагать что если перемнных 2 или 3 то ограничений может быть побольше
             * Но если иначе, то ...
             * Нет не может быть побольше. Для графического только на 2-3 ограничения меньше чем переменных. Соответственно будет
             * 2Д и 3Д объект отрисовываться. 
             * 
             * Вспомная ту домашку с 2 переменными и миллардом ограничений мы можем составить "изначальную" задачу. 
             * Та, из которой мы пришли к этой. В ней будет на 2 переменных больше чем ограничений в этой и соответсвенно она будет выглядеть странно,
             * т.к. она будет уже приведена гауссом ( в каждой строке 1 и остальное 0, кроме 1ой и 2ой переменных) 
             * 
             * Так что бьём по попе пользователя если тот удумает запускать графический метод с неправильным количеством переменных
             */
            // TODO при нажатии нового решения старое бы хорошо полностью отчистить в том окне, в котором собираемся решать задачу.
            StartRowForSSolutionGrid = 0;
            ButtonSimplexStep.Enabled = true;//TODO Допускать кнопки к рабочему состоянию только по if else блоку
            ABStepButton.Enabled = true;

            SSolutionTable.Rows.Clear();
            SSolutionTable.Refresh();

            //TODO Не помешали бы провеки на эти самые переменные прямо тут ;)
            RowCount = (uint)numericUpDownRow.Value;
            ColumCount = (uint)numericUpDownColumn.Value;

            GaussMatrix GaussMat = new GaussMatrix(RowCount, ColumCount);
            SSolver = new SimplexSolver(RowCount, ColumCount - RowCount);

            if (RBSimplexMethod.Checked) // Симплекс
            {
                tabControl.SelectTab(tabPage2);
                if ((GaussMat = FillTheGaussMatFromTable(GaussMat)) == null) return;
                if (SetIndexListBasisElementsFromTable(GaussMat) > 0) return;
            }
            else if (RBArtificial.Checked) // Искуственный
            {
                tabControl.SelectTab(tabPage3);
                GaussMat = new GaussMatrix(RowCount, ColumCount + RowCount);
                SSolver = new SimplexSolver(RowCount, ColumCount);

                if ((GaussMat = FillTheGaussMatFromTable(GaussMat)) == null) return;
                //Нужно заполнить ещё список элементов справа в виде единичной матрицы
                for (int i = 0; i < RowCount; i++)
                {
                    GaussMat.Matrix[i][ColumCount + i] = new Fraction(1);
                    for (int g = 0; g < RowCount; g++)
                    {
                        if (g == i) continue;
                        GaussMat.Matrix[i][ColumCount + g] = new Fraction(0);
                    }
                }
                //Теперь бы заполнить базис этими новыми переменными
                for (int i = 0; i < RowCount; i++)
                {
                    GaussMat.IndexListBasisElements.Add(i + (int)ColumCount);
                }

            }
            else if (RBGraphic.Checked) // Графика
            {
                tabControl.SelectTab(tabPage4);
                if ((GaussMat = FillTheGaussMatFromTable(GaussMat)) == null) return;
                if (SetIndexListBasisElementsFromTable(GaussMat) > 0) return;
            }

            GaussMat.IndexListBasisElements.Sort();
            if (GaussMat.SolveMatrix() == 1)
            {
                MessageBox.Show("Получил ошибку при решении метода гаусса!");
                tabControl.SelectTab(tabPage1);
                return;
            }

            /*-----------------------       SimplexSolver       -----------------------*/
            List<Fraction> objectiveFunctionArr = new List<Fraction>();
            try
            {
                if (RBArtificial.Checked) // Искуственный
                {
                    for (int i = 0; i < ColumCount; i++)
                    {
                        objectiveFunctionArr.Add(new Fraction(0));
                    }
                    for (int g = 0; g < RowCount; g++)
                    {
                        objectiveFunctionArr.Add(new Fraction(1));
                    }
                    SSolver.FillTable(GaussMat, objectiveFunctionArr, false);//TODO проследить за тем, что минимум и максимум парвильно работают
                }
                else
                {
                    foreach (DataGridViewCell cell in objectiveFunctionTable.Rows[0].Cells)
                    {
                        objectiveFunctionArr.Add(new Fraction(cell.Value.ToString()));
                    }

                    SSolver.FillTable(GaussMat, objectiveFunctionArr, optimizationProblem.SelectedIndex == 1);
                }

            }
            catch (FractionException exp)
            {
                MessageBox.Show($"Format Fraction error.\n\nError message: {exp.Message}\n\n");
                return;
            }
            catch (Exception exp)
            {
                MessageBox.Show($"Security error.\n\nError message: {exp.Message}\n\n" +
                $"Details:\n\n{exp.StackTrace}");
                return;
            }
            if (RBSimplexMethod.Checked) // Симплекс
            {
                PrintResultToSoulutionGridView(SSolutionTable, SSolver);
                bool OnRightHaveNegativeEl = SSolver.RightPart.Any(x => x < 0); // Для графического не требуется.
                if (OnRightHaveNegativeEl)
                {
                    ButtonSimplexStep.Enabled = false;
                    SSTextAnswer.Text = "Плохой начальный базис.\r\nПожалуйста, больше так не делайте.";
                    return;
                }

                FindAndCheckBearingElements();
                pivotIndex = SSolver.FindOptimalBearingElement();
                ColorTheBearingEletemts(SSolutionTable);
            }
            else if (RBArtificial.Checked) // Искуственный
            {
                SSolver.GetRidOfNegativeLines();//Избавляемся от отрицательных элементов справа
                PrintResultToSoulutionGridView(ABSolverTable, SSolver);
                FindAndCheckBearingElements();
                pivotIndex = SSolver.FindOptimalBearingElement();
                ColorTheBearingEletemts(ABSolverTable);
            }


        }

        private void ColorTheBearingEletemts(DataGridView WorkTable)
        {
            if (SSolver.bearingEls.Count == 0) return;
            Update();
            int iRow, iColum;
            int StartRowOfCurTable = StartRowForSSolutionGrid - 2 - (int)SSolver.RowCount;
            for (int i = 0; i < SSolver.bearingEls.Count; i++)
            {
                iRow = SSolver.bearingEls[i][0];
                iColum = SSolver.bearingEls[i][1];
                WorkTable[iColum + 1, StartRowOfCurTable + iRow].Style = AquamarineStyle;
            }
            iRow = SSolver.bearingEls[pivotIndex][0];
            iColum = SSolver.bearingEls[pivotIndex][1];
            WorkTable[iColum + 1, StartRowOfCurTable + iRow].Style = LightCoralStyle;
        }
        private string FindAndCheckBearingElements()
        {
            int returnResult;
            returnResult = SSolver.FindBearingElements();
            //TODO добавить на проверки что всё корренктно завершилось и делать выводы из того сколько элементов нашлось
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
            if (returnResult > 0)
            {
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

                    /*SSTextAnswer.Text =*/
                    return answer.ToString();
                }
                else if (returnResult == 2)
                {
                    // Всё плолохо - система несовместна либо зависимые строки
                    //TODO проверить правда ли всегда несовместная система в этому случае? Возможен ли вариант когда система зависима (проблема с рангом)
                    StringBuilder answer = new StringBuilder("Система несовместна\r\nНет решений.");
                    /*SSTextAnswer.Text =*/
                    return answer.ToString();
                }
            }
            return "";
        }
        private void ButtonSimplexStep_Click(object sender, EventArgs e)
        {
            SSolver.SimplexStepWithCurrentEl(pivotIndex); //TODO может тоже потребуется ловить возващаемое значение для проверки
            PrintResultToSoulutionGridView(SSolutionTable, SSolver);
            string str;
            if ((str = FindAndCheckBearingElements()).Length > 0)
            {
                SSTextAnswer.Text = str;
            }
            if ((pivotIndex = SSolver.FindOptimalBearingElement()) < 0)
            {
                //Нету элементов и надо выходить
                return;
            }
            ColorTheBearingEletemts(SSolutionTable);

        }
        private void ABStepButton_Click(object sender, EventArgs e)
        {
            SSolver.SimplexStepWithCurrentEl(pivotIndex); //TODO может тоже потребуется ловить возващаемое значение для проверки
            SSolver.DeleteExtraVariblsFromAB((int)ColumCount);
            PrintResultToSoulutionGridView(ABSolverTable, SSolver);
            string str;
            if ((str = FindAndCheckBearingElements()).Length > 0)
            {
                ABAnswerText.Text = str;
            }
            if ((pivotIndex = SSolver.FindOptimalBearingElement()) < 0)
            {
                //Нету элементов и надо выходить
                return;
            }
            ColorTheBearingEletemts(ABSolverTable);
        }
        private int BearingElsIndexOf(int[] arr)
        {
            int index = -1;
            for (int i = 0; i < SSolver.bearingEls.Count; i++)
            {
                if (SSolver.bearingEls[i][0] == arr[0] && SSolver.bearingEls[i][1] == arr[1])
                {
                    index = i;
                    break;
                }
            }
            return index;
        }
        private void SolutionGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (SSolver == null)
            {
                return;
            }
            int StartRowOfCurTable = StartRowForSSolutionGrid - 2 - (int)SSolver.RowCount;
            int[] tmpArr = new int[2] { e.RowIndex - StartRowOfCurTable, e.ColumnIndex - 1 };
            //bool res = SSolver.bearingEls.Exists(x => (x[0] == tmpArr[0] && x[1] == tmpArr[1]));
            //bool res = SSolver.bearingEls.Contains(tmpArr);

            int coolIndex;
            coolIndex = BearingElsIndexOf(tmpArr);
            //coolIndex = SSolver.bearingEls.IndexOf(tmpArr); // TODO Проверить ещё раз почему indexOf не работал
            if (coolIndex >= 0 && coolIndex != pivotIndex)
            {
                SSolutionTable[SSolver.bearingEls[pivotIndex][1] + 1, SSolver.bearingEls[pivotIndex][0] + StartRowOfCurTable].Style = AquamarineStyle;
                pivotIndex = coolIndex;
                SSolutionTable[SSolver.bearingEls[pivotIndex][1] + 1, SSolver.bearingEls[pivotIndex][0] + StartRowOfCurTable].Style = LightCoralStyle;
            }
            //Иначе Пользователь клацнул не туда, так что ничего не делаем
        }

        private void PrintResultToSoulutionGridView(DataGridView SSolutionTable, SimplexSolver SSolver)
        {
            //TODO Сделать проверку что если новая таблица вылетает за границы то перетаскивать скролл ниже 
            //SolutionGridView.FirstDisplayedScrollingRowIndex = StartRowForSolutionGrid;
            int countOfAddingRows = ((int)SSolver.RowCount + 2) + StartRowForSSolutionGrid - SSolutionTable.Rows.Count;
            if (countOfAddingRows > 0)
            {
                SSolutionTable.Rows.Add(countOfAddingRows);
            }
            //Update();

            int i, g;
            SSolutionTable[0, StartRowForSSolutionGrid].Value = "X (" + SSolver.iteration.ToString() + ")";

            for (i = 0; i < SSolver.ColumCount; i++)
            {
                SSolutionTable[i + 1, StartRowForSSolutionGrid].Value = "X" + (SSolver.ILFreeEl[i] + 1).ToString();
            }

            StartRowForSSolutionGrid++;

            for (i = 0; i < SSolver.RowCount; i++)
            {
                SSolutionTable[0, i + StartRowForSSolutionGrid].Value = "X" + (SSolver.ILBasisEl[i] + 1).ToString();
                for (g = 0; g < SSolver.ColumCount; g++)
                {
                    SSolutionTable[g + 1, i + StartRowForSSolutionGrid].Value = SSolver.Matrix[i][g].ToString();
                }
                SSolutionTable[g + 1, i + StartRowForSSolutionGrid].Value = SSolver.RightPart[i].ToString();
            }


            for (g = 0; g < SSolver.ColumCount; g++)
            {
                SSolutionTable[g + 1, i + StartRowForSSolutionGrid].Value = SSolver.ObjFuncion[g].ToString();
            }
            SSolutionTable[g + 1, i + StartRowForSSolutionGrid].Value = SSolver.OFV.ToString();

            StartRowForSSolutionGrid += i + 2;
            //SolutionGridView[0, StartRowForSolutionGrid].Value = "u here!";
        }



        /* -----------------------------------    ПОБОЧНАЯ МАЛЕНЬКАЯ РАБОТА     --------------------------------------**/


        private void basicVariablesTable_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            BeginSolve.Enabled = true;
        }
        private void objectiveFunctionTable_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            BeginSolve.Enabled = true;
        }
        private void restrictionTable_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            BeginSolve.Enabled = true;
        }


        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (!RBSimplexMethod.Checked) return;

            textBox6.Visible = true;
            basicVariablesTable.Visible = true;
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (!RBArtificial.Checked) return;
            textBox6.Visible = false;
            basicVariablesTable.Visible = false;
        }
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (!RBGraphic.Checked) return;
            textBox6.Visible = true;
            basicVariablesTable.Visible = true;
        }
        private void MainMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void fileToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }



        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }





        private void fractionType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }



        private void optimizationProblem_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


    }


    // TODO:  Не допускать решение когда не прописаны базисные переменные для графического метода.
    class DataReadException : Exception
    {
        public DataReadException(string message)
            : base(message)
        { }
    }
}
