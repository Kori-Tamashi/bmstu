namespace Eventor.Eventor.GUI
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
            label1 = new Label();
            feedbackComment_textBox = new TextBox();
            feedbackRating_numericUpDown = new NumericUpDown();
            label2 = new Label();
            createFeedback_button = new Button();
            ((System.ComponentModel.ISupportInitialize)feedbackRating_numericUpDown).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 204);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(150, 28);
            label1.TabIndex = 0;
            label1.Text = "Комментарий";
            // 
            // feedbackComment_textBox
            // 
            feedbackComment_textBox.Location = new Point(18, 40);
            feedbackComment_textBox.Multiline = true;
            feedbackComment_textBox.Name = "feedbackComment_textBox";
            feedbackComment_textBox.Size = new Size(462, 157);
            feedbackComment_textBox.TabIndex = 1;
            // 
            // feedbackRating_numericUpDown
            // 
            feedbackRating_numericUpDown.DecimalPlaces = 2;
            feedbackRating_numericUpDown.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            feedbackRating_numericUpDown.Location = new Point(18, 231);
            feedbackRating_numericUpDown.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            feedbackRating_numericUpDown.Name = "feedbackRating_numericUpDown";
            feedbackRating_numericUpDown.Size = new Size(150, 27);
            feedbackRating_numericUpDown.TabIndex = 2;
            feedbackRating_numericUpDown.Value = new decimal(new int[] { 10, 0, 0, 0 });
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 204);
            label2.Location = new Point(13, 200);
            label2.Name = "label2";
            label2.Size = new Size(89, 28);
            label2.TabIndex = 3;
            label2.Text = "Рейтинг";
            // 
            // createFeedback_button
            // 
            createFeedback_button.Location = new Point(18, 264);
            createFeedback_button.Name = "createFeedback_button";
            createFeedback_button.Size = new Size(462, 29);
            createFeedback_button.TabIndex = 4;
            createFeedback_button.Text = "Оставить отзыв";
            createFeedback_button.UseVisualStyleBackColor = true;
            createFeedback_button.Click += createFeedback_button_Click;
            // 
            // FeedbackForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(499, 310);
            Controls.Add(createFeedback_button);
            Controls.Add(label2);
            Controls.Add(feedbackRating_numericUpDown);
            Controls.Add(feedbackComment_textBox);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "FeedbackForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Отзыв";
            ((System.ComponentModel.ISupportInitialize)feedbackRating_numericUpDown).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox feedbackComment_textBox;
        private NumericUpDown feedbackRating_numericUpDown;
        private Label label2;
        private Button createFeedback_button;
    }
}