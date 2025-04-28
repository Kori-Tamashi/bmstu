namespace Eventor.GUI
{
    partial class LoginForm
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
            userGender_comboBox = new ComboBox();
            userName_textBox = new TextBox();
            label4 = new Label();
            label3 = new Label();
            registrate_button = new Button();
            authorization_button = new Button();
            lasbel3 = new Label();
            userCheckPassword_textBox = new TextBox();
            label2 = new Label();
            userPassword_textBox = new TextBox();
            label1 = new Label();
            userPhone_maskedTextBox = new MaskedTextBox();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = AnchorStyles.None;
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(userGender_comboBox, 1, 1);
            tableLayoutPanel1.Controls.Add(userName_textBox, 1, 0);
            tableLayoutPanel1.Controls.Add(label4, 0, 1);
            tableLayoutPanel1.Controls.Add(label3, 0, 0);
            tableLayoutPanel1.Controls.Add(registrate_button, 0, 5);
            tableLayoutPanel1.Controls.Add(authorization_button, 1, 5);
            tableLayoutPanel1.Controls.Add(lasbel3, 0, 4);
            tableLayoutPanel1.Controls.Add(userCheckPassword_textBox, 1, 4);
            tableLayoutPanel1.Controls.Add(label2, 0, 3);
            tableLayoutPanel1.Controls.Add(userPassword_textBox, 1, 3);
            tableLayoutPanel1.Controls.Add(label1, 0, 2);
            tableLayoutPanel1.Controls.Add(userPhone_maskedTextBox, 1, 2);
            tableLayoutPanel1.Location = new Point(5, 10);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 6;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 47F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 57F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 8F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(287, 248);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // userGender_comboBox
            // 
            userGender_comboBox.AutoCompleteCustomSource.AddRange(new string[] { "Женщина", "Мужчина" });
            userGender_comboBox.AutoCompleteMode = AutoCompleteMode.Suggest;
            userGender_comboBox.FormattingEnabled = true;
            userGender_comboBox.Items.AddRange(new object[] { "Женщина", "Мужчина" });
            userGender_comboBox.Location = new Point(146, 38);
            userGender_comboBox.Name = "userGender_comboBox";
            userGender_comboBox.Size = new Size(138, 28);
            userGender_comboBox.Sorted = true;
            userGender_comboBox.TabIndex = 14;
            // 
            // userName_textBox
            // 
            userName_textBox.Anchor = AnchorStyles.None;
            userName_textBox.Location = new Point(146, 4);
            userName_textBox.Name = "userName_textBox";
            userName_textBox.PlaceholderText = "Имя";
            userName_textBox.Size = new Size(138, 27);
            userName_textBox.TabIndex = 5;
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.None;
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 204);
            label4.Location = new Point(52, 42);
            label4.Name = "label4";
            label4.Size = new Size(38, 20);
            label4.TabIndex = 2;
            label4.Text = "Пол";
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.None;
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 204);
            label3.Location = new Point(50, 7);
            label3.Name = "label3";
            label3.Size = new Size(42, 20);
            label3.TabIndex = 2;
            label3.Text = "Имя";
            // 
            // registrate_button
            // 
            registrate_button.Anchor = AnchorStyles.None;
            registrate_button.Location = new Point(4, 214);
            registrate_button.Name = "registrate_button";
            registrate_button.Size = new Size(135, 29);
            registrate_button.TabIndex = 1;
            registrate_button.Text = "Регистрация";
            registrate_button.UseVisualStyleBackColor = true;
            registrate_button.Click += registrate_button_Click;
            // 
            // authorization_button
            // 
            authorization_button.Anchor = AnchorStyles.None;
            authorization_button.Location = new Point(147, 214);
            authorization_button.Name = "authorization_button";
            authorization_button.Size = new Size(135, 29);
            authorization_button.TabIndex = 7;
            authorization_button.Text = "Авторизация";
            authorization_button.UseVisualStyleBackColor = true;
            authorization_button.Click += authorization_button_Click;
            // 
            // lasbel3
            // 
            lasbel3.Anchor = AnchorStyles.None;
            lasbel3.AutoSize = true;
            lasbel3.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 204);
            lasbel3.Location = new Point(19, 160);
            lasbel3.Name = "lasbel3";
            lasbel3.Size = new Size(104, 40);
            lasbel3.TabIndex = 5;
            lasbel3.Text = "Подтвердите пароль";
            lasbel3.TextAlign = ContentAlignment.TopCenter;
            // 
            // userCheckPassword_textBox
            // 
            userCheckPassword_textBox.Anchor = AnchorStyles.None;
            userCheckPassword_textBox.Location = new Point(146, 167);
            userCheckPassword_textBox.Name = "userCheckPassword_textBox";
            userCheckPassword_textBox.PlaceholderText = "Пароль";
            userCheckPassword_textBox.Size = new Size(138, 27);
            userCheckPassword_textBox.TabIndex = 6;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.None;
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 204);
            label2.Location = new Point(40, 124);
            label2.Name = "label2";
            label2.Size = new Size(63, 20);
            label2.TabIndex = 2;
            label2.Text = "Пароль";
            // 
            // userPassword_textBox
            // 
            userPassword_textBox.Anchor = AnchorStyles.None;
            userPassword_textBox.Location = new Point(146, 121);
            userPassword_textBox.Name = "userPassword_textBox";
            userPassword_textBox.PlaceholderText = "Пароль";
            userPassword_textBox.Size = new Size(138, 27);
            userPassword_textBox.TabIndex = 4;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.None;
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 204);
            label1.Location = new Point(5, 83);
            label1.Name = "label1";
            label1.Size = new Size(132, 20);
            label1.TabIndex = 1;
            label1.Text = "Номер телефона";
            // 
            // userPhone_maskedTextBox
            // 
            userPhone_maskedTextBox.Anchor = AnchorStyles.None;
            userPhone_maskedTextBox.Location = new Point(146, 80);
            userPhone_maskedTextBox.Mask = "+7 (999) 000-0000";
            userPhone_maskedTextBox.Name = "userPhone_maskedTextBox";
            userPhone_maskedTextBox.Size = new Size(138, 27);
            userPhone_maskedTextBox.TabIndex = 3;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(298, 269);
            Controls.Add(tableLayoutPanel1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Name = "LoginForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Вход в систему";
            Load += LoginForm_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Label label1;
        private Label label2;
        private MaskedTextBox userPhone_maskedTextBox;
        private TextBox userPassword_textBox;
        private Label lasbel3;
        private TextBox userCheckPassword_textBox;
        private Button registrate_button;
        private Button authorization_button;
        private Label label4;
        private Label label3;
        private TextBox userName_textBox;
        private ComboBox userGender_comboBox;
    }
}