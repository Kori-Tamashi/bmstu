namespace Eventor.GUI
{
    partial class DayOrganizationForm
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
            daySettings_groupBox = new GroupBox();
            saveSettings_button = new Button();
            tableLayoutPanel1 = new TableLayoutPanel();
            label2 = new Label();
            dayName_textBox = new TextBox();
            label1 = new Label();
            dayDescription_textBox = new TextBox();
            groupBox1 = new GroupBox();
            dayParticipants_dataGridView = new DataGridView();
            groupBox2 = new GroupBox();
            addItem_button = new Button();
            itemCount_numericUpDown = new NumericUpDown();
            items_comboBox = new ComboBox();
            dayMenu_dataGridView = new DataGridView();
            Column5 = new DataGridViewTextBoxColumn();
            _contextMenuStrip = new ContextMenuStrip(components);
            deleteItem_toolStripMenuItem = new ToolStripMenuItem();
            Column6 = new DataGridViewTextBoxColumn();
            Column7 = new DataGridViewTextBoxColumn();
            groupBox3 = new GroupBox();
            tableLayoutPanel2 = new TableLayoutPanel();
            dayCoefficientValue_label = new Label();
            label7 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            dayCostValue_label = new Label();
            dayPriceValue_label = new Label();
            dayPriceWithPrivilegesValue_label = new Label();
            dayParticipantsCountValue_label = new Label();
            _toolTip = new ToolTip(components);
            statusStrip1 = new StatusStrip();
            dataStatus_toolStripStatusLabel = new ToolStripStatusLabel();
            _timer = new System.Windows.Forms.Timer(components);
            Column1 = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            Column3 = new DataGridViewComboBoxColumn();
            Column4 = new DataGridViewComboBoxColumn();
            daySettings_groupBox.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dayParticipants_dataGridView).BeginInit();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)itemCount_numericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dayMenu_dataGridView).BeginInit();
            _contextMenuStrip.SuspendLayout();
            groupBox3.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // daySettings_groupBox
            // 
            daySettings_groupBox.Controls.Add(saveSettings_button);
            daySettings_groupBox.Controls.Add(tableLayoutPanel1);
            daySettings_groupBox.Location = new Point(12, 12);
            daySettings_groupBox.Name = "daySettings_groupBox";
            daySettings_groupBox.Size = new Size(300, 427);
            daySettings_groupBox.TabIndex = 0;
            daySettings_groupBox.TabStop = false;
            daySettings_groupBox.Text = "Настройки";
            // 
            // saveSettings_button
            // 
            saveSettings_button.Location = new Point(6, 393);
            saveSettings_button.Name = "saveSettings_button";
            saveSettings_button.Size = new Size(288, 28);
            saveSettings_button.TabIndex = 4;
            saveSettings_button.Text = "Сохранить";
            saveSettings_button.UseVisualStyleBackColor = true;
            saveSettings_button.Click += saveSettings_button_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = AnchorStyles.None;
            tableLayoutPanel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.OutsetDouble;
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32.0422554F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 67.95774F));
            tableLayoutPanel1.Controls.Add(label2, 0, 1);
            tableLayoutPanel1.Controls.Add(dayName_textBox, 1, 0);
            tableLayoutPanel1.Controls.Add(label1, 0, 0);
            tableLayoutPanel1.Controls.Add(dayDescription_textBox, 1, 1);
            tableLayoutPanel1.Location = new Point(6, 21);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(288, 366);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.None;
            label2.AutoSize = true;
            label2.Location = new Point(6, 195);
            label2.Name = "label2";
            label2.Size = new Size(82, 20);
            label2.TabIndex = 3;
            label2.Text = "Описание:";
            // 
            // dayName_textBox
            // 
            dayName_textBox.Anchor = AnchorStyles.None;
            dayName_textBox.Location = new Point(98, 10);
            dayName_textBox.Name = "dayName_textBox";
            dayName_textBox.PlaceholderText = "Название дня";
            dayName_textBox.Size = new Size(184, 27);
            dayName_textBox.TabIndex = 2;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.None;
            label1.AutoSize = true;
            label1.Location = new Point(7, 14);
            label1.Name = "label1";
            label1.Size = new Size(80, 20);
            label1.TabIndex = 2;
            label1.Text = "Название:";
            // 
            // dayDescription_textBox
            // 
            dayDescription_textBox.Location = new Point(98, 51);
            dayDescription_textBox.Multiline = true;
            dayDescription_textBox.Name = "dayDescription_textBox";
            dayDescription_textBox.PlaceholderText = "Описание дня";
            dayDescription_textBox.Size = new Size(184, 309);
            dayDescription_textBox.TabIndex = 4;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(dayParticipants_dataGridView);
            groupBox1.Location = new Point(318, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(340, 427);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "Участники";
            // 
            // dayParticipants_dataGridView
            // 
            dayParticipants_dataGridView.AllowUserToAddRows = false;
            dayParticipants_dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dayParticipants_dataGridView.BackgroundColor = SystemColors.Menu;
            dayParticipants_dataGridView.BorderStyle = BorderStyle.None;
            dayParticipants_dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dayParticipants_dataGridView.Columns.AddRange(new DataGridViewColumn[] { Column1, Column2, Column3, Column4 });
            dayParticipants_dataGridView.Location = new Point(6, 26);
            dayParticipants_dataGridView.Name = "dayParticipants_dataGridView";
            dayParticipants_dataGridView.RowHeadersWidth = 51;
            dayParticipants_dataGridView.Size = new Size(328, 396);
            dayParticipants_dataGridView.TabIndex = 0;
            dayParticipants_dataGridView.CellValueChanged += dayParticipants_dataGridView_CellValueChanged;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(addItem_button);
            groupBox2.Controls.Add(itemCount_numericUpDown);
            groupBox2.Controls.Add(items_comboBox);
            groupBox2.Controls.Add(dayMenu_dataGridView);
            groupBox2.Location = new Point(664, 12);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(347, 427);
            groupBox2.TabIndex = 2;
            groupBox2.TabStop = false;
            groupBox2.Text = "Меню";
            // 
            // addItem_button
            // 
            addItem_button.Location = new Point(180, 393);
            addItem_button.Name = "addItem_button";
            addItem_button.Size = new Size(161, 29);
            addItem_button.TabIndex = 4;
            addItem_button.Text = "Добавить предмет";
            addItem_button.UseVisualStyleBackColor = true;
            addItem_button.Click += addItem_button_Click;
            // 
            // itemCount_numericUpDown
            // 
            itemCount_numericUpDown.Location = new Point(6, 394);
            itemCount_numericUpDown.Maximum = new decimal(new int[] { -727379969, 232, 0, 0 });
            itemCount_numericUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            itemCount_numericUpDown.Name = "itemCount_numericUpDown";
            itemCount_numericUpDown.Size = new Size(168, 27);
            itemCount_numericUpDown.TabIndex = 3;
            _toolTip.SetToolTip(itemCount_numericUpDown, "Количество предметов в меню");
            itemCount_numericUpDown.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // items_comboBox
            // 
            items_comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            items_comboBox.FormattingEnabled = true;
            items_comboBox.Location = new Point(6, 360);
            items_comboBox.Name = "items_comboBox";
            items_comboBox.Size = new Size(335, 28);
            items_comboBox.TabIndex = 2;
            // 
            // dayMenu_dataGridView
            // 
            dayMenu_dataGridView.AllowUserToAddRows = false;
            dayMenu_dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dayMenu_dataGridView.BackgroundColor = SystemColors.Menu;
            dayMenu_dataGridView.BorderStyle = BorderStyle.None;
            dayMenu_dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dayMenu_dataGridView.Columns.AddRange(new DataGridViewColumn[] { Column5, Column6, Column7 });
            dayMenu_dataGridView.ContextMenuStrip = _contextMenuStrip;
            dayMenu_dataGridView.Location = new Point(6, 26);
            dayMenu_dataGridView.Name = "dayMenu_dataGridView";
            dayMenu_dataGridView.RowHeadersWidth = 51;
            dayMenu_dataGridView.Size = new Size(335, 323);
            dayMenu_dataGridView.TabIndex = 1;
            dayMenu_dataGridView.CellMouseClick += dayMenu_dataGridView_CellMouseClick;
            // 
            // Column5
            // 
            Column5.ContextMenuStrip = _contextMenuStrip;
            Column5.HeaderText = "Id";
            Column5.MinimumWidth = 6;
            Column5.Name = "Column5";
            Column5.ReadOnly = true;
            Column5.Visible = false;
            // 
            // _contextMenuStrip
            // 
            _contextMenuStrip.ImageScalingSize = new Size(20, 20);
            _contextMenuStrip.Items.AddRange(new ToolStripItem[] { deleteItem_toolStripMenuItem });
            _contextMenuStrip.Name = "_contextMenuStrip";
            _contextMenuStrip.Size = new Size(135, 28);
            _contextMenuStrip.Text = "Удалить";
            // 
            // deleteItem_toolStripMenuItem
            // 
            deleteItem_toolStripMenuItem.Name = "deleteItem_toolStripMenuItem";
            deleteItem_toolStripMenuItem.Size = new Size(134, 24);
            deleteItem_toolStripMenuItem.Text = "Удалить";
            deleteItem_toolStripMenuItem.Click += deleteItem_toolStripMenuItem_Click;
            // 
            // Column6
            // 
            Column6.ContextMenuStrip = _contextMenuStrip;
            Column6.HeaderText = "Название";
            Column6.MinimumWidth = 6;
            Column6.Name = "Column6";
            Column6.ReadOnly = true;
            // 
            // Column7
            // 
            Column7.ContextMenuStrip = _contextMenuStrip;
            Column7.HeaderText = "Количество";
            Column7.MinimumWidth = 6;
            Column7.Name = "Column7";
            Column7.ReadOnly = true;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(tableLayoutPanel2);
            groupBox3.Location = new Point(1017, 12);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(347, 427);
            groupBox3.TabIndex = 3;
            groupBox3.TabStop = false;
            groupBox3.Text = "Анализ";
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.CellBorderStyle = TableLayoutPanelCellBorderStyle.OutsetPartial;
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(dayCoefficientValue_label, 1, 4);
            tableLayoutPanel2.Controls.Add(label7, 0, 4);
            tableLayoutPanel2.Controls.Add(label3, 0, 0);
            tableLayoutPanel2.Controls.Add(label4, 0, 1);
            tableLayoutPanel2.Controls.Add(label5, 0, 3);
            tableLayoutPanel2.Controls.Add(label6, 0, 2);
            tableLayoutPanel2.Controls.Add(dayCostValue_label, 1, 0);
            tableLayoutPanel2.Controls.Add(dayPriceValue_label, 1, 1);
            tableLayoutPanel2.Controls.Add(dayPriceWithPrivilegesValue_label, 1, 2);
            tableLayoutPanel2.Controls.Add(dayParticipantsCountValue_label, 1, 3);
            tableLayoutPanel2.Location = new Point(6, 21);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 5;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel2.Size = new Size(335, 218);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // dayCoefficientValue_label
            // 
            dayCoefficientValue_label.Anchor = AnchorStyles.None;
            dayCoefficientValue_label.AutoSize = true;
            dayCoefficientValue_label.Location = new Point(242, 182);
            dayCoefficientValue_label.Name = "dayCoefficientValue_label";
            dayCoefficientValue_label.Size = new Size(17, 20);
            dayCoefficientValue_label.TabIndex = 12;
            dayCoefficientValue_label.Text = "0";
            // 
            // label7
            // 
            label7.Anchor = AnchorStyles.None;
            label7.AutoSize = true;
            label7.Location = new Point(16, 182);
            label7.Name = "label7";
            label7.Size = new Size(136, 20);
            label7.TabIndex = 11;
            label7.Text = "Коэффициент дня:";
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.None;
            label3.AutoSize = true;
            label3.Location = new Point(41, 9);
            label3.Name = "label3";
            label3.Size = new Size(86, 20);
            label3.TabIndex = 3;
            label3.Text = "Стоимость:";
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.None;
            label4.AutoSize = true;
            label4.Location = new Point(60, 44);
            label4.Name = "label4";
            label4.Size = new Size(48, 20);
            label4.TabIndex = 4;
            label4.Text = "Цена:";
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.None;
            label5.AutoSize = true;
            label5.Location = new Point(37, 123);
            label5.Name = "label5";
            label5.Size = new Size(94, 40);
            label5.TabIndex = 5;
            label5.Text = "Количество участников:";
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.None;
            label6.AutoSize = true;
            label6.Location = new Point(16, 75);
            label6.Name = "label6";
            label6.Size = new Size(136, 40);
            label6.TabIndex = 6;
            label6.Text = "Цена\r\n(с привилегиями):";
            label6.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // dayCostValue_label
            // 
            dayCostValue_label.Anchor = AnchorStyles.None;
            dayCostValue_label.AutoSize = true;
            dayCostValue_label.Location = new Point(242, 9);
            dayCostValue_label.Name = "dayCostValue_label";
            dayCostValue_label.Size = new Size(17, 20);
            dayCostValue_label.TabIndex = 8;
            dayCostValue_label.Text = "0";
            // 
            // dayPriceValue_label
            // 
            dayPriceValue_label.Anchor = AnchorStyles.None;
            dayPriceValue_label.AutoSize = true;
            dayPriceValue_label.Location = new Point(242, 44);
            dayPriceValue_label.Name = "dayPriceValue_label";
            dayPriceValue_label.Size = new Size(17, 20);
            dayPriceValue_label.TabIndex = 7;
            dayPriceValue_label.Text = "0";
            // 
            // dayPriceWithPrivilegesValue_label
            // 
            dayPriceWithPrivilegesValue_label.Anchor = AnchorStyles.None;
            dayPriceWithPrivilegesValue_label.AutoSize = true;
            dayPriceWithPrivilegesValue_label.Location = new Point(242, 85);
            dayPriceWithPrivilegesValue_label.Name = "dayPriceWithPrivilegesValue_label";
            dayPriceWithPrivilegesValue_label.Size = new Size(17, 20);
            dayPriceWithPrivilegesValue_label.TabIndex = 9;
            dayPriceWithPrivilegesValue_label.Text = "0";
            // 
            // dayParticipantsCountValue_label
            // 
            dayParticipantsCountValue_label.Anchor = AnchorStyles.None;
            dayParticipantsCountValue_label.AutoSize = true;
            dayParticipantsCountValue_label.Location = new Point(242, 133);
            dayParticipantsCountValue_label.Name = "dayParticipantsCountValue_label";
            dayParticipantsCountValue_label.Size = new Size(17, 20);
            dayParticipantsCountValue_label.TabIndex = 10;
            dayParticipantsCountValue_label.Text = "0";
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { dataStatus_toolStripStatusLabel });
            statusStrip1.Location = new Point(0, 447);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1371, 26);
            statusStrip1.TabIndex = 4;
            statusStrip1.Text = "statusStrip1";
            // 
            // dataStatus_toolStripStatusLabel
            // 
            dataStatus_toolStripStatusLabel.Name = "dataStatus_toolStripStatusLabel";
            dataStatus_toolStripStatusLabel.Size = new Size(151, 20);
            dataStatus_toolStripStatusLabel.Text = "toolStripStatusLabel1";
            // 
            // _timer
            // 
            _timer.Interval = 5000;
            // 
            // Column1
            // 
            Column1.HeaderText = "Id";
            Column1.MinimumWidth = 6;
            Column1.Name = "Column1";
            Column1.ReadOnly = true;
            Column1.Visible = false;
            // 
            // Column2
            // 
            Column2.HeaderText = "Имя";
            Column2.MinimumWidth = 6;
            Column2.Name = "Column2";
            Column2.ReadOnly = true;
            // 
            // Column3
            // 
            Column3.HeaderText = "Тип";
            Column3.Items.AddRange(new object[] { "Организатор", "VIP-персона", "Простой участник" });
            Column3.MinimumWidth = 6;
            Column3.Name = "Column3";
            Column3.Resizable = DataGridViewTriState.True;
            Column3.SortMode = DataGridViewColumnSortMode.Automatic;
            // 
            // Column4
            // 
            Column4.HeaderText = "Оплата";
            Column4.Items.AddRange(new object[] { "Оплачено", "Не оплачено" });
            Column4.MinimumWidth = 6;
            Column4.Name = "Column4";
            Column4.Resizable = DataGridViewTriState.True;
            Column4.SortMode = DataGridViewColumnSortMode.Automatic;
            // 
            // DayOrganizationForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1371, 473);
            Controls.Add(statusStrip1);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(daySettings_groupBox);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "DayOrganizationForm";
            Text = "Организация дня мероприятия";
            Load += DayOrganizationForm_Load;
            daySettings_groupBox.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dayParticipants_dataGridView).EndInit();
            groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)itemCount_numericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)dayMenu_dataGridView).EndInit();
            _contextMenuStrip.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox daySettings_groupBox;
        private TableLayoutPanel tableLayoutPanel1;
        private Label label1;
        private TextBox dayName_textBox;
        private Label label2;
        private TextBox dayDescription_textBox;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private DataGridView dayParticipants_dataGridView;
        private DataGridView dayMenu_dataGridView;
        private TableLayoutPanel tableLayoutPanel2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label dayPriceValue_label;
        private Label label6;
        private Label dayPriceWithPrivilegesValue_label;
        private Label dayCostValue_label;
        private Label dayParticipantsCountValue_label;
        private Label dayCoefficientValue_label;
        private Label label7;
        private Button saveSettings_button;
        private NumericUpDown itemCount_numericUpDown;
        private ComboBox items_comboBox;
        private ToolTip _toolTip;
        private Button addItem_button;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel dataStatus_toolStripStatusLabel;
        private System.Windows.Forms.Timer _timer;
        private ContextMenuStrip _contextMenuStrip;
        private DataGridViewTextBoxColumn Column5;
        private DataGridViewTextBoxColumn Column6;
        private DataGridViewTextBoxColumn Column7;
        private ToolStripMenuItem deleteItem_toolStripMenuItem;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewComboBoxColumn Column3;
        private DataGridViewComboBoxColumn Column4;
    }
}