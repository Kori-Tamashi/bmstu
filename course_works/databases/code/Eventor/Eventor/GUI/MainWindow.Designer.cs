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
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            groupBox2 = new GroupBox();
            dataGridView1 = new DataGridView();
            name = new DataGridViewTextBoxColumn();
            paid = new DataGridViewTextBoxColumn();
            groupBox1 = new GroupBox();
            userInfo_tableLayoutPanel = new TableLayoutPanel();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            userName_textBox = new TextBox();
            userGender_textBox = new TextBox();
            userRole_textBox = new TextBox();
            userPhone_maskedTextBox = new MaskedTextBox();
            tabPage2 = new TabPage();
            groupBox3 = new GroupBox();
            dataGridView2 = new DataGridView();
            dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            tabPage3 = new TabPage();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            groupBox1.SuspendLayout();
            userInfo_tableLayoutPanel.SuspendLayout();
            tabPage2.SuspendLayout();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Location = new Point(12, 12);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(354, 751);
            tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(groupBox2);
            tabPage1.Controls.Add(groupBox1);
            tabPage1.Location = new Point(4, 29);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(346, 718);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Профиль";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(dataGridView1);
            groupBox2.Location = new Point(6, 190);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(330, 522);
            groupBox2.TabIndex = 4;
            groupBox2.TabStop = false;
            groupBox2.Text = "Выбранные мероприятия";
            // 
            // dataGridView1
            // 
            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.ColumnHeadersHeight = 29;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { name, paid });
            dataGridView1.Location = new Point(6, 26);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(318, 490);
            dataGridView1.TabIndex = 5;
            // 
            // name
            // 
            name.HeaderText = "Название";
            name.MinimumWidth = 6;
            name.Name = "name";
            name.ReadOnly = true;
            name.Width = 125;
            // 
            // paid
            // 
            paid.HeaderText = "Статус оплаты";
            paid.MinimumWidth = 6;
            paid.Name = "paid";
            paid.ReadOnly = true;
            paid.Width = 140;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(userInfo_tableLayoutPanel);
            groupBox1.Location = new Point(6, 6);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(330, 175);
            groupBox1.TabIndex = 3;
            groupBox1.TabStop = false;
            groupBox1.Text = "Информация";
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
            userInfo_tableLayoutPanel.Controls.Add(userGender_textBox, 1, 2);
            userInfo_tableLayoutPanel.Controls.Add(userRole_textBox, 1, 3);
            userInfo_tableLayoutPanel.Controls.Add(userPhone_maskedTextBox, 1, 1);
            userInfo_tableLayoutPanel.Location = new Point(6, 22);
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
            userName_textBox.TextChanged += userName_textBox_TextChanged;
            // 
            // userGender_textBox
            // 
            userGender_textBox.Anchor = AnchorStyles.None;
            userGender_textBox.Location = new Point(162, 77);
            userGender_textBox.Name = "userGender_textBox";
            userGender_textBox.PlaceholderText = "Пол";
            userGender_textBox.Size = new Size(152, 27);
            userGender_textBox.TabIndex = 10;
            userGender_textBox.TextAlign = HorizontalAlignment.Center;
            // 
            // userRole_textBox
            // 
            userRole_textBox.Anchor = AnchorStyles.None;
            userRole_textBox.Location = new Point(162, 113);
            userRole_textBox.Name = "userRole_textBox";
            userRole_textBox.PlaceholderText = "Права доступа";
            userRole_textBox.Size = new Size(152, 27);
            userRole_textBox.TabIndex = 11;
            userRole_textBox.TextAlign = HorizontalAlignment.Center;
            // 
            // userPhone_maskedTextBox
            // 
            userPhone_maskedTextBox.Location = new Point(162, 40);
            userPhone_maskedTextBox.Mask = "+7 (999) 000-0000";
            userPhone_maskedTextBox.Name = "userPhone_maskedTextBox";
            userPhone_maskedTextBox.ReadOnly = true;
            userPhone_maskedTextBox.Size = new Size(152, 27);
            userPhone_maskedTextBox.TabIndex = 12;
            userPhone_maskedTextBox.Text = "9194068111";
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(groupBox3);
            tabPage2.Location = new Point(4, 29);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(346, 718);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Мероприятия";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(dataGridView2);
            groupBox3.Location = new Point(6, 6);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(332, 706);
            groupBox3.TabIndex = 0;
            groupBox3.TabStop = false;
            groupBox3.Text = "Доступные мероприятия";
            // 
            // dataGridView2
            // 
            dataGridView2.BackgroundColor = Color.White;
            dataGridView2.ColumnHeadersHeight = 29;
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridView2.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn1, dataGridViewTextBoxColumn2 });
            dataGridView2.Location = new Point(6, 26);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.RowHeadersWidth = 51;
            dataGridView2.Size = new Size(318, 674);
            dataGridView2.TabIndex = 6;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewTextBoxColumn1.HeaderText = "Название";
            dataGridViewTextBoxColumn1.MinimumWidth = 6;
            dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            dataGridViewTextBoxColumn1.ReadOnly = true;
            dataGridViewTextBoxColumn1.Width = 125;
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewTextBoxColumn2.HeaderText = "Текущая цена";
            dataGridViewTextBoxColumn2.MinimumWidth = 6;
            dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            dataGridViewTextBoxColumn2.ReadOnly = true;
            dataGridViewTextBoxColumn2.Width = 140;
            // 
            // tabPage3
            // 
            tabPage3.Location = new Point(4, 29);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(346, 718);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Организация";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(369, 775);
            Controls.Add(tabControl1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "MainWindow";
            Text = "Планировщик мероприятия";
            Load += MainWindow_Load;
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            groupBox1.ResumeLayout(false);
            userInfo_tableLayoutPanel.ResumeLayout(false);
            userInfo_tableLayoutPanel.PerformLayout();
            tabPage2.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
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
        private TextBox userGender_textBox;
        private TextBox userRole_textBox;
        private MaskedTextBox userPhone_maskedTextBox;
        private GroupBox groupBox2;
        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn name;
        private DataGridViewTextBoxColumn paid;
        private GroupBox groupBox3;
        private DataGridView dataGridView2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    }
}
