namespace code
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.InteractionMenu_tabControl = new System.Windows.Forms.TabControl();
            this.Main_tabPage = new System.Windows.Forms.TabPage();
            this.View_tabPage = new System.Windows.Forms.TabPage();
            this.File_tabPage = new System.Windows.Forms.TabPage();
            this.InteractionMenu_tabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // InteractionMenu_tabControl
            // 
            this.InteractionMenu_tabControl.Controls.Add(this.Main_tabPage);
            this.InteractionMenu_tabControl.Controls.Add(this.View_tabPage);
            this.InteractionMenu_tabControl.Controls.Add(this.File_tabPage);
            this.InteractionMenu_tabControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.InteractionMenu_tabControl.Location = new System.Drawing.Point(12, 2);
            this.InteractionMenu_tabControl.Name = "InteractionMenu_tabControl";
            this.InteractionMenu_tabControl.SelectedIndex = 0;
            this.InteractionMenu_tabControl.Size = new System.Drawing.Size(1489, 136);
            this.InteractionMenu_tabControl.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.InteractionMenu_tabControl.TabIndex = 0;
            // 
            // Main_tabPage
            // 
            this.Main_tabPage.BackColor = System.Drawing.Color.Transparent;
            this.Main_tabPage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Main_tabPage.Location = new System.Drawing.Point(4, 29);
            this.Main_tabPage.Name = "Main_tabPage";
            this.Main_tabPage.Padding = new System.Windows.Forms.Padding(3);
            this.Main_tabPage.Size = new System.Drawing.Size(1481, 103);
            this.Main_tabPage.TabIndex = 0;
            this.Main_tabPage.Text = "Главная";
            // 
            // View_tabPage
            // 
            this.View_tabPage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.View_tabPage.ForeColor = System.Drawing.SystemColors.ControlText;
            this.View_tabPage.Location = new System.Drawing.Point(4, 29);
            this.View_tabPage.Name = "View_tabPage";
            this.View_tabPage.Padding = new System.Windows.Forms.Padding(3);
            this.View_tabPage.Size = new System.Drawing.Size(1481, 103);
            this.View_tabPage.TabIndex = 1;
            this.View_tabPage.Text = "Вид";
            this.View_tabPage.UseVisualStyleBackColor = true;
            // 
            // File_tabPage
            // 
            this.File_tabPage.BackColor = System.Drawing.Color.Transparent;
            this.File_tabPage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.File_tabPage.ForeColor = System.Drawing.SystemColors.ControlText;
            this.File_tabPage.Location = new System.Drawing.Point(4, 29);
            this.File_tabPage.Name = "File_tabPage";
            this.File_tabPage.Size = new System.Drawing.Size(1481, 103);
            this.File_tabPage.TabIndex = 2;
            this.File_tabPage.Text = "Файл";
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(1513, 830);
            this.Controls.Add(this.InteractionMenu_tabControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Конструктор композиции трехмерных многогранных примитивов ";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.InteractionMenu_tabControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl InteractionMenu_tabControl;
        private System.Windows.Forms.TabPage Main_tabPage;
        private System.Windows.Forms.TabPage View_tabPage;
        private System.Windows.Forms.TabPage File_tabPage;
    }
}

