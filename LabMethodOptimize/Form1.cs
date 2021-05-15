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
using System.Threading;

namespace LabMethodOptimize
{

    public partial class Form1 : Form
    {
        /*-------------------     GLOBAL VAR      -----------------*/
        //GaussMatrix GaussMat;
        uint RowCount, ColumCount;
        int StartRowForAnswerTable = 0;
        SimplexSolver SSolver;
        int pivotIndex;

        DataGridViewCellStyle LightCoralStyle = new DataGridViewCellStyle();
        DataGridViewCellStyle AquamarineStyle = new DataGridViewCellStyle();

        List<SimplexSolver> MemoryOfSteps = new List<SimplexSolver>();
        List<Fraction[]> lPoints = new List<Fraction[]>();
        Fraction lA, lB, lC, lD; // предельные точки графика, дальше них рисовать нету смысла. (но мы всё равно будем :) )
        Fraction gA, gB, gC, gD; // истинные пределы рисунка
        Fraction sA, sB, sC, sD; // Переменные для масштабирования рисунка
        int indexOfOptimalPoint = -1;
        Fraction valueOfSolution = null;

        Fraction offset;
        Fraction paramA, paramBForLeft, paramBForRight;
        Fraction distance;
        Fraction step;
        bool fTaskIsLimited = true;
        bool fCanDraw = false;
        bool fItsAScaleFunc = false;


        /*-------------------------------      INICIALIZE    ------------------------------*/


        public Form1()
        {
            this.Text = "Simplex Solver";

            InitializeComponent();
            this.GPanel.MouseWheel += GPanel_MouseWheel;

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
            GSSolutionTable.RowHeadersVisible = false;

            for (int g = 0; g < 18; g++)
            {
                SSolutionTable.Columns.Add(g.ToString(), "");
                SSolutionTable.Columns[g].Width = 60;

                GSSolutionTable.Columns.Add(g.ToString(), "");
                GSSolutionTable.Columns[g].Width = 60;
            }
            for (int g = 0; g < 34; g++)
            {
                ABSolverTable.Columns.Add(g.ToString(), "");
                ABSolverTable.Columns[g].Width = 60;
            }


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
            if (numericUpDownRow.Value > numericUpDownColumn.Value)
            {
                numericUpDownRow.Value = numericUpDownColumn.Value;
            }
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


        /*-------------------------------      MENU     ------------------------------*/


        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {

                tabControl.SelectTab(tabPage1);

                if (basicVariablesTable.Rows.Count != 0)
                {
                    foreach (DataGridViewCell cell in basicVariablesTable.Rows[0].Cells)
                    {
                        cell.Value = null;
                    }
                }
                SSAnswerText.Text = "";

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
                        if (str.Length != ColumCount) //REM ВОзможно тут надо будет добавить +1 т.к. не ясно может ли у целевой функции быть константа
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
                numericUpDownColumn.Focus();//Это для того чтобы снять выделение с элемента таблицы чтобы он записался и сохранился там
                //после уже само сохранение всей таблицы
                StreamWriter st;
                if ((st = new StreamWriter(Path.GetFullPath(saveFileDialog.FileName))) != null)
                {
                    RowCount = (uint)numericUpDownRow.Value;
                    ColumCount = (uint)numericUpDownColumn.Value;
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
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ADD окно которое будет выводить информацию о программе
            //возможно с картинками ;)

            new About().Show();
        }


        /*-------------------------------      WORK WITH ALL 3 METHODS     ------------------------------*/


        private bool CheckTheFillingBasisRow()
        {
            if (basicVariablesTable.Rows.Count < 1)
                return false;

            for (int g = 0; g < basicVariablesTable.Columns.Count; g++)
            {
                if (basicVariablesTable[g, 0].Value is null)
                {
                    return false;
                }
            }

            return true;
        }
        private void BeginSolve_Click(object sender, EventArgs e)
        {
            //Однако у нас же в графическом методе бывает куча ограничений, как быть с ними?
            /*
             * Наверное нужно полагать что если перемнных 2 или 3 то ограничений может быть побольше
             * Но если иначе, то ...
             * Нет не может быть побольше. Для графического только на 2-3 ограничения меньше чем переменных. Соответственно будет
             * 2Д и 3Д объект отрисовываться. 
             * 
             * Вспомная ту домашку с 2 переменными и миллардом ограничений мы можем составить "изначальную" задачу. 
             * Та, из которой мы пришли к этой. В ней будет на 2 переменных больше чем ограничений в этой и соответсвенно она будет выглядеть готовой,
             * т.к. она будет уже приведена гауссом ( в каждой строке 1 и остальное 0, кроме 1ой и 2ой переменных) 
             * 
             * Так что бьём по попе пользователя если тот удумает запускать графический метод с неправильным количеством переменных
             */


            StartRowForAnswerTable = 0;
            MemoryOfSteps.Clear();
            ActivateButtnosOnTab(-1);

            fCanDraw = false;

            RowCount = (uint)numericUpDownRow.Value;
            ColumCount = (uint)numericUpDownColumn.Value;
            if (RowCount == 0 || ColumCount == 0)
            {
                return;
            }

            if (RowCount > ColumCount)
            {
                //ругаемся и выходим сразу же
                //для графического будет повторная проверка чтоб было ограничений на 2 меньше
                MessageBox.Show("Число ограничений больше числа переменных.\r\n" +
                    "Пожалуйста, исправьте и попытайтесь ещё раз");
                ActivateButtnosOnTab(0);
                return;
            }
            GaussMatrix GaussMat = new GaussMatrix(RowCount, ColumCount);
            SSolver = new SimplexSolver(RowCount, ColumCount - RowCount);

            indexOfOptimalPoint = -1;
            valueOfSolution = null;


            if (RBSimplexMethod.Checked) // Симплекс
            {
                SSolutionTable.Rows.Clear();
                SSAnswerText.Text = "";
                if (!CheckTheFillingBasisRow())
                {
                    MessageBox.Show("Введите корректный базис");
                    tabControl.SelectTab(tabPage1);
                    return;
                }
                //Разрешение и блокиовка кнопок
                ActivateButtnosOnTab(1);

                tabControl.SelectTab(tabPage2);

                if ((GaussMat = FillTheGaussMatFromTable(GaussMat)) == null) return;
                if (SetIndexListBasisElementsFromTable(GaussMat) > 0) return;
            }
            else if (RBArtificial.Checked) // Искуственный
            {

                ABSolverTable.Rows.Clear();
                ABAnswerText.Text = "";

                ActivateButtnosOnTab(2);

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

                GaussMat.GetRidOfNegativeLines();
            }
            else if (RBGraphic.Checked) // Графика
            {
                GSSolutionTable.Rows.Clear();
                GAnswerText.Text = "";
                lPoints.Clear();

                if (ColumCount - RowCount != 2 && ColumCount - RowCount != 1)
                {
                    MessageBox.Show("Плохое количество ограничений\n\nДолжно быть на 2 меньше чем количество переменных");
                    tabControl.SelectTab(tabPage1);
                    return;
                }
                if (!CheckTheFillingBasisRow())
                {
                    MessageBox.Show("Введите корректный базис");
                    tabControl.SelectTab(tabPage1);
                    return;
                }
                ActivateButtnosOnTab(3);
                tabControl.SelectTab(tabPage4);
                if (ColumCount - RowCount == 1)
                {
                    GaussMat = new GaussMatrix(RowCount, ColumCount + 1);
                    SSolver = new SimplexSolver(RowCount, ColumCount - RowCount + 1);
                }
                if ((GaussMat = FillTheGaussMatFromTable(GaussMat)) == null) return;
                if (SetIndexListBasisElementsFromTable(GaussMat) > 0) return;
                if (ColumCount - RowCount == 1)
                {
                    for (int i = 0; i < GaussMat.RowCount; i++)
                        GaussMat.Matrix[i][GaussMat.ColumCount - 1] = 0;
                }
            }

            GaussMat.IndexListBasisElements.Sort();

            if (GaussMat.SolveMatrix() == 1)
            {
                MessageBox.Show("Получил ошибку при решении метода Гаусса.\n\n" +
                    "Система несовместна.\n\n" +
                    "Измените значения и попытайтесь ещё раз.");

                ActivateButtnosOnTab(0);

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
                    SSolver.FillTable(GaussMat, objectiveFunctionArr, false);
                    SSolver.isArtificialTask = true;
                }
                else // Симплекс || Графика
                {
                    foreach (DataGridViewCell cell in objectiveFunctionTable.Rows[0].Cells)
                    {
                        objectiveFunctionArr.Add(new Fraction(cell.Value.ToString()));
                    }
                    if (ColumCount - RowCount == 1 && RBGraphic.Checked)
                    {
                        objectiveFunctionArr.Add(new Fraction(0));
                    }
                    SSolver.FillTable(GaussMat, objectiveFunctionArr, optimizationProblem.SelectedIndex == 1);
                }

            }
            catch (NullReferenceException exp)
            {
                MessageBox.Show($"Какие ужасные аргументы задачи боже мой: {exp.Message}\n\n");
                return;
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
                    ActivateButtnosOnTab(0);
                    SSAnswerText.Text = "Плохой начальный базис.\r\nПожалуйста, больше так не делайте.";
                    return;
                }

                string str;
                if ((str = FindAndCheckBearingElements()).Length > 0)
                {
                    SSAnswerText.Text = str;
                    ActivateButtnosOnTab(0);
                }

                pivotIndex = SSolver.FindOptimalBearingElement();
                SSolver.pivotIndex = pivotIndex;
                MemoryOfSteps.Add(new SimplexSolver(SSolver));
                if (pivotIndex < 0)
                {
                    //Нету элементов и надо выходить                
                    return;
                }
                ColorTheBearingEletemts(SSolutionTable);

            }
            else if (RBArtificial.Checked) // Искуственный
            {
                //SSolver.GetRidOfNegativeLines();//Избавляемся от отрицательных элементов справа
                PrintResultToSoulutionGridView(ABSolverTable, SSolver);
                string str;
                if ((str = FindAndCheckBearingElements()).Length > 0)
                {
                    ABAnswerText.Text = "Система несовместна.\r\nНет решения.";
                    ActivateButtnosOnTab(0);
                }
                pivotIndex = SSolver.FindOptimalBearingElement();

                SSolver.pivotIndex = pivotIndex;
                MemoryOfSteps.Add(new SimplexSolver(SSolver));
                if (pivotIndex < 0)
                {
                    //Нету элементов и надо выходить                
                    return;
                }
                ColorTheBearingEletemts(ABSolverTable);
            }
            else if (RBGraphic.Checked)
            {
                //PrintResultToSoulutionGridView(GSSolutionTable, SSolver); //Вывод ограничений сделан несколько красивее чем обыная симплекс таблица
                Make2DModel();
            }
        }
        private void ActivateButtnosOnTab(int whitchTab)
        {
            switch (whitchTab)
            {
                case 1: // Симплекс
                    SStepButton.Enabled = true;
                    AllSStepButton.Enabled = true;

                    ABStepButton.Enabled = false;
                    AllABStepButton.Enabled = false;
                    break;
                case 2: // Искуственный
                    SStepButton.Enabled = false;
                    AllSStepButton.Enabled = false;

                    ABStepButton.Enabled = true;
                    AllABStepButton.Enabled = true;
                    break;
                case 3: // Графика
                    SStepButton.Enabled = false;
                    AllSStepButton.Enabled = false;

                    ABStepButton.Enabled = false;
                    AllABStepButton.Enabled = false;
                    break;
                case -1: // Заблокировать Воообще все кнопки на форме
                    SStepBackButton.Enabled = false;
                    ABStepBackButton.Enabled = false;
                    goto case 0;
                case 0: // Заблокировать все

                    SStepButton.Enabled = false;
                    AllSStepButton.Enabled = false;

                    ABStepButton.Enabled = false;
                    AllABStepButton.Enabled = false;
                    break;
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
                        throw new DataReadException("Элемент начального базиса не задан или выходит за границы количества переменных задачи");
                    }
                }
            }
            catch (DataReadException exp)
            {
                MessageBox.Show($"Data Read error.\n\nПлохой формат ввода данных\n{exp.Message}\n\n");
                tabControl.SelectTab(tabPage1);
                return 1;
            }
            catch (FractionException exp)
            {
                MessageBox.Show($"Format Fraction error.\n\nError message: {exp.Message}\n\n");
                tabControl.SelectTab(tabPage1);
                return 1;
            }
            catch (FormatException exp)
            {
                MessageBox.Show($"Format error.\n\nError message: {exp.Message}");
                tabControl.SelectTab(tabPage1);
                return 1;
            }
            catch (Exception exp)
            {
                MessageBox.Show($"Security error.\n\nError message: {exp.Message}\n\n" +
                $"Details:\n\n{exp.StackTrace}");
                tabControl.SelectTab(tabPage1);
                return 1;
            }
            return 0;
        }
        private GaussMatrix FillTheGaussMatFromTable(GaussMatrix GaussMat)
        {
            try
            {
                for (int i = 0; i < restrictionTable.RowCount; i++)
                {
                    int g;
                    double tmpDouble;
                    for (g = 0; g < restrictionTable.ColumnCount - 1; g++)
                    {
                        if (Double.TryParse(restrictionTable[g, i].Value.ToString(), out tmpDouble))
                            GaussMat.Matrix[i][g] = new Fraction(tmpDouble.ToString("0.00000")); //Ограничиваем double чтобы не было Overflow у Fraction
                        else
                            GaussMat.Matrix[i][g] = new Fraction(restrictionTable[g, i].Value.ToString());
                    }
                    if (Double.TryParse(restrictionTable[g, i].Value.ToString(), out tmpDouble))
                        GaussMat.RightPart[i] = new Fraction(tmpDouble.ToString("0.00000")); //Ограничиваем double чтобы не было Overflow у Fraction
                    else
                        GaussMat.RightPart[i] = new Fraction(restrictionTable[g, i].Value.ToString());

                }
            }
            catch (DataReadException exp)
            {
                MessageBox.Show($"Data Read error.\n\nПлохой формат ввода данных\n{exp.Message}\n\n");
                return null;
            }
            catch (FractionException exp)
            {
                MessageBox.Show($"Format Fraction error.\n\nError message: {exp.Message}\n\n");
                return null;
            }
            catch (FormatException exp)
            {
                MessageBox.Show($"Format error.\n\nError message: {exp.Message}");
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
        private void ColorTheBearingEletemts(DataGridView WorkTable)
        {
            if (SSolver.bearingEls.Count == 0) return;
            Update();
            int iRow, iColum;
            int StartRowOfCurTable = StartRowForAnswerTable - 2 - (int)SSolver.RowCount;
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
            /* 
            * Наверное стоит блокировать кнопку, чтобы пользователь даже  не пытался посчитать таблицу, т.к. нет элементов
            * Но ведь это состояние уже и ответ и надо подумать что выводить. 
            * 
            * Варианта 2:  Либо задача решена ||  Либо есть отрицательные столбцы, но нету элементов для прехода. 
            * Другими словами - система несовместна.
            * 
            * В любом из вариантов мы дальше не шагаем и выходим, но нужно понимать что возвращать пользователю.
            * 
            * Думаю, стоит пробежаться по низу и посмотреть, есть ли элементы 
            */
            if (returnResult > 0)
            {

                if (returnResult == 1)
                {
                    // Всё хорошо - выводим ответ

                    StringBuilder answer;
                    answer = new StringBuilder("x* (");

                    for (int i = 0; i < SSolver.RowCount + SSolver.ColumCount; i++)
                    {
                        if (SSolver.ILBasisEl.Contains(i))
                        {
                            answer.Append(fractionType.SelectedIndex == 0 ?
                                            SSolver.RightPart[SSolver.ILBasisEl.IndexOf(i)].ToString() :
                                            SSolver.RightPart[SSolver.ILBasisEl.IndexOf(i)].ToDouble().ToString("0.000"));
                        }
                        else
                        {
                            answer.Append(0.ToString());
                        }
                        answer.Append(",");
                    }
                    answer.Length--;//Удаляем последний символ.
                    answer.Append(")");
                    if (optimizationProblem.SelectedIndex == 1)
                        SSolver.OFV = -SSolver.OFV;

                    answer.Append($"\r\n\nf(x*) = " + (fractionType.SelectedIndex == 0 ?
                                                    $"{-SSolver.OFV}" :
                                                    $"{(-SSolver.OFV).ToDouble().ToString("0.000")}"));

                    /*SSTextAnswer.Text =*/
                    return answer.ToString();
                }
                else if (returnResult == 2)
                {
                    // Всё плолохо - система несовместна либо зависимые строки
                    //TODO проверить правда ли всегда несовместная система в этому случае? Возможен ли вариант когда система зависима (проблема с рангом)
                    StringBuilder answer = new StringBuilder("Система не ограничена\r\nf(x*) = ∞");
                    /*SSTextAnswer.Text =*/
                    return answer.ToString();
                }
            }
            return "";
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
        private void RefreshButton_Click(object sender, EventArgs e)
        {
            RBGraphic.Checked = true;
            BeginSolve.PerformClick();

        }
        private void GPanel_MouseWheel(object sender, MouseEventArgs e)
        {
            if (!fCanDraw)
                return;

            int zoomFactor = 2;
            int maxZoom = 4;
            if (e.Delta > 0) //Колесо покрутилось наверх
            {
                if (Fraction.Abs(gA) == Fraction.Abs(sA))
                {
                    return;
                }
                //Приближаем рисунок - но не больше чем минимальное расстояние в виде
                if (Fraction.Abs(gA + Fraction.Abs(sA / zoomFactor)) < Fraction.Abs(sA))
                {
                    gA = sA;
                    gC = sC;
                    gB = sB;
                    gD = sD;
                }
                else
                {
                    gA += Fraction.Abs(sA / zoomFactor);
                    gC += Fraction.Abs(sC / zoomFactor);
                    gB -= Fraction.Abs(sB / zoomFactor);
                    gD -= Fraction.Abs(sD / zoomFactor);
                }
            }
            else
            {
                if (Fraction.Abs(gA) == Fraction.Abs(sA) * maxZoom)
                {
                    return;
                }
                //Иначе отдаляем
                if (Fraction.Abs(gA - Fraction.Abs(sA / zoomFactor)) > Fraction.Abs(sA) * maxZoom)
                {
                    gA = sA * maxZoom;
                    gC = sC * maxZoom;
                    gB = sB * maxZoom;
                    gD = sD * maxZoom;
                }
                else
                {
                    gA -= Fraction.Abs(sA / zoomFactor);
                    gC -= Fraction.Abs(sC / zoomFactor);
                    gB += Fraction.Abs(sB / zoomFactor);
                    gD += Fraction.Abs(sD / zoomFactor);
                }
            }

            fItsAScaleFunc = true; //Чтобы Paint не рисовал по второму разу 
            GPanel.Refresh();
            Draw2D();
            fItsAScaleFunc = false;
        }
        private void GPanel_Paint(object sender, PaintEventArgs e)
        {
            if (fItsAScaleFunc)
            {
                return;
            }
            //Чтоб окно не зависало при перетаскивании будем рисовать в отдельном потоке
            Thread myThread = new Thread(new ThreadStart(Draw2D));
            myThread.Start();

        }


        /*-------------------------------      SIMPLEX METHOD     ------------------------------*/


        private void SStepButton_Click(object sender, EventArgs e)
        {
            SSolver.SimplexStepWithCurrentEl(pivotIndex);
            PrintResultToSoulutionGridView(SSolutionTable, SSolver);

            string str;
            if ((str = FindAndCheckBearingElements()).Length > 0)
            {
                SSAnswerText.Text = str;
                ActivateButtnosOnTab(0);
            }


            pivotIndex = SSolver.FindOptimalBearingElement();
            SSolver.pivotIndex = pivotIndex;
            MemoryOfSteps.Add(new SimplexSolver(SSolver));
            SStepBackButton.Enabled = true;
            if (pivotIndex < 0)
            {
                //Нету элементов и надо выходить                
                return;
            }
            ColorTheBearingEletemts(SSolutionTable);
        }
        private void SStepBackButton_Click(object sender, EventArgs e)
        {
            //Обнуляем текст, т.к. мы уже ушли от ответа на шаг назад
            SSAnswerText.Text = "";
            /*
             * Теперь мы должны подстереть в таблице последнюю строку и разумеется удалить из списка последний элемент
             * После проверить что у нас есть ещё более 1 элемент, иначе блокируем кнопку
             * 
             * Как только мы вернулись на шаг назад, нам нужно бы SSolver присвоить новое значение (то бишь старую таблицу)
             * Причём!!! Скопировать, чтоб не портить список! Это важно понимать
             * 
             * Но так же нужно выбрать подходящий опорный элемент. 
             * Список элементов хранит сам SSolver
             * А вот индекс элемента по порядку мы уже утратили. Поэтому необходимо дописать то, чтобы pivotIndex хранил и сам SSolver
             * Эта операция отчасти лишняя, но лишь она хорошо встраивается в логику программы
             * 
             * После того как стёрли последнее состояние решения из таблицы, не забыть и сменить индекс Начала рисования.
             */
            ActivateButtnosOnTab(1);
            MemoryOfSteps.RemoveAt(MemoryOfSteps.Count - 1);//Удаляем последний
            if (MemoryOfSteps.Count <= 1)
            {
                SStepBackButton.Enabled = false;
            }
            SSolver = new SimplexSolver(MemoryOfSteps[MemoryOfSteps.Count - 1]);
            RemoveLastTableFromSolutionGridView(SSolutionTable);
            pivotIndex = SSolver.pivotIndex; //Возвращаем pivot в старое положение.

        }
        private void AllSStepButton_Click(object sender, EventArgs e)
        {
            while (SStepButton.Enabled)
            {
                SStepButton.PerformClick();
            }
            AllSStepButton.Enabled = false;
        }
        private void SSolverTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (SSolver == null)
            {
                return;
            }
            int StartRowOfCurTable = StartRowForAnswerTable - 2 - (int)SSolver.RowCount;
            int[] tmpArr = new int[2] { e.RowIndex - StartRowOfCurTable, e.ColumnIndex - 1 };
            //bool res = SSolver.bearingEls.Exists(x => (x[0] == tmpArr[0] && x[1] == tmpArr[1]));
            //bool res = SSolver.bearingEls.Contains(tmpArr);

            int coolIndex;
            coolIndex = BearingElsIndexOf(tmpArr); //Обычный indexOf не работает для массивов
            if (coolIndex >= 0 && coolIndex != pivotIndex)
            {
                SSolutionTable[SSolver.bearingEls[pivotIndex][1] + 1, SSolver.bearingEls[pivotIndex][0] + StartRowOfCurTable].Style = AquamarineStyle;
                MemoryOfSteps[MemoryOfSteps.Count - 1].pivotIndex = SSolver.pivotIndex = pivotIndex = coolIndex;
                SSolutionTable[SSolver.bearingEls[pivotIndex][1] + 1, SSolver.bearingEls[pivotIndex][0] + StartRowOfCurTable].Style = LightCoralStyle;
            }
            //Иначе Пользователь клацнул не туда, так что ничего не делаем
        }


        /*-------------------------------      ARTIFICTIAL METHOD     ------------------------------*/


        private int BackToDirectTask(SimplexSolver SSolver)
        {
            if (SSolver.isArtificialTask)
            {
                SSolver.isArtificialTask = false;
                SSolver.iteration = 0;
            }
            List<Fraction> objectiveFunctionArr = new List<Fraction>();
            try
            {
                foreach (DataGridViewCell cell in objectiveFunctionTable.Rows[0].Cells)
                {
                    objectiveFunctionArr.Add(new Fraction(cell.Value.ToString()));
                }

                SSolver.CalculateObjectiveFunction(objectiveFunctionArr, optimizationProblem.SelectedIndex == 1);
                SSolver.CalculateObjectiveFunctionValue(objectiveFunctionArr, optimizationProblem.SelectedIndex == 1);

            }
            catch (FractionException exp)
            {
                MessageBox.Show($"Format Fraction error.\n\nError message: {exp.Message}\n\n");
                ActivateButtnosOnTab(0);
                return 1;
            }
            catch (Exception exp)
            {
                MessageBox.Show($"Security error.\n\nError message: {exp.Message}\n\n" +
                $"Details:\n\n{exp.StackTrace}");
                ActivateButtnosOnTab(0);
                return 1;
            }


            PrintResultToSoulutionGridView(ABSolverTable, SSolver);
            string str;
            if ((str = FindAndCheckBearingElements()).Length > 0)
            {
                if (!SSolver.isArtificialTask)
                    ABAnswerText.Text = str;
            }
            pivotIndex = SSolver.FindOptimalBearingElement();
            SSolver.pivotIndex = pivotIndex;
            MemoryOfSteps.Add(new SimplexSolver(SSolver));
            ABStepBackButton.Enabled = true;
            if (pivotIndex < 0)
            {
                //Нету элементов и надо проверить что это вспомогательная задача, тогда продолжать решение.
                if (!SSolver.isArtificialTask)
                {
                    ActivateButtnosOnTab(0);
                    return 1;
                }
            }
            return 0;
        }
        private void ABStepButton_Click(object sender, EventArgs e)
        {
            SSolver.SimplexStepWithCurrentEl(pivotIndex);
            if (SSolver.isArtificialTask) SSolver.DeleteExtraVariblsFromAB((int)ColumCount - 1);
            PrintResultToSoulutionGridView(ABSolverTable, SSolver);
            string str;
            if ((str = FindAndCheckBearingElements()).Length > 0)
            {
                if (!SSolver.isArtificialTask)
                    ABAnswerText.Text = str;
            }
            pivotIndex = SSolver.FindOptimalBearingElement();
            SSolver.pivotIndex = pivotIndex;
            MemoryOfSteps.Add(new SimplexSolver(SSolver));
            ABStepBackButton.Enabled = true;
            if (pivotIndex < 0)
            {
                //Нету элементов и надо проверить что это вспомогательная задача тогда продолжать решение.
                if (!SSolver.isArtificialTask)
                {
                    //Тогда прямая задача решена
                    ActivateButtnosOnTab(0);
                    return;
                }

                /* 
                 * Иначе вспомогательная задача "решена" и надо удостовериться что все базисные переменные выведены
                 *                 
                 * Могут быть следующие развития событий
                 * >У нас система несовместна - не удаётся вывести переменную 
                 * >Зависимы - когда вся строка элементов = 0
                 * >Необходим холостой шаг т.е. целевая функция = 0 а не все переменные выведены 
                 * тогда находим ненулевой элемент в строке с вспомогательной  переменной и выводим её обычным шагом
                 * (на этом же поиске будет определяться Зависимость матрицы)
                 * 
                 * Придётся дополнительно искуственно заполнять список возможных опорных элементов
                 */

                for (int i = 0; i < RowCount; i++)
                {
                    if (SSolver.ILBasisEl[i] > ColumCount - 1)//Есть вспомогательный элемент в строке i
                    {
                        //тогда запиши все ненулевые убедись что справа 0
                        if (SSolver.RightPart[i] != 0)//Справа не ноль
                        {
                            //Рагуемся что у нас несовместны граничные условия задачи
                            ABAnswerText.Text = "Система ограничений несовместна\n\nНет решений!";
                            //MessageBox.Show("Система ограничений несовместна\n\nНет решений!");
                            ActivateButtnosOnTab(0);
                            return;
                        }
                        //иначе искать ненулевые элементы в каждой строке.
                        //Причём, если в строке не нашлось элементов - сигнал к тому что у нас система зависима и надо ругаться.

                        //Для того чтобы определять зависимость строк - будем использовать флаг
                        bool fHaveDependentLines = true;
                        for (int g = 0; g < SSolver.ColumCount; g++)
                        {
                            if (SSolver.Matrix[i][g] != 0)
                            {
                                fHaveDependentLines = false;
                                SSolver.bearingEls.Add(new int[2] { i, g });
                                continue;
                            }

                        }
                        if (fHaveDependentLines)//Проверяем флаг
                        {
                            //ругаемся
                            ABAnswerText.Text = "Система ограничений зависима";
                            //MessageBox.Show("Система ограничений зависима\n\nНеобходимо выкинуть зависимые строки и можете попробовать ещё раз");
                            ActivateButtnosOnTab(0);
                            return;
                        }
                        //иначе есть переменные по которым будем ща скакать
                        //но сначала нужно найти индекс и предоставить пользователю выбирать
                        pivotIndex = SSolver.FindOptimalBearingElement();
                        SSolver.pivotIndex = pivotIndex;
                        MemoryOfSteps.Add(new SimplexSolver(SSolver));
                        ABStepBackButton.Enabled = true;
                        if (pivotIndex < 0) //Конкретно тут проверка откровенно лишняя, т.к. опорные точно есть, но пусть будет
                        {
                            //Нету элементов и надо проверить что это вспомогательная задача тогда продолжать решение.
                            if (!SSolver.isArtificialTask)
                            {
                                ActivateButtnosOnTab(0);
                                return;
                            }
                        }
                        ColorTheBearingEletemts(ABSolverTable);
                        return; //Теперь пользователь может выбрать элемент и сделать холостой шаг
                    }
                }

                //Возрвращаемся к прямой задаче с полученным базисом
                if (BackToDirectTask(SSolver) > 0)
                {
                    //Получили ошибку
                    ActivateButtnosOnTab(0);
                    return;
                }
            }
            ColorTheBearingEletemts(ABSolverTable);
        }
        private void ABStepBackButton_Click(object sender, EventArgs e)
        {
            ABAnswerText.Text = "";
            ActivateButtnosOnTab(2);
            MemoryOfSteps.RemoveAt(MemoryOfSteps.Count - 1);//Удаляем последний
            if (MemoryOfSteps.Count <= 1)
            {
                ABStepBackButton.Enabled = false;
            }
            SSolver = new SimplexSolver(MemoryOfSteps[MemoryOfSteps.Count - 1]);
            RemoveLastTableFromSolutionGridView(ABSolverTable);
            pivotIndex = SSolver.pivotIndex; //Возвращаем pivot в старое положение.

            //Проверим, что этот старый шаг, не был окончанием вспомгательной задачи
            if (SSolver.isArtificialTask)
            {
                if (SSolver.OFV == 0)
                {
                    //Тогда сделать ещё шаг назад в силу реализации кода. 
                    ABStepBackButton.PerformClick();
                }
            }
        }
        private void AllABStepButton_Click(object sender, EventArgs e)
        {
            while (ABStepButton.Enabled)
            {
                ABStepButton.PerformClick();
            }
            AllABStepButton.Enabled = false;
        }
        private void ABSolverTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (SSolver == null)
            {
                return;
            }
            int StartRowOfCurTable = StartRowForAnswerTable - 2 - (int)SSolver.RowCount;
            int[] tmpArr = new int[2] { e.RowIndex - StartRowOfCurTable, e.ColumnIndex - 1 };

            int coolIndex;
            coolIndex = BearingElsIndexOf(tmpArr); //Обычный indexOf не работает для массивов      
            if (coolIndex >= 0 && coolIndex != pivotIndex)
            {
                ABSolverTable[SSolver.bearingEls[pivotIndex][1] + 1, SSolver.bearingEls[pivotIndex][0] + StartRowOfCurTable].Style = AquamarineStyle;
                MemoryOfSteps[MemoryOfSteps.Count - 1].pivotIndex = SSolver.pivotIndex = pivotIndex = coolIndex;
                ABSolverTable[SSolver.bearingEls[pivotIndex][1] + 1, SSolver.bearingEls[pivotIndex][0] + StartRowOfCurTable].Style = LightCoralStyle;
            }
            //Иначе Пользователь клацнул не туда, так что ничего не делаем
        }


        /*-------------------------------      GRAPHIC METHOD     ------------------------------*/


        private bool containPointFraction(List<Fraction[]> lPoints, Fraction[] Fpoint)
        {
            foreach (Fraction[] point in lPoints)
            {
                if (point[0] == Fpoint[0] && point[1] == Fpoint[1])
                {
                    return true;
                }
            }
            return false;
        }
        private void WriteAllRestrictionsAndObjFunc()
        {
            StringBuilder str = new StringBuilder();

            for (int g = 0; g < SSolver.ColumCount; g++)
            {
                if (ColumCount - RowCount == 1 && RBGraphic.Checked &&
                   g + 1 == SSolver.ColumCount)//Не выводим последний элемент списка если это 1D графическая задача
                {
                    continue;
                }
                if (SSolver.ObjFunction[g] < 0 && str.Length > 0)
                    str.Length--;//стираем + в конце строки

                str.Append(fractionType.SelectedIndex == 0 ?
                                 $"{SSolver.ObjFunction[g]}" :
                                 (SSolver.ObjFunction[g]).ToDouble().ToString("0.000"));
                str.Append($" *X{SSolver.ILFreeEl[g] + 1} +");
            }

            if (SSolver.OFV > 0 && str.Length > 0)
                str.Length--;//стираем + в конце строки

            str.Append(" " + (fractionType.SelectedIndex == 0 ?
                                $"{-SSolver.OFV}" :
                                (-SSolver.OFV).ToDouble().ToString("0.000")));
            str.Append(" -> min");
            objFunctionText.Text = str.ToString();

            str.Clear();

            for (int i = 0; i < SSolver.RowCount; i++)
            {
                for (int g = 0; g < SSolver.ColumCount; g++)
                {
                    if (ColumCount - RowCount == 1 && RBGraphic.Checked &&
                        g + 1 == SSolver.ColumCount)//Не выводим последний элемент списка если это 1D графическая задача
                    {
                        continue;
                    }
                    if (SSolver.Matrix[i][g] > 0 && str.Length > 0)
                        str.Length--;//стираем + в конце строки

                    str.Append(fractionType.SelectedIndex == 0 ?
                            $"{-SSolver.Matrix[i][g]}" :
                            (-SSolver.Matrix[i][g]).ToDouble().ToString("0.000"));
                    str.Append($" *X{SSolver.ILFreeEl[g] + 1} +");

                }
                str.Length--;//стираем + в конце строки
                str.Append($" >= " +
                    (fractionType.SelectedIndex == 0 ?
                    $"{-SSolver.RightPart[i]}" :
                    (-SSolver.RightPart[i]).ToDouble().ToString("0.000")) +
                    "\r\n\n");
                ;
            }
            restrictionsText.Text = str.ToString();
        }
        private bool CheckPoint(Fraction[] point)
        {
            //нужно подставить в симплекс таблицу эти самые точки и убедиться что неравеноство верно
            Fraction res = new Fraction(0);
            for (int i = 0; i < SSolver.RowCount; i++)
            {
                res = new Fraction(SSolver.RightPart[i]);
                for (int g = 0; g < SSolver.ColumCount; g++)
                {
                    res -= SSolver.Matrix[i][g] * point[g];
                }

                if (res < 0 || point[0] < 0 || point[1] < 0) //точка не подходит под ограничение
                {
                    return false;
                }
            }

            return true;
        }
        private void Check2DPointForSolution(Fraction[] point)
        {
            Fraction res = new Fraction(0);
            for (int i = 0; i < SSolver.ColumCount; i++)
            {
                res += SSolver.ObjFunction[i] * point[i];
            }
            res -= SSolver.OFV;
            if (valueOfSolution is null)
            {
                valueOfSolution = res;
                indexOfOptimalPoint = lPoints.Count - 1;
            }
            else
            {
                if (valueOfSolution > res)
                {
                    valueOfSolution = res;
                    indexOfOptimalPoint = lPoints.Count - 1;
                }
            }
        }
        private void Find2DPoints()
        {
            GaussMatrix gm = new GaussMatrix(SSolver.ColumCount);
            Fraction[] tmpPoint;
            for (int i = 0; i < SSolver.RowCount; i++)
            {
                for (int g = i + 1; g < SSolver.RowCount; g++)
                {
                    //Нужно решить задачу гауссом,
                    //и если тот вернёт ошибку, то значит прямые параллельны

                    //Заполняем матрицу гаусса
                    for (int j = 0; j < SSolver.ColumCount; j++)
                    {
                        gm.Matrix[0][j] = SSolver.Matrix[i][j];
                        gm.Matrix[1][j] = SSolver.Matrix[g][j];
                    }
                    gm.RightPart[0] = SSolver.RightPart[i];
                    gm.RightPart[1] = SSolver.RightPart[g];

                    if (gm.SolveMatrix() > 0)
                    {
                        //Прямые парараллельны, просто пропускаем этот шаг
                        continue;
                    }
                    //иначе записываем точку из правых частей
                    tmpPoint = new Fraction[2] { gm.RightPart[0], gm.RightPart[1] };
                    if (CheckPoint(tmpPoint)) //Точка подходит в ограничение
                    {
                        if (!containPointFraction(lPoints, tmpPoint))
                            lPoints.Add(tmpPoint);
                        Check2DPointForSolution(tmpPoint);
                    }
                }
                //Найти пересечение с осями

                /*-----------   С осью абцисс    ------------*/
                for (int j = 0; j < SSolver.ColumCount; j++)
                {
                    gm.Matrix[0][j] = SSolver.Matrix[i][j];
                }
                gm.RightPart[0] = SSolver.RightPart[i];

                gm.Matrix[1][0] = 0;
                gm.Matrix[1][1] = 1;
                gm.RightPart[1] = 0;

                if (!(gm.SolveMatrix() > 0))//Прямые парараллельны, просто пропускаем этот шаг
                {
                    tmpPoint = new Fraction[2] { gm.RightPart[0], gm.RightPart[1] };
                    if (CheckPoint(tmpPoint))
                    { //Точка подходит в ограничение
                        if (!containPointFraction(lPoints, tmpPoint))
                            lPoints.Add(tmpPoint);
                        Check2DPointForSolution(tmpPoint);
                    }
                }
                /*-----------   С осью ординат   ------------*/
                for (int j = 0; j < SSolver.ColumCount; j++)
                {
                    gm.Matrix[0][j] = SSolver.Matrix[i][j];
                }
                gm.RightPart[0] = SSolver.RightPart[i];

                gm.Matrix[1][0] = 1;
                gm.Matrix[1][1] = 0;
                gm.RightPart[1] = 0;

                if (!(gm.SolveMatrix() > 0))//Прямые парараллельны, просто пропускаем этот шаг
                {
                    tmpPoint = new Fraction[2] { gm.RightPart[0], gm.RightPart[1] };
                    if (CheckPoint(tmpPoint)) //Точка подходит в ограничение
                    {
                        if (!containPointFraction(lPoints, tmpPoint))
                            lPoints.Add(tmpPoint);
                        Check2DPointForSolution(tmpPoint);
                    }
                }
            }
            tmpPoint = new Fraction[2] { new Fraction(0), new Fraction(0) }; //Вписываем точку начала координат (пересечение осей)
            if (CheckPoint(tmpPoint)) //Точка подходит в ограничение
            {
                if (!containPointFraction(lPoints, tmpPoint))
                    lPoints.Add(tmpPoint);
                Check2DPointForSolution(tmpPoint);
            }
        }
        private void MakeMarkup()
        {
            Graphics gr = this.GPanel.CreateGraphics();
            Font drawFont = new Font("Arial", 10);
            Point lp = new Point(), rp = new Point();
            Pen pen = new Pen(Color.LightGray, 2);
            SolidBrush bruh = new SolidBrush(Color.Green);

            int rpMax = (int)gB.ToDouble();
            if (rpMax < (int)gD.ToDouble())
                rpMax = (int)gD.ToDouble();

            rpMax++;
            for (int i = (int)gA.ToDouble(); i < rpMax; i++)
            {
                // АБЦИССы                
                lp.X = 0;
                lp.Y = GPanel.Height - (int)(GPanel.Height * ((i - gC) / (gD - gC)).ToDouble());
                rp.X = GPanel.Width;
                rp.Y = GPanel.Height - (int)(GPanel.Height * ((i - gC) / (gD - gC)).ToDouble());
                gr.DrawLine(pen, lp, rp);

                lp.Offset(15, -20);
                gr.DrawString(i.ToString(), drawFont, bruh, lp.X, lp.Y);
            }
            for (int i = (int)gC.ToDouble(); i < rpMax; i++)
            {
                // ОРДИНАТы                
                lp.X = (int)(GPanel.Width * ((i - gA) / (gB - gA)).ToDouble());
                lp.Y = GPanel.Height;
                rp.X = (int)(GPanel.Width * ((i - gA) / (gB - gA)).ToDouble());
                rp.Y = 0;
                gr.DrawLine(pen, lp, rp);

                lp.Offset(-20, -20);
                gr.DrawString(i.ToString(), drawFont, bruh, lp.X, lp.Y);
            }

            pen.Color = Color.Black;
            // ЖИРНАЯ ОСЬ АБЦИСС
            lp.X = 0;
            lp.Y = GPanel.Height - (int)(GPanel.Height * ((0 - gC) / (gD - gC)).ToDouble());
            rp.X = GPanel.Width;
            rp.Y = GPanel.Height - (int)(GPanel.Height * ((0 - gC) / (gD - gC)).ToDouble());
            gr.DrawLine(pen, lp, rp);
            lp.Offset(30, -10);
            //gr.DrawString("0", drawFont, bruh, lp.X, lp.Y);
            // ЖИРНАЯ ОСЬ ОРДИНАТ
            lp.X = (int)(GPanel.Width * ((0 - gA) / (gB - gA)).ToDouble());
            lp.Y = 0;
            rp.X = (int)(GPanel.Width * ((0 - gA) / (gB - gA)).ToDouble());
            rp.Y = GPanel.Height;
            gr.DrawLine(pen, lp, rp);

            //RX1 + ((RX2 - RX1) * ((x - A) / (B - A)));
        }
        private void Make2DModel()
        {
            //Вся эта функция рассчитана только на 2D!


            /*
             * Найдём все точки пересечения и только после будем рисовать все все примые
             * Почему так?
             * После того как найдём все точки, и в целом ответ, 
             * можно будет определить границы самого графика путём поиска 4 границ в виде прямоугольника.
             * 
             */


            WriteAllRestrictionsAndObjFunc();

            Find2DPoints();


            //после того как всё завершилось мы должны проверить что точки вообще есть
            if (lPoints.Count == 0)
            {
                //Ругаемся что решений нет и выходим
                ActivateButtnosOnTab(0);
                GAnswerText.Text = "Нет решения для данной системы ограничения";
                return;
            }

            /* Для проверки ОГРАНИЧЕННОСТИ ЗАДАЧИ
             * пробежимся по каждой прямой и отложим от нашей оптимальной точки вектор этой прямой
             * 1-2 прямые должны дать 1-2 точки которые могут оказаться оптимальней и если они не принадлежат  нашему списку решений, то тогда 
             * ругаться что наша функция не ограничена и следовательной всё плохо
             */
            fTaskIsLimited = true;
            Fraction[] posibleOptimalPoint = new Fraction[2];
            Fraction OldValue;
            for (int i = 0; i < SSolver.RowCount; i++)
            {
                posibleOptimalPoint[0] = lPoints[indexOfOptimalPoint][0] + 1;
                if (SSolver.Matrix[i][1] != 0)
                {
                    posibleOptimalPoint[1] = lPoints[indexOfOptimalPoint][1] - SSolver.Matrix[i][0] / SSolver.Matrix[i][1];
                }
                else
                {
                    posibleOptimalPoint[1] = lPoints[indexOfOptimalPoint][1];
                }
                if (CheckPoint(posibleOptimalPoint) && !containPointFraction(lPoints, posibleOptimalPoint))
                {
                    //точка подошла, теперь нужно узнать что она может оказаться оптимальной
                    OldValue = new Fraction(valueOfSolution);
                    Check2DPointForSolution(posibleOptimalPoint);
                    if (OldValue > valueOfSolution)
                    {
                        //всё плохо, наша новая точка не содержалась в списке вершин многоугольника но оказалась оптимальней
                        //поставим в честь этого свечку
                        fTaskIsLimited = false;
                        GAnswerText.Text = "Задача не ограничена, нет решения";
                        break;
                    }
                }
            }
            //Проверим так же оси
            posibleOptimalPoint[0] = lPoints[indexOfOptimalPoint][0] + 1;
            posibleOptimalPoint[1] = lPoints[indexOfOptimalPoint][1];
            if (CheckPoint(posibleOptimalPoint) && !containPointFraction(lPoints, posibleOptimalPoint))
            {
                //точка подошла, теперь нужно узнать что она может оказаться оптимальной
                OldValue = new Fraction(valueOfSolution);
                Check2DPointForSolution(posibleOptimalPoint);
                if (OldValue > valueOfSolution)
                {
                    //всё плохо, наша новая точка не содержалась в списке вершин многоугольника но оказалась оптимальней
                    //поставим в честь этого свечку
                    fTaskIsLimited = false;
                    GAnswerText.Text = "Задача не ограничена, нет решения";
                }
            }
            posibleOptimalPoint[0] = lPoints[indexOfOptimalPoint][0] + 0;
            posibleOptimalPoint[1] = lPoints[indexOfOptimalPoint][1] + 1;
            if (CheckPoint(posibleOptimalPoint) && !containPointFraction(lPoints, posibleOptimalPoint))
            {
                //точка подошла, теперь нужно узнать что она может оказаться оптимальной
                OldValue = new Fraction(valueOfSolution);
                Check2DPointForSolution(posibleOptimalPoint);
                if (OldValue > valueOfSolution)
                {
                    //всё плохо, наша новая точка не содержалась в списке вершин многоугольника но оказалась оптимальней
                    //поставим в честь этого свечку
                    fTaskIsLimited = false;
                    GAnswerText.Text = "Задача не ограничена, нет решения";
                }
            }
            //выводим ответ
            StringBuilder answer;
            answer = new StringBuilder("x* (");

            for (int i = 0; i < SSolver.RowCount + SSolver.ColumCount; i++)
            {
                if (ColumCount - RowCount == 1 && RBGraphic.Checked &&
                    i + 1 == SSolver.RowCount + SSolver.ColumCount)
                {
                    continue;
                }
                Fraction res = new Fraction(0);
                int indexOfIEl = -1;
                if (SSolver.ILBasisEl.Contains(i))
                {
                    indexOfIEl = SSolver.ILBasisEl.IndexOf(i);
                    res += SSolver.RightPart[indexOfIEl];
                    for (int g = 0; g < SSolver.ColumCount; g++)
                    {
                        res -= SSolver.Matrix[indexOfIEl][g] * lPoints[indexOfOptimalPoint][g];
                    }
                    answer.Append($"{(fractionType.SelectedIndex == 0 ? res : res.ToDouble())}");
                }
                else
                {
                    indexOfIEl = SSolver.ILFreeEl.IndexOf(i);
                    res = lPoints[indexOfOptimalPoint][indexOfIEl];
                    answer.Append($"{(fractionType.SelectedIndex == 0 ? res : res.ToDouble())}");
                }
                answer.Append(", ");
            }
            answer.Length--;//Удаляем последний символ.
            answer.Length--;
            answer.Append(")");
            if (optimizationProblem.SelectedIndex == 1)
                valueOfSolution = -valueOfSolution;

            answer.Append($"\r\n\nf(x*) = " + (fractionType.SelectedIndex == 0 ? $"{valueOfSolution}" : $"{valueOfSolution.ToDouble()}"));
            if (fTaskIsLimited)
                GAnswerText.Text = answer.ToString();

            /*----   Зададим несколько параметров для прямой которая будет закрашивать нашу фигуру на плоскости    ----*/
            int indexOfLeftPoint = 0;
            int indexOfRightPoint = 0;


            paramA = 3;
            paramBForRight = paramBForLeft = lPoints[0][1] - paramA * lPoints[0][0];

            //тогда можно сделать следующие - нам нужно найти 4 границы 
            //2 по горизонтоли и 2 по вертикали
            lA = lB = lPoints[0][0];
            lC = lD = lPoints[0][1];
            for (int i = 1; i < lPoints.Count; i++)
            {
                if (lA > lPoints[i][0])
                    lA = lPoints[i][0];
                if (lB < lPoints[i][0])
                    lB = lPoints[i][0];

                if (lC > lPoints[i][1])
                    lC = lPoints[i][1];
                if (lD < lPoints[i][1])
                    lD = lPoints[i][1];

                //параллельно будем искать самую высокую и самую низкую для прямой зарисовки точки
                if (lPoints[i][1] - paramA * lPoints[i][0] - paramBForLeft > 0)// точка выше текущей прямой
                {
                    indexOfLeftPoint = i;
                    paramBForLeft = lPoints[i][1] - paramA * lPoints[i][0];
                }
                else if (lPoints[i][1] - paramA * lPoints[i][0] - paramBForRight < 0)//точка ниже 
                {
                    indexOfRightPoint = i;
                    paramBForRight = lPoints[i][1] - paramA * lPoints[i][0];
                }
            }

            offset = (lB - lA) / 7;
            if (offset == 0)
                offset += 1;
            //Нашли границы рисунка, но будем рисовать с отступом, чтобы картинка не была грубой            
            gA = -offset;
            gC = -offset;
            gB = lB + offset;
            gD = lD + offset;

            if ((gB - gA) / GPanel.Width > (gD - gC) / GPanel.Height)
            {
                gD = ((gB - gA) / GPanel.Width) * GPanel.Height + gC;
            }
            else
            {
                gB = ((gD - gC) / GPanel.Height) * GPanel.Width + gA;
            }

            //Сохраним первоначальные значения границ рисунка и после позволим их себе портить
            sA = new Fraction(gA);
            sB = new Fraction(gB);
            sC = new Fraction(gC);
            sD = new Fraction(gD);
            fCanDraw = true;
            Draw2D();
        }
        private void Draw2D()
        {
            /*-------------     НАЧАЛО ВСЕГО РИСОВАНИЯ     -------------*/
            if (!fCanDraw)
                return;
            SolidBrush bruh = new SolidBrush(Color.Red);
            Graphics gr = this.GPanel.CreateGraphics();
            MakeMarkup();


            Fraction[] leftPoint = new Fraction[2], rightPoint = new Fraction[2];
            Pen pen = new Pen(bruh);

            Point lp = new Point(), rp = new Point();
            for (int i = 0; i < SSolver.RowCount; i++)
            {
                //Для каждого уравнения прямой рисуем эту прямую путём поиска 2 точек на концах панельки

                if (SSolver.Matrix[i][1] != 0) //Не вертикальная линия
                {
                    leftPoint[0] = gA;
                    rightPoint[0] = gB;
                    leftPoint[1] = (SSolver.RightPart[i] - SSolver.Matrix[i][0] * leftPoint[0]) / SSolver.Matrix[i][1];
                    rightPoint[1] = (SSolver.RightPart[i] - SSolver.Matrix[i][0] * rightPoint[0]) / SSolver.Matrix[i][1];
                }
                else //Вертикальная линия
                {
                    if (SSolver.Matrix[i][0] == 0)
                    {
                        continue;
                    }
                    leftPoint[0] = rightPoint[0] = SSolver.RightPart[i] / SSolver.Matrix[i][0];
                    leftPoint[1] = gC;
                    rightPoint[1] = gD;
                }

                lp.X = (int)(GPanel.Width * ((leftPoint[0] - gA) / (gB - gA)).ToDouble());
                lp.Y = GPanel.Height - (int)(GPanel.Height * ((leftPoint[1] - gC) / (gD - gC)).ToDouble());

                rp.X = (int)(GPanel.Width * ((rightPoint[0] - gA) / (gB - gA)).ToDouble());
                rp.Y = GPanel.Height - (int)(GPanel.Height * ((rightPoint[1] - gC) / (gD - gC)).ToDouble());

                gr.DrawLine(pen, lp, rp);
            }
            pen.Color = Color.Aqua;
            /*---------    ЗАКРАШИВАЕМ    ---------*/
            GaussMatrix gm = new GaussMatrix(SSolver.ColumCount);
            Fraction[] tmpPoint;
            List<Fraction[]> lPrintPoints = new List<Fraction[]>();

            distance = Fraction.Abs((gB * paramA) + gD);
            step = distance / 60;
            paramBForLeft = gD;
            for (Fraction iter = step; iter < distance; iter += step)
            {
                for (int i = 0; i < SSolver.RowCount; i++)
                {
                    //Заполняем матрицу гаусса
                    for (int j = 0; j < SSolver.ColumCount; j++)
                    {
                        gm.Matrix[0][j] = SSolver.Matrix[i][j];
                    }
                    gm.RightPart[0] = SSolver.RightPart[i];

                    gm.Matrix[1][0] = -paramA;
                    gm.Matrix[1][1] = 1;
                    gm.RightPart[1] = paramBForLeft - iter;

                    if (gm.SolveMatrix() > 0)
                    {
                        //Прямые парараллельны, просто пропускаем этот шаг
                        continue;
                    }
                    //иначе записываем точку из правых частей
                    tmpPoint = new Fraction[2] { gm.RightPart[0], gm.RightPart[1] };
                    if (CheckPoint(tmpPoint)) //Точка подходит в ограничение
                    {
                        if (!containPointFraction(lPrintPoints, tmpPoint))
                            lPrintPoints.Add(tmpPoint);
                    }

                }
                /*-----------   С осью абцисс    ------------*/
                gm.Matrix[0][0] = 0;
                gm.Matrix[0][1] = 1;
                gm.RightPart[0] = 0;

                gm.Matrix[1][0] = -paramA;
                gm.Matrix[1][1] = 1;
                gm.RightPart[1] = paramBForLeft - iter;

                if (!(gm.SolveMatrix() > 0))//Прямые парараллельны, просто пропускаем этот шаг
                {
                    tmpPoint = new Fraction[2] { gm.RightPart[0], gm.RightPart[1] };
                    if (CheckPoint(tmpPoint))
                    { //Точка подходит в ограничение
                        if (!containPointFraction(lPrintPoints, tmpPoint))
                            lPrintPoints.Add(tmpPoint);
                    }
                }
                /*-----------   С осью ординат   ------------*/
                gm.Matrix[0][0] = 1;
                gm.Matrix[0][1] = 0;
                gm.RightPart[0] = 0;

                gm.Matrix[1][0] = -paramA;
                gm.Matrix[1][1] = 1;
                gm.RightPart[1] = paramBForLeft - iter;

                if (!(gm.SolveMatrix() > 0))//Прямые парараллельны, просто пропускаем этот шаг
                {
                    tmpPoint = new Fraction[2] { gm.RightPart[0], gm.RightPart[1] };
                    if (CheckPoint(tmpPoint)) //Точка подходит в ограничение
                    {
                        if (!containPointFraction(lPrintPoints, tmpPoint))
                            lPrintPoints.Add(tmpPoint);
                    }
                }


                if (lPrintPoints.Count == 1)
                {
                    //Если одна точка, то значит что можно продлить прямую до верхнего ограничения

                    /*-----------   по оси абцисс    ------------*/
                    gm.Matrix[0][0] = 0;
                    gm.Matrix[0][1] = 1;
                    gm.RightPart[0] = gD;

                    gm.Matrix[1][0] = -paramA;
                    gm.Matrix[1][1] = 1;
                    gm.RightPart[1] = paramBForLeft - iter;

                    if (!(gm.SolveMatrix() > 0))//Прямые парараллельны, просто пропускаем этот шаг
                    {
                        tmpPoint = new Fraction[2] { gm.RightPart[0], gm.RightPart[1] };
                        if (CheckPoint(tmpPoint))
                        { //Точка подходит в ограничение
                            if (!containPointFraction(lPrintPoints, tmpPoint))
                                lPrintPoints.Add(tmpPoint);
                        }
                    }
                    /*-----------   по оси ординат   ------------*/
                    gm.Matrix[0][0] = 1;
                    gm.Matrix[0][1] = 0;
                    gm.RightPart[0] = gB;

                    gm.Matrix[1][0] = -paramA;
                    gm.Matrix[1][1] = 1;
                    gm.RightPart[1] = paramBForLeft - iter;

                    if (!(gm.SolveMatrix() > 0))//Прямые парараллельны, просто пропускаем этот шаг
                    {
                        tmpPoint = new Fraction[2] { gm.RightPart[0], gm.RightPart[1] };
                        if (CheckPoint(tmpPoint)) //Точка подходит в ограничение
                        {
                            if (!containPointFraction(lPrintPoints, tmpPoint))
                                lPrintPoints.Add(tmpPoint);
                        }
                    }

                    if (lPrintPoints.Count > 1)
                    {
                        lp.X = (int)(GPanel.Width * ((lPrintPoints[0][0] - gA) / (gB - gA)).ToDouble());
                        lp.Y = GPanel.Height - (int)(GPanel.Height * ((lPrintPoints[0][1] - gC) / (gD - gC)).ToDouble());

                        rp.X = (int)(GPanel.Width * ((lPrintPoints[1][0] - gA) / (gB - gA)).ToDouble());
                        rp.Y = GPanel.Height - (int)(GPanel.Height * ((lPrintPoints[1][1] - gC) / (gD - gC)).ToDouble());

                        gr.DrawLine(pen, lp, rp);
                        lPrintPoints.Clear();
                        continue;
                    }
                }

                // Проверяем что у нас 2 точки
                if (lPrintPoints.Count != 2)
                {
                    //у нас не 2 точки а следовательно - это плохо
                    lPrintPoints.Clear();
                    continue;
                }
                lp.X = (int)(GPanel.Width * ((lPrintPoints[0][0] - gA) / (gB - gA)).ToDouble());
                lp.Y = GPanel.Height - (int)(GPanel.Height * ((lPrintPoints[0][1] - gC) / (gD - gC)).ToDouble());

                rp.X = (int)(GPanel.Width * ((lPrintPoints[1][0] - gA) / (gB - gA)).ToDouble());
                rp.Y = GPanel.Height - (int)(GPanel.Height * ((lPrintPoints[1][1] - gC) / (gD - gC)).ToDouble());

                gr.DrawLine(pen, lp, rp);

                lPrintPoints.Clear(); //очищаем список для следующих рисовашек                
            }


            /*-------------     ВЫДЕЛИМ ТУ САМУЮ ОПТИМАЛЬНУЮ ТОЧКУ     -------------*/
            pen.Color = Color.Green;
            pen.Width = 3;
            lp.X = (int)(GPanel.Width * ((lPoints[indexOfOptimalPoint][0] - gA) / (gB - gA)).ToDouble());
            lp.Y = GPanel.Height - (int)(GPanel.Height * ((lPoints[indexOfOptimalPoint][1] - gC) / (gD - gC))).ToDouble();
            float Radius = 17;
            if (fTaskIsLimited)
                gr.DrawEllipse(pen, lp.X - Radius / 2, lp.Y - Radius / 2, Radius, Radius);


            /*-------------     РИСУЕМ ПРЯМУЮ СОБ. ФУНКЦИИ     -------------*/
            if (SSolver.ObjFunction[1] != 0 && SSolver.ObjFunction[0] != 0) //Значит это НЕ точка и можно что-то нарисовать
            {
                pen.Color = Color.DarkGreen;
                pen.Width = 2;

                if (SSolver.ObjFunction[1] != 0)
                {
                    leftPoint[0] = gA;
                    rightPoint[0] = gB;
                    leftPoint[1] = (0 - SSolver.ObjFunction[0] * leftPoint[0]) / SSolver.ObjFunction[1];
                    rightPoint[1] = (0 - SSolver.ObjFunction[0] * rightPoint[0]) / SSolver.ObjFunction[1];
                }
                else
                {
                    leftPoint[0] = rightPoint[0] = 0;
                    leftPoint[1] = gC;
                    rightPoint[1] = gD;
                }

                lp.X = (int)(GPanel.Width * ((leftPoint[0] - gA) / (gB - gA)).ToDouble());
                lp.Y = GPanel.Height - (int)(GPanel.Height * ((leftPoint[1] - gC) / (gD - gC)).ToDouble());

                rp.X = (int)(GPanel.Width * ((rightPoint[0] - gA) / (gB - gA)).ToDouble());
                rp.Y = GPanel.Height - (int)(GPanel.Height * ((rightPoint[1] - gC) / (gD - gC)).ToDouble());

                gr.DrawLine(pen, lp, rp);
            }

            /*-------------     РИСУЕМ НОРМАЛЬ ПРЯМОЙ СОБ. ФУНКЦИИ     -------------*/
            pen.Color = Color.DarkRed;


            leftPoint[0] = 0;
            rightPoint[0] = -SSolver.ObjFunction[0];

            leftPoint[1] = 0; /*(0 + SSolver.ObjFunction[0] * leftPoint[0]) / SSolver.ObjFunction[1];*/
            rightPoint[1] = -SSolver.ObjFunction[1];

            //Normalize
            double verctorLength;
            verctorLength = Math.Sqrt(Math.Pow((rightPoint[0] - leftPoint[0]).ToDouble(), 2) +
                                      Math.Pow((rightPoint[1] - leftPoint[1]).ToDouble(), 2));
            verctorLength /= offset.ToDouble() / 1.3; //Допольнительно уменьшаем длину вектора
            //double tanges = (rightPoint[0] - leftPoint[0]).ToDouble() / (rightPoint[1] - leftPoint[1]).ToDouble();
            //rightPoint[0] /= verctorLength;
            //rightPoint[1] /= verctorLength;
            if (verctorLength != 0)
            {
                lp.X = (int)(GPanel.Width * ((leftPoint[0] - gA) / (gB - gA)).ToDouble());
                lp.Y = GPanel.Height - (int)(GPanel.Height * ((leftPoint[1] - gC) / (gD - gC))).ToDouble();

                rp.X = (int)(GPanel.Width * ((rightPoint[0].ToDouble() / verctorLength - gA.ToDouble()) / (gB.ToDouble() - gA.ToDouble())));
                rp.Y = GPanel.Height - (int)(GPanel.Height * ((rightPoint[1].ToDouble() / verctorLength - gC.ToDouble()) / (gD - gC).ToDouble()));


                gr.DrawLine(pen, lp, rp);


                //нарисуем стрелочки
                double angel = 20 * Math.PI / 180;
                /* 
                 * lp.X = (int)(GPanel.Width * ((leftPoint[0] - gA.ToDouble()) / (gB.ToDouble() - gA.ToDouble())));
                 * lp.Y = GPanel.Height - (int)(GPanel.Height * ((leftPoint[1] - gC.ToDouble()) / (gD.ToDouble() - gC.ToDouble())));
                 * 
                 * leftPoint[0] = (Ex / 3);
                 * leftPoint[1] = (Ey / 3);
                 * 
                 * => смещение (ЛИШНЕЕ!!!) они уже в 0
                 * ((Ex / 3) - Ex)
                 * ((Ey / 3) - Ey)
                 * 
                 * => *как поворачивать
                 * 
                 * x` = x * cos(угол) - y * sin(угол)
                 * y` = x * sin(угол) + y * cos(угол)
                 * 
                 * =>
                 * 
                 * leftPoint[0] = leftPoint[0].ToDouble() * Math.Cos(angel) - leftPoint[1].ToDouble() * Math.Sin(angel);
                 * leftPoint[1] = leftPoint[0].ToDouble() * Math.Sin(angel) + leftPoint[1].ToDouble() * Math.Cos(angel);
                 * 
                 * =>
                 * 
                 * (Ex / 3) ) * Math.Cos(angel) - (((Ey / 3) )) * Math.Sin(angel);
                 * (Ex / 3) ) * Math.Sin(angel) + (((Ey / 3) )) * Math.Cos(angel);
                 * 
                 * => сместить обратно (а не нужно т.к. мы были в нуле уже)
                 * 
                 * (Ex / 3) * Math.Cos(Math.PI + angel) - (Ey / 3) * Math.Sin(Math.PI + angel)) + Ex
                 * (Ex / 3) * Math.Sin(Math.PI + angel) + (Ey / 3) * Math.Cos(Math.PI + angel)) + Ey
                 * 
                 * всё тоже самое с -angel для другой прямой...
                 */

                double Ex = rightPoint[0].ToDouble() / verctorLength;
                double Ey = rightPoint[1].ToDouble() / verctorLength;
                lp.X = (int)(GPanel.Width * ((Ex / 3 * Math.Cos(Math.PI + angel)) - Ey / 3 * Math.Sin(Math.PI + angel) + Ex - gA.ToDouble()) / (gB.ToDouble() - gA.ToDouble()));
                lp.Y = GPanel.Height - (int)(GPanel.Height * ((Ex / 3 * Math.Sin(Math.PI + angel)) + (Ey / 3 * Math.Cos(Math.PI + angel)) + Ey - gC.ToDouble()) / (gD.ToDouble() - gC.ToDouble()));

                gr.DrawLine(pen, lp, rp);
                angel = -angel;
                lp.X = (int)(GPanel.Width * ((Ex / 3 * Math.Cos(Math.PI + angel)) - Ey / 3 * Math.Sin(Math.PI + angel) + Ex - gA.ToDouble()) / (gB.ToDouble() - gA.ToDouble()));
                lp.Y = GPanel.Height - (int)(GPanel.Height * ((Ex / 3 * Math.Sin(Math.PI + angel)) + (Ey / 3 * Math.Cos(Math.PI + angel)) + Ey - gC.ToDouble()) / (gD.ToDouble() - gC.ToDouble()));


                gr.DrawLine(pen, lp, rp);
            }
        }

        private void Make3DModel()
        {
            /*
             * В целом ничего сложного, но!
             * Оказалось что подключить тот же OpenGL без траблов не получится
             * Библиотека ДОЛЖНА быть установлена в систему. 
             * Увы, под тз это слабо подходит
             */
        }


        /*-------------------------------      PRINT TASKS     ------------------------------*/


        private void PrintResultToSoulutionGridView(DataGridView SSolutionTable, SimplexSolver SSolver)
        {
            int countOfAddingRows = ((int)SSolver.RowCount + 2) + StartRowForAnswerTable - SSolutionTable.Rows.Count;
            if (countOfAddingRows > 0)
            {
                SSolutionTable.Rows.Add(countOfAddingRows);
            }

            int countRowsInView = SSolutionTable.Height / SSolutionTable.RowTemplate.Height; //Количество строк помещающихся во View
            if (StartRowForAnswerTable + SSolver.RowCount + 2 > countRowsInView) //Если таблица вылезает за границы отображения
            {
                SSolutionTable.FirstDisplayedScrollingRowIndex = StartRowForAnswerTable - ((countRowsInView - (int)SSolver.RowCount + 2) / 2);
                Update();
            }

            int i, g;
            SSolutionTable[0, StartRowForAnswerTable].Value = "X (" + SSolver.iteration.ToString() + ")";

            for (i = 0; i < SSolver.ColumCount; i++)
            {
                SSolutionTable[i + 1, StartRowForAnswerTable].Value = "X" + (SSolver.ILFreeEl[i] + 1).ToString();
            }

            StartRowForAnswerTable++;

            for (i = 0; i < SSolver.RowCount; i++)
            {
                SSolutionTable[0, i + StartRowForAnswerTable].Value = "X" + (SSolver.ILBasisEl[i] + 1).ToString();
                for (g = 0; g < SSolver.ColumCount; g++)
                {
                    SSolutionTable[g + 1, i + StartRowForAnswerTable].Value = fractionType.SelectedIndex == 0 ?
                                                                                SSolver.Matrix[i][g].ToString() :
                                                                                SSolver.Matrix[i][g].ToDouble().ToString();
                }
                SSolutionTable[g + 1, i + StartRowForAnswerTable].Value = fractionType.SelectedIndex == 0 ?
                                                                            SSolver.RightPart[i].ToString() :
                                                                            SSolver.RightPart[i].ToDouble().ToString();
            }


            for (g = 0; g < SSolver.ColumCount; g++)
            {
                SSolutionTable[g + 1, i + StartRowForAnswerTable].Value = fractionType.SelectedIndex == 0 ?
                                                                            SSolver.ObjFunction[g].ToString() :
                                                                            SSolver.ObjFunction[g].ToDouble().ToString();
            }
            SSolutionTable[g + 1, i + StartRowForAnswerTable].Value = fractionType.SelectedIndex == 0 ?
                                                                            SSolver.OFV.ToString() :
                                                                            SSolver.OFV.ToDouble().ToString();

            StartRowForAnswerTable += i + 2;
        }
        private void RemoveLastTableFromSolutionGridView(DataGridView SSolutionTable)
        {
            if (SSolutionTable.RowCount < SSolver.RowCount + 3)
            {
                SSolutionTable.Rows.Clear(); //Удаляем все
                StartRowForAnswerTable = 0;
                return;
            }
            for (int i = 0; i < SSolver.RowCount + 3; i++)
            {
                SSolutionTable.Rows.RemoveAt(SSolutionTable.RowCount - 1);// n число раз удаляем последнюю строку
            }
            StartRowForAnswerTable = StartRowForAnswerTable - 3 - (int)SSolver.RowCount;
        }


        /* -----------------------------------    SUB WORK (кнопочки там всякие)    --------------------------------------**/


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
            BeginSolve.Enabled = true;
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (!RBArtificial.Checked) return;
            textBox6.Visible = false;
            basicVariablesTable.Visible = false;
            BeginSolve.Enabled = true;
        }
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (!RBGraphic.Checked) return;
            textBox6.Visible = true;
            basicVariablesTable.Visible = true;
            BeginSolve.Enabled = true;
        }

    }


    class DataReadException : Exception
    {
        public DataReadException(string message)
       : base(message)
        { }
    }
}
