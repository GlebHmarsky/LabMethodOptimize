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

namespace LabMethodOptimize
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonAddRow_Click(object sender, EventArgs e)
        {
            if (this.SimpleTable.Columns.Count > 0)
                this.SimpleTable.Rows.Add();
        }
        private void buttonDeletRow_Click(object sender, EventArgs e)
        {
            if (this.SimpleTable.Rows.Count > 0)
                this.SimpleTable.Rows.RemoveAt(this.SimpleTable.Rows.Count - 1);

        }


        private void buttonAddColumn_Click(object sender, EventArgs e)
        {
            this.SimpleTable.Columns.Add("jopa", "WowThisButt");
        }
        private void buttonDelColumn_Click(object sender, EventArgs e)
        {
            if (this.SimpleTable.Columns.Count > 0)
                this.SimpleTable.Columns.RemoveAt(this.SimpleTable.Columns.Count - 1);

        }


        private void numericUpDownColumn_ValueChanged(object sender, EventArgs e)
        {
            if (SimpleTable.Columns.Count < 1)
            {
                this.SimpleTable.Columns.Add(1.ToString(), "C");
            }


            if ((int)numericUpDownColumn.Value + 1 > SimpleTable.Columns.Count)
            {
                for (int i = SimpleTable.Columns.Count; i < (int)numericUpDownColumn.Value + 1; i++)
                {
                    this.SimpleTable.Columns.Add(i.ToString(), "C" + i);
                }
            }
            else
            {
                for (int i = SimpleTable.Columns.Count; i > (int)numericUpDownColumn.Value + 1; i--)
                {
                    if (i > 0)
                        this.SimpleTable.Columns.RemoveAt(i - 1);
                }
                if (numericUpDownColumn.Value == 0)
                {
                    numericUpDownRow.Value = 0;
                    this.SimpleTable.Columns.RemoveAt(0);

                }
            }

            foreach (DataGridViewColumn column in SimpleTable.Columns)
            {

                column.HeaderText = String.Concat("C",
                    (column.Index + 1).ToString());
            }
            if (SimpleTable.Columns.Count != 0)
                SimpleTable.Columns[SimpleTable.Columns.Count - 1].HeaderText = "C";
        }
        private void numericUpDownRow_ValueChanged(object sender, EventArgs e)
        {

            if ((int)numericUpDownRow.Value > SimpleTable.Rows.Count)
                this.SimpleTable.Rows.Add((int)numericUpDownRow.Value - SimpleTable.Rows.Count);
            else
            {

                for (int i = SimpleTable.Rows.Count; i > (int)numericUpDownRow.Value; i--)
                {
                    if (i > 0)
                        this.SimpleTable.Rows.RemoveAt(i - 1);
                }
                //if (numericUpDownRow.Value == 0)
                //{
                //    this.SimpleTable.Rows.RemoveAt(0);
                //}
            }

            foreach (DataGridViewRow row in SimpleTable.Rows)
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
                    uint RowCount, ColumnCount;
                    RowCount = (uint)Convert.ToInt32(str[0]);
                    ColumnCount = (uint)Convert.ToInt32(str[1]);
                    //Solution = new FractionGausMethod(RowCount, ColumnCount);

                    for (int i = 0; i < RowCount; i++)
                    {
                        str = sr.ReadLine().Split(' ');
                        //UNDONE Проверки на дурака 2
                        for (int g = 0; g < ColumnCount; g++)
                        {
                            //Solution.Matrix[i][g] = Convert.ToInt32(str[g]);
                            SimpleTable.Rows[i].Cells[g].Value = Convert.ToInt32(str[g]);
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
                Console.WriteLine(exp.Message);
            }


            //Solution.SolveMatrix(); // решаем СЛАУ
            //Console.WriteLine(Solution.ToString());// Печатаем результат


        }
    }


}
