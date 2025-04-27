namespace Eventor.Eventor.GUI.ParticipationForm
{
    partial class ParticipationForm
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
            days_checkedListBox = new CheckedListBox();
            button1 = new Button();
            SuspendLayout();
            // 
            // days_checkedListBox
            // 
            days_checkedListBox.FormattingEnabled = true;
            days_checkedListBox.Location = new Point(12, 12);
            days_checkedListBox.Name = "days_checkedListBox";
            days_checkedListBox.Size = new Size(337, 114);
            days_checkedListBox.TabIndex = 0;
            // 
            // button1
            // 
            button1.Location = new Point(12, 132);
            button1.Name = "button1";
            button1.Size = new Size(337, 29);
            button1.TabIndex = 1;
            button1.Text = "Подтвердить";
            button1.UseVisualStyleBackColor = true;
            // 
            // ParticipationForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(361, 171);
            Controls.Add(button1);
            Controls.Add(days_checkedListBox);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "ParticipationForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Дни присутствия";
            ResumeLayout(false);
        }

        #endregion

        private CheckedListBox days_checkedListBox;
        private Button button1;
    }
}