namespace code
{
    partial class ErrorMessage
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
            richTextBox1 = new RichTextBox();
            SuspendLayout();
            // 
            // richTextBox1
            // 
            richTextBox1.BackColor = SystemColors.Control;
            richTextBox1.BorderStyle = BorderStyle.None;
            richTextBox1.Cursor = Cursors.Help;
            richTextBox1.Font = new Font("Segoe UI", 10F);
            richTextBox1.Location = new Point(12, 12);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.ReadOnly = true;
            richTextBox1.Size = new Size(504, 126);
            richTextBox1.TabIndex = 0;
            richTextBox1.Text = "";
            // 
            // ErrorMessage
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(528, 150);
            Controls.Add(richTextBox1);
            MaximizeBox = false;
            MaximumSize = new Size(546, 197);
            MinimizeBox = false;
            MinimumSize = new Size(546, 197);
            Name = "ErrorMessage";
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Show;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Ошибка";
            TopMost = true;
            Load += ErrorMessage_Load;
            ResumeLayout(false);
        }

        #endregion

        private RichTextBox richTextBox1;
    }
}