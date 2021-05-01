
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
            this.restrictionTable = new System.Windows.Forms.DataGridView();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.numericUpDownColumn = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownRow = new System.Windows.Forms.NumericUpDown();
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.BeginSolve = new System.Windows.Forms.Button();
            this.groupBoxMethod = new System.Windows.Forms.GroupBox();
            this.RBGraphic = new System.Windows.Forms.RadioButton();
            this.RBArtificial = new System.Windows.Forms.RadioButton();
            this.RBSimplexMethod = new System.Windows.Forms.RadioButton();
            this.fractionType = new System.Windows.Forms.ComboBox();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.optimizationProblem = new System.Windows.Forms.ComboBox();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.basicVariablesTable = new System.Windows.Forms.DataGridView();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.objectiveFunctionTable = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.SStepBackButton = new System.Windows.Forms.Button();
            this.AllSStepButton = new System.Windows.Forms.Button();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SSTextAnswer = new System.Windows.Forms.TextBox();
            this.SStepButton = new System.Windows.Forms.Button();
            this.SSolutionTable = new System.Windows.Forms.DataGridView();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.AllABStepButton = new System.Windows.Forms.Button();
            this.textBox10 = new System.Windows.Forms.TextBox();
            this.textBox11 = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.ABAnswerText = new System.Windows.Forms.TextBox();
            this.ABStepButton = new System.Windows.Forms.Button();
            this.ABSolverTable = new System.Windows.Forms.DataGridView();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.GAnswerText = new System.Windows.Forms.TextBox();
            this.GSSolutionTable = new System.Windows.Forms.DataGridView();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.ABStepBackButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.restrictionTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownColumn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRow)).BeginInit();
            this.MainMenu.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBoxMethod.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.basicVariablesTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectiveFunctionTable)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSolutionTable)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ABSolverTable)).BeginInit();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GSSolutionTable)).BeginInit();
            this.SuspendLayout();
            // 
            // restrictionTable
            // 
            this.restrictionTable.AllowUserToAddRows = false;
            this.restrictionTable.AllowUserToDeleteRows = false;
            this.restrictionTable.AllowUserToResizeColumns = false;
            this.restrictionTable.AllowUserToResizeRows = false;
            this.restrictionTable.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(229)))), ((int)(((byte)(238)))));
            this.restrictionTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.restrictionTable.Location = new System.Drawing.Point(195, 111);
            this.restrictionTable.Name = "restrictionTable";
            this.restrictionTable.RowHeadersWidth = 80;
            this.restrictionTable.Size = new System.Drawing.Size(550, 344);
            this.restrictionTable.TabIndex = 0;
            this.restrictionTable.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.restrictionTable_CellValueChanged);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.Ivory;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.textBox1.Location = new System.Drawing.Point(18, 92);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(130, 14);
            this.textBox1.TabIndex = 5;
            this.textBox1.Text = "Число ограничений";
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.Ivory;
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.textBox2.Location = new System.Drawing.Point(18, 29);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(130, 14);
            this.textBox2.TabIndex = 6;
            this.textBox2.Text = "Число переменных";
            // 
            // numericUpDownColumn
            // 
            this.numericUpDownColumn.Location = new System.Drawing.Point(18, 48);
            this.numericUpDownColumn.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.numericUpDownColumn.Name = "numericUpDownColumn";
            this.numericUpDownColumn.Size = new System.Drawing.Size(154, 21);
            this.numericUpDownColumn.TabIndex = 7;
            this.numericUpDownColumn.ValueChanged += new System.EventHandler(this.numericUpDownColumn_ValueChanged);
            // 
            // numericUpDownRow
            // 
            this.numericUpDownRow.Location = new System.Drawing.Point(18, 111);
            this.numericUpDownRow.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.numericUpDownRow.Name = "numericUpDownRow";
            this.numericUpDownRow.Size = new System.Drawing.Size(154, 21);
            this.numericUpDownRow.TabIndex = 9;
            this.numericUpDownRow.ValueChanged += new System.EventHandler(this.numericUpDownRow_ValueChanged);
            // 
            // MainMenu
            // 
            this.MainMenu.BackColor = System.Drawing.Color.LemonChiffon;
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem1,
            this.aboutToolStripMenuItem});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(1410, 24);
            this.MainMenu.TabIndex = 11;
            this.MainMenu.Text = "menuStrip1";
            this.MainMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.MainMenu_ItemClicked);
            // 
            // fileToolStripMenuItem1
            // 
            this.fileToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem});
            this.fileToolStripMenuItem1.Name = "fileToolStripMenuItem1";
            this.fileToolStripMenuItem1.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem1.Text = "File";
            this.fileToolStripMenuItem1.Click += new System.EventHandler(this.fileToolStripMenuItem1_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Controls.Add(this.tabPage2);
            this.tabControl.Controls.Add(this.tabPage3);
            this.tabControl.Controls.Add(this.tabPage4);
            this.tabControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.tabControl.Location = new System.Drawing.Point(0, 27);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1400, 800);
            this.tabControl.TabIndex = 12;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.Ivory;
            this.tabPage1.Controls.Add(this.BeginSolve);
            this.tabPage1.Controls.Add(this.groupBoxMethod);
            this.tabPage1.Controls.Add(this.fractionType);
            this.tabPage1.Controls.Add(this.textBox8);
            this.tabPage1.Controls.Add(this.optimizationProblem);
            this.tabPage1.Controls.Add(this.textBox7);
            this.tabPage1.Controls.Add(this.textBox6);
            this.tabPage1.Controls.Add(this.basicVariablesTable);
            this.tabPage1.Controls.Add(this.textBox4);
            this.tabPage1.Controls.Add(this.textBox3);
            this.tabPage1.Controls.Add(this.objectiveFunctionTable);
            this.tabPage1.Controls.Add(this.restrictionTable);
            this.tabPage1.Controls.Add(this.numericUpDownRow);
            this.tabPage1.Controls.Add(this.numericUpDownColumn);
            this.tabPage1.Controls.Add(this.textBox2);
            this.tabPage1.Controls.Add(this.textBox1);
            this.tabPage1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1392, 772);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "Условия задачи";
            // 
            // BeginSolve
            // 
            this.BeginSolve.BackColor = System.Drawing.Color.YellowGreen;
            this.BeginSolve.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.BeginSolve.Location = new System.Drawing.Point(35, 483);
            this.BeginSolve.Name = "BeginSolve";
            this.BeginSolve.Size = new System.Drawing.Size(121, 77);
            this.BeginSolve.TabIndex = 23;
            this.BeginSolve.Text = "Решать";
            this.BeginSolve.UseVisualStyleBackColor = false;
            this.BeginSolve.Click += new System.EventHandler(this.BeginSolve_Click);
            // 
            // groupBoxMethod
            // 
            this.groupBoxMethod.Controls.Add(this.RBGraphic);
            this.groupBoxMethod.Controls.Add(this.RBArtificial);
            this.groupBoxMethod.Controls.Add(this.RBSimplexMethod);
            this.groupBoxMethod.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.groupBoxMethod.Location = new System.Drawing.Point(18, 152);
            this.groupBoxMethod.Name = "groupBoxMethod";
            this.groupBoxMethod.Size = new System.Drawing.Size(154, 141);
            this.groupBoxMethod.TabIndex = 22;
            this.groupBoxMethod.TabStop = false;
            this.groupBoxMethod.Text = "Метод решения";
            // 
            // RBGraphic
            // 
            this.RBGraphic.AutoSize = true;
            this.RBGraphic.Location = new System.Drawing.Point(6, 99);
            this.RBGraphic.Name = "RBGraphic";
            this.RBGraphic.Size = new System.Drawing.Size(143, 19);
            this.RBGraphic.TabIndex = 23;
            this.RBGraphic.TabStop = true;
            this.RBGraphic.Text = "Графический метод";
            this.RBGraphic.UseVisualStyleBackColor = true;
            this.RBGraphic.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
            // 
            // RBArtificial
            // 
            this.RBArtificial.AutoSize = true;
            this.RBArtificial.Location = new System.Drawing.Point(6, 67);
            this.RBArtificial.Name = "RBArtificial";
            this.RBArtificial.Size = new System.Drawing.Size(132, 19);
            this.RBArtificial.TabIndex = 22;
            this.RBArtificial.TabStop = true;
            this.RBArtificial.Text = "Метод иск. базиса";
            this.RBArtificial.UseVisualStyleBackColor = true;
            this.RBArtificial.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // RBSimplexMethod
            // 
            this.RBSimplexMethod.AutoSize = true;
            this.RBSimplexMethod.Location = new System.Drawing.Point(6, 35);
            this.RBSimplexMethod.Name = "RBSimplexMethod";
            this.RBSimplexMethod.Size = new System.Drawing.Size(122, 19);
            this.RBSimplexMethod.TabIndex = 21;
            this.RBSimplexMethod.TabStop = true;
            this.RBSimplexMethod.Text = "Симплекс метод";
            this.RBSimplexMethod.UseVisualStyleBackColor = true;
            this.RBSimplexMethod.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // fractionType
            // 
            this.fractionType.FormattingEnabled = true;
            this.fractionType.Items.AddRange(new object[] {
            "Простые",
            "Десятичные"});
            this.fractionType.Location = new System.Drawing.Point(18, 410);
            this.fractionType.Name = "fractionType";
            this.fractionType.Size = new System.Drawing.Size(154, 23);
            this.fractionType.TabIndex = 20;
            this.fractionType.SelectedIndexChanged += new System.EventHandler(this.fractionType_SelectedIndexChanged);
            // 
            // textBox8
            // 
            this.textBox8.BackColor = System.Drawing.Color.Ivory;
            this.textBox8.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.textBox8.Location = new System.Drawing.Point(18, 391);
            this.textBox8.Name = "textBox8";
            this.textBox8.ReadOnly = true;
            this.textBox8.Size = new System.Drawing.Size(130, 14);
            this.textBox8.TabIndex = 19;
            this.textBox8.Text = "Вид дробей";
            // 
            // optimizationProblem
            // 
            this.optimizationProblem.DisplayMember = "0";
            this.optimizationProblem.FormattingEnabled = true;
            this.optimizationProblem.Items.AddRange(new object[] {
            "Мин",
            "Макс"});
            this.optimizationProblem.Location = new System.Drawing.Point(18, 346);
            this.optimizationProblem.Name = "optimizationProblem";
            this.optimizationProblem.Size = new System.Drawing.Size(154, 23);
            this.optimizationProblem.TabIndex = 18;
            this.optimizationProblem.SelectedIndexChanged += new System.EventHandler(this.optimizationProblem_SelectedIndexChanged);
            // 
            // textBox7
            // 
            this.textBox7.BackColor = System.Drawing.Color.Ivory;
            this.textBox7.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.textBox7.Location = new System.Drawing.Point(18, 327);
            this.textBox7.Name = "textBox7";
            this.textBox7.ReadOnly = true;
            this.textBox7.Size = new System.Drawing.Size(130, 14);
            this.textBox7.TabIndex = 17;
            this.textBox7.Text = "Задача оптимизации";
            // 
            // textBox6
            // 
            this.textBox6.BackColor = System.Drawing.Color.Ivory;
            this.textBox6.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox6.Font = new System.Drawing.Font("Microsoft Sans Serif", 17F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.textBox6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.textBox6.Location = new System.Drawing.Point(769, 9);
            this.textBox6.Name = "textBox6";
            this.textBox6.ReadOnly = true;
            this.textBox6.Size = new System.Drawing.Size(202, 20);
            this.textBox6.TabIndex = 16;
            this.textBox6.Text = "Базисные переменные";
            // 
            // basicVariablesTable
            // 
            this.basicVariablesTable.AllowUserToAddRows = false;
            this.basicVariablesTable.AllowUserToDeleteRows = false;
            this.basicVariablesTable.AllowUserToResizeColumns = false;
            this.basicVariablesTable.AllowUserToResizeRows = false;
            this.basicVariablesTable.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(229)))), ((int)(((byte)(238)))));
            this.basicVariablesTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.basicVariablesTable.Location = new System.Drawing.Point(767, 32);
            this.basicVariablesTable.Name = "basicVariablesTable";
            this.basicVariablesTable.RowTemplate.Height = 30;
            this.basicVariablesTable.Size = new System.Drawing.Size(284, 53);
            this.basicVariablesTable.TabIndex = 15;
            this.basicVariablesTable.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.basicVariablesTable_CellValueChanged);
            // 
            // textBox4
            // 
            this.textBox4.BackColor = System.Drawing.Color.Ivory;
            this.textBox4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 17F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.textBox4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.textBox4.Location = new System.Drawing.Point(197, 6);
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(168, 20);
            this.textBox4.TabIndex = 12;
            this.textBox4.Text = "Целевая функция";
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.Color.Ivory;
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 17F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.textBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.textBox3.Location = new System.Drawing.Point(197, 88);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(130, 20);
            this.textBox3.TabIndex = 11;
            this.textBox3.Text = "Ограничения";
            // 
            // objectiveFunctionTable
            // 
            this.objectiveFunctionTable.AllowUserToAddRows = false;
            this.objectiveFunctionTable.AllowUserToDeleteRows = false;
            this.objectiveFunctionTable.AllowUserToResizeRows = false;
            this.objectiveFunctionTable.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(229)))), ((int)(((byte)(238)))));
            this.objectiveFunctionTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.objectiveFunctionTable.Location = new System.Drawing.Point(195, 32);
            this.objectiveFunctionTable.Name = "objectiveFunctionTable";
            this.objectiveFunctionTable.RowTemplate.Height = 23;
            this.objectiveFunctionTable.Size = new System.Drawing.Size(550, 53);
            this.objectiveFunctionTable.TabIndex = 10;
            this.objectiveFunctionTable.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.objectiveFunctionTable_CellValueChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.Ivory;
            this.tabPage2.Controls.Add(this.SStepBackButton);
            this.tabPage2.Controls.Add(this.AllSStepButton);
            this.tabPage2.Controls.Add(this.textBox9);
            this.tabPage2.Controls.Add(this.textBox5);
            this.tabPage2.Controls.Add(this.panel2);
            this.tabPage2.Controls.Add(this.panel1);
            this.tabPage2.Controls.Add(this.SSTextAnswer);
            this.tabPage2.Controls.Add(this.SStepButton);
            this.tabPage2.Controls.Add(this.SSolutionTable);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1392, 772);
            this.tabPage2.TabIndex = 4;
            this.tabPage2.Text = "Симплекс метод";
            // 
            // SStepBackButton
            // 
            this.SStepBackButton.Enabled = false;
            this.SStepBackButton.Location = new System.Drawing.Point(1002, 45);
            this.SStepBackButton.Name = "SStepBackButton";
            this.SStepBackButton.Size = new System.Drawing.Size(211, 78);
            this.SStepBackButton.TabIndex = 8;
            this.SStepBackButton.Text = "Шаг назад";
            this.SStepBackButton.UseVisualStyleBackColor = true;
            this.SStepBackButton.Click += new System.EventHandler(this.SStepBackButton_Click);
            // 
            // AllSStepButton
            // 
            this.AllSStepButton.Enabled = false;
            this.AllSStepButton.Location = new System.Drawing.Point(1002, 213);
            this.AllSStepButton.Name = "AllSStepButton";
            this.AllSStepButton.Size = new System.Drawing.Size(211, 78);
            this.AllSStepButton.TabIndex = 7;
            this.AllSStepButton.Text = "Решить до конца";
            this.AllSStepButton.UseVisualStyleBackColor = true;
            this.AllSStepButton.Click += new System.EventHandler(this.AllSStepButton_Click);
            // 
            // textBox9
            // 
            this.textBox9.BackColor = System.Drawing.Color.Ivory;
            this.textBox9.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.textBox9.Location = new System.Drawing.Point(565, 515);
            this.textBox9.Name = "textBox9";
            this.textBox9.ReadOnly = true;
            this.textBox9.Size = new System.Drawing.Size(256, 24);
            this.textBox9.TabIndex = 6;
            this.textBox9.Text = "Выбранный опорный элемент";
            // 
            // textBox5
            // 
            this.textBox5.BackColor = System.Drawing.Color.Ivory;
            this.textBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.textBox5.Location = new System.Drawing.Point(565, 471);
            this.textBox5.Name = "textBox5";
            this.textBox5.ReadOnly = true;
            this.textBox5.Size = new System.Drawing.Size(256, 24);
            this.textBox5.TabIndex = 5;
            this.textBox5.Text = "Возможный опорный элемент";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.LightCoral;
            this.panel2.Location = new System.Drawing.Point(533, 515);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(26, 24);
            this.panel2.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Aquamarine;
            this.panel1.Location = new System.Drawing.Point(533, 472);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(26, 23);
            this.panel1.TabIndex = 3;
            // 
            // SSTextAnswer
            // 
            this.SSTextAnswer.BackColor = System.Drawing.Color.OldLace;
            this.SSTextAnswer.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.SSTextAnswer.Location = new System.Drawing.Point(1002, 375);
            this.SSTextAnswer.Multiline = true;
            this.SSTextAnswer.Name = "SSTextAnswer";
            this.SSTextAnswer.ReadOnly = true;
            this.SSTextAnswer.Size = new System.Drawing.Size(260, 60);
            this.SSTextAnswer.TabIndex = 2;
            // 
            // SStepButton
            // 
            this.SStepButton.Enabled = false;
            this.SStepButton.Location = new System.Drawing.Point(1002, 129);
            this.SStepButton.Name = "SStepButton";
            this.SStepButton.Size = new System.Drawing.Size(211, 78);
            this.SStepButton.TabIndex = 1;
            this.SStepButton.Text = "Симплекс шаг";
            this.SStepButton.UseVisualStyleBackColor = true;
            this.SStepButton.Click += new System.EventHandler(this.SStepButton_Click);
            // 
            // SSolutionTable
            // 
            this.SSolutionTable.AllowUserToAddRows = false;
            this.SSolutionTable.AllowUserToDeleteRows = false;
            this.SSolutionTable.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(229)))), ((int)(((byte)(238)))));
            this.SSolutionTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.SSolutionTable.GridColor = System.Drawing.SystemColors.ActiveBorder;
            this.SSolutionTable.Location = new System.Drawing.Point(8, 6);
            this.SSolutionTable.Name = "SSolutionTable";
            this.SSolutionTable.ReadOnly = true;
            this.SSolutionTable.Size = new System.Drawing.Size(965, 429);
            this.SSolutionTable.TabIndex = 0;
            this.SSolutionTable.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.SSolverTable_CellClick);
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.Ivory;
            this.tabPage3.Controls.Add(this.ABStepBackButton);
            this.tabPage3.Controls.Add(this.AllABStepButton);
            this.tabPage3.Controls.Add(this.textBox10);
            this.tabPage3.Controls.Add(this.textBox11);
            this.tabPage3.Controls.Add(this.panel3);
            this.tabPage3.Controls.Add(this.panel4);
            this.tabPage3.Controls.Add(this.ABAnswerText);
            this.tabPage3.Controls.Add(this.ABStepButton);
            this.tabPage3.Controls.Add(this.ABSolverTable);
            this.tabPage3.Location = new System.Drawing.Point(4, 24);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1392, 772);
            this.tabPage3.TabIndex = 5;
            this.tabPage3.Text = "Метод искуственного базиса";
            // 
            // AllABStepButton
            // 
            this.AllABStepButton.Enabled = false;
            this.AllABStepButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.AllABStepButton.Location = new System.Drawing.Point(776, 241);
            this.AllABStepButton.Name = "AllABStepButton";
            this.AllABStepButton.Size = new System.Drawing.Size(211, 78);
            this.AllABStepButton.TabIndex = 14;
            this.AllABStepButton.Text = "Решить до конца";
            this.AllABStepButton.UseVisualStyleBackColor = true;
            this.AllABStepButton.Click += new System.EventHandler(this.AllABStepButton_Click);
            // 
            // textBox10
            // 
            this.textBox10.BackColor = System.Drawing.Color.Ivory;
            this.textBox10.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.textBox10.Location = new System.Drawing.Point(205, 518);
            this.textBox10.Name = "textBox10";
            this.textBox10.ReadOnly = true;
            this.textBox10.Size = new System.Drawing.Size(256, 24);
            this.textBox10.TabIndex = 13;
            this.textBox10.Text = "Выбранный опорный элемент";
            // 
            // textBox11
            // 
            this.textBox11.BackColor = System.Drawing.Color.Ivory;
            this.textBox11.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.textBox11.Location = new System.Drawing.Point(205, 474);
            this.textBox11.Name = "textBox11";
            this.textBox11.ReadOnly = true;
            this.textBox11.Size = new System.Drawing.Size(256, 24);
            this.textBox11.TabIndex = 12;
            this.textBox11.Text = "Возможный опорный элемент";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.LightCoral;
            this.panel3.Location = new System.Drawing.Point(173, 518);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(26, 24);
            this.panel3.TabIndex = 11;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Aquamarine;
            this.panel4.Location = new System.Drawing.Point(173, 475);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(26, 23);
            this.panel4.TabIndex = 10;
            // 
            // ABAnswerText
            // 
            this.ABAnswerText.BackColor = System.Drawing.Color.OldLace;
            this.ABAnswerText.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ABAnswerText.Location = new System.Drawing.Point(776, 375);
            this.ABAnswerText.Multiline = true;
            this.ABAnswerText.Name = "ABAnswerText";
            this.ABAnswerText.ReadOnly = true;
            this.ABAnswerText.Size = new System.Drawing.Size(260, 60);
            this.ABAnswerText.TabIndex = 9;
            // 
            // ABStepButton
            // 
            this.ABStepButton.Enabled = false;
            this.ABStepButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.ABStepButton.Location = new System.Drawing.Point(776, 157);
            this.ABStepButton.Name = "ABStepButton";
            this.ABStepButton.Size = new System.Drawing.Size(211, 78);
            this.ABStepButton.TabIndex = 8;
            this.ABStepButton.Text = "Шаг";
            this.ABStepButton.UseVisualStyleBackColor = true;
            this.ABStepButton.Click += new System.EventHandler(this.ABStepButton_Click);
            // 
            // ABSolverTable
            // 
            this.ABSolverTable.AllowUserToAddRows = false;
            this.ABSolverTable.AllowUserToDeleteRows = false;
            this.ABSolverTable.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(229)))), ((int)(((byte)(238)))));
            this.ABSolverTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ABSolverTable.GridColor = System.Drawing.SystemColors.ActiveBorder;
            this.ABSolverTable.Location = new System.Drawing.Point(8, 6);
            this.ABSolverTable.Name = "ABSolverTable";
            this.ABSolverTable.ReadOnly = true;
            this.ABSolverTable.Size = new System.Drawing.Size(750, 429);
            this.ABSolverTable.TabIndex = 7;
            this.ABSolverTable.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ABSolverTable_CellClick);
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.Color.Ivory;
            this.tabPage4.Controls.Add(this.GAnswerText);
            this.tabPage4.Controls.Add(this.GSSolutionTable);
            this.tabPage4.Location = new System.Drawing.Point(4, 24);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(1392, 772);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Графический метод";
            // 
            // GAnswerText
            // 
            this.GAnswerText.BackColor = System.Drawing.Color.OldLace;
            this.GAnswerText.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.GAnswerText.Location = new System.Drawing.Point(394, 390);
            this.GAnswerText.Multiline = true;
            this.GAnswerText.Name = "GAnswerText";
            this.GAnswerText.ReadOnly = true;
            this.GAnswerText.Size = new System.Drawing.Size(260, 60);
            this.GAnswerText.TabIndex = 12;
            // 
            // GSSolutionTable
            // 
            this.GSSolutionTable.AllowUserToAddRows = false;
            this.GSSolutionTable.AllowUserToDeleteRows = false;
            this.GSSolutionTable.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(229)))), ((int)(((byte)(238)))));
            this.GSSolutionTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GSSolutionTable.GridColor = System.Drawing.SystemColors.ActiveBorder;
            this.GSSolutionTable.Location = new System.Drawing.Point(8, 6);
            this.GSSolutionTable.Name = "GSSolutionTable";
            this.GSSolutionTable.ReadOnly = true;
            this.GSSolutionTable.Size = new System.Drawing.Size(646, 378);
            this.GSSolutionTable.TabIndex = 10;
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Text files (*.txt)|*.txt";
            // 
            // ABStepBackButton
            // 
            this.ABStepBackButton.Enabled = false;
            this.ABStepBackButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.ABStepBackButton.Location = new System.Drawing.Point(776, 73);
            this.ABStepBackButton.Name = "ABStepBackButton";
            this.ABStepBackButton.Size = new System.Drawing.Size(211, 78);
            this.ABStepBackButton.TabIndex = 15;
            this.ABStepBackButton.Text = "Шаг Назад";
            this.ABStepBackButton.UseVisualStyleBackColor = true;
            this.ABStepBackButton.Click += new System.EventHandler(this.ABStepBackButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Ivory;
            this.ClientSize = new System.Drawing.Size(1410, 831);
            this.Controls.Add(this.MainMenu);
            this.Controls.Add(this.tabControl);
            this.MainMenuStrip = this.MainMenu;
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.restrictionTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownColumn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRow)).EndInit();
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBoxMethod.ResumeLayout(false);
            this.groupBoxMethod.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.basicVariablesTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectiveFunctionTable)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSolutionTable)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ABSolverTable)).EndInit();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GSSolutionTable)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView restrictionTable;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.NumericUpDown numericUpDownColumn;
        private System.Windows.Forms.NumericUpDown numericUpDownRow;
        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.DataGridView objectiveFunctionTable;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.DataGridView SSolutionTable;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.DataGridView basicVariablesTable;
        private System.Windows.Forms.ComboBox optimizationProblem;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.ComboBox fractionType;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.GroupBox groupBoxMethod;
        private System.Windows.Forms.RadioButton RBGraphic;
        private System.Windows.Forms.RadioButton RBArtificial;
        private System.Windows.Forms.RadioButton RBSimplexMethod;
        private System.Windows.Forms.Button BeginSolve;
        private System.Windows.Forms.Button SStepButton;
        private System.Windows.Forms.TextBox SSTextAnswer;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textBox10;
        private System.Windows.Forms.TextBox textBox11;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TextBox ABAnswerText;
        private System.Windows.Forms.Button ABStepButton;
        private System.Windows.Forms.DataGridView ABSolverTable;
        private System.Windows.Forms.TextBox GAnswerText;
        private System.Windows.Forms.DataGridView GSSolutionTable;
        private System.Windows.Forms.Button AllSStepButton;
        private System.Windows.Forms.Button AllABStepButton;
        private System.Windows.Forms.Button SStepBackButton;
        private System.Windows.Forms.Button ABStepBackButton;
    }
}

