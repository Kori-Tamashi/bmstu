namespace code
{
    partial class Form2
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
            components = new System.ComponentModel.Container();
            pictureBox_editModel = new PictureBox();
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel4 = new TableLayoutPanel();
            comboBox_texture = new ComboBox();
            textBox6 = new TextBox();
            textBox8 = new TextBox();
            comboBox_color = new ComboBox();
            tableLayoutPanel2 = new TableLayoutPanel();
            numericUpDown_height = new NumericUpDown();
            numericUpDown_width = new NumericUpDown();
            textBox7 = new TextBox();
            textBox5 = new TextBox();
            textBox3 = new TextBox();
            textBox1 = new TextBox();
            numericUpDown_length = new NumericUpDown();
            label1 = new Label();
            label2 = new Label();
            toolTip = new ToolTip(components);
            tableLayoutPanel3 = new TableLayoutPanel();
            colorDialog1 = new ColorDialog();
            numericUpDown_angle = new NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)pictureBox_editModel).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_height).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_width).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_length).BeginInit();
            tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_angle).BeginInit();
            SuspendLayout();
            // 
            // pictureBox_editModel
            // 
            pictureBox_editModel.Location = new Point(4, 4);
            pictureBox_editModel.Name = "pictureBox_editModel";
            pictureBox_editModel.Size = new Size(371, 372);
            pictureBox_editModel.TabIndex = 0;
            pictureBox_editModel.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.AutoScroll = true;
            tableLayoutPanel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(tableLayoutPanel4, 0, 3);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 1);
            tableLayoutPanel1.Controls.Add(label1, 0, 0);
            tableLayoutPanel1.Controls.Add(label2, 0, 2);
            tableLayoutPanel1.Location = new Point(382, 4);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 5;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 159F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 81F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 10F));
            tableLayoutPanel1.Size = new Size(372, 372);
            tableLayoutPanel1.TabIndex = 2;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tableLayoutPanel4.ColumnCount = 2;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42.44186F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 57.55814F));
            tableLayoutPanel4.Controls.Add(comboBox_texture, 1, 1);
            tableLayoutPanel4.Controls.Add(textBox6, 0, 1);
            tableLayoutPanel4.Controls.Add(textBox8, 0, 0);
            tableLayoutPanel4.Controls.Add(comboBox_color, 1, 0);
            tableLayoutPanel4.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            tableLayoutPanel4.Location = new Point(4, 210);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 2;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel4.Size = new Size(359, 74);
            tableLayoutPanel4.TabIndex = 4;
            // 
            // comboBox_texture
            // 
            comboBox_texture.FormattingEnabled = true;
            comboBox_texture.ImeMode = ImeMode.Off;
            comboBox_texture.Items.AddRange(new object[] { "<без материала>", "Дерево", "Камень", "Металл" });
            comboBox_texture.Location = new Point(156, 39);
            comboBox_texture.Name = "comboBox_texture";
            comboBox_texture.Size = new Size(199, 28);
            comboBox_texture.TabIndex = 17;
            // 
            // textBox6
            // 
            textBox6.BorderStyle = BorderStyle.None;
            textBox6.Cursor = Cursors.Help;
            textBox6.Location = new Point(4, 39);
            textBox6.Name = "textBox6";
            textBox6.ReadOnly = true;
            textBox6.Size = new Size(139, 20);
            textBox6.TabIndex = 10;
            textBox6.Text = "Материал";
            toolTip.SetToolTip(textBox6, "Задает ширину основания (только для призм)");
            // 
            // textBox8
            // 
            textBox8.BorderStyle = BorderStyle.None;
            textBox8.Cursor = Cursors.Help;
            textBox8.Location = new Point(4, 4);
            textBox8.Name = "textBox8";
            textBox8.ReadOnly = true;
            textBox8.Size = new Size(126, 20);
            textBox8.TabIndex = 0;
            textBox8.Text = "Цвет";
            toolTip.SetToolTip(textBox8, "Задает длину ребра основания");
            // 
            // comboBox_color
            // 
            comboBox_color.FormattingEnabled = true;
            comboBox_color.ImeMode = ImeMode.Off;
            comboBox_color.Items.AddRange(new object[] { "<без цвета>", "выбрать цвет", " " });
            comboBox_color.Location = new Point(156, 4);
            comboBox_color.Name = "comboBox_color";
            comboBox_color.Size = new Size(199, 28);
            comboBox_color.TabIndex = 16;
            comboBox_color.SelectedIndexChanged += comboBox1_SelectedIndexChanged_1;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42.44186F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 57.55814F));
            tableLayoutPanel2.Controls.Add(numericUpDown_angle, 1, 3);
            tableLayoutPanel2.Controls.Add(numericUpDown_height, 1, 2);
            tableLayoutPanel2.Controls.Add(numericUpDown_width, 1, 1);
            tableLayoutPanel2.Controls.Add(textBox7, 0, 3);
            tableLayoutPanel2.Controls.Add(textBox5, 0, 2);
            tableLayoutPanel2.Controls.Add(textBox3, 0, 1);
            tableLayoutPanel2.Controls.Add(textBox1, 0, 0);
            tableLayoutPanel2.Controls.Add(numericUpDown_length, 1, 0);
            tableLayoutPanel2.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            tableLayoutPanel2.Location = new Point(4, 27);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 4;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.Size = new Size(359, 143);
            tableLayoutPanel2.TabIndex = 3;
            // 
            // numericUpDown_height
            // 
            numericUpDown_height.Location = new Point(156, 74);
            numericUpDown_height.Maximum = new decimal(new int[] { 3000, 0, 0, 0 });
            numericUpDown_height.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            numericUpDown_height.Name = "numericUpDown_height";
            numericUpDown_height.Size = new Size(191, 27);
            numericUpDown_height.TabIndex = 16;
            numericUpDown_height.Value = new decimal(new int[] { 10, 0, 0, 0 });
            // 
            // numericUpDown_width
            // 
            numericUpDown_width.Location = new Point(156, 39);
            numericUpDown_width.Maximum = new decimal(new int[] { 3000, 0, 0, 0 });
            numericUpDown_width.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            numericUpDown_width.Name = "numericUpDown_width";
            numericUpDown_width.Size = new Size(191, 27);
            numericUpDown_width.TabIndex = 15;
            numericUpDown_width.Value = new decimal(new int[] { 10, 0, 0, 0 });
            // 
            // textBox7
            // 
            textBox7.BorderStyle = BorderStyle.None;
            textBox7.Cursor = Cursors.Help;
            textBox7.Location = new Point(4, 109);
            textBox7.Name = "textBox7";
            textBox7.ReadOnly = true;
            textBox7.Size = new Size(126, 20);
            textBox7.TabIndex = 12;
            textBox7.Text = "Угол наклона";
            toolTip.SetToolTip(textBox7, "Задает угол между боковым ребром и ребром основания (только для призм)");
            // 
            // textBox5
            // 
            textBox5.BorderStyle = BorderStyle.None;
            textBox5.Cursor = Cursors.Help;
            textBox5.Location = new Point(4, 74);
            textBox5.Name = "textBox5";
            textBox5.ReadOnly = true;
            textBox5.Size = new Size(76, 20);
            textBox5.TabIndex = 11;
            textBox5.Text = "Высота";
            toolTip.SetToolTip(textBox5, "Задает высоту или длину бокового ребра");
            textBox5.TextChanged += textBox5_TextChanged;
            // 
            // textBox3
            // 
            textBox3.BorderStyle = BorderStyle.None;
            textBox3.Cursor = Cursors.Help;
            textBox3.Location = new Point(4, 39);
            textBox3.Name = "textBox3";
            textBox3.ReadOnly = true;
            textBox3.Size = new Size(139, 20);
            textBox3.TabIndex = 10;
            textBox3.Text = "Ширина осн.";
            toolTip.SetToolTip(textBox3, "Задает ширину основания (только для призм)");
            textBox3.TextChanged += textBox3_TextChanged;
            // 
            // textBox1
            // 
            textBox1.BorderStyle = BorderStyle.None;
            textBox1.Cursor = Cursors.Help;
            textBox1.Location = new Point(4, 4);
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(126, 20);
            textBox1.TabIndex = 0;
            textBox1.Text = "Длина ребра осн.";
            toolTip.SetToolTip(textBox1, "Задает длину ребра основания");
            textBox1.TextChanged += textBox1_TextChanged_1;
            // 
            // numericUpDown_length
            // 
            numericUpDown_length.Location = new Point(156, 4);
            numericUpDown_length.Maximum = new decimal(new int[] { 3000, 0, 0, 0 });
            numericUpDown_length.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            numericUpDown_length.Name = "numericUpDown_length";
            numericUpDown_length.Size = new Size(191, 27);
            numericUpDown_length.TabIndex = 14;
            numericUpDown_length.Value = new decimal(new int[] { 10, 0, 0, 0 });
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F);
            label1.ForeColor = SystemColors.MenuHighlight;
            label1.Location = new Point(4, 1);
            label1.Name = "label1";
            label1.Size = new Size(345, 20);
            label1.TabIndex = 0;
            label1.Text = "Размеры ——————————————————";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F);
            label2.ForeColor = SystemColors.MenuHighlight;
            label2.Location = new Point(4, 184);
            label2.Name = "label2";
            label2.Size = new Size(347, 20);
            label2.TabIndex = 4;
            label2.Text = "Внешний вид ————————————————";
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tableLayoutPanel3.ColumnCount = 2;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Controls.Add(tableLayoutPanel1, 1, 0);
            tableLayoutPanel3.Controls.Add(pictureBox_editModel, 0, 0);
            tableLayoutPanel3.Location = new Point(12, 12);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel3.Size = new Size(758, 380);
            tableLayoutPanel3.TabIndex = 2;
            // 
            // numericUpDown_angle
            // 
            numericUpDown_angle.Location = new Point(156, 109);
            numericUpDown_angle.Maximum = new decimal(new int[] { 3000, 0, 0, 0 });
            numericUpDown_angle.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            numericUpDown_angle.Name = "numericUpDown_angle";
            numericUpDown_angle.Size = new Size(191, 27);
            numericUpDown_angle.TabIndex = 17;
            numericUpDown_angle.Value = new decimal(new int[] { 10, 0, 0, 0 });
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(782, 963);
            Controls.Add(tableLayoutPanel3);
            MaximumSize = new Size(1800, 1800);
            MinimumSize = new Size(800, 800);
            Name = "Form2";
            Text = "Редактор объектов";
            Load += Form2_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox_editModel).EndInit();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_height).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_width).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_length).EndInit();
            tableLayoutPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)numericUpDown_angle).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private PictureBox pictureBox_editModel;
        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private TableLayoutPanel tableLayoutPanel1;
        private Label label1;
        private TableLayoutPanel tableLayoutPanel2;
        private TextBox textBox1;
        private TextBox textBox7;
        private TextBox textBox5;
        private TextBox textBox3;
        private NumericUpDown numericUpDown_length;
        private ToolTip toolTip;
        private NumericUpDown numericUpDown_height;
        private NumericUpDown numericUpDown_width;
        private NumericUpDown numericUpDown1;
        private Label label2;
        private TableLayoutPanel tableLayoutPanel3;
        private TableLayoutPanel tableLayoutPanel4;
        private NumericUpDown numericUpDown4;
        private TextBox textBox6;
        private TextBox textBox8;
        private ComboBox comboBox_color;
        private ColorDialog colorDialog1;
        private ComboBox comboBox_texture;
        private NumericUpDown numericUpDown_angle;
    }
}