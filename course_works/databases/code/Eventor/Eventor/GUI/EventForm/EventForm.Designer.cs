namespace Eventor.GUI
{
    partial class EventForm
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
            eventName_label = new Label();
            eventDescription_textBox = new TextBox();
            eventDays_dataGridView = new DataGridView();
            Column5 = new DataGridViewTextBoxColumn();
            Column1 = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            Column4 = new DataGridViewTextBoxColumn();
            Column3 = new DataGridViewTextBoxColumn();
            eventPersonCount_label = new Label();
            eventDaysCount_label = new Label();
            groupBox1 = new GroupBox();
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            participation_button = new Button();
            feedback_button = new Button();
            _timer = new System.Windows.Forms.Timer(components);
            ((System.ComponentModel.ISupportInitialize)eventDays_dataGridView).BeginInit();
            groupBox1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // eventName_label
            // 
            eventName_label.Anchor = AnchorStyles.None;
            eventName_label.AutoSize = true;
            eventName_label.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 204);
            eventName_label.Location = new Point(424, 7);
            eventName_label.Name = "eventName_label";
            eventName_label.Size = new Size(107, 28);
            eventName_label.TabIndex = 1;
            eventName_label.Text = "Название";
            // 
            // eventDescription_textBox
            // 
            eventDescription_textBox.BackColor = SystemColors.Menu;
            eventDescription_textBox.BorderStyle = BorderStyle.None;
            eventDescription_textBox.Font = new Font("Arial", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 204);
            eventDescription_textBox.Location = new Point(3, 45);
            eventDescription_textBox.Multiline = true;
            eventDescription_textBox.Name = "eventDescription_textBox";
            eventDescription_textBox.PlaceholderText = "Текст описания";
            eventDescription_textBox.Size = new Size(949, 85);
            eventDescription_textBox.TabIndex = 2;
            // 
            // eventDays_dataGridView
            // 
            eventDays_dataGridView.AllowUserToAddRows = false;
            eventDays_dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            eventDays_dataGridView.BackgroundColor = SystemColors.Menu;
            eventDays_dataGridView.BorderStyle = BorderStyle.None;
            eventDays_dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            eventDays_dataGridView.Columns.AddRange(new DataGridViewColumn[] { Column5, Column1, Column2, Column4, Column3 });
            eventDays_dataGridView.Location = new Point(6, 29);
            eventDays_dataGridView.Name = "eventDays_dataGridView";
            eventDays_dataGridView.RowHeadersWidth = 51;
            eventDays_dataGridView.Size = new Size(943, 356);
            eventDays_dataGridView.TabIndex = 3;
            eventDays_dataGridView.CellClick += eventDays_dataGridView_CellClick;
            // 
            // Column5
            // 
            Column5.DataPropertyName = "Id";
            Column5.HeaderText = "Id";
            Column5.MinimumWidth = 6;
            Column5.Name = "Column5";
            Column5.Visible = false;
            // 
            // Column1
            // 
            Column1.DataPropertyName = "SequenceNumber";
            Column1.HeaderText = "Номер";
            Column1.MinimumWidth = 6;
            Column1.Name = "Column1";
            // 
            // Column2
            // 
            Column2.DataPropertyName = "Name";
            Column2.HeaderText = "Название";
            Column2.MinimumWidth = 6;
            Column2.Name = "Column2";
            // 
            // Column4
            // 
            Column4.DataPropertyName = "Price";
            Column4.HeaderText = "Цена";
            Column4.MinimumWidth = 6;
            Column4.Name = "Column4";
            // 
            // Column3
            // 
            Column3.DataPropertyName = "PersonCount";
            Column3.HeaderText = "Количество участников";
            Column3.MinimumWidth = 6;
            Column3.Name = "Column3";
            // 
            // eventPersonCount_label
            // 
            eventPersonCount_label.Anchor = AnchorStyles.None;
            eventPersonCount_label.AutoSize = true;
            eventPersonCount_label.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 204);
            eventPersonCount_label.Location = new Point(248, 7);
            eventPersonCount_label.Name = "eventPersonCount_label";
            eventPersonCount_label.Size = new Size(217, 23);
            eventPersonCount_label.TabIndex = 4;
            eventPersonCount_label.Text = "Количество участников: 0";
            // 
            // eventDaysCount_label
            // 
            eventDaysCount_label.Anchor = AnchorStyles.None;
            eventDaysCount_label.AutoSize = true;
            eventDaysCount_label.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 204);
            eventDaysCount_label.Location = new Point(36, 7);
            eventDaysCount_label.Name = "eventDaysCount_label";
            eventDaysCount_label.Size = new Size(165, 23);
            eventDaysCount_label.TabIndex = 5;
            eventDaysCount_label.Text = "Количество дней: 0";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(eventDays_dataGridView);
            groupBox1.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 204);
            groupBox1.Location = new Point(12, 151);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(955, 391);
            groupBox1.TabIndex = 6;
            groupBox1.TabStop = false;
            groupBox1.Text = "Информация о днях";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(eventName_label, 0, 0);
            tableLayoutPanel1.Controls.Add(eventDescription_textBox, 0, 1);
            tableLayoutPanel1.Location = new Point(12, 12);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 31.5789471F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 68.42105F));
            tableLayoutPanel1.Size = new Size(955, 133);
            tableLayoutPanel1.TabIndex = 7;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.Anchor = AnchorStyles.None;
            tableLayoutPanel2.ColumnCount = 4;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel2.Controls.Add(eventDaysCount_label, 0, 0);
            tableLayoutPanel2.Controls.Add(eventPersonCount_label, 1, 0);
            tableLayoutPanel2.Controls.Add(participation_button, 3, 0);
            tableLayoutPanel2.Controls.Add(feedback_button, 2, 0);
            tableLayoutPanel2.Location = new Point(12, 548);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(955, 37);
            tableLayoutPanel2.TabIndex = 8;
            // 
            // participation_button
            // 
            participation_button.Anchor = AnchorStyles.None;
            participation_button.Location = new Point(717, 4);
            participation_button.Name = "participation_button";
            participation_button.Size = new Size(235, 29);
            participation_button.TabIndex = 6;
            participation_button.Text = "Участвовать/покинуть";
            participation_button.UseVisualStyleBackColor = true;
            participation_button.Click += participation_button_Click;
            // 
            // feedback_button
            // 
            feedback_button.Anchor = AnchorStyles.None;
            feedback_button.Location = new Point(479, 4);
            feedback_button.Name = "feedback_button";
            feedback_button.Size = new Size(232, 29);
            feedback_button.TabIndex = 7;
            feedback_button.Text = "Оставить отзыв";
            feedback_button.UseVisualStyleBackColor = true;
            feedback_button.Click += feedback_button_Click;
            // 
            // _timer
            // 
            _timer.Interval = 15000;
            // 
            // EventForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(978, 589);
            Controls.Add(tableLayoutPanel2);
            Controls.Add(groupBox1);
            Controls.Add(tableLayoutPanel1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "EventForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Информация о мероприятии";
            Load += EventForm_Load;
            ((System.ComponentModel.ISupportInitialize)eventDays_dataGridView).EndInit();
            groupBox1.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private Label eventName_label;
        private TextBox eventDescription_textBox;
        private DataGridView eventDays_dataGridView;
        private Label eventPersonCount_label;
        private Label eventDaysCount_label;
        private GroupBox groupBox1;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private Button participation_button;
        private Button feedback_button;
        private System.Windows.Forms.Timer _timer;
        private DataGridViewTextBoxColumn Column5;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Column4;
        private DataGridViewTextBoxColumn Column3;
    }
}