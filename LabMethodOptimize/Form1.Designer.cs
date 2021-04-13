
namespace LabMethodOptimize
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SimpleTable = new System.Windows.Forms.DataGridView();
            this.buttonAddRow = new System.Windows.Forms.Button();
            this.buttonDeletRow = new System.Windows.Forms.Button();
            this.buttonDelColumn = new System.Windows.Forms.Button();
            this.buttonAddColumn = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.numericUpDownColumn = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.SimpleTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownColumn)).BeginInit();
            this.SuspendLayout();
            // 
            // SimpleTable
            // 
            this.SimpleTable.AllowUserToAddRows = false;
            this.SimpleTable.AllowUserToDeleteRows = false;
            this.SimpleTable.AllowUserToResizeColumns = false;
            this.SimpleTable.AllowUserToResizeRows = false;
            this.SimpleTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.SimpleTable.Location = new System.Drawing.Point(12, 12);
            this.SimpleTable.Name = "SimpleTable";
            this.SimpleTable.Size = new System.Drawing.Size(445, 248);
            this.SimpleTable.TabIndex = 0;
            // 
            // buttonAddRow
            // 
            this.buttonAddRow.Location = new System.Drawing.Point(491, 79);
            this.buttonAddRow.Name = "buttonAddRow";
            this.buttonAddRow.Size = new System.Drawing.Size(144, 47);
            this.buttonAddRow.TabIndex = 1;
            this.buttonAddRow.Text = "Add";
            this.buttonAddRow.UseVisualStyleBackColor = true;
            this.buttonAddRow.Click += new System.EventHandler(this.buttonAddRow_Click);
            // 
            // buttonDeletRow
            // 
            this.buttonDeletRow.Location = new System.Drawing.Point(491, 151);
            this.buttonDeletRow.Name = "buttonDeletRow";
            this.buttonDeletRow.Size = new System.Drawing.Size(144, 53);
            this.buttonDeletRow.TabIndex = 2;
            this.buttonDeletRow.Text = "Del";
            this.buttonDeletRow.UseVisualStyleBackColor = true;
            this.buttonDeletRow.Click += new System.EventHandler(this.buttonDeletRow_Click);
            // 
            // buttonDelColumn
            // 
            this.buttonDelColumn.Location = new System.Drawing.Point(641, 151);
            this.buttonDelColumn.Name = "buttonDelColumn";
            this.buttonDelColumn.Size = new System.Drawing.Size(144, 53);
            this.buttonDelColumn.TabIndex = 4;
            this.buttonDelColumn.Text = "Del";
            this.buttonDelColumn.UseVisualStyleBackColor = true;
            this.buttonDelColumn.Click += new System.EventHandler(this.buttonDelColumn_Click);
            // 
            // buttonAddColumn
            // 
            this.buttonAddColumn.Location = new System.Drawing.Point(641, 79);
            this.buttonAddColumn.Name = "buttonAddColumn";
            this.buttonAddColumn.Size = new System.Drawing.Size(144, 47);
            this.buttonAddColumn.TabIndex = 3;
            this.buttonAddColumn.Text = "Add";
            this.buttonAddColumn.UseVisualStyleBackColor = true;
            this.buttonAddColumn.Click += new System.EventHandler(this.buttonAddColumn_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(491, 48);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(130, 20);
            this.textBox1.TabIndex = 5;
            this.textBox1.Text = "Rows";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(641, 48);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(130, 20);
            this.textBox2.TabIndex = 6;
            this.textBox2.Text = "Columns";
            // 
            // numericUpDownColumn
            // 
            this.numericUpDownColumn.Location = new System.Drawing.Point(641, 240);
            this.numericUpDownColumn.Name = "numericUpDownColumn";
            this.numericUpDownColumn.Size = new System.Drawing.Size(144, 20);
            this.numericUpDownColumn.TabIndex = 7;
            this.numericUpDownColumn.ValueChanged += new System.EventHandler(this.numericUpDownColumn_ValueChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(825, 476);
            this.Controls.Add(this.numericUpDownColumn);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.buttonDelColumn);
            this.Controls.Add(this.buttonAddColumn);
            this.Controls.Add(this.buttonDeletRow);
            this.Controls.Add(this.buttonAddRow);
            this.Controls.Add(this.SimpleTable);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.SimpleTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownColumn)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView SimpleTable;
        private System.Windows.Forms.Button buttonAddRow;
        private System.Windows.Forms.Button buttonDeletRow;
        private System.Windows.Forms.Button buttonDelColumn;
        private System.Windows.Forms.Button buttonAddColumn;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.NumericUpDown numericUpDownColumn;
    }
}

