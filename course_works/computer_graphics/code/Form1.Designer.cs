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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            InteractionMenu_tabControl = new TabControl();
            Main_tabPage = new TabPage();
            flowLayoutPanel2 = new FlowLayoutPanel();
            Primitives_groupBox = new GroupBox();
            flowLayoutPanel1 = new FlowLayoutPanel();
            Cube_button = new Button();
            triangularPyramid_button = new Button();
            directPrism_button = new Button();
            Icosahedron_button = new Button();
            truncatedPentagonalPyramid_button = new Button();
            inclinedPrism_button = new Button();
            View_tabPage = new TabPage();
            File_tabPage = new TabPage();
            colorDialog1 = new ColorDialog();
            toolTip = new ToolTip(components);
            panel1 = new Panel();
            picture = new PictureBox();
            InteractionMenu_tabControl.SuspendLayout();
            Main_tabPage.SuspendLayout();
            flowLayoutPanel2.SuspendLayout();
            Primitives_groupBox.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picture).BeginInit();
            SuspendLayout();
            // 
            // InteractionMenu_tabControl
            // 
            InteractionMenu_tabControl.Controls.Add(Main_tabPage);
            InteractionMenu_tabControl.Controls.Add(View_tabPage);
            InteractionMenu_tabControl.Controls.Add(File_tabPage);
            InteractionMenu_tabControl.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold, GraphicsUnit.Point, 204);
            InteractionMenu_tabControl.Location = new Point(12, 12);
            InteractionMenu_tabControl.Name = "InteractionMenu_tabControl";
            InteractionMenu_tabControl.SelectedIndex = 0;
            InteractionMenu_tabControl.Size = new Size(1489, 183);
            InteractionMenu_tabControl.SizeMode = TabSizeMode.FillToRight;
            InteractionMenu_tabControl.TabIndex = 0;
            // 
            // Main_tabPage
            // 
            Main_tabPage.BackColor = Color.Transparent;
            Main_tabPage.Controls.Add(flowLayoutPanel2);
            Main_tabPage.Cursor = Cursors.Hand;
            Main_tabPage.Location = new Point(4, 29);
            Main_tabPage.Name = "Main_tabPage";
            Main_tabPage.Padding = new Padding(3);
            Main_tabPage.Size = new Size(1481, 150);
            Main_tabPage.TabIndex = 0;
            Main_tabPage.Text = "Главная";
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.Controls.Add(Primitives_groupBox);
            flowLayoutPanel2.Location = new Point(4, 7);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new Size(1471, 140);
            flowLayoutPanel2.TabIndex = 0;
            // 
            // Primitives_groupBox
            // 
            Primitives_groupBox.AutoSize = true;
            Primitives_groupBox.Controls.Add(flowLayoutPanel1);
            Primitives_groupBox.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
            Primitives_groupBox.Location = new Point(3, 3);
            Primitives_groupBox.Name = "Primitives_groupBox";
            Primitives_groupBox.Size = new Size(180, 162);
            Primitives_groupBox.TabIndex = 1;
            Primitives_groupBox.TabStop = false;
            Primitives_groupBox.Text = "Примитивы";
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(Cube_button);
            flowLayoutPanel1.Controls.Add(triangularPyramid_button);
            flowLayoutPanel1.Controls.Add(directPrism_button);
            flowLayoutPanel1.Controls.Add(Icosahedron_button);
            flowLayoutPanel1.Controls.Add(truncatedPentagonalPyramid_button);
            flowLayoutPanel1.Controls.Add(inclinedPrism_button);
            flowLayoutPanel1.Location = new Point(6, 24);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(168, 113);
            flowLayoutPanel1.TabIndex = 2;
            // 
            // Cube_button
            // 
            Cube_button.BackgroundImage = (Image)resources.GetObject("Cube_button.BackgroundImage");
            Cube_button.Location = new Point(3, 3);
            Cube_button.Name = "Cube_button";
            Cube_button.Size = new Size(50, 50);
            Cube_button.TabIndex = 3;
            toolTip.SetToolTip(Cube_button, "Куб");
            Cube_button.UseVisualStyleBackColor = true;
            Cube_button.Click += Cube_button_Click;
            // 
            // triangularPyramid_button
            // 
            triangularPyramid_button.BackgroundImage = (Image)resources.GetObject("triangularPyramid_button.BackgroundImage");
            triangularPyramid_button.BackgroundImageLayout = ImageLayout.Stretch;
            triangularPyramid_button.Location = new Point(59, 3);
            triangularPyramid_button.Name = "triangularPyramid_button";
            triangularPyramid_button.Size = new Size(50, 50);
            triangularPyramid_button.TabIndex = 4;
            toolTip.SetToolTip(triangularPyramid_button, "Треугольная пирамида");
            triangularPyramid_button.UseVisualStyleBackColor = true;
            triangularPyramid_button.Click += triangularPyramid_button_Click;
            // 
            // directPrism_button
            // 
            directPrism_button.BackgroundImage = (Image)resources.GetObject("directPrism_button.BackgroundImage");
            directPrism_button.Location = new Point(115, 3);
            directPrism_button.Name = "directPrism_button";
            directPrism_button.Size = new Size(50, 50);
            directPrism_button.TabIndex = 2;
            toolTip.SetToolTip(directPrism_button, "Прямая призма");
            directPrism_button.UseVisualStyleBackColor = true;
            directPrism_button.Click += directPrism_button_Click;
            // 
            // Icosahedron_button
            // 
            Icosahedron_button.AutoSize = true;
            Icosahedron_button.BackgroundImage = (Image)resources.GetObject("Icosahedron_button.BackgroundImage");
            Icosahedron_button.Location = new Point(3, 59);
            Icosahedron_button.Name = "Icosahedron_button";
            Icosahedron_button.Size = new Size(50, 50);
            Icosahedron_button.TabIndex = 5;
            toolTip.SetToolTip(Icosahedron_button, "Икосаэдр");
            Icosahedron_button.UseVisualStyleBackColor = true;
            // 
            // truncatedPentagonalPyramid_button
            // 
            truncatedPentagonalPyramid_button.AutoSize = true;
            truncatedPentagonalPyramid_button.BackgroundImage = (Image)resources.GetObject("truncatedPentagonalPyramid_button.BackgroundImage");
            truncatedPentagonalPyramid_button.BackgroundImageLayout = ImageLayout.Stretch;
            truncatedPentagonalPyramid_button.Location = new Point(59, 59);
            truncatedPentagonalPyramid_button.Name = "truncatedPentagonalPyramid_button";
            truncatedPentagonalPyramid_button.Size = new Size(50, 50);
            truncatedPentagonalPyramid_button.TabIndex = 6;
            toolTip.SetToolTip(truncatedPentagonalPyramid_button, "Пятиугольная усеченная пирамида");
            truncatedPentagonalPyramid_button.UseVisualStyleBackColor = true;
            // 
            // inclinedPrism_button
            // 
            inclinedPrism_button.BackgroundImage = (Image)resources.GetObject("inclinedPrism_button.BackgroundImage");
            inclinedPrism_button.Location = new Point(115, 59);
            inclinedPrism_button.Name = "inclinedPrism_button";
            inclinedPrism_button.Size = new Size(50, 50);
            inclinedPrism_button.TabIndex = 4;
            toolTip.SetToolTip(inclinedPrism_button, "Наклонная призма");
            inclinedPrism_button.UseVisualStyleBackColor = true;
            // 
            // View_tabPage
            // 
            View_tabPage.Cursor = Cursors.Hand;
            View_tabPage.ForeColor = SystemColors.ControlText;
            View_tabPage.Location = new Point(4, 29);
            View_tabPage.Name = "View_tabPage";
            View_tabPage.Padding = new Padding(3);
            View_tabPage.Size = new Size(1481, 150);
            View_tabPage.TabIndex = 1;
            View_tabPage.Text = "Вид";
            View_tabPage.UseVisualStyleBackColor = true;
            // 
            // File_tabPage
            // 
            File_tabPage.BackColor = Color.Transparent;
            File_tabPage.Cursor = Cursors.Hand;
            File_tabPage.ForeColor = SystemColors.ControlText;
            File_tabPage.Location = new Point(4, 29);
            File_tabPage.Name = "File_tabPage";
            File_tabPage.Size = new Size(1481, 150);
            File_tabPage.TabIndex = 2;
            File_tabPage.Text = "Файл";
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(picture);
            panel1.Location = new Point(12, 201);
            panel1.Name = "panel1";
            panel1.Size = new Size(1485, 617);
            panel1.TabIndex = 1;
            // 
            // picture
            // 
            picture.Cursor = Cursors.Cross;
            picture.Location = new Point(0, 0);
            picture.Name = "picture";
            picture.Size = new Size(1484, 616);
            picture.TabIndex = 0;
            picture.TabStop = false;
            // 
            // Form1
            // 
            AutoScaleMode = AutoScaleMode.Inherit;
            ClientSize = new Size(1513, 830);
            Controls.Add(panel1);
            Controls.Add(InteractionMenu_tabControl);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4);
            Name = "Form1";
            Text = "Конструктор композиции трехмерных многогранных примитивов ";
            Load += Form1_Load;
            InteractionMenu_tabControl.ResumeLayout(false);
            Main_tabPage.ResumeLayout(false);
            flowLayoutPanel2.ResumeLayout(false);
            flowLayoutPanel2.PerformLayout();
            Primitives_groupBox.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picture).EndInit();
            ResumeLayout(false);
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

