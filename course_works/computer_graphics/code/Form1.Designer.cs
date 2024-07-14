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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.InteractionMenu_tabControl = new System.Windows.Forms.TabControl();
            this.Main_tabPage = new System.Windows.Forms.TabPage();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.Primitives_groupBox = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.Cube_button = new System.Windows.Forms.Button();
            this.triangularPyramid_button = new System.Windows.Forms.Button();
            this.directPrism_button = new System.Windows.Forms.Button();
            this.Icosahedron_button = new System.Windows.Forms.Button();
            this.truncatedPentagonalPyramid_button = new System.Windows.Forms.Button();
            this.inclinedPrism_button = new System.Windows.Forms.Button();
            this.View_tabPage = new System.Windows.Forms.TabPage();
            this.File_tabPage = new System.Windows.Forms.TabPage();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.picture = new System.Windows.Forms.PictureBox();
            this.InteractionMenu_tabControl.SuspendLayout();
            this.Main_tabPage.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.Primitives_groupBox.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picture)).BeginInit();
            this.SuspendLayout();
            // 
            // InteractionMenu_tabControl
            // 
            this.InteractionMenu_tabControl.Controls.Add(this.Main_tabPage);
            this.InteractionMenu_tabControl.Controls.Add(this.View_tabPage);
            this.InteractionMenu_tabControl.Controls.Add(this.File_tabPage);
            this.InteractionMenu_tabControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.InteractionMenu_tabControl.Location = new System.Drawing.Point(12, 12);
            this.InteractionMenu_tabControl.Name = "InteractionMenu_tabControl";
            this.InteractionMenu_tabControl.SelectedIndex = 0;
            this.InteractionMenu_tabControl.Size = new System.Drawing.Size(1489, 183);
            this.InteractionMenu_tabControl.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.InteractionMenu_tabControl.TabIndex = 0;
            // 
            // Main_tabPage
            // 
            this.Main_tabPage.BackColor = System.Drawing.Color.Transparent;
            this.Main_tabPage.Controls.Add(this.flowLayoutPanel2);
            this.Main_tabPage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Main_tabPage.Location = new System.Drawing.Point(4, 29);
            this.Main_tabPage.Name = "Main_tabPage";
            this.Main_tabPage.Padding = new System.Windows.Forms.Padding(3);
            this.Main_tabPage.Size = new System.Drawing.Size(1481, 150);
            this.Main_tabPage.TabIndex = 0;
            this.Main_tabPage.Text = "Главная";
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.Primitives_groupBox);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(4, 7);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(1471, 140);
            this.flowLayoutPanel2.TabIndex = 0;
            // 
            // Primitives_groupBox
            // 
            this.Primitives_groupBox.AutoSize = true;
            this.Primitives_groupBox.Controls.Add(this.flowLayoutPanel1);
            this.Primitives_groupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Primitives_groupBox.Location = new System.Drawing.Point(3, 3);
            this.Primitives_groupBox.Name = "Primitives_groupBox";
            this.Primitives_groupBox.Size = new System.Drawing.Size(180, 162);
            this.Primitives_groupBox.TabIndex = 1;
            this.Primitives_groupBox.TabStop = false;
            this.Primitives_groupBox.Text = "Примитивы";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.Cube_button);
            this.flowLayoutPanel1.Controls.Add(this.triangularPyramid_button);
            this.flowLayoutPanel1.Controls.Add(this.directPrism_button);
            this.flowLayoutPanel1.Controls.Add(this.Icosahedron_button);
            this.flowLayoutPanel1.Controls.Add(this.truncatedPentagonalPyramid_button);
            this.flowLayoutPanel1.Controls.Add(this.inclinedPrism_button);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(6, 24);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(168, 113);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // Cube_button
            // 
            this.Cube_button.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Cube_button.BackgroundImage")));
            this.Cube_button.Location = new System.Drawing.Point(3, 3);
            this.Cube_button.Name = "Cube_button";
            this.Cube_button.Size = new System.Drawing.Size(50, 50);
            this.Cube_button.TabIndex = 3;
            this.toolTip.SetToolTip(this.Cube_button, "Куб");
            this.Cube_button.UseVisualStyleBackColor = true;
            this.Cube_button.Click += new System.EventHandler(this.Cube_button_Click);
            // 
            // triangularPyramid_button
            // 
            this.triangularPyramid_button.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("triangularPyramid_button.BackgroundImage")));
            this.triangularPyramid_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.triangularPyramid_button.Location = new System.Drawing.Point(59, 3);
            this.triangularPyramid_button.Name = "triangularPyramid_button";
            this.triangularPyramid_button.Size = new System.Drawing.Size(50, 50);
            this.triangularPyramid_button.TabIndex = 4;
            this.toolTip.SetToolTip(this.triangularPyramid_button, "Треугольная пирамида");
            this.triangularPyramid_button.UseVisualStyleBackColor = true;
            this.triangularPyramid_button.Click += new System.EventHandler(this.triangularPyramid_button_Click);
            // 
            // directPrism_button
            // 
            this.directPrism_button.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("directPrism_button.BackgroundImage")));
            this.directPrism_button.Location = new System.Drawing.Point(115, 3);
            this.directPrism_button.Name = "directPrism_button";
            this.directPrism_button.Size = new System.Drawing.Size(50, 50);
            this.directPrism_button.TabIndex = 2;
            this.toolTip.SetToolTip(this.directPrism_button, "Прямая призма");
            this.directPrism_button.UseVisualStyleBackColor = true;
            // 
            // Icosahedron_button
            // 
            this.Icosahedron_button.AutoSize = true;
            this.Icosahedron_button.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Icosahedron_button.BackgroundImage")));
            this.Icosahedron_button.Location = new System.Drawing.Point(3, 59);
            this.Icosahedron_button.Name = "Icosahedron_button";
            this.Icosahedron_button.Size = new System.Drawing.Size(50, 50);
            this.Icosahedron_button.TabIndex = 5;
            this.toolTip.SetToolTip(this.Icosahedron_button, "Икосаэдр");
            this.Icosahedron_button.UseVisualStyleBackColor = true;
            // 
            // truncatedPentagonalPyramid_button
            // 
            this.truncatedPentagonalPyramid_button.AutoSize = true;
            this.truncatedPentagonalPyramid_button.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("truncatedPentagonalPyramid_button.BackgroundImage")));
            this.truncatedPentagonalPyramid_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.truncatedPentagonalPyramid_button.Location = new System.Drawing.Point(59, 59);
            this.truncatedPentagonalPyramid_button.Name = "truncatedPentagonalPyramid_button";
            this.truncatedPentagonalPyramid_button.Size = new System.Drawing.Size(50, 50);
            this.truncatedPentagonalPyramid_button.TabIndex = 6;
            this.toolTip.SetToolTip(this.truncatedPentagonalPyramid_button, "Пятиугольная усеченная пирамида");
            this.truncatedPentagonalPyramid_button.UseVisualStyleBackColor = true;
            // 
            // inclinedPrism_button
            // 
            this.inclinedPrism_button.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("inclinedPrism_button.BackgroundImage")));
            this.inclinedPrism_button.Location = new System.Drawing.Point(115, 59);
            this.inclinedPrism_button.Name = "inclinedPrism_button";
            this.inclinedPrism_button.Size = new System.Drawing.Size(50, 50);
            this.inclinedPrism_button.TabIndex = 4;
            this.toolTip.SetToolTip(this.inclinedPrism_button, "Наклонная призма");
            this.inclinedPrism_button.UseVisualStyleBackColor = true;
            // 
            // View_tabPage
            // 
            this.View_tabPage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.View_tabPage.ForeColor = System.Drawing.SystemColors.ControlText;
            this.View_tabPage.Location = new System.Drawing.Point(4, 29);
            this.View_tabPage.Name = "View_tabPage";
            this.View_tabPage.Padding = new System.Windows.Forms.Padding(3);
            this.View_tabPage.Size = new System.Drawing.Size(1481, 150);
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
            this.File_tabPage.Size = new System.Drawing.Size(1481, 150);
            this.File_tabPage.TabIndex = 2;
            this.File_tabPage.Text = "Файл";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.picture);
            this.panel1.Location = new System.Drawing.Point(12, 201);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1485, 617);
            this.panel1.TabIndex = 1;
            // 
            // picture
            // 
            this.picture.Cursor = System.Windows.Forms.Cursors.Cross;
            this.picture.Location = new System.Drawing.Point(0, 0);
            this.picture.Name = "picture";
            this.picture.Size = new System.Drawing.Size(1484, 616);
            this.picture.TabIndex = 0;
            this.picture.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(1513, 830);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.InteractionMenu_tabControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Конструктор композиции трехмерных многогранных примитивов ";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.InteractionMenu_tabControl.ResumeLayout(false);
            this.Main_tabPage.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.Primitives_groupBox.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picture)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl InteractionMenu_tabControl;
        private System.Windows.Forms.TabPage Main_tabPage;
        private System.Windows.Forms.TabPage View_tabPage;
        private System.Windows.Forms.TabPage File_tabPage;
        private System.Windows.Forms.GroupBox Primitives_groupBox;
        private System.Windows.Forms.Button directPrism_button;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button Cube_button;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button inclinedPrism_button;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button triangularPyramid_button;
        private System.Windows.Forms.Button Icosahedron_button;
        private System.Windows.Forms.Button truncatedPentagonalPyramid_button;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox picture;
    }
}

