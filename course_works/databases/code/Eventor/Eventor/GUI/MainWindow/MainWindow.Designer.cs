namespace Eventor
{
    partial class MainWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            userEvents_groupBox = new GroupBox();
            userEvents_dataGridView = new DataGridView();
            Column1 = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            Column3 = new DataGridViewTextBoxColumn();
            groupBox1 = new GroupBox();
            userInfoSave_button = new Button();
            userInfo_tableLayoutPanel = new TableLayoutPanel();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            userName_textBox = new TextBox();
            userRole_textBox = new TextBox();
            userPhone_maskedTextBox = new MaskedTextBox();
            userGender_comboBox = new ComboBox();
            tabPage2 = new TabPage();
            groupBox3 = new GroupBox();
            events_dataGridView = new DataGridView();
            Column4 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn6 = new DataGridViewTextBoxColumn();
            tabPage3 = new TabPage();
            _timer = new System.Windows.Forms.Timer(components);
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            userEvents_groupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)userEvents_dataGridView).BeginInit();
            groupBox1.SuspendLayout();
            userInfo_tableLayoutPanel.SuspendLayout();
            tabPage2.SuspendLayout();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)events_dataGridView).BeginInit();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Location = new Point(6, 12);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1019, 751);
            tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(userEvents_groupBox);
            tabPage1.Controls.Add(groupBox1);
            tabPage1.Location = new Point(4, 29);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(1011, 718);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Профиль";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // userEvents_groupBox
            // 
            userEvents_groupBox.Controls.Add(userEvents_dataGridView);
            userEvents_groupBox.Location = new Point(6, 224);
            userEvents_groupBox.Name = "userEvents_groupBox";
            userEvents_groupBox.Size = new Size(999, 488);
            userEvents_groupBox.TabIndex = 4;
            userEvents_groupBox.TabStop = false;
            userEvents_groupBox.Text = "Выбранные мероприятия";
            // 
            // userEvents_dataGridView
            // 
            userEvents_dataGridView.AllowUserToAddRows = false;
            userEvents_dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            userEvents_dataGridView.BackgroundColor = Color.White;
            userEvents_dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            userEvents_dataGridView.Columns.AddRange(new DataGridViewColumn[] { Column1, Column2, Column3 });
            userEvents_dataGridView.Location = new Point(6, 26);
            userEvents_dataGridView.Name = "userEvents_dataGridView";
            userEvents_dataGridView.RowHeadersWidth = 51;
            userEvents_dataGridView.Size = new Size(987, 456);
            userEvents_dataGridView.TabIndex = 0;
            // 
            // Column1
            // 
            Column1.DataPropertyName = "Name";
            Column1.HeaderText = "Название";
            Column1.MinimumWidth = 6;
            Column1.Name = "Column1";
            // 
            // Column2
            // 
            Column2.DataPropertyName = "Date";
            Column2.HeaderText = "Дата";
            Column2.MinimumWidth = 6;
            Column2.Name = "Column2";
            // 
            // Column3
            // 
            Column3.DataPropertyName = "Id";
            Column3.HeaderText = "Id";
            Column3.MinimumWidth = 6;
            Column3.Name = "Column3";
            Column3.Visible = false;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(userInfoSave_button);
            groupBox1.Controls.Add(userInfo_tableLayoutPanel);
            groupBox1.Location = new Point(6, 6);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(999, 212);
            groupBox1.TabIndex = 3;
            groupBox1.TabStop = false;
            groupBox1.Text = "Информация";
            // 
            // userInfoSave_button
            // 
            userInfoSave_button.Location = new Point(340, 176);
            userInfoSave_button.Name = "userInfoSave_button";
            userInfoSave_button.Size = new Size(318, 29);
            userInfoSave_button.TabIndex = 5;
            userInfoSave_button.Text = "Сохранить";
            userInfoSave_button.UseVisualStyleBackColor = true;
            userInfoSave_button.Click += userInfoSave_button_Click;
            // 
            // userInfo_tableLayoutPanel
            // 
            userInfo_tableLayoutPanel.Anchor = AnchorStyles.None;
            userInfo_tableLayoutPanel.BackgroundImageLayout = ImageLayout.None;
            userInfo_tableLayoutPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            userInfo_tableLayoutPanel.ColumnCount = 2;
            userInfo_tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            userInfo_tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            userInfo_tableLayoutPanel.Controls.Add(label1, 0, 0);
            userInfo_tableLayoutPanel.Controls.Add(label2, 0, 1);
            userInfo_tableLayoutPanel.Controls.Add(label3, 0, 2);
            userInfo_tableLayoutPanel.Controls.Add(label4, 0, 3);
            userInfo_tableLayoutPanel.Controls.Add(userName_textBox, 1, 0);
            userInfo_tableLayoutPanel.Controls.Add(userRole_textBox, 1, 3);
            userInfo_tableLayoutPanel.Controls.Add(userPhone_maskedTextBox, 1, 1);
            userInfo_tableLayoutPanel.Controls.Add(userGender_comboBox, 1, 2);
            userInfo_tableLayoutPanel.Location = new Point(340, 25);
            userInfo_tableLayoutPanel.Name = "userInfo_tableLayoutPanel";
            userInfo_tableLayoutPanel.RowCount = 4;
            userInfo_tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            userInfo_tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            userInfo_tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            userInfo_tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            userInfo_tableLayoutPanel.Size = new Size(318, 145);
            userInfo_tableLayoutPanel.TabIndex = 4;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.None;
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 204);
            label1.Location = new Point(58, 8);
            label1.Name = "label1";
            label1.Size = new Size(42, 20);
            label1.TabIndex = 5;
            label1.Text = "Имя";
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.None;
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 204);
            label2.Location = new Point(13, 44);
            label2.Name = "label2";
            label2.Size = new Size(132, 20);
            label2.TabIndex = 6;
            label2.Text = "Номер телефона";
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.None;
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 204);
            label3.Location = new Point(60, 80);
            label3.Name = "label3";
            label3.Size = new Size(38, 20);
            label3.TabIndex = 7;
            label3.Text = "Пол";
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.None;
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 204);
            label4.Location = new Point(22, 116);
            label4.Name = "label4";
            label4.Size = new Size(115, 20);
            label4.TabIndex = 8;
            label4.Text = "Права доступа";
            // 
            // userName_textBox
            // 
            userName_textBox.Anchor = AnchorStyles.None;
            userName_textBox.Location = new Point(162, 5);
            userName_textBox.Name = "userName_textBox";
            userName_textBox.PlaceholderText = "Имя пользователя";
            userName_textBox.Size = new Size(152, 27);
            userName_textBox.TabIndex = 9;
            userName_textBox.TextAlign = HorizontalAlignment.Center;
            // 
            // userRole_textBox
            // 
            userRole_textBox.Anchor = AnchorStyles.None;
            userRole_textBox.Location = new Point(162, 113);
            userRole_textBox.Name = "userRole_textBox";
            userRole_textBox.PlaceholderText = "Права доступа";
            userRole_textBox.ReadOnly = true;
            userRole_textBox.Size = new Size(152, 27);
            userRole_textBox.TabIndex = 11;
            userRole_textBox.TextAlign = HorizontalAlignment.Center;
            // 
            // userPhone_maskedTextBox
            // 
            userPhone_maskedTextBox.Location = new Point(162, 40);
            userPhone_maskedTextBox.Mask = "+7 (999) 000-0000";
            userPhone_maskedTextBox.Name = "userPhone_maskedTextBox";
            userPhone_maskedTextBox.Size = new Size(152, 27);
            userPhone_maskedTextBox.TabIndex = 12;
            // 
            // userGender_comboBox
            // 
            userGender_comboBox.AutoCompleteCustomSource.AddRange(new string[] { "Женщина", "Мужчина" });
            userGender_comboBox.FormattingEnabled = true;
            userGender_comboBox.Items.AddRange(new object[] { "Женщина", "Мужчина" });
            userGender_comboBox.Location = new Point(162, 76);
            userGender_comboBox.Name = "userGender_comboBox";
            userGender_comboBox.Size = new Size(151, 28);
            userGender_comboBox.Sorted = true;
            userGender_comboBox.TabIndex = 13;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(groupBox3);
            tabPage2.Location = new Point(4, 29);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(1011, 718);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Мероприятия";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(events_dataGridView);
            groupBox3.Location = new Point(6, 6);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(999, 706);
            groupBox3.TabIndex = 0;
            groupBox3.TabStop = false;
            groupBox3.Text = "Доступные мероприятия";
            // 
            // events_dataGridView
            // 
            events_dataGridView.AllowUserToAddRows = false;
            events_dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            events_dataGridView.BackgroundColor = Color.White;
            events_dataGridView.ColumnHeadersHeight = 29;
            events_dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            events_dataGridView.Columns.AddRange(new DataGridViewColumn[] { Column4, dataGridViewTextBoxColumn1, dataGridViewTextBoxColumn2, dataGridViewTextBoxColumn3, dataGridViewTextBoxColumn4, dataGridViewTextBoxColumn5, dataGridViewTextBoxColumn6 });
            events_dataGridView.Location = new Point(6, 26);
            events_dataGridView.Name = "events_dataGridView";
            events_dataGridView.ReadOnly = true;
            events_dataGridView.RowHeadersWidth = 51;
            events_dataGridView.Size = new Size(986, 674);
            events_dataGridView.TabIndex = 6;
            events_dataGridView.CellClick += events_dataGridView_CellClick;
            // 
            // Column4
            // 
            Column4.HeaderText = "Id";
            Column4.MinimumWidth = 6;
            Column4.Name = "Column4";
            Column4.ReadOnly = true;
            Column4.Visible = false;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewTextBoxColumn1.DataPropertyName = "Name";
            dataGridViewTextBoxColumn1.HeaderText = "Название";
            dataGridViewTextBoxColumn1.MinimumWidth = 6;
            dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewTextBoxColumn2.DataPropertyName = "Description";
            dataGridViewTextBoxColumn2.HeaderText = "Описание";
            dataGridViewTextBoxColumn2.MinimumWidth = 6;
            dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            dataGridViewTextBoxColumn3.DataPropertyName = "Date";
            dataGridViewTextBoxColumn3.HeaderText = "Дата";
            dataGridViewTextBoxColumn3.MinimumWidth = 6;
            dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            dataGridViewTextBoxColumn4.DataPropertyName = "PersonCount";
            dataGridViewTextBoxColumn4.HeaderText = "Количество человек";
            dataGridViewTextBoxColumn4.MinimumWidth = 6;
            dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            dataGridViewTextBoxColumn5.DataPropertyName = "DaysCount";
            dataGridViewTextBoxColumn5.HeaderText = "Количество дней";
            dataGridViewTextBoxColumn5.MinimumWidth = 6;
            dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn6
            // 
            dataGridViewTextBoxColumn6.DataPropertyName = "Rating";
            dataGridViewTextBoxColumn6.HeaderText = "Рейтинг";
            dataGridViewTextBoxColumn6.MinimumWidth = 6;
            dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            dataGridViewTextBoxColumn6.ReadOnly = true;
            // 
            // tabPage3
            // 
            tabPage3.Location = new Point(4, 29);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(1011, 718);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Организация";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // _timer
            // 
            _timer.Interval = 15000;
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1031, 769);
            Controls.Add(tabControl1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "MainWindow";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Планировщик мероприятия";
            Load += MainWindow_Load;
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            userEvents_groupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)userEvents_dataGridView).EndInit();
            groupBox1.ResumeLayout(false);
            userInfo_tableLayoutPanel.ResumeLayout(false);
            userInfo_tableLayoutPanel.PerformLayout();
            tabPage2.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)events_dataGridView).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private GroupBox groupBox1;
        private TableLayoutPanel userInfo_tableLayoutPanel;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private TextBox userName_textBox;
        private TextBox userRole_textBox;
        private MaskedTextBox userPhone_maskedTextBox;
        private GroupBox userEvents_groupBox;
        private GroupBox groupBox3;
        private DataGridView events_dataGridView;
        private Button userInfoSave_button;
        private ComboBox userGender_comboBox;
        private System.Windows.Forms.Timer _timer;
        private DataGridView userEvents_dataGridView;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Column3;
        private DataGridViewTextBoxColumn Column4;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
    }
}
