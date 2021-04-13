using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
                for (int i = SimpleTable.Columns.Count ; i < (int)numericUpDownColumn.Value + 1; i++)
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
                if(numericUpDownColumn.Value == 0)
                {
                    this.SimpleTable.Columns.RemoveAt(0);
                }
            }

            foreach (DataGridViewColumn column in SimpleTable.Columns)
            {

                column.HeaderText = String.Concat("C",
                    (column.Index+1).ToString());
            }
            if (SimpleTable.Columns.Count != 0) 
                SimpleTable.Columns[SimpleTable.Columns.Count-1].HeaderText = "C";
        }
    }


}
