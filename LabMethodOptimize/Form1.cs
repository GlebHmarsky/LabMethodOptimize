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

namespace LabMethodOptimize
{
    public partial class Form1 : Form
    {
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
                    basicVariablesTable.Columns.Add(i.ToString(), "X" + i);
                }
            }
            else
            {
                for (int i = restrictionTable.Columns.Count; i > (int)numericUpDownColumn.Value + 1; i--)
                {

                    this.restrictionTable.Columns.RemoveAt(i - 1);
                    objectiveFunctionTable.Columns.RemoveAt(i - 2);
                    basicVariablesTable.Columns.RemoveAt(i - 2);

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
            if (basicVariablesTable.Rows.Count == 0 && objectiveFunctionTable.Columns.Count != 0) basicVariablesTable.Rows.Add(1);
        }
        private void numericUpDownRow_ValueChanged(object sender, EventArgs e)
        {
            if (restrictionTable.Columns.Count == 0)
            {
                numericUpDownRow.Value = 0;
                return;
            }
            if ((int)numericUpDownRow.Value > restrictionTable.Rows.Count)
                this.restrictionTable.Rows.Add((int)numericUpDownRow.Value - restrictionTable.Rows.Count);
            else
            {

                for (int i = restrictionTable.Rows.Count; i > (int)numericUpDownRow.Value; i--)
                {
                    if (i > 0)
                        this.restrictionTable.Rows.RemoveAt(i - 1);
                }
                //if (numericUpDownRow.Value == 0)
                //{
                //    this.SimpleTable.Rows.RemoveAt(0);
                //}
            }

            foreach (DataGridViewRow row in restrictionTable.Rows)
            {

                row.HeaderCell.Value = String.Concat("f",
                    (row.Index + 1).ToString());
            }

            //if (SimpleTable.Rows.Count != 0)
            //    SimpleTable.Rows[SimpleTable.Rows.Count - 1].HeaderCell.Value = "Z";
        }
        private void tableInitButton_Click(object sender, EventArgs e)
        {

            string path = @"R:\Programming\C#\LabMethodOptimize\LabMethodOptimize\MethodOptimize.txt";

            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    string[] str = sr.ReadLine().Split(' ');
                    // TODO:  Проверки на дурака

                    if (Convert.ToInt32(str[0]) == 0)
                        optimizationProblem.SelectedIndex = 0;
                    else
                        optimizationProblem.SelectedIndex = 1;
                    str = sr.ReadLine().Split(' ');
                    uint RowCount, ColumnCount;
                    RowCount = (uint)Convert.ToInt32(str[0]);
                    ColumnCount = (uint)Convert.ToInt32(str[1]);
                    //Solution = new FractionGausMethod(RowCount, ColumnCount);

                    numericUpDownColumn.Value = ColumnCount;
                    numericUpDownRow.Value = RowCount;

                    for (int i = 0; i < RowCount; i++)
                    {
                        str = sr.ReadLine().Split(' ');
                        //UNDONE Проверки на дурака 2
                        for (int g = 0; g < ColumnCount; g++)
                        {
                            //Solution.Matrix[i][g] = Convert.ToInt32(str[g]);
                            restrictionTable[g, i].Value = Convert.ToInt32(str[g]);
                        }
                    }

                    str = sr.ReadLine().Split(' ');
                    for (int i = 0; i < RowCount; i++)
                    {
                        //Solution.RightPart[i] = Convert.ToInt32(str[i]);
                    }
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }


            //Solution.SolveMatrix(); // решаем СЛАУ
            //Console.WriteLine(Solution.ToString());// Печатаем результат


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
                        
                        uint RowCount, ColumnCount;
                        RowCount = (uint)Convert.ToInt32(str[0]);
                        ColumnCount = (uint)Convert.ToInt32(str[1]);
                        //Solution = new FractionGausMethod(RowCount, ColumnCount);

                        numericUpDownColumn.Value = ColumnCount;
                        numericUpDownRow.Value = RowCount;

                        str = sr.ReadLine().Split(' ');
                        for (int g = 0; g < RowCount; g++)
                        {
                            //Solution.RightPart[i] = Convert.ToInt32(str[i]);
                            objectiveFunctionTable[g,0].Value = Convert.ToInt32(str[g]);
                        }

                        for (int i = 0; i < RowCount; i++)
                        {
                            str = sr.ReadLine().Split(' ');
                            //UNDONE Проверки на дурака 2
                            int g;
                            for (g = 0; g < ColumnCount; g++)
                            {
                                //Solution.Matrix[i][g] = Convert.ToInt32(str[g]);
                                restrictionTable[g, i].Value = Convert.ToInt32(str[g]);
                            }
                            restrictionTable[g, i].Value = Convert.ToInt32(str[g]);
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

        }
    }


    // TODO:  Не допускать решение когда не прописаны базисные переменные для графического метода.

}
