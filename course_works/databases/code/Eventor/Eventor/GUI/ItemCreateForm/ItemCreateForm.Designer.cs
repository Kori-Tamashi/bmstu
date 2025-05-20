namespace Eventor.GUI
{
    partial class ItemCreateForm
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
            label2 = new Label();
            itemName_textBox = new TextBox();
            label1 = new Label();
            itemPrice_numericUpDown = new NumericUpDown();
            itemCreate_button = new Button();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)itemPrice_numericUpDown).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = AnchorStyles.None;
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 29.18919F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70.8108139F));
            tableLayoutPanel1.Controls.Add(label2, 0, 1);
            tableLayoutPanel1.Controls.Add(itemName_textBox, 1, 0);
            tableLayoutPanel1.Controls.Add(label1, 0, 0);
            tableLayoutPanel1.Controls.Add(itemPrice_numericUpDown, 1, 1);
            tableLayoutPanel1.Location = new Point(12, 12);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel1.Size = new Size(301, 69);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.None;
            label2.AutoSize = true;
            label2.Location = new Point(21, 40);
            label2.Name = "label2";
            label2.Size = new Size(45, 20);
            label2.TabIndex = 2;
            label2.Text = "Цена";
            // 
            // itemName_textBox
            // 
            itemName_textBox.Anchor = AnchorStyles.None;
            itemName_textBox.Location = new Point(90, 3);
            itemName_textBox.Name = "itemName_textBox";
            itemName_textBox.PlaceholderText = "Название предмета";
            itemName_textBox.Size = new Size(208, 27);
            itemName_textBox.TabIndex = 1;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.None;
            label1.AutoSize = true;
            label1.Location = new Point(5, 6);
            label1.Name = "label1";
            label1.Size = new Size(77, 20);
            label1.TabIndex = 1;
            label1.Text = "Название";
            // 
            // itemPrice_numericUpDown
            // 
            itemPrice_numericUpDown.Anchor = AnchorStyles.None;
            itemPrice_numericUpDown.DecimalPlaces = 2;
            itemPrice_numericUpDown.Location = new Point(90, 37);
            itemPrice_numericUpDown.Maximum = new decimal(new int[] { 999999999, 0, 0, 0 });
            itemPrice_numericUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 131072 });
            itemPrice_numericUpDown.Name = "itemPrice_numericUpDown";
            itemPrice_numericUpDown.Size = new Size(208, 27);
            itemPrice_numericUpDown.TabIndex = 3;
            itemPrice_numericUpDown.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // itemCreate_button
            // 
            itemCreate_button.Location = new Point(12, 87);
            itemCreate_button.Name = "itemCreate_button";
            itemCreate_button.Size = new Size(301, 29);
            itemCreate_button.TabIndex = 1;
            itemCreate_button.Text = "Создать предмет";
            itemCreate_button.UseVisualStyleBackColor = true;
            itemCreate_button.Click += itemCreate_button_Click;
            // 
            // ItemCreateForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(325, 121);
            Controls.Add(itemCreate_button);
            Controls.Add(tableLayoutPanel1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "ItemCreateForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Создание предмета";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)itemPrice_numericUpDown).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Label label1;
        private TextBox itemName_textBox;
        private Label label2;
        private NumericUpDown itemPrice_numericUpDown;
        private Button itemCreate_button;
    }
}