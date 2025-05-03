namespace Eventor.Eventor.GUI.EventOrganizationForm
{
    partial class EventOrganizationForm
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
            eventInfo_groupBox = new GroupBox();
            tableLayoutPanel1 = new TableLayoutPanel();
            eventName_label = new Label();
            eventDescription_label = new Label();
            eventName_textBox = new TextBox();
            eventDescription_textBox = new TextBox();
            eventLocation_label = new Label();
            eventLocation_comboBox = new ComboBox();
            eventDate_label = new Label();
            eventDate_maskedTextBox = new MaskedTextBox();
            eventDaysCount_label = new Label();
            eventDaysCount_numericUpDown = new NumericUpDown();
            eventPercent_label = new Label();
            eventPercent_numericUpDown = new NumericUpDown();
            eventSettingSave_button = new Button();
            groupBox1 = new GroupBox();
            tableLayoutPanel2 = new TableLayoutPanel();
            label1 = new Label();
            label2 = new Label();
            groupBox2 = new GroupBox();
            eventDays_dataGridView = new DataGridView();
            Column1 = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            Column3 = new DataGridViewTextBoxColumn();
            Column4 = new DataGridViewTextBoxColumn();
            Column5 = new DataGridViewTextBoxColumn();
            eventInfo_groupBox.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)eventDaysCount_numericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)eventPercent_numericUpDown).BeginInit();
            groupBox1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)eventDays_dataGridView).BeginInit();
            SuspendLayout();
            // 
            // eventInfo_groupBox
            // 
            eventInfo_groupBox.Controls.Add(eventSettingSave_button);
            eventInfo_groupBox.Controls.Add(tableLayoutPanel1);
            eventInfo_groupBox.Location = new Point(12, 12);
            eventInfo_groupBox.Name = "eventInfo_groupBox";
            eventInfo_groupBox.Size = new Size(381, 364);
            eventInfo_groupBox.TabIndex = 0;
            eventInfo_groupBox.TabStop = false;
            eventInfo_groupBox.Text = "Настройки";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = AnchorStyles.None;
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28.1842823F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 71.81572F));
            tableLayoutPanel1.Controls.Add(eventPercent_label, 0, 5);
            tableLayoutPanel1.Controls.Add(eventDaysCount_label, 0, 4);
            tableLayoutPanel1.Controls.Add(eventLocation_label, 0, 2);
            tableLayoutPanel1.Controls.Add(eventDescription_textBox, 1, 1);
            tableLayoutPanel1.Controls.Add(eventDescription_label, 0, 1);
            tableLayoutPanel1.Controls.Add(eventName_label, 0, 0);
            tableLayoutPanel1.Controls.Add(eventName_textBox, 1, 0);
            tableLayoutPanel1.Controls.Add(eventLocation_comboBox, 1, 2);
            tableLayoutPanel1.Controls.Add(eventDate_label, 0, 3);
            tableLayoutPanel1.Controls.Add(eventDate_maskedTextBox, 1, 3);
            tableLayoutPanel1.Controls.Add(eventDaysCount_numericUpDown, 1, 4);
            tableLayoutPanel1.Controls.Add(eventPercent_numericUpDown, 1, 5);
            tableLayoutPanel1.Location = new Point(6, 26);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 6;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 24.4755249F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 75.5244751F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            tableLayoutPanel1.Size = new Size(369, 295);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // eventName_label
            // 
            eventName_label.Anchor = AnchorStyles.None;
            eventName_label.AutoSize = true;
            eventName_label.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            eventName_label.Location = new Point(13, 7);
            eventName_label.Name = "eventName_label";
            eventName_label.Size = new Size(77, 20);
            eventName_label.TabIndex = 0;
            eventName_label.Text = "Название";
            // 
            // eventDescription_label
            // 
            eventDescription_label.Anchor = AnchorStyles.None;
            eventDescription_label.AutoSize = true;
            eventDescription_label.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            eventDescription_label.Location = new Point(12, 79);
            eventDescription_label.Name = "eventDescription_label";
            eventDescription_label.Size = new Size(79, 20);
            eventDescription_label.TabIndex = 2;
            eventDescription_label.Text = "Описание";
            // 
            // eventName_textBox
            // 
            eventName_textBox.Anchor = AnchorStyles.None;
            eventName_textBox.Location = new Point(107, 4);
            eventName_textBox.MaxLength = 255;
            eventName_textBox.Name = "eventName_textBox";
            eventName_textBox.PlaceholderText = "Название мероприятия";
            eventName_textBox.Size = new Size(259, 27);
            eventName_textBox.TabIndex = 3;
            // 
            // eventDescription_textBox
            // 
            eventDescription_textBox.Anchor = AnchorStyles.None;
            eventDescription_textBox.Location = new Point(107, 40);
            eventDescription_textBox.Multiline = true;
            eventDescription_textBox.Name = "eventDescription_textBox";
            eventDescription_textBox.PlaceholderText = "Описание мероприятия";
            eventDescription_textBox.Size = new Size(259, 98);
            eventDescription_textBox.TabIndex = 4;
            // 
            // eventLocation_label
            // 
            eventLocation_label.Anchor = AnchorStyles.None;
            eventLocation_label.AutoSize = true;
            eventLocation_label.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            eventLocation_label.Location = new Point(17, 150);
            eventLocation_label.Name = "eventLocation_label";
            eventLocation_label.Size = new Size(69, 20);
            eventLocation_label.TabIndex = 5;
            eventLocation_label.Text = "Локация";
            // 
            // eventLocation_comboBox
            // 
            eventLocation_comboBox.Anchor = AnchorStyles.None;
            eventLocation_comboBox.DisplayMember = "е";
            eventLocation_comboBox.FormattingEnabled = true;
            eventLocation_comboBox.Location = new Point(107, 146);
            eventLocation_comboBox.Name = "eventLocation_comboBox";
            eventLocation_comboBox.Size = new Size(259, 28);
            eventLocation_comboBox.TabIndex = 6;
            // 
            // eventDate_label
            // 
            eventDate_label.Anchor = AnchorStyles.None;
            eventDate_label.AutoSize = true;
            eventDate_label.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            eventDate_label.Location = new Point(31, 183);
            eventDate_label.Name = "eventDate_label";
            eventDate_label.Size = new Size(41, 20);
            eventDate_label.TabIndex = 7;
            eventDate_label.Text = "Дата";
            // 
            // eventDate_maskedTextBox
            // 
            eventDate_maskedTextBox.Anchor = AnchorStyles.None;
            eventDate_maskedTextBox.Location = new Point(107, 180);
            eventDate_maskedTextBox.Mask = "00/00/0000";
            eventDate_maskedTextBox.Name = "eventDate_maskedTextBox";
            eventDate_maskedTextBox.Size = new Size(259, 27);
            eventDate_maskedTextBox.TabIndex = 8;
            eventDate_maskedTextBox.ValidatingType = typeof(DateTime);
            // 
            // eventDaysCount_label
            // 
            eventDaysCount_label.Anchor = AnchorStyles.None;
            eventDaysCount_label.AutoSize = true;
            eventDaysCount_label.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            eventDaysCount_label.Location = new Point(5, 214);
            eventDaysCount_label.Name = "eventDaysCount_label";
            eventDaysCount_label.Size = new Size(94, 40);
            eventDaysCount_label.TabIndex = 9;
            eventDaysCount_label.Text = "Количество дней";
            eventDaysCount_label.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // eventDaysCount_numericUpDown
            // 
            eventDaysCount_numericUpDown.Anchor = AnchorStyles.None;
            eventDaysCount_numericUpDown.Location = new Point(107, 217);
            eventDaysCount_numericUpDown.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            eventDaysCount_numericUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            eventDaysCount_numericUpDown.Name = "eventDaysCount_numericUpDown";
            eventDaysCount_numericUpDown.Size = new Size(259, 27);
            eventDaysCount_numericUpDown.TabIndex = 10;
            eventDaysCount_numericUpDown.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // eventPercent_label
            // 
            eventPercent_label.Anchor = AnchorStyles.None;
            eventPercent_label.AutoSize = true;
            eventPercent_label.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            eventPercent_label.Location = new Point(17, 266);
            eventPercent_label.Name = "eventPercent_label";
            eventPercent_label.Size = new Size(69, 20);
            eventPercent_label.TabIndex = 11;
            eventPercent_label.Text = "Наценка";
            // 
            // eventPercent_numericUpDown
            // 
            eventPercent_numericUpDown.Anchor = AnchorStyles.None;
            eventPercent_numericUpDown.DecimalPlaces = 2;
            eventPercent_numericUpDown.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            eventPercent_numericUpDown.Location = new Point(107, 263);
            eventPercent_numericUpDown.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            eventPercent_numericUpDown.Name = "eventPercent_numericUpDown";
            eventPercent_numericUpDown.Size = new Size(259, 27);
            eventPercent_numericUpDown.TabIndex = 12;
            // 
            // eventSettingSave_button
            // 
            eventSettingSave_button.Location = new Point(6, 328);
            eventSettingSave_button.Name = "eventSettingSave_button";
            eventSettingSave_button.Size = new Size(369, 29);
            eventSettingSave_button.TabIndex = 2;
            eventSettingSave_button.Text = "Сохранить";
            eventSettingSave_button.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(tableLayoutPanel2);
            groupBox1.Location = new Point(12, 382);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(381, 78);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "Сведения";
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 48.5094833F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 51.4905167F));
            tableLayoutPanel2.Controls.Add(label1, 0, 0);
            tableLayoutPanel2.Controls.Add(label2, 1, 0);
            tableLayoutPanel2.Location = new Point(6, 26);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Size = new Size(369, 45);
            tableLayoutPanel2.TabIndex = 2;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.None;
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label1.Location = new Point(31, 2);
            label1.Name = "label1";
            label1.Size = new Size(116, 40);
            label1.TabIndex = 1;
            label1.Text = "Количество участников: 0/0";
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.None;
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label2.Location = new Point(223, 12);
            label2.Name = "label2";
            label2.Size = new Size(101, 20);
            label2.TabIndex = 2;
            label2.Text = "Рейтинг: 0/10";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(eventDays_dataGridView);
            groupBox2.Location = new Point(399, 12);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(425, 448);
            groupBox2.TabIndex = 2;
            groupBox2.TabStop = false;
            groupBox2.Text = "Дни мероприятия";
            // 
            // eventDays_dataGridView
            // 
            eventDays_dataGridView.AllowUserToAddRows = false;
            eventDays_dataGridView.AllowUserToDeleteRows = false;
            eventDays_dataGridView.AllowUserToResizeRows = false;
            eventDays_dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            eventDays_dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllHeaders;
            eventDays_dataGridView.BackgroundColor = SystemColors.Menu;
            eventDays_dataGridView.BorderStyle = BorderStyle.None;
            eventDays_dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            eventDays_dataGridView.Columns.AddRange(new DataGridViewColumn[] { Column1, Column2, Column3, Column4, Column5 });
            eventDays_dataGridView.Location = new Point(6, 26);
            eventDays_dataGridView.Name = "eventDays_dataGridView";
            eventDays_dataGridView.RowHeadersWidth = 10;
            eventDays_dataGridView.Size = new Size(413, 415);
            eventDays_dataGridView.TabIndex = 0;
            // 
            // Column1
            // 
            Column1.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            Column1.HeaderText = "Id";
            Column1.MinimumWidth = 6;
            Column1.Name = "Column1";
            Column1.ReadOnly = true;
            Column1.Visible = false;
            Column1.Width = 51;
            // 
            // Column2
            // 
            Column2.HeaderText = "Порядковый номер";
            Column2.MinimumWidth = 6;
            Column2.Name = "Column2";
            Column2.ReadOnly = true;
            // 
            // Column3
            // 
            Column3.HeaderText = "Название";
            Column3.MinimumWidth = 6;
            Column3.Name = "Column3";
            Column3.ReadOnly = true;
            // 
            // Column4
            // 
            Column4.HeaderText = "Цена";
            Column4.MinimumWidth = 6;
            Column4.Name = "Column4";
            Column4.ReadOnly = true;
            // 
            // Column5
            // 
            Column5.HeaderText = "Количество участников";
            Column5.MinimumWidth = 6;
            Column5.Name = "Column5";
            Column5.ReadOnly = true;
            // 
            // EventOrganizationForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1313, 749);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(eventInfo_groupBox);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "EventOrganizationForm";
            Text = "Организация мероприятия";
            eventInfo_groupBox.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)eventDaysCount_numericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)eventPercent_numericUpDown).EndInit();
            groupBox1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)eventDays_dataGridView).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox eventInfo_groupBox;
        private TableLayoutPanel tableLayoutPanel1;
        private Label eventDescription_label;
        private Label eventName_label;
        private TextBox eventDescription_textBox;
        private TextBox eventName_textBox;
        private Label eventLocation_label;
        private ComboBox eventLocation_comboBox;
        private Label eventDate_label;
        private MaskedTextBox eventDate_maskedTextBox;
        private Label eventDaysCount_label;
        private NumericUpDown eventDaysCount_numericUpDown;
        private Label eventPercent_label;
        private NumericUpDown eventPercent_numericUpDown;
        private Button eventSettingSave_button;
        private GroupBox groupBox1;
        private TableLayoutPanel tableLayoutPanel2;
        private Label label1;
        private Label label2;
        private GroupBox groupBox2;
        private DataGridView eventDays_dataGridView;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Column3;
        private DataGridViewTextBoxColumn Column4;
        private DataGridViewTextBoxColumn Column5;
    }
}