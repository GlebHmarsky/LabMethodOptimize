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

        public Form1()
        {
            InitializeComponent();
            optimizationProblem.SelectedIndex = 0;
            fractionType.SelectedIndex = 0;
            radioButton1.Checked = true;

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
            for (int g = 0; g < basicVariablesTable.Columns.Count; g++)
            {
                tmpInteger = Convert.ToInt32(basicVariablesTable[g, 0].Value);
                if (tmpInteger != 0)
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

            SimplexSolver SSolver = new SimplexSolver(RowCount, ColumnCount);
            SSolver.FillTable(GaussMat, objectiveFunctionTable.Rows[0]);

            for (int i = 0; i < RowCount; i++)
            {
                int g;
                for (g = 0; g < ColumnCount; g++)
                {
                    SolutionGridView[g, i].Value = GaussMat.Matrix[i][g].ToString();
                }
                SolutionGridView[g, i].Value = GaussMat.RightPart[i].ToString();
            }
        }
    }


    // TODO:  Не допускать решение когда не прописаны базисные переменные для графического метода.

}
