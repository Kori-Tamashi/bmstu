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
            groupBox6 = new GroupBox();
            button6 = new Button();
            button5 = new Button();
            button_Clear = new Button();
            button_dialogEdit = new Button();
            Primitives_groupBox = new GroupBox();
            flowLayoutPanel1 = new FlowLayoutPanel();
            Cube_button = new Button();
            triangularPyramid_button = new Button();
            directPrism_button = new Button();
            Icosahedron_button = new Button();
            truncatedPentagonalPyramid_button = new Button();
            inclinedPrism_button = new Button();
            listView_modelsMain = new ListView();
            imageList = new ImageList(components);
            groupBox3 = new GroupBox();
            button_moveModel = new Button();
            tableLayoutPanel2 = new TableLayoutPanel();
            label2 = new Label();
            numericUpDown_moveZ = new NumericUpDown();
            numericUpDown_moveY = new NumericUpDown();
            numericUpDown_moveX = new NumericUpDown();
            label6 = new Label();
            label4 = new Label();
            groupBox4 = new GroupBox();
            button_rotateModel = new Button();
            tableLayoutPanel7 = new TableLayoutPanel();
            label5 = new Label();
            numericUpDown_angleOz = new NumericUpDown();
            numericUpDown_angleOy = new NumericUpDown();
            numericUpDown_angleOx = new NumericUpDown();
            label8 = new Label();
            label9 = new Label();
            groupBox5 = new GroupBox();
            button_scaleModel = new Button();
            tableLayoutPanel3 = new TableLayoutPanel();
            label3 = new Label();
            numericUpDown_scaleZ = new NumericUpDown();
            numericUpDown_scaleY = new NumericUpDown();
            numericUpDown_scaleX = new NumericUpDown();
            label10 = new Label();
            label11 = new Label();
            View_tabPage = new TabPage();
            tableLayoutPanel4 = new TableLayoutPanel();
            button_cameraRIghtDown = new Button();
            button_cameraDown = new Button();
            button_cameraLeftDown = new Button();
            button_cameraRight = new Button();
            button_CmeraLeft = new Button();
            button_cameraRightUp = new Button();
            button_cameraUp = new Button();
            button_cameraLeftUp = new Button();
            File_tabPage = new TabPage();
            colorDialog1 = new ColorDialog();
            toolTip = new ToolTip(components);
            panel1 = new Panel();
            picture = new PictureBox();
            groupBox1 = new GroupBox();
            button_move = new Button();
            tableLayoutPanel6 = new TableLayoutPanel();
            label7 = new Label();
            numericUpDown2 = new NumericUpDown();
            button1 = new Button();
            tableLayoutPanel1 = new TableLayoutPanel();
            label1 = new Label();
            numericUpDown1 = new NumericUpDown();
            groupBox2 = new GroupBox();
            InteractionMenu_tabControl.SuspendLayout();
            Main_tabPage.SuspendLayout();
            flowLayoutPanel2.SuspendLayout();
            groupBox6.SuspendLayout();
            Primitives_groupBox.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            groupBox3.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_moveZ).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_moveY).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_moveX).BeginInit();
            groupBox4.SuspendLayout();
            tableLayoutPanel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_angleOz).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_angleOy).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_angleOx).BeginInit();
            groupBox5.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_scaleZ).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_scaleY).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_scaleX).BeginInit();
            View_tabPage.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picture).BeginInit();
            groupBox1.SuspendLayout();
            tableLayoutPanel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            groupBox2.SuspendLayout();
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
            InteractionMenu_tabControl.Size = new Size(1489, 220);
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
            Main_tabPage.Size = new Size(1481, 187);
            Main_tabPage.TabIndex = 0;
            Main_tabPage.Text = "Главная";
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.Anchor = AnchorStyles.None;
            flowLayoutPanel2.Controls.Add(groupBox6);
            flowLayoutPanel2.Controls.Add(Primitives_groupBox);
            flowLayoutPanel2.Controls.Add(listView_modelsMain);
            flowLayoutPanel2.Controls.Add(groupBox3);
            flowLayoutPanel2.Controls.Add(groupBox4);
            flowLayoutPanel2.Controls.Add(groupBox5);
            flowLayoutPanel2.Location = new Point(3, 2);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new Size(1471, 182);
            flowLayoutPanel2.TabIndex = 0;
            flowLayoutPanel2.Paint += flowLayoutPanel2_Paint;
            // 
            // groupBox6
            // 
            groupBox6.Anchor = AnchorStyles.None;
            groupBox6.Controls.Add(button6);
            groupBox6.Controls.Add(button5);
            groupBox6.Controls.Add(button_Clear);
            groupBox6.Controls.Add(button_dialogEdit);
            groupBox6.Font = new Font("Segoe UI", 10F);
            groupBox6.Location = new Point(3, 3);
            groupBox6.Name = "groupBox6";
            groupBox6.Size = new Size(196, 179);
            groupBox6.TabIndex = 8;
            groupBox6.TabStop = false;
            groupBox6.Text = "Действия";
            // 
            // button6
            // 
            button6.Location = new Point(3, 129);
            button6.Name = "button6";
            button6.Size = new Size(187, 29);
            button6.TabIndex = 3;
            button6.Text = "button6";
            button6.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            button5.Location = new Point(3, 94);
            button5.Name = "button5";
            button5.Size = new Size(187, 29);
            button5.TabIndex = 2;
            button5.Text = "Обработать полотно";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // button_Clear
            // 
            button_Clear.Location = new Point(3, 24);
            button_Clear.Name = "button_Clear";
            button_Clear.Size = new Size(187, 29);
            button_Clear.TabIndex = 1;
            button_Clear.Text = "Очистка";
            button_Clear.UseVisualStyleBackColor = true;
            button_Clear.Click += button_Clear_Click;
            // 
            // button_dialogEdit
            // 
            button_dialogEdit.Location = new Point(3, 59);
            button_dialogEdit.Name = "button_dialogEdit";
            button_dialogEdit.Size = new Size(187, 29);
            button_dialogEdit.TabIndex = 0;
            button_dialogEdit.Text = "Редактор моделей";
            button_dialogEdit.UseVisualStyleBackColor = true;
            button_dialogEdit.Click += button_dialogEdit_Click;
            // 
            // Primitives_groupBox
            // 
            Primitives_groupBox.AutoSize = true;
            Primitives_groupBox.Controls.Add(flowLayoutPanel1);
            Primitives_groupBox.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
            Primitives_groupBox.Location = new Point(205, 3);
            Primitives_groupBox.Name = "Primitives_groupBox";
            Primitives_groupBox.Size = new Size(180, 179);
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
            flowLayoutPanel1.Size = new Size(168, 130);
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
            Icosahedron_button.Click += Icosahedron_button_Click;
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
            inclinedPrism_button.Click += inclinedPrism_button_Click;
            // 
            // listView_modelsMain
            // 
            listView_modelsMain.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            listView_modelsMain.GridLines = true;
            listView_modelsMain.LargeImageList = imageList;
            listView_modelsMain.Location = new Point(391, 3);
            listView_modelsMain.MultiSelect = false;
            listView_modelsMain.Name = "listView_modelsMain";
            listView_modelsMain.Size = new Size(338, 175);
            listView_modelsMain.SmallImageList = imageList;
            listView_modelsMain.TabIndex = 4;
            listView_modelsMain.UseCompatibleStateImageBehavior = false;
            // 
            // imageList
            // 
            imageList.ColorDepth = ColorDepth.Depth32Bit;
            imageList.ImageStream = (ImageListStreamer)resources.GetObject("imageList.ImageStream");
            imageList.TransparentColor = Color.Transparent;
            imageList.Images.SetKeyName(0, "cube.png");
            imageList.Images.SetKeyName(1, "direct-prism.png");
            imageList.Images.SetKeyName(2, "inclined-prism.png");
            imageList.Images.SetKeyName(3, "triangular-pyramid.png");
            imageList.Images.SetKeyName(4, "truncated-pentagonal-pyramid.png");
            imageList.Images.SetKeyName(5, "icosahedron.png");
            imageList.Images.SetKeyName(6, "arrow-down.png");
            imageList.Images.SetKeyName(7, "arrow-left.png");
            imageList.Images.SetKeyName(8, "arrow-leftDown.png");
            imageList.Images.SetKeyName(9, "arrow-leftUp.png");
            imageList.Images.SetKeyName(10, "arrow-right.png");
            imageList.Images.SetKeyName(11, "arrow-rightDown.png");
            imageList.Images.SetKeyName(12, "arrow-rightUp.png");
            imageList.Images.SetKeyName(13, "arrow-up.png");
            // 
            // groupBox3
            // 
            groupBox3.Anchor = AnchorStyles.None;
            groupBox3.Controls.Add(button_moveModel);
            groupBox3.Controls.Add(tableLayoutPanel2);
            groupBox3.Font = new Font("Segoe UI", 10F);
            groupBox3.Location = new Point(735, 3);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(196, 178);
            groupBox3.TabIndex = 5;
            groupBox3.TabStop = false;
            groupBox3.Text = "Перемещение";
            // 
            // button_moveModel
            // 
            button_moveModel.Font = new Font("Segoe UI", 10F);
            button_moveModel.Location = new Point(6, 143);
            button_moveModel.Name = "button_moveModel";
            button_moveModel.Size = new Size(184, 29);
            button_moveModel.TabIndex = 5;
            button_moveModel.Text = "Переместить";
            button_moveModel.UseVisualStyleBackColor = true;
            button_moveModel.Click += button_moveModel_Click;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel2.CellBorderStyle = TableLayoutPanelCellBorderStyle.Outset;
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 28F));
            tableLayoutPanel2.Controls.Add(label2, 0, 2);
            tableLayoutPanel2.Controls.Add(numericUpDown_moveZ, 1, 2);
            tableLayoutPanel2.Controls.Add(numericUpDown_moveY, 1, 1);
            tableLayoutPanel2.Controls.Add(numericUpDown_moveX, 1, 0);
            tableLayoutPanel2.Controls.Add(label6, 0, 1);
            tableLayoutPanel2.Controls.Add(label4, 0, 0);
            tableLayoutPanel2.Location = new Point(6, 26);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 3;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tableLayoutPanel2.Size = new Size(180, 111);
            tableLayoutPanel2.TabIndex = 3;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.None;
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10F);
            label2.Location = new Point(6, 80);
            label2.Name = "label2";
            label2.Size = new Size(31, 23);
            label2.TabIndex = 7;
            label2.Text = "ΔZ";
            // 
            // numericUpDown_moveZ
            // 
            numericUpDown_moveZ.Font = new Font("Segoe UI", 10F);
            numericUpDown_moveZ.Location = new Point(47, 77);
            numericUpDown_moveZ.Maximum = new decimal(new int[] { 99999999, 0, 0, 0 });
            numericUpDown_moveZ.Minimum = new decimal(new int[] { 99999999, 0, 0, int.MinValue });
            numericUpDown_moveZ.Name = "numericUpDown_moveZ";
            numericUpDown_moveZ.Size = new Size(128, 30);
            numericUpDown_moveZ.TabIndex = 8;
            // 
            // numericUpDown_moveY
            // 
            numericUpDown_moveY.Font = new Font("Segoe UI", 10F);
            numericUpDown_moveY.Location = new Point(47, 41);
            numericUpDown_moveY.Maximum = new decimal(new int[] { 99999999, 0, 0, 0 });
            numericUpDown_moveY.Minimum = new decimal(new int[] { 99999999, 0, 0, int.MinValue });
            numericUpDown_moveY.Name = "numericUpDown_moveY";
            numericUpDown_moveY.Size = new Size(128, 30);
            numericUpDown_moveY.TabIndex = 7;
            // 
            // numericUpDown_moveX
            // 
            numericUpDown_moveX.Font = new Font("Segoe UI", 10F);
            numericUpDown_moveX.Location = new Point(47, 5);
            numericUpDown_moveX.Maximum = new decimal(new int[] { 99999999, 0, 0, 0 });
            numericUpDown_moveX.Minimum = new decimal(new int[] { 99999999, 0, 0, int.MinValue });
            numericUpDown_moveX.Name = "numericUpDown_moveX";
            numericUpDown_moveX.Size = new Size(128, 30);
            numericUpDown_moveX.TabIndex = 6;
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.None;
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 10F);
            label6.Location = new Point(7, 43);
            label6.Name = "label6";
            label6.Size = new Size(30, 23);
            label6.TabIndex = 6;
            label6.Text = "ΔY";
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.None;
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 10F);
            label4.Location = new Point(6, 7);
            label4.Name = "label4";
            label4.Size = new Size(31, 23);
            label4.TabIndex = 4;
            label4.Text = "ΔX";
            // 
            // groupBox4
            // 
            groupBox4.Anchor = AnchorStyles.None;
            groupBox4.Controls.Add(button_rotateModel);
            groupBox4.Controls.Add(tableLayoutPanel7);
            groupBox4.Font = new Font("Segoe UI", 10F);
            groupBox4.Location = new Point(937, 3);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(196, 179);
            groupBox4.TabIndex = 6;
            groupBox4.TabStop = false;
            groupBox4.Text = "Поворот";
            // 
            // button_rotateModel
            // 
            button_rotateModel.Font = new Font("Segoe UI", 9F);
            button_rotateModel.Location = new Point(6, 143);
            button_rotateModel.Name = "button_rotateModel";
            button_rotateModel.Size = new Size(180, 29);
            button_rotateModel.TabIndex = 5;
            button_rotateModel.Text = "Повернуть";
            button_rotateModel.UseVisualStyleBackColor = true;
            button_rotateModel.Click += button_rotateModel_Click;
            // 
            // tableLayoutPanel7
            // 
            tableLayoutPanel7.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel7.CellBorderStyle = TableLayoutPanelCellBorderStyle.Outset;
            tableLayoutPanel7.ColumnCount = 2;
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 55F));
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 21F));
            tableLayoutPanel7.Controls.Add(label5, 0, 2);
            tableLayoutPanel7.Controls.Add(numericUpDown_angleOz, 1, 2);
            tableLayoutPanel7.Controls.Add(numericUpDown_angleOy, 1, 1);
            tableLayoutPanel7.Controls.Add(numericUpDown_angleOx, 1, 0);
            tableLayoutPanel7.Controls.Add(label8, 0, 1);
            tableLayoutPanel7.Controls.Add(label9, 0, 0);
            tableLayoutPanel7.Location = new Point(6, 26);
            tableLayoutPanel7.Name = "tableLayoutPanel7";
            tableLayoutPanel7.RowCount = 3;
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tableLayoutPanel7.Size = new Size(176, 111);
            tableLayoutPanel7.TabIndex = 3;
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.None;
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 10F);
            label5.Location = new Point(7, 80);
            label5.Name = "label5";
            label5.Size = new Size(45, 23);
            label5.TabIndex = 7;
            label5.Text = "∠OZ";
            // 
            // numericUpDown_angleOz
            // 
            numericUpDown_angleOz.Font = new Font("Segoe UI", 10F);
            numericUpDown_angleOz.Location = new Point(62, 77);
            numericUpDown_angleOz.Maximum = new decimal(new int[] { 99999999, 0, 0, 0 });
            numericUpDown_angleOz.Minimum = new decimal(new int[] { 99999999, 0, 0, int.MinValue });
            numericUpDown_angleOz.Name = "numericUpDown_angleOz";
            numericUpDown_angleOz.Size = new Size(109, 30);
            numericUpDown_angleOz.TabIndex = 8;
            // 
            // numericUpDown_angleOy
            // 
            numericUpDown_angleOy.Font = new Font("Segoe UI", 10F);
            numericUpDown_angleOy.Location = new Point(62, 41);
            numericUpDown_angleOy.Maximum = new decimal(new int[] { 99999999, 0, 0, 0 });
            numericUpDown_angleOy.Minimum = new decimal(new int[] { 99999999, 0, 0, int.MinValue });
            numericUpDown_angleOy.Name = "numericUpDown_angleOy";
            numericUpDown_angleOy.Size = new Size(109, 30);
            numericUpDown_angleOy.TabIndex = 7;
            // 
            // numericUpDown_angleOx
            // 
            numericUpDown_angleOx.Font = new Font("Segoe UI", 10F);
            numericUpDown_angleOx.Location = new Point(62, 5);
            numericUpDown_angleOx.Maximum = new decimal(new int[] { 99999999, 0, 0, 0 });
            numericUpDown_angleOx.Minimum = new decimal(new int[] { 99999999, 0, 0, int.MinValue });
            numericUpDown_angleOx.Name = "numericUpDown_angleOx";
            numericUpDown_angleOx.Size = new Size(109, 30);
            numericUpDown_angleOx.TabIndex = 6;
            // 
            // label8
            // 
            label8.Anchor = AnchorStyles.None;
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 10F);
            label8.Location = new Point(7, 43);
            label8.Name = "label8";
            label8.Size = new Size(44, 23);
            label8.TabIndex = 6;
            label8.Text = "∠OY";
            // 
            // label9
            // 
            label9.Anchor = AnchorStyles.None;
            label9.AutoSize = true;
            label9.Font = new Font("Segoe UI", 10F);
            label9.Location = new Point(7, 7);
            label9.Name = "label9";
            label9.Size = new Size(45, 23);
            label9.TabIndex = 4;
            label9.Text = "∠OX";
            // 
            // groupBox5
            // 
            groupBox5.Anchor = AnchorStyles.None;
            groupBox5.Controls.Add(button_scaleModel);
            groupBox5.Controls.Add(tableLayoutPanel3);
            groupBox5.Font = new Font("Segoe UI", 10F);
            groupBox5.Location = new Point(1139, 3);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(196, 179);
            groupBox5.TabIndex = 7;
            groupBox5.TabStop = false;
            groupBox5.Text = "Масштабирование";
            // 
            // button_scaleModel
            // 
            button_scaleModel.Font = new Font("Segoe UI", 9F);
            button_scaleModel.Location = new Point(6, 143);
            button_scaleModel.Name = "button_scaleModel";
            button_scaleModel.Size = new Size(180, 29);
            button_scaleModel.TabIndex = 5;
            button_scaleModel.Text = "Масштабировать";
            button_scaleModel.UseVisualStyleBackColor = true;
            button_scaleModel.Click += button_scaleModel_Click;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel3.CellBorderStyle = TableLayoutPanelCellBorderStyle.Outset;
            tableLayoutPanel3.ColumnCount = 2;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 55F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 21F));
            tableLayoutPanel3.Controls.Add(label3, 0, 2);
            tableLayoutPanel3.Controls.Add(numericUpDown_scaleZ, 1, 2);
            tableLayoutPanel3.Controls.Add(numericUpDown_scaleY, 1, 1);
            tableLayoutPanel3.Controls.Add(numericUpDown_scaleX, 1, 0);
            tableLayoutPanel3.Controls.Add(label10, 0, 1);
            tableLayoutPanel3.Controls.Add(label11, 0, 0);
            tableLayoutPanel3.Location = new Point(6, 26);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 3;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tableLayoutPanel3.Size = new Size(184, 111);
            tableLayoutPanel3.TabIndex = 3;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.None;
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 10F);
            label3.Location = new Point(14, 80);
            label3.Name = "label3";
            label3.Size = new Size(30, 23);
            label3.TabIndex = 7;
            label3.Text = "KZ";
            // 
            // numericUpDown_scaleZ
            // 
            numericUpDown_scaleZ.Font = new Font("Segoe UI", 10F);
            numericUpDown_scaleZ.Location = new Point(62, 77);
            numericUpDown_scaleZ.Maximum = new decimal(new int[] { 99999999, 0, 0, 0 });
            numericUpDown_scaleZ.Minimum = new decimal(new int[] { 99999999, 0, 0, int.MinValue });
            numericUpDown_scaleZ.Name = "numericUpDown_scaleZ";
            numericUpDown_scaleZ.Size = new Size(109, 30);
            numericUpDown_scaleZ.TabIndex = 8;
            numericUpDown_scaleZ.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // numericUpDown_scaleY
            // 
            numericUpDown_scaleY.Font = new Font("Segoe UI", 10F);
            numericUpDown_scaleY.Location = new Point(62, 41);
            numericUpDown_scaleY.Maximum = new decimal(new int[] { 99999999, 0, 0, 0 });
            numericUpDown_scaleY.Minimum = new decimal(new int[] { 99999999, 0, 0, int.MinValue });
            numericUpDown_scaleY.Name = "numericUpDown_scaleY";
            numericUpDown_scaleY.Size = new Size(109, 30);
            numericUpDown_scaleY.TabIndex = 7;
            numericUpDown_scaleY.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // numericUpDown_scaleX
            // 
            numericUpDown_scaleX.Font = new Font("Segoe UI", 10F);
            numericUpDown_scaleX.Location = new Point(62, 5);
            numericUpDown_scaleX.Maximum = new decimal(new int[] { 99999999, 0, 0, 0 });
            numericUpDown_scaleX.Minimum = new decimal(new int[] { 99999999, 0, 0, int.MinValue });
            numericUpDown_scaleX.Name = "numericUpDown_scaleX";
            numericUpDown_scaleX.Size = new Size(109, 30);
            numericUpDown_scaleX.TabIndex = 6;
            numericUpDown_scaleX.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // label10
            // 
            label10.Anchor = AnchorStyles.None;
            label10.AutoSize = true;
            label10.Font = new Font("Segoe UI", 10F);
            label10.Location = new Point(15, 43);
            label10.Name = "label10";
            label10.Size = new Size(29, 23);
            label10.TabIndex = 6;
            label10.Text = "KY";
            // 
            // label11
            // 
            label11.Anchor = AnchorStyles.None;
            label11.AutoSize = true;
            label11.Font = new Font("Segoe UI", 10F);
            label11.Location = new Point(14, 7);
            label11.Name = "label11";
            label11.Size = new Size(30, 23);
            label11.TabIndex = 4;
            label11.Text = "KX";
            // 
            // View_tabPage
            // 
            View_tabPage.Controls.Add(tableLayoutPanel4);
            View_tabPage.Cursor = Cursors.Hand;
            View_tabPage.ForeColor = SystemColors.ControlText;
            View_tabPage.Location = new Point(4, 29);
            View_tabPage.Name = "View_tabPage";
            View_tabPage.Padding = new Padding(3);
            View_tabPage.Size = new Size(1481, 187);
            View_tabPage.TabIndex = 1;
            View_tabPage.Text = "Вид";
            View_tabPage.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tableLayoutPanel4.ColumnCount = 3;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333359F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel4.Controls.Add(button_cameraRIghtDown, 2, 2);
            tableLayoutPanel4.Controls.Add(button_cameraDown, 1, 2);
            tableLayoutPanel4.Controls.Add(button_cameraLeftDown, 0, 2);
            tableLayoutPanel4.Controls.Add(button_cameraRight, 2, 1);
            tableLayoutPanel4.Controls.Add(button_CmeraLeft, 0, 1);
            tableLayoutPanel4.Controls.Add(button_cameraRightUp, 2, 0);
            tableLayoutPanel4.Controls.Add(button_cameraUp, 1, 0);
            tableLayoutPanel4.Controls.Add(button_cameraLeftUp, 0, 0);
            tableLayoutPanel4.Location = new Point(401, 6);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 3;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333359F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333359F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333359F));
            tableLayoutPanel4.Size = new Size(180, 175);
            tableLayoutPanel4.TabIndex = 2;
            // 
            // button_cameraRIghtDown
            // 
            button_cameraRIghtDown.ImageKey = "arrow-rightDown.png";
            button_cameraRIghtDown.ImageList = imageList;
            button_cameraRIghtDown.Location = new Point(122, 120);
            button_cameraRIghtDown.Name = "button_cameraRIghtDown";
            button_cameraRIghtDown.Size = new Size(54, 51);
            button_cameraRIghtDown.TabIndex = 11;
            button_cameraRIghtDown.UseVisualStyleBackColor = true;
            // 
            // button_cameraDown
            // 
            button_cameraDown.ImageKey = "arrow-down.png";
            button_cameraDown.ImageList = imageList;
            button_cameraDown.Location = new Point(63, 120);
            button_cameraDown.Name = "button_cameraDown";
            button_cameraDown.Size = new Size(52, 51);
            button_cameraDown.TabIndex = 10;
            button_cameraDown.UseVisualStyleBackColor = true;
            // 
            // button_cameraLeftDown
            // 
            button_cameraLeftDown.ImageKey = "arrow-leftDown.png";
            button_cameraLeftDown.ImageList = imageList;
            button_cameraLeftDown.Location = new Point(4, 120);
            button_cameraLeftDown.Name = "button_cameraLeftDown";
            button_cameraLeftDown.Size = new Size(52, 51);
            button_cameraLeftDown.TabIndex = 9;
            button_cameraLeftDown.UseVisualStyleBackColor = true;
            // 
            // button_cameraRight
            // 
            button_cameraRight.ImageKey = "arrow-right.png";
            button_cameraRight.ImageList = imageList;
            button_cameraRight.Location = new Point(122, 62);
            button_cameraRight.Name = "button_cameraRight";
            button_cameraRight.Size = new Size(54, 51);
            button_cameraRight.TabIndex = 8;
            button_cameraRight.UseVisualStyleBackColor = true;
            // 
            // button_CmeraLeft
            // 
            button_CmeraLeft.ImageKey = "arrow-left.png";
            button_CmeraLeft.ImageList = imageList;
            button_CmeraLeft.Location = new Point(4, 62);
            button_CmeraLeft.Name = "button_CmeraLeft";
            button_CmeraLeft.Size = new Size(52, 51);
            button_CmeraLeft.TabIndex = 6;
            button_CmeraLeft.UseVisualStyleBackColor = true;
            // 
            // button_cameraRightUp
            // 
            button_cameraRightUp.ImageKey = "arrow-rightUp.png";
            button_cameraRightUp.ImageList = imageList;
            button_cameraRightUp.Location = new Point(122, 4);
            button_cameraRightUp.Name = "button_cameraRightUp";
            button_cameraRightUp.Size = new Size(54, 51);
            button_cameraRightUp.TabIndex = 5;
            button_cameraRightUp.UseVisualStyleBackColor = true;
            // 
            // button_cameraUp
            // 
            button_cameraUp.ImageKey = "arrow-up.png";
            button_cameraUp.ImageList = imageList;
            button_cameraUp.Location = new Point(63, 4);
            button_cameraUp.Name = "button_cameraUp";
            button_cameraUp.Size = new Size(52, 51);
            button_cameraUp.TabIndex = 4;
            button_cameraUp.TabStop = false;
            button_cameraUp.UseVisualStyleBackColor = true;
            button_cameraUp.Click += button_cameraUp_Click;
            // 
            // button_cameraLeftUp
            // 
            button_cameraLeftUp.ImageKey = "arrow-leftUp.png";
            button_cameraLeftUp.ImageList = imageList;
            button_cameraLeftUp.Location = new Point(4, 4);
            button_cameraLeftUp.Name = "button_cameraLeftUp";
            button_cameraLeftUp.Size = new Size(52, 51);
            button_cameraLeftUp.TabIndex = 3;
            button_cameraLeftUp.UseVisualStyleBackColor = true;
            // 
            // File_tabPage
            // 
            File_tabPage.BackColor = Color.Transparent;
            File_tabPage.Cursor = Cursors.Hand;
            File_tabPage.ForeColor = SystemColors.ControlText;
            File_tabPage.Location = new Point(4, 29);
            File_tabPage.Name = "File_tabPage";
            File_tabPage.Size = new Size(1481, 187);
            File_tabPage.TabIndex = 2;
            File_tabPage.Text = "Файл";
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(picture);
            panel1.Location = new Point(12, 231);
            panel1.Name = "panel1";
            panel1.Size = new Size(1485, 587);
            panel1.TabIndex = 1;
            // 
            // picture
            // 
            picture.Cursor = Cursors.Cross;
            picture.Location = new Point(3, 3);
            picture.Name = "picture";
            picture.Size = new Size(1477, 572);
            picture.TabIndex = 0;
            picture.TabStop = false;
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.None;
            groupBox1.Controls.Add(button_move);
            groupBox1.Location = new Point(0, 0);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(200, 100);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            // 
            // button_move
            // 
            button_move.Font = new Font("Segoe UI", 10F);
            button_move.Location = new Point(6, 143);
            button_move.Name = "button_move";
            button_move.Size = new Size(184, 29);
            button_move.TabIndex = 5;
            button_move.Text = "Переместить";
            button_move.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel6
            // 
            tableLayoutPanel6.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel6.CellBorderStyle = TableLayoutPanelCellBorderStyle.Outset;
            tableLayoutPanel6.ColumnCount = 2;
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 28F));
            tableLayoutPanel6.Controls.Add(label7, 0, 2);
            tableLayoutPanel6.Location = new Point(0, 0);
            tableLayoutPanel6.Name = "tableLayoutPanel6";
            tableLayoutPanel6.RowCount = 3;
            tableLayoutPanel6.Size = new Size(200, 100);
            tableLayoutPanel6.TabIndex = 0;
            // 
            // label7
            // 
            label7.Anchor = AnchorStyles.None;
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 10F);
            label7.Location = new Point(6, 40);
            label7.Name = "label7";
            label7.Size = new Size(31, 23);
            label7.TabIndex = 7;
            label7.Text = "ΔZ";
            // 
            // numericUpDown2
            // 
            numericUpDown2.Font = new Font("Segoe UI", 10F);
            numericUpDown2.Location = new Point(47, 9);
            numericUpDown2.Maximum = new decimal(new int[] { 99999999, 0, 0, 0 });
            numericUpDown2.Minimum = new decimal(new int[] { 99999999, 0, 0, int.MinValue });
            numericUpDown2.Name = "numericUpDown2";
            numericUpDown2.Size = new Size(132, 30);
            numericUpDown2.TabIndex = 8;
            // 
            // button1
            // 
            button1.Font = new Font("Segoe UI", 10F);
            button1.Location = new Point(6, 143);
            button1.Name = "button1";
            button1.Size = new Size(184, 29);
            button1.TabIndex = 5;
            button1.Text = "Переместить";
            button1.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.Outset;
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 28F));
            tableLayoutPanel1.Controls.Add(label1, 0, 2);
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.Size = new Size(200, 100);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.None;
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 10F);
            label1.Location = new Point(6, 40);
            label1.Name = "label1";
            label1.Size = new Size(31, 23);
            label1.TabIndex = 7;
            label1.Text = "ΔZ";
            // 
            // numericUpDown1
            // 
            numericUpDown1.Font = new Font("Segoe UI", 10F);
            numericUpDown1.Location = new Point(47, 9);
            numericUpDown1.Maximum = new decimal(new int[] { 99999999, 0, 0, 0 });
            numericUpDown1.Minimum = new decimal(new int[] { 99999999, 0, 0, int.MinValue });
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(132, 30);
            numericUpDown1.TabIndex = 8;
            // 
            // groupBox2
            // 
            groupBox2.Anchor = AnchorStyles.None;
            groupBox2.Controls.Add(button1);
            groupBox2.Location = new Point(0, 0);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(200, 100);
            groupBox2.TabIndex = 0;
            groupBox2.TabStop = false;
            // 
            // Form1
            // 
            AutoScaleMode = AutoScaleMode.Inherit;
            ClientSize = new Size(1924, 830);
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
            groupBox6.ResumeLayout(false);
            Primitives_groupBox.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            groupBox3.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_moveZ).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_moveY).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_moveX).EndInit();
            groupBox4.ResumeLayout(false);
            tableLayoutPanel7.ResumeLayout(false);
            tableLayoutPanel7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_angleOz).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_angleOy).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_angleOx).EndInit();
            groupBox5.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_scaleZ).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_scaleY).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_scaleX).EndInit();
            View_tabPage.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picture).EndInit();
            groupBox1.ResumeLayout(false);
            tableLayoutPanel6.ResumeLayout(false);
            tableLayoutPanel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).EndInit();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            groupBox2.ResumeLayout(false);
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
        private GroupBox groupBox1;
        private Button button_move;
        private TableLayoutPanel tableLayoutPanel6;
        private Label label7;
        private NumericUpDown numericUpDown2;
        private Button button1;
        private TableLayoutPanel tableLayoutPanel1;
        private Label label1;
        private NumericUpDown numericUpDown1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private Button button_moveModel;
        private TableLayoutPanel tableLayoutPanel2;
        private Label label2;
        private NumericUpDown numericUpDown_moveZ;
        private NumericUpDown numericUpDown_moveY;
        private NumericUpDown numericUpDown_moveX;
        private Label label6;
        private Label label4;
        private GroupBox groupBox4;
        private Button button_rotateModel;
        private TableLayoutPanel tableLayoutPanel7;
        private Label label5;
        private NumericUpDown numericUpDown_angleOz;
        private NumericUpDown numericUpDown_angleOy;
        private NumericUpDown numericUpDown_angleOx;
        private Label label8;
        private Label label9;
        private GroupBox groupBox5;
        private Button button_scaleModel;
        private TableLayoutPanel tableLayoutPanel3;
        private Label label3;
        private NumericUpDown numericUpDown_scaleZ;
        private NumericUpDown numericUpDown_scaleY;
        private NumericUpDown numericUpDown_scaleX;
        private Label label10;
        private Label label11;
        private ListView listView_modelsMain;
        private ImageList imageList;
        private GroupBox groupBox6;
        private Button button6;
        private Button button5;
        private Button button_Clear;
        private Button button_dialogEdit;
        private TableLayoutPanel tableLayoutPanel4;
        private Button button_cameraRIghtDown;
        private Button button_cameraDown;
        private Button button_cameraLeftDown;
        private Button button_cameraRight;
        private Button button_CmeraLeft;
        private Button button_cameraRightUp;
        private Button button_cameraUp;
        private Button button_cameraLeftUp;
    }
}

