namespace code
{
    partial class Form2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            pictureBox_editModel = new PictureBox();
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel4 = new TableLayoutPanel();
            button_color = new Button();
            contextMenuStrip_buttonColor = new ContextMenuStrip(components);
            toolStripMenuItem_chooseColor = new ToolStripMenuItem();
            toolStripMenuItem_resetColor = new ToolStripMenuItem();
            textBox6 = new TextBox();
            textBox8 = new TextBox();
            button_material = new Button();
            contextMenuStrip_buttonMaterial = new ContextMenuStrip(components);
            toolStripMenuItem_chooseMaterial = new ToolStripMenuItem();
            Wood = new ToolStripMenuItem();
            Stone = new ToolStripMenuItem();
            Metal = new ToolStripMenuItem();
            toolStripMenuItem_resetMaterial = new ToolStripMenuItem();
            tableLayoutPanel5 = new TableLayoutPanel();
            textBox9 = new TextBox();
            textBox4 = new TextBox();
            textBox10 = new TextBox();
            textBox_modelType = new TextBox();
            tableLayoutPanel2 = new TableLayoutPanel();
            numericUpDown_radius = new NumericUpDown();
            textBox2 = new TextBox();
            numericUpDown_angle = new NumericUpDown();
            numericUpDown_height = new NumericUpDown();
            numericUpDown_width = new NumericUpDown();
            textBox7 = new TextBox();
            textBox5 = new TextBox();
            textBox3 = new TextBox();
            textBox1 = new TextBox();
            numericUpDown_length = new NumericUpDown();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            toolTip = new ToolTip(components);
            tableLayoutPanel3 = new TableLayoutPanel();
            listView_models = new ListView();
            imageList = new ImageList(components);
            colorDialog1 = new ColorDialog();
            ((System.ComponentModel.ISupportInitialize)pictureBox_editModel).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            contextMenuStrip_buttonColor.SuspendLayout();
            contextMenuStrip_buttonMaterial.SuspendLayout();
            tableLayoutPanel5.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_radius).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_angle).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_height).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_width).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_length).BeginInit();
            tableLayoutPanel3.SuspendLayout();
            SuspendLayout();
            // 
            // pictureBox_editModel
            // 
            pictureBox_editModel.Location = new Point(160, 4);
            pictureBox_editModel.Name = "pictureBox_editModel";
            pictureBox_editModel.Size = new Size(372, 372);
            pictureBox_editModel.TabIndex = 0;
            pictureBox_editModel.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.AutoScroll = true;
            tableLayoutPanel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(tableLayoutPanel4, 0, 3);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel5, 0, 5);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 1);
            tableLayoutPanel1.Controls.Add(label1, 0, 0);
            tableLayoutPanel1.Controls.Add(label2, 0, 2);
            tableLayoutPanel1.Controls.Add(label3, 0, 4);
            tableLayoutPanel1.Location = new Point(539, 4);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 6;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 27F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 210F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 27F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 79F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 27F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 143F));
            tableLayoutPanel1.Size = new Size(353, 372);
            tableLayoutPanel1.TabIndex = 2;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tableLayoutPanel4.ColumnCount = 2;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40.871933F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 59.128067F));
            tableLayoutPanel4.Controls.Add(button_color, 1, 0);
            tableLayoutPanel4.Controls.Add(textBox6, 0, 1);
            tableLayoutPanel4.Controls.Add(textBox8, 0, 0);
            tableLayoutPanel4.Controls.Add(button_material, 1, 1);
            tableLayoutPanel4.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            tableLayoutPanel4.Location = new Point(4, 271);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 2;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel4.Size = new Size(332, 73);
            tableLayoutPanel4.TabIndex = 4;
            // 
            // button_color
            // 
            button_color.ContextMenuStrip = contextMenuStrip_buttonColor;
            button_color.Location = new Point(139, 4);
            button_color.Name = "button_color";
            button_color.Size = new Size(189, 28);
            button_color.TabIndex = 3;
            button_color.UseVisualStyleBackColor = true;
            button_color.Click += buttonColor_Click;
            // 
            // contextMenuStrip_buttonColor
            // 
            contextMenuStrip_buttonColor.ImageScalingSize = new Size(20, 20);
            contextMenuStrip_buttonColor.Items.AddRange(new ToolStripItem[] { toolStripMenuItem_chooseColor, toolStripMenuItem_resetColor });
            contextMenuStrip_buttonColor.Name = "contextMenuStrip1";
            contextMenuStrip_buttonColor.Size = new Size(180, 52);
            contextMenuStrip_buttonColor.Text = "Изменение цвета";
            // 
            // toolStripMenuItem_chooseColor
            // 
            toolStripMenuItem_chooseColor.Name = "toolStripMenuItem_chooseColor";
            toolStripMenuItem_chooseColor.Size = new Size(179, 24);
            toolStripMenuItem_chooseColor.Text = "Выбрать цвет";
            toolStripMenuItem_chooseColor.Click += toolStripMenuItem2_Click;
            // 
            // toolStripMenuItem_resetColor
            // 
            toolStripMenuItem_resetColor.Name = "toolStripMenuItem_resetColor";
            toolStripMenuItem_resetColor.Size = new Size(179, 24);
            toolStripMenuItem_resetColor.Text = "Сбросить цвет";
            toolStripMenuItem_resetColor.Click += toolStripMenuItem1_Click;
            // 
            // textBox6
            // 
            textBox6.BorderStyle = BorderStyle.None;
            textBox6.Cursor = Cursors.Help;
            textBox6.Location = new Point(4, 39);
            textBox6.Name = "textBox6";
            textBox6.ReadOnly = true;
            textBox6.Size = new Size(128, 20);
            textBox6.TabIndex = 10;
            textBox6.Text = "Материал";
            toolTip.SetToolTip(textBox6, "Задает материал ");
            // 
            // textBox8
            // 
            textBox8.BorderStyle = BorderStyle.None;
            textBox8.Cursor = Cursors.Help;
            textBox8.Location = new Point(4, 4);
            textBox8.Name = "textBox8";
            textBox8.ReadOnly = true;
            textBox8.Size = new Size(126, 20);
            textBox8.TabIndex = 0;
            textBox8.Text = "Цвет";
            toolTip.SetToolTip(textBox8, "Задает цвет");
            // 
            // button_material
            // 
            button_material.ContextMenuStrip = contextMenuStrip_buttonMaterial;
            button_material.Location = new Point(139, 39);
            button_material.Name = "button_material";
            button_material.Size = new Size(189, 29);
            button_material.TabIndex = 4;
            button_material.UseVisualStyleBackColor = true;
            button_material.Click += button_material_Click;
            // 
            // contextMenuStrip_buttonMaterial
            // 
            contextMenuStrip_buttonMaterial.ImageScalingSize = new Size(20, 20);
            contextMenuStrip_buttonMaterial.Items.AddRange(new ToolStripItem[] { toolStripMenuItem_chooseMaterial, toolStripMenuItem_resetMaterial });
            contextMenuStrip_buttonMaterial.Name = "contextMenuStrip_buttonMaterial";
            contextMenuStrip_buttonMaterial.Size = new Size(216, 52);
            // 
            // toolStripMenuItem_chooseMaterial
            // 
            toolStripMenuItem_chooseMaterial.DropDownItems.AddRange(new ToolStripItem[] { Wood, Stone, Metal });
            toolStripMenuItem_chooseMaterial.Name = "toolStripMenuItem_chooseMaterial";
            toolStripMenuItem_chooseMaterial.Size = new Size(215, 24);
            toolStripMenuItem_chooseMaterial.Text = "Выбрать материал";
            // 
            // Wood
            // 
            Wood.Name = "Wood";
            Wood.Size = new Size(145, 26);
            Wood.Text = "Дерево";
            Wood.Click += Wood_Click;
            // 
            // Stone
            // 
            Stone.Name = "Stone";
            Stone.Size = new Size(145, 26);
            Stone.Text = "Камень";
            Stone.Click += Stone_Click;
            // 
            // Metal
            // 
            Metal.Name = "Metal";
            Metal.Size = new Size(145, 26);
            Metal.Text = "Металл";
            Metal.Click += Metal_Click;
            // 
            // toolStripMenuItem_resetMaterial
            // 
            toolStripMenuItem_resetMaterial.Name = "toolStripMenuItem_resetMaterial";
            toolStripMenuItem_resetMaterial.Size = new Size(215, 24);
            toolStripMenuItem_resetMaterial.Text = "Сбросить материал";
            toolStripMenuItem_resetMaterial.Click += toolStripMenuItem_resetMaterial_Click;
            // 
            // tableLayoutPanel5
            // 
            tableLayoutPanel5.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tableLayoutPanel5.ColumnCount = 2;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40.81081F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 59.18919F));
            tableLayoutPanel5.Controls.Add(textBox9, 1, 1);
            tableLayoutPanel5.Controls.Add(textBox4, 0, 1);
            tableLayoutPanel5.Controls.Add(textBox10, 0, 0);
            tableLayoutPanel5.Controls.Add(textBox_modelType, 1, 0);
            tableLayoutPanel5.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            tableLayoutPanel5.Location = new Point(4, 379);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 2;
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tableLayoutPanel5.Size = new Size(332, 84);
            tableLayoutPanel5.TabIndex = 4;
            // 
            // textBox9
            // 
            textBox9.BackColor = SystemColors.Control;
            textBox9.BorderStyle = BorderStyle.None;
            textBox9.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            textBox9.Location = new Point(139, 39);
            textBox9.Name = "textBox9";
            textBox9.Size = new Size(189, 20);
            textBox9.TabIndex = 15;
            textBox9.Text = "<коэффициент>";
            // 
            // textBox4
            // 
            textBox4.BorderStyle = BorderStyle.None;
            textBox4.Cursor = Cursors.Help;
            textBox4.Location = new Point(4, 39);
            textBox4.Multiline = true;
            textBox4.Name = "textBox4";
            textBox4.ReadOnly = true;
            textBox4.Size = new Size(126, 41);
            textBox4.TabIndex = 14;
            textBox4.Text = "Коэффициент\r\nотражения\r\n";
            toolTip.SetToolTip(textBox4, "Коэффициент отражения света материала модели");
            // 
            // textBox10
            // 
            textBox10.BorderStyle = BorderStyle.None;
            textBox10.Cursor = Cursors.Help;
            textBox10.Location = new Point(4, 4);
            textBox10.Name = "textBox10";
            textBox10.ReadOnly = true;
            textBox10.Size = new Size(126, 20);
            textBox10.TabIndex = 0;
            textBox10.Text = "Тип";
            toolTip.SetToolTip(textBox10, "Тип модели");
            // 
            // textBox_modelType
            // 
            textBox_modelType.BackColor = SystemColors.Control;
            textBox_modelType.BorderStyle = BorderStyle.None;
            textBox_modelType.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            textBox_modelType.Location = new Point(139, 4);
            textBox_modelType.Name = "textBox_modelType";
            textBox_modelType.Size = new Size(189, 20);
            textBox_modelType.TabIndex = 13;
            textBox_modelType.Text = "<тип>";
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40.9356728F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 59.0643272F));
            tableLayoutPanel2.Controls.Add(numericUpDown_radius, 1, 4);
            tableLayoutPanel2.Controls.Add(textBox2, 0, 4);
            tableLayoutPanel2.Controls.Add(numericUpDown_angle, 1, 3);
            tableLayoutPanel2.Controls.Add(numericUpDown_height, 1, 2);
            tableLayoutPanel2.Controls.Add(numericUpDown_width, 1, 1);
            tableLayoutPanel2.Controls.Add(textBox7, 0, 3);
            tableLayoutPanel2.Controls.Add(textBox5, 0, 2);
            tableLayoutPanel2.Controls.Add(textBox3, 0, 1);
            tableLayoutPanel2.Controls.Add(textBox1, 0, 0);
            tableLayoutPanel2.Controls.Add(numericUpDown_length, 1, 0);
            tableLayoutPanel2.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            tableLayoutPanel2.Location = new Point(4, 32);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 5;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 47F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 49F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 8F));
            tableLayoutPanel2.Size = new Size(332, 204);
            tableLayoutPanel2.TabIndex = 3;
            // 
            // numericUpDown_radius
            // 
            numericUpDown_radius.Location = new Point(139, 171);
            numericUpDown_radius.Maximum = new decimal(new int[] { 999999999, 0, 0, 0 });
            numericUpDown_radius.Name = "numericUpDown_radius";
            numericUpDown_radius.Size = new Size(189, 27);
            numericUpDown_radius.TabIndex = 19;
            numericUpDown_radius.ThousandsSeparator = true;
            // 
            // textBox2
            // 
            textBox2.BorderStyle = BorderStyle.None;
            textBox2.Cursor = Cursors.Help;
            textBox2.Location = new Point(4, 171);
            textBox2.Name = "textBox2";
            textBox2.ReadOnly = true;
            textBox2.Size = new Size(126, 20);
            textBox2.TabIndex = 18;
            textBox2.Text = "Радиус";
            toolTip.SetToolTip(textBox2, "Задает радиус фигуры (только для икосаэдра)");
            // 
            // numericUpDown_angle
            // 
            numericUpDown_angle.Location = new Point(139, 137);
            numericUpDown_angle.Maximum = new decimal(new int[] { 999999999, 0, 0, 0 });
            numericUpDown_angle.Name = "numericUpDown_angle";
            numericUpDown_angle.Size = new Size(189, 27);
            numericUpDown_angle.TabIndex = 17;
            numericUpDown_angle.ThousandsSeparator = true;
            // 
            // numericUpDown_height
            // 
            numericUpDown_height.Location = new Point(139, 102);
            numericUpDown_height.Maximum = new decimal(new int[] { 999999999, 0, 0, 0 });
            numericUpDown_height.Name = "numericUpDown_height";
            numericUpDown_height.Size = new Size(189, 27);
            numericUpDown_height.TabIndex = 16;
            numericUpDown_height.ThousandsSeparator = true;
            // 
            // numericUpDown_width
            // 
            numericUpDown_width.Location = new Point(139, 52);
            numericUpDown_width.Maximum = new decimal(new int[] { 999999999, 0, 0, 0 });
            numericUpDown_width.Name = "numericUpDown_width";
            numericUpDown_width.Size = new Size(189, 27);
            numericUpDown_width.TabIndex = 15;
            numericUpDown_width.ThousandsSeparator = true;
            // 
            // textBox7
            // 
            textBox7.BorderStyle = BorderStyle.None;
            textBox7.Cursor = Cursors.Help;
            textBox7.Location = new Point(4, 137);
            textBox7.Name = "textBox7";
            textBox7.ReadOnly = true;
            textBox7.Size = new Size(126, 20);
            textBox7.TabIndex = 12;
            textBox7.Text = "Угол наклона";
            toolTip.SetToolTip(textBox7, "Задает угол между боковым ребром и ребром основания (только для призм)");
            // 
            // textBox5
            // 
            textBox5.BorderStyle = BorderStyle.None;
            textBox5.Cursor = Cursors.Help;
            textBox5.Location = new Point(4, 102);
            textBox5.Name = "textBox5";
            textBox5.ReadOnly = true;
            textBox5.Size = new Size(76, 20);
            textBox5.TabIndex = 11;
            textBox5.Text = "Высота";
            toolTip.SetToolTip(textBox5, "Задает высоту ");
            textBox5.TextChanged += textBox5_TextChanged;
            // 
            // textBox3
            // 
            textBox3.BorderStyle = BorderStyle.None;
            textBox3.Cursor = Cursors.Help;
            textBox3.Location = new Point(4, 52);
            textBox3.Multiline = true;
            textBox3.Name = "textBox3";
            textBox3.ReadOnly = true;
            textBox3.Size = new Size(128, 43);
            textBox3.TabIndex = 10;
            textBox3.Text = "Ширина основания";
            toolTip.SetToolTip(textBox3, "Задает ширину основания (только для призм)");
            textBox3.TextChanged += textBox3_TextChanged;
            // 
            // textBox1
            // 
            textBox1.BorderStyle = BorderStyle.None;
            textBox1.Cursor = Cursors.Help;
            textBox1.Location = new Point(4, 4);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(126, 41);
            textBox1.TabIndex = 0;
            textBox1.Text = "Длина ребра основания";
            toolTip.SetToolTip(textBox1, "Задает длину ребра основания");
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // numericUpDown_length
            // 
            numericUpDown_length.Location = new Point(139, 4);
            numericUpDown_length.Maximum = new decimal(new int[] { 999999999, 0, 0, 0 });
            numericUpDown_length.Name = "numericUpDown_length";
            numericUpDown_length.Size = new Size(189, 27);
            numericUpDown_length.TabIndex = 14;
            numericUpDown_length.ThousandsSeparator = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F);
            label1.ForeColor = SystemColors.MenuHighlight;
            label1.Location = new Point(4, 1);
            label1.Name = "label1";
            label1.Size = new Size(279, 27);
            label1.TabIndex = 0;
            label1.Text = "Размеры ——————————————————";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F);
            label2.ForeColor = SystemColors.MenuHighlight;
            label2.Location = new Point(4, 240);
            label2.Name = "label2";
            label2.Size = new Size(249, 27);
            label2.TabIndex = 4;
            label2.Text = "Внешний вид ————————————————";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F);
            label3.ForeColor = SystemColors.MenuHighlight;
            label3.Location = new Point(4, 348);
            label3.Name = "label3";
            label3.Size = new Size(249, 27);
            label3.TabIndex = 5;
            label3.Text = "Информация ————————————————";
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tableLayoutPanel3.ColumnCount = 3;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 29.032259F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70.96774F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 358F));
            tableLayoutPanel3.Controls.Add(listView_models, 0, 0);
            tableLayoutPanel3.Controls.Add(tableLayoutPanel1, 2, 0);
            tableLayoutPanel3.Controls.Add(pictureBox_editModel, 1, 0);
            tableLayoutPanel3.Location = new Point(12, 12);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel3.Size = new Size(896, 380);
            tableLayoutPanel3.TabIndex = 2;
            // 
            // listView_models
            // 
            listView_models.GridLines = true;
            listView_models.LargeImageList = imageList;
            listView_models.Location = new Point(4, 4);
            listView_models.MultiSelect = false;
            listView_models.Name = "listView_models";
            listView_models.Size = new Size(148, 372);
            listView_models.SmallImageList = imageList;
            listView_models.TabIndex = 3;
            listView_models.UseCompatibleStateImageBehavior = false;
            listView_models.ItemSelectionChanged += listView_models_ItemSelectionChanged;
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
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(960, 963);
            Controls.Add(tableLayoutPanel3);
            MaximumSize = new Size(1800, 1800);
            MinimumSize = new Size(800, 800);
            Name = "Form2";
            Text = "Редактор моделей";
            Load += Form2_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox_editModel).EndInit();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            contextMenuStrip_buttonColor.ResumeLayout(false);
            contextMenuStrip_buttonMaterial.ResumeLayout(false);
            tableLayoutPanel5.ResumeLayout(false);
            tableLayoutPanel5.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_radius).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_angle).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_height).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_width).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_length).EndInit();
            tableLayoutPanel3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private PictureBox pictureBox_editModel;
        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private TableLayoutPanel tableLayoutPanel1;
        private Label label1;
        private TableLayoutPanel tableLayoutPanel2;
        private TextBox textBox1;
        private TextBox textBox7;
        private TextBox textBox5;
        private TextBox textBox3;
        private NumericUpDown numericUpDown_length;
        private ToolTip toolTip;
        private NumericUpDown numericUpDown_height;
        private NumericUpDown numericUpDown_width;
        private Label label2;
        private TableLayoutPanel tableLayoutPanel3;
        private TableLayoutPanel tableLayoutPanel4;
        private TextBox textBox6;
        private TextBox textBox8;
        private ColorDialog colorDialog1;
        private NumericUpDown numericUpDown_angle;
        private Label label3;
        private TextBox textBox10;
        private TableLayoutPanel tableLayoutPanel5;
        private TextBox textBox_modelType;
        private NumericUpDown numericUpDown_radius;
        private TextBox textBox2;
        private TextBox textBox4;
        private TextBox textBox9;
        private ListView listView_models;
        private ImageList imageList;
        private ContextMenuStrip contextMenuStrip_buttonColor;
        private ToolStripMenuItem toolStripMenuItem_resetColor;
        private ToolStripMenuItem toolStripMenuItem_chooseColor;
        private Button button_color;
        private ContextMenuStrip contextMenuStrip_buttonMaterial;
        private ToolStripMenuItem toolStripMenuItem_chooseMaterial;
        private ToolStripMenuItem toolStripMenuItem_resetMaterial;
        private Button button_material;
        private ToolStripMenuItem Wood;
        private ToolStripMenuItem Stone;
        private ToolStripMenuItem Metal;
    }
}