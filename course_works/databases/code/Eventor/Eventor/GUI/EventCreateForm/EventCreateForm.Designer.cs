namespace Eventor.GUI
{
    partial class EventCreateForm
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
            eventPercent_label = new Label();
            eventDaysCount_label = new Label();
            eventLocation_label = new Label();
            eventDescription_textBox = new TextBox();
            eventDescription_label = new Label();
            eventName_label = new Label();
            eventLocation_comboBox = new ComboBox();
            eventDate_label = new Label();
            eventDate_maskedTextBox = new MaskedTextBox();
            eventDaysCount_numericUpDown = new NumericUpDown();
            eventPercent_numericUpDown = new NumericUpDown();
            eventName_textBox = new TextBox();
            eventCreate_button = new Button();
            _toolTip = new ToolTip(components);
            _timer = new System.Windows.Forms.Timer(components);
            statusStrip1 = new StatusStrip();
            dataStatus_toolStripStatusLabel = new ToolStripStatusLabel();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)eventDaysCount_numericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)eventPercent_numericUpDown).BeginInit();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = AnchorStyles.None;
            tableLayoutPanel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.OutsetPartial;
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32.52032F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 67.4796753F));
            tableLayoutPanel1.Controls.Add(eventPercent_label, 0, 5);
            tableLayoutPanel1.Controls.Add(eventDaysCount_label, 0, 4);
            tableLayoutPanel1.Controls.Add(eventLocation_label, 0, 2);
            tableLayoutPanel1.Controls.Add(eventDescription_textBox, 1, 1);
            tableLayoutPanel1.Controls.Add(eventDescription_label, 0, 1);
            tableLayoutPanel1.Controls.Add(eventName_label, 0, 0);
            tableLayoutPanel1.Controls.Add(eventLocation_comboBox, 1, 2);
            tableLayoutPanel1.Controls.Add(eventDate_label, 0, 3);
            tableLayoutPanel1.Controls.Add(eventDate_maskedTextBox, 1, 3);
            tableLayoutPanel1.Controls.Add(eventDaysCount_numericUpDown, 1, 4);
            tableLayoutPanel1.Controls.Add(eventPercent_numericUpDown, 1, 5);
            tableLayoutPanel1.Controls.Add(eventName_textBox, 1, 0);
            tableLayoutPanel1.Location = new Point(12, 12);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 6;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 47F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tableLayoutPanel1.Size = new Size(370, 314);
            tableLayoutPanel1.TabIndex = 2;
            // 
            // eventPercent_label
            // 
            eventPercent_label.Anchor = AnchorStyles.None;
            eventPercent_label.AutoSize = true;
            eventPercent_label.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            eventPercent_label.Location = new Point(25, 279);
            eventPercent_label.Name = "eventPercent_label";
            eventPercent_label.Size = new Size(72, 20);
            eventPercent_label.TabIndex = 11;
            eventPercent_label.Text = "Наценка:";
            // 
            // eventDaysCount_label
            // 
            eventDaysCount_label.Anchor = AnchorStyles.None;
            eventDaysCount_label.AutoSize = true;
            eventDaysCount_label.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            eventDaysCount_label.Location = new Point(16, 220);
            eventDaysCount_label.Name = "eventDaysCount_label";
            eventDaysCount_label.Size = new Size(90, 40);
            eventDaysCount_label.TabIndex = 9;
            eventDaysCount_label.Text = "Количество дней:";
            eventDaysCount_label.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // eventLocation_label
            // 
            eventLocation_label.Anchor = AnchorStyles.None;
            eventLocation_label.AutoSize = true;
            eventLocation_label.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            eventLocation_label.Location = new Point(25, 150);
            eventLocation_label.Name = "eventLocation_label";
            eventLocation_label.Size = new Size(72, 20);
            eventLocation_label.TabIndex = 5;
            eventLocation_label.Text = "Локация:";
            // 
            // eventDescription_textBox
            // 
            eventDescription_textBox.Anchor = AnchorStyles.None;
            eventDescription_textBox.Location = new Point(126, 43);
            eventDescription_textBox.Multiline = true;
            eventDescription_textBox.Name = "eventDescription_textBox";
            eventDescription_textBox.PlaceholderText = "Описание мероприятия";
            eventDescription_textBox.Size = new Size(237, 94);
            eventDescription_textBox.TabIndex = 4;
            // 
            // eventDescription_label
            // 
            eventDescription_label.Anchor = AnchorStyles.None;
            eventDescription_label.AutoSize = true;
            eventDescription_label.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            eventDescription_label.Location = new Point(20, 80);
            eventDescription_label.Name = "eventDescription_label";
            eventDescription_label.Size = new Size(82, 20);
            eventDescription_label.TabIndex = 2;
            eventDescription_label.Text = "Описание:";
            // 
            // eventName_label
            // 
            eventName_label.Anchor = AnchorStyles.None;
            eventName_label.AutoSize = true;
            eventName_label.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            eventName_label.Location = new Point(21, 10);
            eventName_label.Name = "eventName_label";
            eventName_label.Size = new Size(80, 20);
            eventName_label.TabIndex = 0;
            eventName_label.Text = "Название:";
            // 
            // eventLocation_comboBox
            // 
            eventLocation_comboBox.Anchor = AnchorStyles.None;
            eventLocation_comboBox.DisplayMember = "е";
            eventLocation_comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            eventLocation_comboBox.FormattingEnabled = true;
            eventLocation_comboBox.Location = new Point(126, 146);
            eventLocation_comboBox.Name = "eventLocation_comboBox";
            eventLocation_comboBox.Size = new Size(237, 28);
            eventLocation_comboBox.TabIndex = 6;
            eventLocation_comboBox.SelectedIndexChanged += eventLocation_comboBox_SelectedIndexChanged;
            // 
            // eventDate_label
            // 
            eventDate_label.Anchor = AnchorStyles.None;
            eventDate_label.AutoSize = true;
            eventDate_label.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            eventDate_label.Location = new Point(39, 187);
            eventDate_label.Name = "eventDate_label";
            eventDate_label.Size = new Size(44, 20);
            eventDate_label.TabIndex = 7;
            eventDate_label.Text = "Дата:";
            // 
            // eventDate_maskedTextBox
            // 
            eventDate_maskedTextBox.Anchor = AnchorStyles.None;
            eventDate_maskedTextBox.Location = new Point(126, 183);
            eventDate_maskedTextBox.Mask = "00/00/0000";
            eventDate_maskedTextBox.Name = "eventDate_maskedTextBox";
            eventDate_maskedTextBox.Size = new Size(237, 27);
            eventDate_maskedTextBox.TabIndex = 8;
            eventDate_maskedTextBox.ValidatingType = typeof(DateTime);
            // 
            // eventDaysCount_numericUpDown
            // 
            eventDaysCount_numericUpDown.Anchor = AnchorStyles.None;
            eventDaysCount_numericUpDown.Location = new Point(126, 227);
            eventDaysCount_numericUpDown.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            eventDaysCount_numericUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            eventDaysCount_numericUpDown.Name = "eventDaysCount_numericUpDown";
            eventDaysCount_numericUpDown.Size = new Size(237, 27);
            eventDaysCount_numericUpDown.TabIndex = 10;
            eventDaysCount_numericUpDown.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // eventPercent_numericUpDown
            // 
            eventPercent_numericUpDown.Anchor = AnchorStyles.None;
            eventPercent_numericUpDown.DecimalPlaces = 2;
            eventPercent_numericUpDown.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            eventPercent_numericUpDown.Location = new Point(126, 275);
            eventPercent_numericUpDown.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            eventPercent_numericUpDown.Name = "eventPercent_numericUpDown";
            eventPercent_numericUpDown.Size = new Size(237, 27);
            eventPercent_numericUpDown.TabIndex = 12;
            // 
            // eventName_textBox
            // 
            eventName_textBox.Anchor = AnchorStyles.None;
            eventName_textBox.Location = new Point(126, 6);
            eventName_textBox.MaxLength = 255;
            eventName_textBox.Name = "eventName_textBox";
            eventName_textBox.PlaceholderText = "Название мероприятия";
            eventName_textBox.Size = new Size(237, 27);
            eventName_textBox.TabIndex = 3;
            // 
            // eventCreate_button
            // 
            eventCreate_button.Location = new Point(12, 338);
            eventCreate_button.Name = "eventCreate_button";
            eventCreate_button.Size = new Size(370, 29);
            eventCreate_button.TabIndex = 3;
            eventCreate_button.Text = "Создать мероприятие";
            eventCreate_button.UseVisualStyleBackColor = true;
            eventCreate_button.Click += eventCreate_button_Click;
            // 
            // _timer
            // 
            _timer.Interval = 5500;
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { dataStatus_toolStripStatusLabel });
            statusStrip1.Location = new Point(0, 381);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(394, 26);
            statusStrip1.TabIndex = 4;
            statusStrip1.Text = "statusStrip1";
            // 
            // dataStatus_toolStripStatusLabel
            // 
            dataStatus_toolStripStatusLabel.Name = "dataStatus_toolStripStatusLabel";
            dataStatus_toolStripStatusLabel.Size = new Size(151, 20);
            dataStatus_toolStripStatusLabel.Text = "toolStripStatusLabel1";
            // 
            // EventCreateForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(394, 407);
            Controls.Add(statusStrip1);
            Controls.Add(eventCreate_button);
            Controls.Add(tableLayoutPanel1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "EventCreateForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Создание мероприятия";
            Load += EventCreateForm_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)eventDaysCount_numericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)eventPercent_numericUpDown).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Label eventPercent_label;
        private Label eventDaysCount_label;
        private Label eventLocation_label;
        private TextBox eventDescription_textBox;
        private Label eventDescription_label;
        private Label eventName_label;
        private ComboBox eventLocation_comboBox;
        private Label eventDate_label;
        private MaskedTextBox eventDate_maskedTextBox;
        private NumericUpDown eventDaysCount_numericUpDown;
        private NumericUpDown eventPercent_numericUpDown;
        private TextBox eventName_textBox;
        private Button eventCreate_button;
        private ToolTip _toolTip;
        private System.Windows.Forms.Timer _timer;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel dataStatus_toolStripStatusLabel;
    }
}