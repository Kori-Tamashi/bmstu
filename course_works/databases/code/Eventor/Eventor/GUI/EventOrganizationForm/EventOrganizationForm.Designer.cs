namespace Eventor.GUI
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
            components = new System.ComponentModel.Container();
            eventInfo_groupBox = new GroupBox();
            eventSettingSave_button = new Button();
            tableLayoutPanel1 = new TableLayoutPanel();
            eventPercent_label = new Label();
            eventDaysCount_label = new Label();
            eventLocation_label = new Label();
            eventDescription_textBox = new TextBox();
            eventDescription_label = new Label();
            eventName_label = new Label();
            eventName_textBox = new TextBox();
            eventLocation_comboBox = new ComboBox();
            eventDate_label = new Label();
            eventDate_maskedTextBox = new MaskedTextBox();
            eventDaysCount_numericUpDown = new NumericUpDown();
            eventPercent_numericUpDown = new NumericUpDown();
            eventParticipantBudget_numericUpDown = new NumericUpDown();
            label4 = new Label();
            tableLayoutPanel2 = new TableLayoutPanel();
            eventDaysBeforeValue_label = new Label();
            eventDaysBefore_label = new Label();
            eventNValue_label = new Label();
            eventN_label = new Label();
            eventSolutionExistsValue_label = new Label();
            eventRating_label = new Label();
            eventParticipants_label = new Label();
            eventParticipantsValue_label = new Label();
            eventRatingValue_label = new Label();
            eventCost_label = new Label();
            eventCostValue_label = new Label();
            eventAverageDayCost_label = new Label();
            eventAverageDayCostValue_label = new Label();
            eventSolutionExists_label = new Label();
            groupBox2 = new GroupBox();
            eventDays_dataGridView = new DataGridView();
            Column1 = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            Column3 = new DataGridViewTextBoxColumn();
            Column4 = new DataGridViewTextBoxColumn();
            Column5 = new DataGridViewTextBoxColumn();
            groupBox3 = new GroupBox();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            tabPage2 = new TabPage();
            tableLayoutPanel4 = new TableLayoutPanel();
            eventPriceValue_label = new Label();
            label2 = new Label();
            eventFundamentalPriceWithPrivilegesValue_label = new Label();
            eventFundamentalPriceValue_label = new Label();
            eventFundamentalPriceWithPrivileges_label = new Label();
            eventFundamentalPrice_label = new Label();
            eventlPrice_label = new Label();
            label1 = new Label();
            eventAveragePrice_label = new Label();
            eventAveragePriceWithPrivilegesValue_label = new Label();
            eventAveragePriceValue_label = new Label();
            label5 = new Label();
            eventFundamentalPriceRelativeDifferenceValue_label = new Label();
            eventPriceWithPrivilegesValue_label = new Label();
            tabPage3 = new TabPage();
            tableLayoutPanel3 = new TableLayoutPanel();
            eventExpensesValue_label = new Label();
            label6 = new Label();
            label = new Label();
            label3 = new Label();
            eventTheoryProfit_label = new Label();
            eventRealProfit_label = new Label();
            eventIncome_label = new Label();
            eventMinParticipantsCountValue_label = new Label();
            eventMaxPercent_label = new Label();
            eventTheoryProfitValue_label = new Label();
            eventRealProfitValue_label = new Label();
            eventIncomeValue_label = new Label();
            eventLocationId_label = new Label();
            _timer = new System.Windows.Forms.Timer(components);
            statusStrip1 = new StatusStrip();
            dataStatus_toolStripStatusLabel = new ToolStripStatusLabel();
            _toolTip = new ToolTip(components);
            tableLayoutPanel5 = new TableLayoutPanel();
            eventFeedbacks_button = new Button();
            feedback_button = new Button();
            participation_button = new Button();
            eventInfo_groupBox.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)eventDaysCount_numericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)eventPercent_numericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)eventParticipantBudget_numericUpDown).BeginInit();
            tableLayoutPanel2.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)eventDays_dataGridView).BeginInit();
            groupBox3.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            tabPage3.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            statusStrip1.SuspendLayout();
            tableLayoutPanel5.SuspendLayout();
            SuspendLayout();
            // 
            // eventInfo_groupBox
            // 
            eventInfo_groupBox.Controls.Add(eventSettingSave_button);
            eventInfo_groupBox.Controls.Add(tableLayoutPanel1);
            eventInfo_groupBox.Location = new Point(12, 12);
            eventInfo_groupBox.Name = "eventInfo_groupBox";
            eventInfo_groupBox.Size = new Size(381, 437);
            eventInfo_groupBox.TabIndex = 0;
            eventInfo_groupBox.TabStop = false;
            eventInfo_groupBox.Text = "Настройки";
            // 
            // eventSettingSave_button
            // 
            eventSettingSave_button.Location = new Point(6, 402);
            eventSettingSave_button.Name = "eventSettingSave_button";
            eventSettingSave_button.Size = new Size(369, 29);
            eventSettingSave_button.TabIndex = 2;
            eventSettingSave_button.Text = "Сохранить";
            eventSettingSave_button.UseVisualStyleBackColor = true;
            eventSettingSave_button.Click += eventSettingSave_button_Click;
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
            tableLayoutPanel1.Controls.Add(eventName_textBox, 1, 0);
            tableLayoutPanel1.Controls.Add(eventLocation_comboBox, 1, 2);
            tableLayoutPanel1.Controls.Add(eventDate_label, 0, 3);
            tableLayoutPanel1.Controls.Add(eventDate_maskedTextBox, 1, 3);
            tableLayoutPanel1.Controls.Add(eventDaysCount_numericUpDown, 1, 4);
            tableLayoutPanel1.Controls.Add(eventPercent_numericUpDown, 1, 5);
            tableLayoutPanel1.Controls.Add(eventParticipantBudget_numericUpDown, 1, 6);
            tableLayoutPanel1.Controls.Add(label4, 0, 6);
            tableLayoutPanel1.Location = new Point(6, 26);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 7;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 47F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            tableLayoutPanel1.Size = new Size(369, 370);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // eventPercent_label
            // 
            eventPercent_label.Anchor = AnchorStyles.None;
            eventPercent_label.AutoSize = true;
            eventPercent_label.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            eventPercent_label.Location = new Point(25, 274);
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
            eventPercent_numericUpDown.Location = new Point(126, 270);
            eventPercent_numericUpDown.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            eventPercent_numericUpDown.Name = "eventPercent_numericUpDown";
            eventPercent_numericUpDown.Size = new Size(237, 27);
            eventPercent_numericUpDown.TabIndex = 12;
            // 
            // eventParticipantBudget_numericUpDown
            // 
            eventParticipantBudget_numericUpDown.Anchor = AnchorStyles.None;
            eventParticipantBudget_numericUpDown.DecimalPlaces = 2;
            eventParticipantBudget_numericUpDown.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            eventParticipantBudget_numericUpDown.Location = new Point(126, 322);
            eventParticipantBudget_numericUpDown.Maximum = new decimal(new int[] { 999999999, 0, 0, 0 });
            eventParticipantBudget_numericUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            eventParticipantBudget_numericUpDown.Name = "eventParticipantBudget_numericUpDown";
            eventParticipantBudget_numericUpDown.Size = new Size(237, 27);
            eventParticipantBudget_numericUpDown.TabIndex = 14;
            eventParticipantBudget_numericUpDown.Value = new decimal(new int[] { 30000, 0, 0, 0 });
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.None;
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label4.Location = new Point(21, 315);
            label4.Name = "label4";
            label4.Size = new Size(81, 40);
            label4.TabIndex = 13;
            label4.Text = "Бюджет участника:";
            label4.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.Anchor = AnchorStyles.None;
            tableLayoutPanel2.CellBorderStyle = TableLayoutPanelCellBorderStyle.OutsetPartial;
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 54F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 46F));
            tableLayoutPanel2.Controls.Add(eventDaysBeforeValue_label, 1, 6);
            tableLayoutPanel2.Controls.Add(eventDaysBefore_label, 0, 6);
            tableLayoutPanel2.Controls.Add(eventNValue_label, 1, 5);
            tableLayoutPanel2.Controls.Add(eventN_label, 0, 5);
            tableLayoutPanel2.Controls.Add(eventSolutionExistsValue_label, 1, 4);
            tableLayoutPanel2.Controls.Add(eventRating_label, 0, 1);
            tableLayoutPanel2.Controls.Add(eventParticipants_label, 0, 0);
            tableLayoutPanel2.Controls.Add(eventParticipantsValue_label, 1, 0);
            tableLayoutPanel2.Controls.Add(eventRatingValue_label, 1, 1);
            tableLayoutPanel2.Controls.Add(eventCost_label, 0, 2);
            tableLayoutPanel2.Controls.Add(eventCostValue_label, 1, 2);
            tableLayoutPanel2.Controls.Add(eventAverageDayCost_label, 0, 3);
            tableLayoutPanel2.Controls.Add(eventAverageDayCostValue_label, 1, 3);
            tableLayoutPanel2.Controls.Add(eventSolutionExists_label, 0, 4);
            tableLayoutPanel2.Location = new Point(6, 15);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 7;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.Size = new Size(402, 250);
            tableLayoutPanel2.TabIndex = 2;
            // 
            // eventDaysBeforeValue_label
            // 
            eventDaysBeforeValue_label.Anchor = AnchorStyles.None;
            eventDaysBeforeValue_label.AutoSize = true;
            eventDaysBeforeValue_label.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            eventDaysBeforeValue_label.Location = new Point(300, 214);
            eventDaysBeforeValue_label.Name = "eventDaysBeforeValue_label";
            eventDaysBeforeValue_label.Size = new Size(17, 20);
            eventDaysBeforeValue_label.TabIndex = 13;
            eventDaysBeforeValue_label.Text = "0";
            // 
            // eventDaysBefore_label
            // 
            eventDaysBefore_label.Anchor = AnchorStyles.None;
            eventDaysBefore_label.AutoSize = true;
            eventDaysBefore_label.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            eventDaysBefore_label.Location = new Point(15, 214);
            eventDaysBefore_label.Name = "eventDaysBefore_label";
            eventDaysBefore_label.Size = new Size(188, 20);
            eventDaysBefore_label.TabIndex = 12;
            eventDaysBefore_label.Text = "Осталось дней до начала:";
            // 
            // eventNValue_label
            // 
            eventNValue_label.Anchor = AnchorStyles.None;
            eventNValue_label.AutoSize = true;
            eventNValue_label.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            eventNValue_label.Location = new Point(300, 173);
            eventNValue_label.Name = "eventNValue_label";
            eventNValue_label.Size = new Size(17, 20);
            eventNValue_label.TabIndex = 11;
            eventNValue_label.Text = "0";
            // 
            // eventN_label
            // 
            eventN_label.Anchor = AnchorStyles.None;
            eventN_label.AutoSize = true;
            eventN_label.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            eventN_label.Location = new Point(11, 173);
            eventN_label.Name = "eventN_label";
            eventN_label.Size = new Size(195, 20);
            eventN_label.TabIndex = 4;
            eventN_label.Text = "N-мерность мероприятия:";
            // 
            // eventSolutionExistsValue_label
            // 
            eventSolutionExistsValue_label.Anchor = AnchorStyles.None;
            eventSolutionExistsValue_label.AutoSize = true;
            eventSolutionExistsValue_label.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            eventSolutionExistsValue_label.Location = new Point(235, 140);
            eventSolutionExistsValue_label.Name = "eventSolutionExistsValue_label";
            eventSolutionExistsValue_label.Size = new Size(147, 20);
            eventSolutionExistsValue_label.TabIndex = 10;
            eventSolutionExistsValue_label.Text = "Расчет невозможен";
            // 
            // eventRating_label
            // 
            eventRating_label.Anchor = AnchorStyles.None;
            eventRating_label.AutoSize = true;
            eventRating_label.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            eventRating_label.Location = new Point(75, 41);
            eventRating_label.Name = "eventRating_label";
            eventRating_label.Size = new Size(67, 20);
            eventRating_label.TabIndex = 3;
            eventRating_label.Text = "Рейтинг:";
            // 
            // eventParticipants_label
            // 
            eventParticipants_label.Anchor = AnchorStyles.None;
            eventParticipants_label.AutoSize = true;
            eventParticipants_label.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            eventParticipants_label.Location = new Point(21, 8);
            eventParticipants_label.Name = "eventParticipants_label";
            eventParticipants_label.Size = new Size(175, 20);
            eventParticipants_label.TabIndex = 1;
            eventParticipants_label.Text = "Количество участников:";
            // 
            // eventParticipantsValue_label
            // 
            eventParticipantsValue_label.Anchor = AnchorStyles.None;
            eventParticipantsValue_label.AutoSize = true;
            eventParticipantsValue_label.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            eventParticipantsValue_label.Location = new Point(300, 8);
            eventParticipantsValue_label.Name = "eventParticipantsValue_label";
            eventParticipantsValue_label.Size = new Size(17, 20);
            eventParticipantsValue_label.TabIndex = 4;
            eventParticipantsValue_label.Text = "0";
            // 
            // eventRatingValue_label
            // 
            eventRatingValue_label.Anchor = AnchorStyles.None;
            eventRatingValue_label.AutoSize = true;
            eventRatingValue_label.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            eventRatingValue_label.Location = new Point(289, 41);
            eventRatingValue_label.Name = "eventRatingValue_label";
            eventRatingValue_label.Size = new Size(39, 20);
            eventRatingValue_label.TabIndex = 5;
            eventRatingValue_label.Text = "0/10";
            // 
            // eventCost_label
            // 
            eventCost_label.Anchor = AnchorStyles.None;
            eventCost_label.AutoSize = true;
            eventCost_label.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            eventCost_label.Location = new Point(16, 74);
            eventCost_label.Name = "eventCost_label";
            eventCost_label.Size = new Size(185, 20);
            eventCost_label.TabIndex = 6;
            eventCost_label.Text = "Стоимость мероприятия:";
            // 
            // eventCostValue_label
            // 
            eventCostValue_label.Anchor = AnchorStyles.None;
            eventCostValue_label.AutoSize = true;
            eventCostValue_label.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            eventCostValue_label.Location = new Point(300, 74);
            eventCostValue_label.Name = "eventCostValue_label";
            eventCostValue_label.Size = new Size(17, 20);
            eventCostValue_label.TabIndex = 7;
            eventCostValue_label.Text = "0";
            // 
            // eventAverageDayCost_label
            // 
            eventAverageDayCost_label.Anchor = AnchorStyles.None;
            eventAverageDayCost_label.AutoSize = true;
            eventAverageDayCost_label.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            eventAverageDayCost_label.Location = new Point(21, 107);
            eventAverageDayCost_label.Name = "eventAverageDayCost_label";
            eventAverageDayCost_label.Size = new Size(176, 20);
            eventAverageDayCost_label.TabIndex = 8;
            eventAverageDayCost_label.Text = "Средняя стоимость дня:";
            // 
            // eventAverageDayCostValue_label
            // 
            eventAverageDayCostValue_label.Anchor = AnchorStyles.None;
            eventAverageDayCostValue_label.AutoSize = true;
            eventAverageDayCostValue_label.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            eventAverageDayCostValue_label.Location = new Point(300, 107);
            eventAverageDayCostValue_label.Name = "eventAverageDayCostValue_label";
            eventAverageDayCostValue_label.Size = new Size(17, 20);
            eventAverageDayCostValue_label.TabIndex = 9;
            eventAverageDayCostValue_label.Text = "0";
            // 
            // eventSolutionExists_label
            // 
            eventSolutionExists_label.Anchor = AnchorStyles.None;
            eventSolutionExists_label.AutoSize = true;
            eventSolutionExists_label.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            eventSolutionExists_label.Location = new Point(6, 140);
            eventSolutionExists_label.Name = "eventSolutionExists_label";
            eventSolutionExists_label.Size = new Size(206, 20);
            eventSolutionExists_label.TabIndex = 3;
            eventSolutionExists_label.Text = "Возможность расчета цены:";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(eventDays_dataGridView);
            groupBox2.Location = new Point(399, 12);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(462, 437);
            groupBox2.TabIndex = 2;
            groupBox2.TabStop = false;
            groupBox2.Text = "Дни мероприятия";
            // 
            // eventDays_dataGridView
            // 
            eventDays_dataGridView.AllowUserToAddRows = false;
            eventDays_dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            eventDays_dataGridView.BackgroundColor = SystemColors.Menu;
            eventDays_dataGridView.BorderStyle = BorderStyle.None;
            eventDays_dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            eventDays_dataGridView.Columns.AddRange(new DataGridViewColumn[] { Column1, Column2, Column3, Column4, Column5 });
            eventDays_dataGridView.Location = new Point(6, 26);
            eventDays_dataGridView.Name = "eventDays_dataGridView";
            eventDays_dataGridView.RowHeadersWidth = 10;
            eventDays_dataGridView.Size = new Size(450, 405);
            eventDays_dataGridView.TabIndex = 0;
            eventDays_dataGridView.CellClick += eventDays_dataGridView_CellClick;
            // 
            // Column1
            // 
            Column1.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            Column1.DataPropertyName = "Id";
            Column1.HeaderText = "Id";
            Column1.MinimumWidth = 6;
            Column1.Name = "Column1";
            Column1.ReadOnly = true;
            Column1.Visible = false;
            Column1.Width = 125;
            // 
            // Column2
            // 
            Column2.DataPropertyName = "SequenceNumber";
            Column2.HeaderText = "Порядковый номер";
            Column2.MinimumWidth = 6;
            Column2.Name = "Column2";
            Column2.ReadOnly = true;
            // 
            // Column3
            // 
            Column3.DataPropertyName = "Name";
            Column3.HeaderText = "Название";
            Column3.MinimumWidth = 6;
            Column3.Name = "Column3";
            Column3.ReadOnly = true;
            // 
            // Column4
            // 
            Column4.DataPropertyName = "Price";
            Column4.HeaderText = "Цена";
            Column4.MinimumWidth = 6;
            Column4.Name = "Column4";
            Column4.ReadOnly = true;
            // 
            // Column5
            // 
            Column5.DataPropertyName = "PersonCount";
            Column5.HeaderText = "Количество участников";
            Column5.MinimumWidth = 6;
            Column5.Name = "Column5";
            Column5.ReadOnly = true;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(tabControl1);
            groupBox3.Location = new Point(867, 12);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(434, 437);
            groupBox3.TabIndex = 3;
            groupBox3.TabStop = false;
            groupBox3.Text = "Анализ мероприятия";
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Location = new Point(6, 26);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(422, 405);
            tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(tableLayoutPanel2);
            tabPage1.Location = new Point(4, 29);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(414, 372);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Общая информация";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(tableLayoutPanel4);
            tabPage2.Location = new Point(4, 29);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(414, 372);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Ценообразование";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.CellBorderStyle = TableLayoutPanelCellBorderStyle.OutsetPartial;
            tableLayoutPanel4.ColumnCount = 2;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 54F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 46F));
            tableLayoutPanel4.Controls.Add(eventPriceValue_label, 1, 5);
            tableLayoutPanel4.Controls.Add(label2, 0, 6);
            tableLayoutPanel4.Controls.Add(eventFundamentalPriceWithPrivilegesValue_label, 1, 1);
            tableLayoutPanel4.Controls.Add(eventFundamentalPriceValue_label, 1, 0);
            tableLayoutPanel4.Controls.Add(eventFundamentalPriceWithPrivileges_label, 0, 1);
            tableLayoutPanel4.Controls.Add(eventFundamentalPrice_label, 0, 0);
            tableLayoutPanel4.Controls.Add(eventlPrice_label, 0, 5);
            tableLayoutPanel4.Controls.Add(label1, 0, 4);
            tableLayoutPanel4.Controls.Add(eventAveragePrice_label, 0, 3);
            tableLayoutPanel4.Controls.Add(eventAveragePriceWithPrivilegesValue_label, 1, 4);
            tableLayoutPanel4.Controls.Add(eventAveragePriceValue_label, 1, 3);
            tableLayoutPanel4.Controls.Add(label5, 0, 2);
            tableLayoutPanel4.Controls.Add(eventFundamentalPriceRelativeDifferenceValue_label, 1, 2);
            tableLayoutPanel4.Controls.Add(eventPriceWithPrivilegesValue_label, 1, 6);
            tableLayoutPanel4.Location = new Point(6, 7);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 7;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            tableLayoutPanel4.Size = new Size(402, 298);
            tableLayoutPanel4.TabIndex = 0;
            // 
            // eventPriceValue_label
            // 
            eventPriceValue_label.Anchor = AnchorStyles.None;
            eventPriceValue_label.AutoSize = true;
            eventPriceValue_label.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            eventPriceValue_label.Location = new Point(300, 219);
            eventPriceValue_label.Name = "eventPriceValue_label";
            eventPriceValue_label.Size = new Size(17, 20);
            eventPriceValue_label.TabIndex = 17;
            eventPriceValue_label.Text = "0\r\n";
            eventPriceValue_label.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.None;
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label2.Location = new Point(37, 251);
            label2.Name = "label2";
            label2.Size = new Size(144, 40);
            label2.TabIndex = 18;
            label2.Text = "Цена мероприятия\r\n(с привилегиями):";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // eventFundamentalPriceWithPrivilegesValue_label
            // 
            eventFundamentalPriceWithPrivilegesValue_label.Anchor = AnchorStyles.None;
            eventFundamentalPriceWithPrivilegesValue_label.AutoSize = true;
            eventFundamentalPriceWithPrivilegesValue_label.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            eventFundamentalPriceWithPrivilegesValue_label.Location = new Point(300, 48);
            eventFundamentalPriceWithPrivilegesValue_label.Name = "eventFundamentalPriceWithPrivilegesValue_label";
            eventFundamentalPriceWithPrivilegesValue_label.Size = new Size(17, 20);
            eventFundamentalPriceWithPrivilegesValue_label.TabIndex = 8;
            eventFundamentalPriceWithPrivilegesValue_label.Text = "0";
            // 
            // eventFundamentalPriceValue_label
            // 
            eventFundamentalPriceValue_label.Anchor = AnchorStyles.None;
            eventFundamentalPriceValue_label.AutoSize = true;
            eventFundamentalPriceValue_label.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            eventFundamentalPriceValue_label.Location = new Point(300, 8);
            eventFundamentalPriceValue_label.Name = "eventFundamentalPriceValue_label";
            eventFundamentalPriceValue_label.Size = new Size(17, 20);
            eventFundamentalPriceValue_label.TabIndex = 7;
            eventFundamentalPriceValue_label.Text = "0";
            // 
            // eventFundamentalPriceWithPrivileges_label
            // 
            eventFundamentalPriceWithPrivileges_label.Anchor = AnchorStyles.None;
            eventFundamentalPriceWithPrivileges_label.AutoSize = true;
            eventFundamentalPriceWithPrivileges_label.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            eventFundamentalPriceWithPrivileges_label.Location = new Point(22, 38);
            eventFundamentalPriceWithPrivileges_label.Name = "eventFundamentalPriceWithPrivileges_label";
            eventFundamentalPriceWithPrivileges_label.Size = new Size(173, 40);
            eventFundamentalPriceWithPrivileges_label.TabIndex = 6;
            eventFundamentalPriceWithPrivileges_label.Text = "Фундаментальная цена\r\n(с привилегиями):\r\n";
            eventFundamentalPriceWithPrivileges_label.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // eventFundamentalPrice_label
            // 
            eventFundamentalPrice_label.Anchor = AnchorStyles.None;
            eventFundamentalPrice_label.AutoSize = true;
            eventFundamentalPrice_label.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            eventFundamentalPrice_label.Location = new Point(21, 8);
            eventFundamentalPrice_label.Name = "eventFundamentalPrice_label";
            eventFundamentalPrice_label.Size = new Size(176, 20);
            eventFundamentalPrice_label.TabIndex = 5;
            eventFundamentalPrice_label.Text = "Фундаментальная цена:";
            // 
            // eventlPrice_label
            // 
            eventlPrice_label.Anchor = AnchorStyles.None;
            eventlPrice_label.AutoSize = true;
            eventlPrice_label.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            eventlPrice_label.Location = new Point(35, 219);
            eventlPrice_label.Name = "eventlPrice_label";
            eventlPrice_label.Size = new Size(147, 20);
            eventlPrice_label.TabIndex = 13;
            eventlPrice_label.Text = "Цена мероприятия:";
            eventlPrice_label.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.None;
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label1.Location = new Point(41, 167);
            label1.Name = "label1";
            label1.Size = new Size(136, 40);
            label1.TabIndex = 11;
            label1.Text = "Средняя цена дня\r\n(с привилегиями):";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // eventAveragePrice_label
            // 
            eventAveragePrice_label.Anchor = AnchorStyles.None;
            eventAveragePrice_label.AutoSize = true;
            eventAveragePrice_label.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            eventAveragePrice_label.Location = new Point(40, 137);
            eventAveragePrice_label.Name = "eventAveragePrice_label";
            eventAveragePrice_label.Size = new Size(138, 20);
            eventAveragePrice_label.TabIndex = 9;
            eventAveragePrice_label.Text = "Средняя цена дня:";
            eventAveragePrice_label.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // eventAveragePriceWithPrivilegesValue_label
            // 
            eventAveragePriceWithPrivilegesValue_label.Anchor = AnchorStyles.None;
            eventAveragePriceWithPrivilegesValue_label.AutoSize = true;
            eventAveragePriceWithPrivilegesValue_label.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            eventAveragePriceWithPrivilegesValue_label.Location = new Point(300, 177);
            eventAveragePriceWithPrivilegesValue_label.Name = "eventAveragePriceWithPrivilegesValue_label";
            eventAveragePriceWithPrivilegesValue_label.Size = new Size(17, 20);
            eventAveragePriceWithPrivilegesValue_label.TabIndex = 12;
            eventAveragePriceWithPrivilegesValue_label.Text = "0\r\n";
            eventAveragePriceWithPrivilegesValue_label.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // eventAveragePriceValue_label
            // 
            eventAveragePriceValue_label.Anchor = AnchorStyles.None;
            eventAveragePriceValue_label.AutoSize = true;
            eventAveragePriceValue_label.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            eventAveragePriceValue_label.Location = new Point(300, 137);
            eventAveragePriceValue_label.Name = "eventAveragePriceValue_label";
            eventAveragePriceValue_label.Size = new Size(17, 20);
            eventAveragePriceValue_label.TabIndex = 10;
            eventAveragePriceValue_label.Text = "0";
            eventAveragePriceValue_label.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.None;
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label5.Location = new Point(20, 86);
            label5.Name = "label5";
            label5.Size = new Size(178, 40);
            label5.TabIndex = 15;
            label5.Text = "Относительная разница фундаментальных цен:";
            label5.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // eventFundamentalPriceRelativeDifferenceValue_label
            // 
            eventFundamentalPriceRelativeDifferenceValue_label.Anchor = AnchorStyles.None;
            eventFundamentalPriceRelativeDifferenceValue_label.AutoSize = true;
            eventFundamentalPriceRelativeDifferenceValue_label.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            eventFundamentalPriceRelativeDifferenceValue_label.Location = new Point(300, 96);
            eventFundamentalPriceRelativeDifferenceValue_label.Name = "eventFundamentalPriceRelativeDifferenceValue_label";
            eventFundamentalPriceRelativeDifferenceValue_label.Size = new Size(17, 20);
            eventFundamentalPriceRelativeDifferenceValue_label.TabIndex = 16;
            eventFundamentalPriceRelativeDifferenceValue_label.Text = "0";
            eventFundamentalPriceRelativeDifferenceValue_label.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // eventPriceWithPrivilegesValue_label
            // 
            eventPriceWithPrivilegesValue_label.Anchor = AnchorStyles.None;
            eventPriceWithPrivilegesValue_label.AutoSize = true;
            eventPriceWithPrivilegesValue_label.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            eventPriceWithPrivilegesValue_label.Location = new Point(300, 261);
            eventPriceWithPrivilegesValue_label.Name = "eventPriceWithPrivilegesValue_label";
            eventPriceWithPrivilegesValue_label.Size = new Size(17, 20);
            eventPriceWithPrivilegesValue_label.TabIndex = 19;
            eventPriceWithPrivilegesValue_label.Text = "0\r\n";
            eventPriceWithPrivilegesValue_label.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(tableLayoutPanel3);
            tabPage3.Location = new Point(4, 29);
            tabPage3.Name = "tabPage3";
            tabPage3.Size = new Size(414, 372);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Прибыль";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.CellBorderStyle = TableLayoutPanelCellBorderStyle.OutsetPartial;
            tableLayoutPanel3.ColumnCount = 2;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 54F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 46F));
            tableLayoutPanel3.Controls.Add(eventExpensesValue_label, 1, 0);
            tableLayoutPanel3.Controls.Add(label6, 0, 0);
            tableLayoutPanel3.Controls.Add(label, 0, 5);
            tableLayoutPanel3.Controls.Add(label3, 0, 4);
            tableLayoutPanel3.Controls.Add(eventTheoryProfit_label, 0, 3);
            tableLayoutPanel3.Controls.Add(eventRealProfit_label, 0, 2);
            tableLayoutPanel3.Controls.Add(eventIncome_label, 0, 1);
            tableLayoutPanel3.Controls.Add(eventMinParticipantsCountValue_label, 1, 5);
            tableLayoutPanel3.Controls.Add(eventMaxPercent_label, 1, 4);
            tableLayoutPanel3.Controls.Add(eventTheoryProfitValue_label, 1, 3);
            tableLayoutPanel3.Controls.Add(eventRealProfitValue_label, 1, 2);
            tableLayoutPanel3.Controls.Add(eventIncomeValue_label, 1, 1);
            tableLayoutPanel3.Location = new Point(3, 3);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 6;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel3.Size = new Size(408, 233);
            tableLayoutPanel3.TabIndex = 0;
            // 
            // eventExpensesValue_label
            // 
            eventExpensesValue_label.Anchor = AnchorStyles.None;
            eventExpensesValue_label.AutoSize = true;
            eventExpensesValue_label.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            eventExpensesValue_label.Location = new Point(304, 8);
            eventExpensesValue_label.Name = "eventExpensesValue_label";
            eventExpensesValue_label.Size = new Size(17, 20);
            eventExpensesValue_label.TabIndex = 18;
            eventExpensesValue_label.Text = "0";
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.None;
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label6.Location = new Point(75, 8);
            label6.Name = "label6";
            label6.Size = new Size(70, 20);
            label6.TabIndex = 17;
            label6.Text = "Расходы:";
            // 
            // label
            // 
            label.Anchor = AnchorStyles.None;
            label.AutoSize = true;
            label.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label.Location = new Point(14, 186);
            label.Name = "label";
            label.Size = new Size(193, 40);
            label.TabIndex = 15;
            label.Text = "Минимальное количество участников:";
            label.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.None;
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label3.Location = new Point(21, 147);
            label3.Name = "label3";
            label3.Size = new Size(178, 20);
            label3.TabIndex = 12;
            label3.Text = "Максимальная наценка:";
            // 
            // eventTheoryProfit_label
            // 
            eventTheoryProfit_label.Anchor = AnchorStyles.None;
            eventTheoryProfit_label.AutoSize = true;
            eventTheoryProfit_label.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            eventTheoryProfit_label.Location = new Point(19, 107);
            eventTheoryProfit_label.Name = "eventTheoryProfit_label";
            eventTheoryProfit_label.Size = new Size(182, 20);
            eventTheoryProfit_label.TabIndex = 10;
            eventTheoryProfit_label.Text = "Теоретическая прибыль:";
            // 
            // eventRealProfit_label
            // 
            eventRealProfit_label.Anchor = AnchorStyles.None;
            eventRealProfit_label.AutoSize = true;
            eventRealProfit_label.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            eventRealProfit_label.Location = new Point(27, 74);
            eventRealProfit_label.Name = "eventRealProfit_label";
            eventRealProfit_label.Size = new Size(166, 20);
            eventRealProfit_label.TabIndex = 8;
            eventRealProfit_label.Text = "Фактическая прибыль:";
            // 
            // eventIncome_label
            // 
            eventIncome_label.Anchor = AnchorStyles.None;
            eventIncome_label.AutoSize = true;
            eventIncome_label.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            eventIncome_label.Location = new Point(83, 41);
            eventIncome_label.Name = "eventIncome_label";
            eventIncome_label.Size = new Size(55, 20);
            eventIncome_label.TabIndex = 6;
            eventIncome_label.Text = "Доход:";
            // 
            // eventMinParticipantsCountValue_label
            // 
            eventMinParticipantsCountValue_label.Anchor = AnchorStyles.None;
            eventMinParticipantsCountValue_label.AutoSize = true;
            eventMinParticipantsCountValue_label.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            eventMinParticipantsCountValue_label.Location = new Point(304, 196);
            eventMinParticipantsCountValue_label.Name = "eventMinParticipantsCountValue_label";
            eventMinParticipantsCountValue_label.Size = new Size(17, 20);
            eventMinParticipantsCountValue_label.TabIndex = 16;
            eventMinParticipantsCountValue_label.Text = "0";
            eventMinParticipantsCountValue_label.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // eventMaxPercent_label
            // 
            eventMaxPercent_label.Anchor = AnchorStyles.None;
            eventMaxPercent_label.AutoSize = true;
            eventMaxPercent_label.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            eventMaxPercent_label.Location = new Point(304, 147);
            eventMaxPercent_label.Name = "eventMaxPercent_label";
            eventMaxPercent_label.Size = new Size(17, 20);
            eventMaxPercent_label.TabIndex = 13;
            eventMaxPercent_label.Text = "0";
            // 
            // eventTheoryProfitValue_label
            // 
            eventTheoryProfitValue_label.Anchor = AnchorStyles.None;
            eventTheoryProfitValue_label.AutoSize = true;
            eventTheoryProfitValue_label.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            eventTheoryProfitValue_label.Location = new Point(304, 107);
            eventTheoryProfitValue_label.Name = "eventTheoryProfitValue_label";
            eventTheoryProfitValue_label.Size = new Size(17, 20);
            eventTheoryProfitValue_label.TabIndex = 11;
            eventTheoryProfitValue_label.Text = "0";
            // 
            // eventRealProfitValue_label
            // 
            eventRealProfitValue_label.Anchor = AnchorStyles.None;
            eventRealProfitValue_label.AutoSize = true;
            eventRealProfitValue_label.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            eventRealProfitValue_label.Location = new Point(304, 74);
            eventRealProfitValue_label.Name = "eventRealProfitValue_label";
            eventRealProfitValue_label.Size = new Size(17, 20);
            eventRealProfitValue_label.TabIndex = 9;
            eventRealProfitValue_label.Text = "0";
            // 
            // eventIncomeValue_label
            // 
            eventIncomeValue_label.Anchor = AnchorStyles.None;
            eventIncomeValue_label.AutoSize = true;
            eventIncomeValue_label.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            eventIncomeValue_label.Location = new Point(304, 41);
            eventIncomeValue_label.Name = "eventIncomeValue_label";
            eventIncomeValue_label.Size = new Size(17, 20);
            eventIncomeValue_label.TabIndex = 7;
            eventIncomeValue_label.Text = "0";
            // 
            // eventLocationId_label
            // 
            eventLocationId_label.AutoSize = true;
            eventLocationId_label.Location = new Point(12, 528);
            eventLocationId_label.Name = "eventLocationId_label";
            eventLocationId_label.Size = new Size(154, 20);
            eventLocationId_label.TabIndex = 4;
            eventLocationId_label.Text = "eventLocationId_label";
            // 
            // _timer
            // 
            _timer.Interval = 7500;
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { dataStatus_toolStripStatusLabel });
            statusStrip1.Location = new Point(0, 493);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1313, 26);
            statusStrip1.TabIndex = 5;
            statusStrip1.Text = "statusStrip1";
            // 
            // dataStatus_toolStripStatusLabel
            // 
            dataStatus_toolStripStatusLabel.Name = "dataStatus_toolStripStatusLabel";
            dataStatus_toolStripStatusLabel.Size = new Size(151, 20);
            dataStatus_toolStripStatusLabel.Text = "toolStripStatusLabel1";
            // 
            // tableLayoutPanel5
            // 
            tableLayoutPanel5.Anchor = AnchorStyles.None;
            tableLayoutPanel5.ColumnCount = 5;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35.00456F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 37.6325073F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15.989399F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 11.3947124F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 155F));
            tableLayoutPanel5.Controls.Add(eventFeedbacks_button, 3, 0);
            tableLayoutPanel5.Controls.Add(feedback_button, 4, 0);
            tableLayoutPanel5.Controls.Add(participation_button, 2, 0);
            tableLayoutPanel5.Location = new Point(12, 451);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 1;
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel5.Size = new Size(1289, 37);
            tableLayoutPanel5.TabIndex = 9;
            // 
            // eventFeedbacks_button
            // 
            eventFeedbacks_button.Anchor = AnchorStyles.None;
            eventFeedbacks_button.Location = new Point(1006, 4);
            eventFeedbacks_button.Name = "eventFeedbacks_button";
            eventFeedbacks_button.Size = new Size(123, 29);
            eventFeedbacks_button.TabIndex = 8;
            eventFeedbacks_button.Text = "Отзывы";
            eventFeedbacks_button.UseVisualStyleBackColor = true;
            eventFeedbacks_button.Click += eventFeedbacks_button_Click;
            // 
            // feedback_button
            // 
            feedback_button.Anchor = AnchorStyles.None;
            feedback_button.Location = new Point(1135, 4);
            feedback_button.Name = "feedback_button";
            feedback_button.Size = new Size(151, 29);
            feedback_button.TabIndex = 7;
            feedback_button.Text = "Оставить отзыв";
            feedback_button.UseVisualStyleBackColor = true;
            feedback_button.Click += feedback_button_Click;
            // 
            // participation_button
            // 
            participation_button.Anchor = AnchorStyles.None;
            participation_button.Location = new Point(825, 4);
            participation_button.Name = "participation_button";
            participation_button.Size = new Size(174, 29);
            participation_button.TabIndex = 6;
            participation_button.Text = "Участвовать/покинуть";
            participation_button.UseVisualStyleBackColor = true;
            participation_button.Visible = false;
            participation_button.Click += participation_button_Click;
            // 
            // EventOrganizationForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1313, 519);
            Controls.Add(tableLayoutPanel5);
            Controls.Add(statusStrip1);
            Controls.Add(eventLocationId_label);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(eventInfo_groupBox);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "EventOrganizationForm";
            Text = "Организация мероприятия";
            Load += EventOrganizationForm_Load;
            eventInfo_groupBox.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)eventDaysCount_numericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)eventPercent_numericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)eventParticipantBudget_numericUpDown).EndInit();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)eventDays_dataGridView).EndInit();
            groupBox3.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            tabPage3.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            tableLayoutPanel5.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
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
        private TableLayoutPanel tableLayoutPanel2;
        private Label eventParticipants_label;
        private GroupBox groupBox2;
        private DataGridView eventDays_dataGridView;
        private GroupBox groupBox3;
        private Label eventSolutionExists_label;
        private Label eventN_label;
        private Label eventFundamentalPrice_label;
        private Label eventFundamentalPriceWithPrivileges_label;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Label eventParticipantsValue_label;
        private Label eventRating_label;
        private Label eventRatingValue_label;
        private Label eventCost_label;
        private Label eventCostValue_label;
        private Label eventAverageDayCost_label;
        private Label eventAverageDayCostValue_label;
        private Label eventSolutionExistsValue_label;
        private Label eventNValue_label;
        private Label eventDaysBefore_label;
        private Label eventDaysBeforeValue_label;
        private TableLayoutPanel tableLayoutPanel4;
        private Label eventFundamentalPriceValue_label;
        private Label eventFundamentalPriceWithPrivilegesValue_label;
        private Label eventAveragePriceValue_label;
        private Label eventAveragePrice_label;
        private Label label1;
        private Label eventAveragePriceWithPrivilegesValue_label;
        private TabPage tabPage3;
        private Label eventlPrice_label;
        private TableLayoutPanel tableLayoutPanel3;
        private Label eventIncome_label;
        private Label eventIncomeValue_label;
        private Label eventRealProfit_label;
        private Label eventRealProfitValue_label;
        private Label eventTheoryProfit_label;
        private Label eventTheoryProfitValue_label;
        private Label label3;
        private Label label4;
        private NumericUpDown eventParticipantBudget_numericUpDown;
        private Label eventMaxPercent_label;
        private Label label;
        private Label label5;
        private Label eventFundamentalPriceRelativeDifferenceValue_label;
        private Label eventMinParticipantsCountValue_label;
        private Label eventExpensesValue_label;
        private Label label6;
        private Label eventLocationId_label;
        private System.Windows.Forms.Timer _timer;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Column3;
        private DataGridViewTextBoxColumn Column4;
        private DataGridViewTextBoxColumn Column5;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel dataStatus_toolStripStatusLabel;
        private Label label2;
        private Label eventPriceValue_label;
        private Label eventPriceWithPrivilegesValue_label;
        private ToolTip _toolTip;
        private TableLayoutPanel tableLayoutPanel5;
        private Button eventFeedbacks_button;
        private Button feedback_button;
        private Button participation_button;
    }
}