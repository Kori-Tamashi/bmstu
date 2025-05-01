namespace Eventor.GUI
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
            saveDays_button = new Button();
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
            // saveDays_button
            // 
            saveDays_button.Location = new Point(12, 132);
            saveDays_button.Name = "saveDays_button";
            saveDays_button.Size = new Size(337, 29);
            saveDays_button.TabIndex = 1;
            saveDays_button.Text = "Подтвердить";
            saveDays_button.UseVisualStyleBackColor = true;
            saveDays_button.Click += saveDays_button_Click;
            // 
            // ParticipationForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(361, 171);
            Controls.Add(saveDays_button);
            Controls.Add(days_checkedListBox);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "ParticipationForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Дни присутствия";
            Load += ParticipationForm_Load;
            ResumeLayout(false);
        }

        #endregion

        private CheckedListBox days_checkedListBox;
        private Button saveDays_button;
    }
}