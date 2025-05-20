namespace Eventor.GUI
{
    partial class LocationCreateForm
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
            tableLayoutPanel1 = new TableLayoutPanel();
            locationDescription_textBox = new TextBox();
            label2 = new Label();
            locationName_textBox = new TextBox();
            label1 = new Label();
            label3 = new Label();
            locationPrice_numericUpDown = new NumericUpDown();
            label4 = new Label();
            locationCapacity_numericUpDown = new NumericUpDown();
            locationSave_button = new Button();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)locationPrice_numericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)locationCapacity_numericUpDown).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = AnchorStyles.None;
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25.40046F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 74.59954F));
            tableLayoutPanel1.Controls.Add(locationDescription_textBox, 1, 1);
            tableLayoutPanel1.Controls.Add(label2, 0, 1);
            tableLayoutPanel1.Controls.Add(locationName_textBox, 1, 0);
            tableLayoutPanel1.Controls.Add(label1, 0, 0);
            tableLayoutPanel1.Controls.Add(label3, 0, 2);
            tableLayoutPanel1.Controls.Add(locationPrice_numericUpDown, 1, 2);
            tableLayoutPanel1.Controls.Add(label4, 0, 3);
            tableLayoutPanel1.Controls.Add(locationCapacity_numericUpDown, 1, 3);
            tableLayoutPanel1.Location = new Point(12, 12);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 4;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 160F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel1.Size = new Size(437, 259);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // locationDescription_textBox
            // 
            locationDescription_textBox.Anchor = AnchorStyles.None;
            locationDescription_textBox.Location = new Point(114, 35);
            locationDescription_textBox.Multiline = true;
            locationDescription_textBox.Name = "locationDescription_textBox";
            locationDescription_textBox.PlaceholderText = "Описание локации";
            locationDescription_textBox.Size = new Size(320, 154);
            locationDescription_textBox.TabIndex = 3;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.None;
            label2.AutoSize = true;
            label2.Location = new Point(14, 102);
            label2.Name = "label2";
            label2.Size = new Size(82, 20);
            label2.TabIndex = 2;
            label2.Text = "Описание:";
            // 
            // locationName_textBox
            // 
            locationName_textBox.Anchor = AnchorStyles.None;
            locationName_textBox.Location = new Point(114, 3);
            locationName_textBox.Name = "locationName_textBox";
            locationName_textBox.PlaceholderText = "Название локации";
            locationName_textBox.Size = new Size(320, 27);
            locationName_textBox.TabIndex = 1;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.None;
            label1.AutoSize = true;
            label1.Location = new Point(15, 6);
            label1.Name = "label1";
            label1.Size = new Size(80, 20);
            label1.TabIndex = 1;
            label1.Text = "Название:";
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.None;
            label3.AutoSize = true;
            label3.Location = new Point(31, 198);
            label3.Name = "label3";
            label3.Size = new Size(48, 20);
            label3.TabIndex = 4;
            label3.Text = "Цена:";
            // 
            // locationPrice_numericUpDown
            // 
            locationPrice_numericUpDown.Anchor = AnchorStyles.None;
            locationPrice_numericUpDown.DecimalPlaces = 2;
            locationPrice_numericUpDown.Location = new Point(114, 195);
            locationPrice_numericUpDown.Maximum = new decimal(new int[] { -727379969, 232, 0, 0 });
            locationPrice_numericUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 131072 });
            locationPrice_numericUpDown.Name = "locationPrice_numericUpDown";
            locationPrice_numericUpDown.Size = new Size(320, 27);
            locationPrice_numericUpDown.TabIndex = 5;
            locationPrice_numericUpDown.Value = new decimal(new int[] { 100, 0, 0, 0 });
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.None;
            label4.AutoSize = true;
            label4.Location = new Point(4, 231);
            label4.Name = "label4";
            label4.Size = new Size(103, 20);
            label4.TabIndex = 6;
            label4.Text = "Вместимость:";
            // 
            // locationCapacity_numericUpDown
            // 
            locationCapacity_numericUpDown.Anchor = AnchorStyles.None;
            locationCapacity_numericUpDown.Location = new Point(114, 228);
            locationCapacity_numericUpDown.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            locationCapacity_numericUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            locationCapacity_numericUpDown.Name = "locationCapacity_numericUpDown";
            locationCapacity_numericUpDown.Size = new Size(320, 27);
            locationCapacity_numericUpDown.TabIndex = 7;
            locationCapacity_numericUpDown.Value = new decimal(new int[] { 100, 0, 0, 0 });
            // 
            // locationSave_button
            // 
            locationSave_button.Location = new Point(12, 277);
            locationSave_button.Name = "locationSave_button";
            locationSave_button.Size = new Size(437, 29);
            locationSave_button.TabIndex = 1;
            locationSave_button.Text = "Создать локацию";
            locationSave_button.UseVisualStyleBackColor = true;
            locationSave_button.Click += locationSave_button_Click;
            // 
            // LocationCreateForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(461, 315);
            Controls.Add(locationSave_button);
            Controls.Add(tableLayoutPanel1);
            MaximizeBox = false;
            Name = "LocationCreateForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Создание локации";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)locationPrice_numericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)locationCapacity_numericUpDown).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Label label1;
        private TextBox locationName_textBox;
        private Label label2;
        private TextBox locationDescription_textBox;
        private Label label3;
        private NumericUpDown locationPrice_numericUpDown;
        private Label label4;
        private NumericUpDown locationCapacity_numericUpDown;
        private Button locationSave_button;
    }
}