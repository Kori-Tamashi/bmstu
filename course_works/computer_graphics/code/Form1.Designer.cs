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
            groupBox8 = new GroupBox();
            checkedListBox_renderMode = new CheckedListBox();
            groupBox5 = new GroupBox();
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
            Primitives_groupBox = new GroupBox();
            flowLayoutPanel1 = new FlowLayoutPanel();
            Cube_button = new Button();
            triangularPyramid_button = new Button();
            directPrism_button = new Button();
            listView_modelsMain = new ListView();
            imageList = new ImageList(components);
            groupBox6 = new GroupBox();
            button2 = new Button();
            button5 = new Button();
            button_Clear = new Button();
            button_dialogEdit = new Button();
            View_tabPage = new TabPage();
            groupBox11 = new GroupBox();
            numericUpDown_pitch = new NumericUpDown();
            label_pitchMax = new Label();
            label_pitchMin = new Label();
            trackBar_pitch = new TrackBar();
            groupBox10 = new GroupBox();
            numericUpDown_yaw = new NumericUpDown();
            label_yawMax = new Label();
            label_yawMin = new Label();
            trackBar_yaw = new TrackBar();
            groupBox7 = new GroupBox();
            button_zoom_minus = new Button();
            button_cameraForward = new Button();
            tableLayoutPanel4 = new TableLayoutPanel();
            button_cameraRightDown = new Button();
            button_cameraDown = new Button();
            button_cameraLeftDown = new Button();
            button_cameraRight = new Button();
            button_cameraLeft = new Button();
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
            groupBox8.SuspendLayout();
            groupBox5.SuspendLayout();
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
            Primitives_groupBox.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            groupBox6.SuspendLayout();
            View_tabPage.SuspendLayout();
            groupBox11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_pitch).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBar_pitch).BeginInit();
            groupBox10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_yaw).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBar_yaw).BeginInit();
            groupBox7.SuspendLayout();
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
            InteractionMenu_tabControl.Size = new Size(1332, 253);
            InteractionMenu_tabControl.SizeMode = TabSizeMode.FillToRight;
            InteractionMenu_tabControl.TabIndex = 0;
            // 
            // Main_tabPage
            // 
            Main_tabPage.BackColor = Color.White;
            Main_tabPage.BorderStyle = BorderStyle.FixedSingle;
            Main_tabPage.Controls.Add(groupBox8);
            Main_tabPage.Controls.Add(groupBox5);
            Main_tabPage.Controls.Add(Primitives_groupBox);
            Main_tabPage.Controls.Add(listView_modelsMain);
            Main_tabPage.Controls.Add(groupBox6);
            Main_tabPage.Cursor = Cursors.Hand;
            Main_tabPage.Location = new Point(4, 29);
            Main_tabPage.Name = "Main_tabPage";
            Main_tabPage.Padding = new Padding(3);
            Main_tabPage.Size = new Size(1324, 220);
            Main_tabPage.TabIndex = 0;
            Main_tabPage.Text = "Главная";
            // 
            // groupBox8
            // 
            groupBox8.Controls.Add(checkedListBox_renderMode);
            groupBox8.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
            groupBox8.Location = new Point(1030, 3);
            groupBox8.Name = "groupBox8";
            groupBox8.Size = new Size(287, 206);
            groupBox8.TabIndex = 13;
            groupBox8.TabStop = false;
            groupBox8.Text = "Изображение";
            // 
            // checkedListBox_renderMode
            // 
            checkedListBox_renderMode.BorderStyle = BorderStyle.None;
            checkedListBox_renderMode.CausesValidation = false;
            checkedListBox_renderMode.CheckOnClick = true;
            checkedListBox_renderMode.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
            checkedListBox_renderMode.FormattingEnabled = true;
            checkedListBox_renderMode.Items.AddRange(new object[] { "Теневые эффекты", "Световые эффекты", "Реалистичное отображение", "Каркасное отображение" });
            checkedListBox_renderMode.Location = new Point(6, 23);
            checkedListBox_renderMode.Name = "checkedListBox_renderMode";
            checkedListBox_renderMode.Size = new Size(271, 84);
            checkedListBox_renderMode.TabIndex = 14;
            checkedListBox_renderMode.ItemCheck += checkedListBox_renderMode_ItemCheck;
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(groupBox3);
            groupBox5.Controls.Add(groupBox4);
            groupBox5.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
            groupBox5.Location = new Point(612, 3);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(408, 206);
            groupBox5.TabIndex = 9;
            groupBox5.TabStop = false;
            groupBox5.Text = "Изменение положения в пространстве";
            // 
            // groupBox3
            // 
            groupBox3.Anchor = AnchorStyles.None;
            groupBox3.Controls.Add(button_moveModel);
            groupBox3.Controls.Add(tableLayoutPanel2);
            groupBox3.Font = new Font("Segoe UI", 10F);
            groupBox3.Location = new Point(6, 21);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(196, 181);
            groupBox3.TabIndex = 5;
            groupBox3.TabStop = false;
            groupBox3.Text = "Перемещение";
            // 
            // button_moveModel
            // 
            button_moveModel.Font = new Font("Segoe UI", 10F);
            button_moveModel.Location = new Point(6, 143);
            button_moveModel.Name = "button_moveModel";
            button_moveModel.Size = new Size(180, 29);
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
            tableLayoutPanel2.Size = new Size(180, 114);
            tableLayoutPanel2.TabIndex = 3;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.None;
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10F);
            label2.Location = new Point(6, 81);
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
            numericUpDown_moveZ.Value = new decimal(new int[] { 15, 0, 0, 0 });
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
            numericUpDown_moveY.Value = new decimal(new int[] { 15, 0, 0, 0 });
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
            numericUpDown_moveX.Value = new decimal(new int[] { 15, 0, 0, 0 });
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
            groupBox4.Location = new Point(206, 21);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(196, 180);
            groupBox4.TabIndex = 6;
            groupBox4.TabStop = false;
            groupBox4.Text = "Поворот";
            // 
            // button_rotateModel
            // 
            button_rotateModel.Font = new Font("Segoe UI", 9F);
            button_rotateModel.Location = new Point(6, 143);
            button_rotateModel.Name = "button_rotateModel";
            button_rotateModel.Size = new Size(184, 29);
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
            tableLayoutPanel7.Size = new Size(184, 112);
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
            numericUpDown_angleOz.Size = new Size(117, 30);
            numericUpDown_angleOz.TabIndex = 8;
            numericUpDown_angleOz.Value = new decimal(new int[] { 15, 0, 0, 0 });
            // 
            // numericUpDown_angleOy
            // 
            numericUpDown_angleOy.Font = new Font("Segoe UI", 10F);
            numericUpDown_angleOy.Location = new Point(62, 41);
            numericUpDown_angleOy.Maximum = new decimal(new int[] { 99999999, 0, 0, 0 });
            numericUpDown_angleOy.Minimum = new decimal(new int[] { 99999999, 0, 0, int.MinValue });
            numericUpDown_angleOy.Name = "numericUpDown_angleOy";
            numericUpDown_angleOy.Size = new Size(117, 30);
            numericUpDown_angleOy.TabIndex = 7;
            numericUpDown_angleOy.Value = new decimal(new int[] { 15, 0, 0, 0 });
            // 
            // numericUpDown_angleOx
            // 
            numericUpDown_angleOx.Font = new Font("Segoe UI", 10F);
            numericUpDown_angleOx.Location = new Point(62, 5);
            numericUpDown_angleOx.Maximum = new decimal(new int[] { 99999999, 0, 0, 0 });
            numericUpDown_angleOx.Minimum = new decimal(new int[] { 99999999, 0, 0, int.MinValue });
            numericUpDown_angleOx.Name = "numericUpDown_angleOx";
            numericUpDown_angleOx.Size = new Size(117, 30);
            numericUpDown_angleOx.TabIndex = 6;
            numericUpDown_angleOx.Value = new decimal(new int[] { 15, 0, 0, 0 });
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
            // Primitives_groupBox
            // 
            Primitives_groupBox.AutoSize = true;
            Primitives_groupBox.Controls.Add(flowLayoutPanel1);
            Primitives_groupBox.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
            Primitives_groupBox.Location = new Point(6, 3);
            Primitives_groupBox.Name = "Primitives_groupBox";
            Primitives_groupBox.Size = new Size(127, 206);
            Primitives_groupBox.TabIndex = 1;
            Primitives_groupBox.TabStop = false;
            Primitives_groupBox.Text = "Примитивы";
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(Cube_button);
            flowLayoutPanel1.Controls.Add(triangularPyramid_button);
            flowLayoutPanel1.Controls.Add(directPrism_button);
            flowLayoutPanel1.Location = new Point(6, 24);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(115, 126);
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
            directPrism_button.Location = new Point(3, 59);
            directPrism_button.Name = "directPrism_button";
            directPrism_button.Size = new Size(50, 50);
            directPrism_button.TabIndex = 2;
            toolTip.SetToolTip(directPrism_button, "Прямая призма");
            directPrism_button.UseVisualStyleBackColor = true;
            directPrism_button.Click += directPrism_button_Click;
            // 
            // listView_modelsMain
            // 
            listView_modelsMain.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            listView_modelsMain.GridLines = true;
            listView_modelsMain.LargeImageList = imageList;
            listView_modelsMain.Location = new Point(139, 12);
            listView_modelsMain.MultiSelect = false;
            listView_modelsMain.Name = "listView_modelsMain";
            listView_modelsMain.Size = new Size(202, 197);
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
            imageList.Images.SetKeyName(14, "rotate-left.png");
            imageList.Images.SetKeyName(15, "rotate-right.png");
            imageList.Images.SetKeyName(16, "rotate-down.png");
            imageList.Images.SetKeyName(17, "rotate-up.png");
            // 
            // groupBox6
            // 
            groupBox6.Anchor = AnchorStyles.None;
            groupBox6.Controls.Add(button2);
            groupBox6.Controls.Add(button5);
            groupBox6.Controls.Add(button_Clear);
            groupBox6.Controls.Add(button_dialogEdit);
            groupBox6.Font = new Font("Segoe UI", 10F);
            groupBox6.Location = new Point(347, 2);
            groupBox6.Name = "groupBox6";
            groupBox6.Size = new Size(259, 207);
            groupBox6.TabIndex = 8;
            groupBox6.TabStop = false;
            groupBox6.Text = "Действия";
            // 
            // button2
            // 
            button2.Location = new Point(68, 151);
            button2.Name = "button2";
            button2.Size = new Size(94, 29);
            button2.TabIndex = 10;
            button2.Text = "button2";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button5
            // 
            button5.Location = new Point(3, 94);
            button5.Name = "button5";
            button5.Size = new Size(252, 29);
            button5.TabIndex = 2;
            button5.Text = "Обработать полотно";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // button_Clear
            // 
            button_Clear.Location = new Point(3, 24);
            button_Clear.Name = "button_Clear";
            button_Clear.Size = new Size(252, 29);
            button_Clear.TabIndex = 1;
            button_Clear.Text = "Очистка сцены";
            button_Clear.UseVisualStyleBackColor = true;
            button_Clear.Click += button_Clear_Click;
            // 
            // button_dialogEdit
            // 
            button_dialogEdit.Location = new Point(3, 59);
            button_dialogEdit.Name = "button_dialogEdit";
            button_dialogEdit.Size = new Size(252, 29);
            button_dialogEdit.TabIndex = 0;
            button_dialogEdit.Text = "Редактор моделей";
            button_dialogEdit.UseVisualStyleBackColor = true;
            button_dialogEdit.Click += button_dialogEdit_Click;
            // 
            // View_tabPage
            // 
            View_tabPage.BackColor = Color.White;
            View_tabPage.BorderStyle = BorderStyle.FixedSingle;
            View_tabPage.Controls.Add(groupBox11);
            View_tabPage.Controls.Add(groupBox10);
            View_tabPage.Controls.Add(groupBox7);
            View_tabPage.Cursor = Cursors.Hand;
            View_tabPage.ForeColor = SystemColors.ControlText;
            View_tabPage.Location = new Point(4, 29);
            View_tabPage.Name = "View_tabPage";
            View_tabPage.Padding = new Padding(3);
            View_tabPage.Size = new Size(1324, 220);
            View_tabPage.TabIndex = 1;
            View_tabPage.Text = "Вид";
            // 
            // groupBox11
            // 
            groupBox11.Controls.Add(numericUpDown_pitch);
            groupBox11.Controls.Add(label_pitchMax);
            groupBox11.Controls.Add(label_pitchMin);
            groupBox11.Controls.Add(trackBar_pitch);
            groupBox11.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
            groupBox11.Location = new Point(295, 107);
            groupBox11.Name = "groupBox11";
            groupBox11.Size = new Size(269, 97);
            groupBox11.TabIndex = 14;
            groupBox11.TabStop = false;
            groupBox11.Text = "Тангаж";
            // 
            // numericUpDown_pitch
            // 
            numericUpDown_pitch.Location = new Point(106, 63);
            numericUpDown_pitch.Maximum = new decimal(new int[] { 90, 0, 0, 0 });
            numericUpDown_pitch.Minimum = new decimal(new int[] { 90, 0, 0, int.MinValue });
            numericUpDown_pitch.Name = "numericUpDown_pitch";
            numericUpDown_pitch.Size = new Size(62, 26);
            numericUpDown_pitch.TabIndex = 18;
            numericUpDown_pitch.ValueChanged += numericUpDown_pitch_ValueChanged;
            // 
            // label_pitchMax
            // 
            label_pitchMax.AutoSize = true;
            label_pitchMax.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label_pitchMax.Location = new Point(236, 65);
            label_pitchMax.Name = "label_pitchMax";
            label_pitchMax.Size = new Size(27, 20);
            label_pitchMax.TabIndex = 16;
            label_pitchMax.Text = "90";
            // 
            // label_pitchMin
            // 
            label_pitchMin.AutoSize = true;
            label_pitchMin.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label_pitchMin.Location = new Point(14, 65);
            label_pitchMin.Name = "label_pitchMin";
            label_pitchMin.Size = new Size(33, 20);
            label_pitchMin.TabIndex = 15;
            label_pitchMin.Text = "-90";
            // 
            // trackBar_pitch
            // 
            trackBar_pitch.BackColor = SystemColors.ControlLightLight;
            trackBar_pitch.Location = new Point(6, 26);
            trackBar_pitch.Maximum = 90;
            trackBar_pitch.Minimum = -90;
            trackBar_pitch.Name = "trackBar_pitch";
            trackBar_pitch.Size = new Size(257, 56);
            trackBar_pitch.TabIndex = 14;
            trackBar_pitch.TickFrequency = 10;
            trackBar_pitch.ValueChanged += trackBar_pitch_ValueChanged;
            // 
            // groupBox10
            // 
            groupBox10.Controls.Add(numericUpDown_yaw);
            groupBox10.Controls.Add(label_yawMax);
            groupBox10.Controls.Add(label_yawMin);
            groupBox10.Controls.Add(trackBar_yaw);
            groupBox10.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
            groupBox10.Location = new Point(295, 0);
            groupBox10.Name = "groupBox10";
            groupBox10.Size = new Size(269, 101);
            groupBox10.TabIndex = 10;
            groupBox10.TabStop = false;
            groupBox10.Text = "Рыскание";
            // 
            // numericUpDown_yaw
            // 
            numericUpDown_yaw.Location = new Point(106, 63);
            numericUpDown_yaw.Maximum = new decimal(new int[] { 180, 0, 0, 0 });
            numericUpDown_yaw.Minimum = new decimal(new int[] { 180, 0, 0, int.MinValue });
            numericUpDown_yaw.Name = "numericUpDown_yaw";
            numericUpDown_yaw.Size = new Size(62, 26);
            numericUpDown_yaw.TabIndex = 17;
            numericUpDown_yaw.ValueChanged += numericUpDown_yaw_ValueChanged;
            // 
            // label_yawMax
            // 
            label_yawMax.AutoSize = true;
            label_yawMax.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label_yawMax.Location = new Point(227, 65);
            label_yawMax.Name = "label_yawMax";
            label_yawMax.Size = new Size(36, 20);
            label_yawMax.TabIndex = 16;
            label_yawMax.Text = "180";
            // 
            // label_yawMin
            // 
            label_yawMin.AutoSize = true;
            label_yawMin.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label_yawMin.Location = new Point(14, 65);
            label_yawMin.Name = "label_yawMin";
            label_yawMin.Size = new Size(42, 20);
            label_yawMin.TabIndex = 15;
            label_yawMin.Text = "-180";
            // 
            // trackBar_yaw
            // 
            trackBar_yaw.BackColor = SystemColors.ControlLightLight;
            trackBar_yaw.Location = new Point(6, 26);
            trackBar_yaw.Maximum = 180;
            trackBar_yaw.Minimum = -180;
            trackBar_yaw.Name = "trackBar_yaw";
            trackBar_yaw.Size = new Size(257, 56);
            trackBar_yaw.TabIndex = 14;
            trackBar_yaw.TickFrequency = 10;
            trackBar_yaw.ValueChanged += trackBar_yaw_ValueChanged;
            // 
            // groupBox7
            // 
            groupBox7.Controls.Add(button_zoom_minus);
            groupBox7.Controls.Add(button_cameraForward);
            groupBox7.Controls.Add(tableLayoutPanel4);
            groupBox7.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 204);
            groupBox7.Location = new Point(6, 0);
            groupBox7.Name = "groupBox7";
            groupBox7.Size = new Size(283, 204);
            groupBox7.TabIndex = 3;
            groupBox7.TabStop = false;
            groupBox7.Text = "Перемещение камеры";
            // 
            // button_zoom_minus
            // 
            button_zoom_minus.BackgroundImage = (Image)resources.GetObject("button_zoom_minus.BackgroundImage");
            button_zoom_minus.BackgroundImageLayout = ImageLayout.Stretch;
            button_zoom_minus.ImageAlign = ContentAlignment.TopRight;
            button_zoom_minus.ImageKey = "(нет)";
            button_zoom_minus.ImageList = imageList;
            button_zoom_minus.Location = new Point(211, 113);
            button_zoom_minus.Name = "button_zoom_minus";
            button_zoom_minus.Size = new Size(54, 51);
            button_zoom_minus.TabIndex = 7;
            button_zoom_minus.UseVisualStyleBackColor = true;
            button_zoom_minus.Click += button_cameraBack_Click;
            // 
            // button_cameraForward
            // 
            button_cameraForward.BackgroundImage = (Image)resources.GetObject("button_cameraForward.BackgroundImage");
            button_cameraForward.BackgroundImageLayout = ImageLayout.Stretch;
            button_cameraForward.ImageAlign = ContentAlignment.TopRight;
            button_cameraForward.ImageKey = "(нет)";
            button_cameraForward.ImageList = imageList;
            button_cameraForward.Location = new Point(211, 56);
            button_cameraForward.Name = "button_cameraForward";
            button_cameraForward.Size = new Size(54, 51);
            button_cameraForward.TabIndex = 6;
            button_cameraForward.UseVisualStyleBackColor = true;
            button_cameraForward.Click += button_cameraForward_Click;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 3;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333359F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel4.Controls.Add(button_cameraRightDown, 2, 2);
            tableLayoutPanel4.Controls.Add(button_cameraDown, 1, 2);
            tableLayoutPanel4.Controls.Add(button_cameraLeftDown, 0, 2);
            tableLayoutPanel4.Controls.Add(button_cameraRight, 2, 1);
            tableLayoutPanel4.Controls.Add(button_cameraLeft, 0, 1);
            tableLayoutPanel4.Controls.Add(button_cameraRightUp, 2, 0);
            tableLayoutPanel4.Controls.Add(button_cameraUp, 1, 0);
            tableLayoutPanel4.Controls.Add(button_cameraLeftUp, 0, 0);
            tableLayoutPanel4.Location = new Point(15, 25);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 3;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333359F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333359F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333359F));
            tableLayoutPanel4.Size = new Size(180, 175);
            tableLayoutPanel4.TabIndex = 2;
            // 
            // button_cameraRightDown
            // 
            button_cameraRightDown.BackgroundImage = (Image)resources.GetObject("button_cameraRightDown.BackgroundImage");
            button_cameraRightDown.BackgroundImageLayout = ImageLayout.Stretch;
            button_cameraRightDown.ImageAlign = ContentAlignment.BottomRight;
            button_cameraRightDown.ImageKey = "(нет)";
            button_cameraRightDown.ImageList = imageList;
            button_cameraRightDown.Location = new Point(123, 119);
            button_cameraRightDown.Name = "button_cameraRightDown";
            button_cameraRightDown.Size = new Size(54, 51);
            button_cameraRightDown.TabIndex = 11;
            button_cameraRightDown.UseVisualStyleBackColor = true;
            button_cameraRightDown.Click += button_cameraRightDown_Click;
            // 
            // button_cameraDown
            // 
            button_cameraDown.BackgroundImage = (Image)resources.GetObject("button_cameraDown.BackgroundImage");
            button_cameraDown.BackgroundImageLayout = ImageLayout.Stretch;
            button_cameraDown.ImageAlign = ContentAlignment.BottomCenter;
            button_cameraDown.ImageKey = "(нет)";
            button_cameraDown.ImageList = imageList;
            button_cameraDown.Location = new Point(63, 119);
            button_cameraDown.Name = "button_cameraDown";
            button_cameraDown.Size = new Size(52, 51);
            button_cameraDown.TabIndex = 10;
            button_cameraDown.UseVisualStyleBackColor = true;
            button_cameraDown.Click += button_cameraDown_Click;
            // 
            // button_cameraLeftDown
            // 
            button_cameraLeftDown.BackgroundImage = (Image)resources.GetObject("button_cameraLeftDown.BackgroundImage");
            button_cameraLeftDown.BackgroundImageLayout = ImageLayout.Stretch;
            button_cameraLeftDown.ImageAlign = ContentAlignment.BottomLeft;
            button_cameraLeftDown.ImageKey = "(нет)";
            button_cameraLeftDown.ImageList = imageList;
            button_cameraLeftDown.Location = new Point(3, 119);
            button_cameraLeftDown.Name = "button_cameraLeftDown";
            button_cameraLeftDown.Size = new Size(52, 51);
            button_cameraLeftDown.TabIndex = 9;
            button_cameraLeftDown.UseVisualStyleBackColor = true;
            button_cameraLeftDown.Click += button_cameraLeftDown_Click;
            // 
            // button_cameraRight
            // 
            button_cameraRight.BackgroundImage = (Image)resources.GetObject("button_cameraRight.BackgroundImage");
            button_cameraRight.BackgroundImageLayout = ImageLayout.Stretch;
            button_cameraRight.ImageAlign = ContentAlignment.MiddleRight;
            button_cameraRight.ImageKey = "(нет)";
            button_cameraRight.ImageList = imageList;
            button_cameraRight.Location = new Point(123, 61);
            button_cameraRight.Name = "button_cameraRight";
            button_cameraRight.Size = new Size(54, 51);
            button_cameraRight.TabIndex = 8;
            button_cameraRight.UseVisualStyleBackColor = true;
            button_cameraRight.Click += button_cameraRight_Click;
            // 
            // button_cameraLeft
            // 
            button_cameraLeft.BackgroundImage = (Image)resources.GetObject("button_cameraLeft.BackgroundImage");
            button_cameraLeft.BackgroundImageLayout = ImageLayout.Stretch;
            button_cameraLeft.ImageAlign = ContentAlignment.MiddleLeft;
            button_cameraLeft.ImageKey = "(нет)";
            button_cameraLeft.ImageList = imageList;
            button_cameraLeft.Location = new Point(3, 61);
            button_cameraLeft.Name = "button_cameraLeft";
            button_cameraLeft.Size = new Size(52, 51);
            button_cameraLeft.TabIndex = 6;
            button_cameraLeft.UseVisualStyleBackColor = true;
            button_cameraLeft.Click += button_cameraLeft_Click;
            // 
            // button_cameraRightUp
            // 
            button_cameraRightUp.BackgroundImage = (Image)resources.GetObject("button_cameraRightUp.BackgroundImage");
            button_cameraRightUp.BackgroundImageLayout = ImageLayout.Stretch;
            button_cameraRightUp.ImageAlign = ContentAlignment.TopRight;
            button_cameraRightUp.ImageKey = "(нет)";
            button_cameraRightUp.ImageList = imageList;
            button_cameraRightUp.Location = new Point(123, 3);
            button_cameraRightUp.Name = "button_cameraRightUp";
            button_cameraRightUp.Size = new Size(54, 51);
            button_cameraRightUp.TabIndex = 5;
            button_cameraRightUp.UseVisualStyleBackColor = true;
            button_cameraRightUp.Click += button_cameraRightUp_Click;
            // 
            // button_cameraUp
            // 
            button_cameraUp.BackgroundImage = (Image)resources.GetObject("button_cameraUp.BackgroundImage");
            button_cameraUp.BackgroundImageLayout = ImageLayout.Stretch;
            button_cameraUp.ImageAlign = ContentAlignment.TopCenter;
            button_cameraUp.ImageKey = "(нет)";
            button_cameraUp.ImageList = imageList;
            button_cameraUp.Location = new Point(63, 3);
            button_cameraUp.Name = "button_cameraUp";
            button_cameraUp.Size = new Size(52, 51);
            button_cameraUp.TabIndex = 4;
            button_cameraUp.TabStop = false;
            button_cameraUp.UseVisualStyleBackColor = true;
            button_cameraUp.Click += button_cameraUp_Click;
            // 
            // button_cameraLeftUp
            // 
            button_cameraLeftUp.Anchor = AnchorStyles.None;
            button_cameraLeftUp.BackgroundImage = (Image)resources.GetObject("button_cameraLeftUp.BackgroundImage");
            button_cameraLeftUp.BackgroundImageLayout = ImageLayout.Stretch;
            button_cameraLeftUp.ImageAlign = ContentAlignment.TopLeft;
            button_cameraLeftUp.ImageList = imageList;
            button_cameraLeftUp.Location = new Point(4, 3);
            button_cameraLeftUp.Name = "button_cameraLeftUp";
            button_cameraLeftUp.Size = new Size(52, 51);
            button_cameraLeftUp.TabIndex = 3;
            button_cameraLeftUp.UseVisualStyleBackColor = true;
            button_cameraLeftUp.Click += button_cameraLeftUp_Click;
            // 
            // File_tabPage
            // 
            File_tabPage.BackColor = Color.Transparent;
            File_tabPage.Cursor = Cursors.Hand;
            File_tabPage.ForeColor = SystemColors.ControlText;
            File_tabPage.Location = new Point(4, 29);
            File_tabPage.Name = "File_tabPage";
            File_tabPage.Size = new Size(1324, 220);
            File_tabPage.TabIndex = 2;
            File_tabPage.Text = "Файл";
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(picture);
            panel1.Location = new Point(12, 267);
            panel1.Name = "panel1";
            panel1.Size = new Size(1332, 551);
            panel1.TabIndex = 1;
            // 
            // picture
            // 
            picture.Cursor = Cursors.Cross;
            picture.Location = new Point(3, 3);
            picture.Name = "picture";
            picture.Size = new Size(1324, 543);
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
            ClientSize = new Size(1348, 830);
            Controls.Add(panel1);
            Controls.Add(InteractionMenu_tabControl);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4);
            Name = "Form1";
            Text = "Конструктор композиции трехмерных многогранных примитивов ";
            Load += Form1_Load;
            InteractionMenu_tabControl.ResumeLayout(false);
            Main_tabPage.ResumeLayout(false);
            Main_tabPage.PerformLayout();
            groupBox8.ResumeLayout(false);
            groupBox5.ResumeLayout(false);
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
            Primitives_groupBox.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            groupBox6.ResumeLayout(false);
            View_tabPage.ResumeLayout(false);
            groupBox11.ResumeLayout(false);
            groupBox11.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_pitch).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBar_pitch).EndInit();
            groupBox10.ResumeLayout(false);
            groupBox10.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_yaw).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBar_yaw).EndInit();
            groupBox7.ResumeLayout(false);
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
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.ToolTip toolTip;
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
        private ImageList imageList;
        private TableLayoutPanel tableLayoutPanel4;
        private Button button_cameraRightDown;
        private Button button_cameraDown;
        private Button button_cameraLeftDown;
        private Button button_cameraLeft;
        private Button button_cameraRightUp;
        private Button button_cameraUp;
        private Button button_cameraLeftUp;
        private GroupBox groupBox7;
        private GroupBox Primitives_groupBox;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button Cube_button;
        private Button triangularPyramid_button;
        private Button directPrism_button;
        private ListView listView_modelsMain;
        private GroupBox groupBox4;
        private Button button_rotateModel;
        private TableLayoutPanel tableLayoutPanel7;
        private Label label5;
        private NumericUpDown numericUpDown_angleOz;
        private NumericUpDown numericUpDown_angleOy;
        private NumericUpDown numericUpDown_angleOx;
        private Label label8;
        private Label label9;
        private GroupBox groupBox3;
        private Button button_moveModel;
        private TableLayoutPanel tableLayoutPanel2;
        private Label label2;
        private NumericUpDown numericUpDown_moveZ;
        private NumericUpDown numericUpDown_moveY;
        private NumericUpDown numericUpDown_moveX;
        private Label label6;
        private Label label4;
        private GroupBox groupBox6;
        private Button button5;
        private Button button_Clear;
        private Button button_dialogEdit;
        private Button button2;
        private Button button_cameraRight;
        private TrackBar trackBar_yaw;
        private GroupBox groupBox10;
        private Label label_yawMax;
        private Label label_yawMin;
        private GroupBox groupBox11;
        private Label label_pitchMax;
        private Label label_pitchMin;
        private TrackBar trackBar_pitch;
        private NumericUpDown numericUpDown_pitch;
        private NumericUpDown numericUpDown_yaw;
        private Button button_zoom_minus;
        private Button button_cameraForward;
        private GroupBox groupBox5;
        private GroupBox groupBox8;
        private CheckedListBox checkedListBox_renderMode;
    }
}

