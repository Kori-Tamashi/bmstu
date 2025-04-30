namespace Eventor.GUI
{
    partial class DayForm
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
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            label1 = new Label();
            dayName_label = new Label();
            daySequenceNumber_label = new Label();
            dayDescription_textBox = new TextBox();
            tableLayoutPanel3 = new TableLayoutPanel();
            groupBox2 = new GroupBox();
            groupBox1 = new GroupBox();
            dayPersons_dataGridView = new DataGridView();
            Column1 = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            Column3 = new DataGridViewTextBoxColumn();
            _timer = new System.Windows.Forms.Timer(components);
            dayMenu_dataGridView = new DataGridView();
            Column4 = new DataGridViewTextBoxColumn();
            Column5 = new DataGridViewTextBoxColumn();
            Column6 = new DataGridViewTextBoxColumn();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dayPersons_dataGridView).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dayMenu_dataGridView).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 0);
            tableLayoutPanel1.Controls.Add(dayDescription_textBox, 0, 1);
            tableLayoutPanel1.Location = new Point(12, 12);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 45F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 55F));
            tableLayoutPanel1.Size = new Size(955, 169);
            tableLayoutPanel1.TabIndex = 8;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 3;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 58.57843F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 41.42157F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 132F));
            tableLayoutPanel2.Controls.Add(label1, 0, 0);
            tableLayoutPanel2.Controls.Add(dayName_label, 0, 0);
            tableLayoutPanel2.Controls.Add(daySequenceNumber_label, 2, 0);
            tableLayoutPanel2.Location = new Point(3, 3);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Size = new Size(949, 70);
            tableLayoutPanel2.TabIndex = 9;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 204);
            label1.Location = new Point(595, 21);
            label1.Name = "label1";
            label1.Size = new Size(218, 28);
            label1.TabIndex = 3;
            label1.Text = "Порядковый номер: ";
            // 
            // dayName_label
            // 
            dayName_label.Anchor = AnchorStyles.None;
            dayName_label.AutoSize = true;
            dayName_label.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 204);
            dayName_label.Location = new Point(185, 21);
            dayName_label.Name = "dayName_label";
            dayName_label.Size = new Size(107, 28);
            dayName_label.TabIndex = 1;
            dayName_label.Text = "Название";
            // 
            // daySequenceNumber_label
            // 
            daySequenceNumber_label.Anchor = AnchorStyles.Left;
            daySequenceNumber_label.AutoSize = true;
            daySequenceNumber_label.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 204);
            daySequenceNumber_label.Location = new Point(819, 21);
            daySequenceNumber_label.Name = "daySequenceNumber_label";
            daySequenceNumber_label.Size = new Size(36, 28);
            daySequenceNumber_label.TabIndex = 2;
            daySequenceNumber_label.Text = "№";
            // 
            // dayDescription_textBox
            // 
            dayDescription_textBox.BackColor = SystemColors.Menu;
            dayDescription_textBox.BorderStyle = BorderStyle.None;
            dayDescription_textBox.Font = new Font("Arial", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 204);
            dayDescription_textBox.Location = new Point(3, 79);
            dayDescription_textBox.Multiline = true;
            dayDescription_textBox.Name = "dayDescription_textBox";
            dayDescription_textBox.PlaceholderText = "Текст описания";
            dayDescription_textBox.Size = new Size(949, 87);
            dayDescription_textBox.TabIndex = 2;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 2;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Controls.Add(groupBox2, 1, 0);
            tableLayoutPanel3.Controls.Add(groupBox1, 0, 0);
            tableLayoutPanel3.Location = new Point(12, 184);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Size = new Size(955, 457);
            tableLayoutPanel3.TabIndex = 9;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(dayMenu_dataGridView);
            groupBox2.Location = new Point(480, 3);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(471, 451);
            groupBox2.TabIndex = 10;
            groupBox2.TabStop = false;
            groupBox2.Text = "Меню";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(dayPersons_dataGridView);
            groupBox1.Location = new Point(3, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(471, 451);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Участники";
            // 
            // dayPersons_dataGridView
            // 
            dayPersons_dataGridView.AllowUserToAddRows = false;
            dayPersons_dataGridView.AllowUserToResizeRows = false;
            dayPersons_dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dayPersons_dataGridView.BackgroundColor = SystemColors.Menu;
            dayPersons_dataGridView.BorderStyle = BorderStyle.None;
            dayPersons_dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dayPersons_dataGridView.Columns.AddRange(new DataGridViewColumn[] { Column1, Column2, Column3 });
            dayPersons_dataGridView.Location = new Point(6, 26);
            dayPersons_dataGridView.Name = "dayPersons_dataGridView";
            dayPersons_dataGridView.RowHeadersWidth = 51;
            dayPersons_dataGridView.Size = new Size(459, 419);
            dayPersons_dataGridView.TabIndex = 0;
            // 
            // Column1
            // 
            Column1.DataPropertyName = "Id";
            Column1.HeaderText = "Id";
            Column1.MinimumWidth = 6;
            Column1.Name = "Column1";
            Column1.ReadOnly = true;
            Column1.Visible = false;
            // 
            // Column2
            // 
            Column2.DataPropertyName = "Name";
            Column2.HeaderText = "Имя";
            Column2.MinimumWidth = 6;
            Column2.Name = "Column2";
            Column2.ReadOnly = true;
            // 
            // Column3
            // 
            Column3.DataPropertyName = "Paid";
            Column3.HeaderText = "Оплата";
            Column3.MinimumWidth = 6;
            Column3.Name = "Column3";
            Column3.ReadOnly = true;
            // 
            // _timer
            // 
            _timer.Interval = 15000;
            // 
            // dayMenu_dataGridView
            // 
            dayMenu_dataGridView.AllowUserToAddRows = false;
            dayMenu_dataGridView.AllowUserToResizeRows = false;
            dayMenu_dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dayMenu_dataGridView.BackgroundColor = SystemColors.Menu;
            dayMenu_dataGridView.BorderStyle = BorderStyle.None;
            dayMenu_dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dayMenu_dataGridView.Columns.AddRange(new DataGridViewColumn[] { Column4, Column5, Column6 });
            dayMenu_dataGridView.Location = new Point(6, 26);
            dayMenu_dataGridView.Name = "dayMenu_dataGridView";
            dayMenu_dataGridView.RowHeadersWidth = 51;
            dayMenu_dataGridView.Size = new Size(459, 419);
            dayMenu_dataGridView.TabIndex = 0;
            // 
            // Column4
            // 
            Column4.DataPropertyName = "Id";
            Column4.HeaderText = "Id";
            Column4.MinimumWidth = 6;
            Column4.Name = "Column4";
            Column4.ReadOnly = true;
            Column4.Visible = false;
            // 
            // Column5
            // 
            Column5.DataPropertyName = "Name";
            Column5.HeaderText = "Название";
            Column5.MinimumWidth = 6;
            Column5.Name = "Column5";
            Column5.ReadOnly = true;
            // 
            // Column6
            // 
            Column6.DataPropertyName = "Amount";
            Column6.HeaderText = "Количество";
            Column6.MinimumWidth = 6;
            Column6.Name = "Column6";
            Column6.ReadOnly = true;
            // 
            // DayForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(978, 652);
            Controls.Add(tableLayoutPanel3);
            Controls.Add(tableLayoutPanel1);
            Name = "DayForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Информация о дне мероприятия";
            Load += DayForm_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            tableLayoutPanel3.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dayPersons_dataGridView).EndInit();
            ((System.ComponentModel.ISupportInitialize)dayMenu_dataGridView).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Label dayName_label;
        private TextBox dayDescription_textBox;
        private TableLayoutPanel tableLayoutPanel2;
        private Label daySequenceNumber_label;
        private TableLayoutPanel tableLayoutPanel3;
        private GroupBox groupBox2;
        private GroupBox groupBox1;
        private DataGridView dayPersons_dataGridView;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Column3;
        private DataGridView dayMenu_dataGridView;
        private DataGridViewTextBoxColumn Column4;
        private DataGridViewTextBoxColumn Column5;
        private DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.Timer _timer;
        private Label label1;
    }
}