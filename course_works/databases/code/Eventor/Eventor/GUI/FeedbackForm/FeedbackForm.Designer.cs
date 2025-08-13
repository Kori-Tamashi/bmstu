namespace Eventor.GUI
{
    partial class FeedbackForm
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
            feedbackTitle_label = new Label();
            comment_label = new Label();
            comment_textBox = new TextBox();
            rating_label = new Label();
            delete_button = new Button();
            _timer = new System.Windows.Forms.Timer(components);
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = AnchorStyles.None;
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(feedbackTitle_label, 0, 0);
            tableLayoutPanel1.Location = new Point(15, 12);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(446, 76);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // feedbackTitle_label
            // 
            feedbackTitle_label.Anchor = AnchorStyles.None;
            feedbackTitle_label.AutoSize = true;
            feedbackTitle_label.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 204);
            feedbackTitle_label.Location = new Point(133, 24);
            feedbackTitle_label.Name = "feedbackTitle_label";
            feedbackTitle_label.Size = new Size(180, 28);
            feedbackTitle_label.TabIndex = 1;
            feedbackTitle_label.Text = "Отзыв участника";
            // 
            // comment_label
            // 
            comment_label.Anchor = AnchorStyles.None;
            comment_label.AutoSize = true;
            comment_label.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 204);
            comment_label.Location = new Point(15, 91);
            comment_label.Name = "comment_label";
            comment_label.Size = new Size(150, 28);
            comment_label.TabIndex = 2;
            comment_label.Text = "Комментарий";
            // 
            // comment_textBox
            // 
            comment_textBox.BackColor = SystemColors.Menu;
            comment_textBox.BorderStyle = BorderStyle.None;
            comment_textBox.Location = new Point(18, 122);
            comment_textBox.Multiline = true;
            comment_textBox.Name = "comment_textBox";
            comment_textBox.PlaceholderText = "Комментарий участника";
            comment_textBox.Size = new Size(443, 124);
            comment_textBox.TabIndex = 3;
            // 
            // rating_label
            // 
            rating_label.Anchor = AnchorStyles.None;
            rating_label.AutoSize = true;
            rating_label.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 204);
            rating_label.Location = new Point(15, 249);
            rating_label.Name = "rating_label";
            rating_label.Size = new Size(89, 28);
            rating_label.TabIndex = 4;
            rating_label.Text = "Рейтинг";
            // 
            // delete_button
            // 
            delete_button.Location = new Point(310, 281);
            delete_button.Name = "delete_button";
            delete_button.Size = new Size(151, 29);
            delete_button.TabIndex = 5;
            delete_button.Text = "Удалить отзыв";
            delete_button.UseVisualStyleBackColor = true;
            delete_button.Click += delete_button_Click;
            // 
            // _timer
            // 
            _timer.Interval = 15000;
            // 
            // FeedbackForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(473, 322);
            Controls.Add(delete_button);
            Controls.Add(rating_label);
            Controls.Add(comment_textBox);
            Controls.Add(comment_label);
            Controls.Add(tableLayoutPanel1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "FeedbackForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Отзыв";
            Load += FeedbackForm_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Label feedbackTitle_label;
        private Label comment_label;
        private TextBox comment_textBox;
        private Label rating_label;
        private Button delete_button;
        private System.Windows.Forms.Timer _timer;
    }
}