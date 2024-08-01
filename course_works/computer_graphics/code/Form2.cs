using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace code
{
    partial class Form2 : Form
    {
        Canvas mainCanvas;
        Canvas selfCanvas;
        Facade facade;

        DrawsCommand drawCommand;
        SceneCommand sceneCommand;
        TransformationCommand transformationCommand;

        Model currentModel;
        int currentModelIndex;

        public Form2(ref Canvas mainCanvas)
        {
            this.mainCanvas = mainCanvas;

            InitializeComponent();
            InitializeCanvas();
            InitializeFacade();
            InitializeListModels();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        #region Initialize

        private void InitializeCanvas()
        {
            selfCanvas = new Canvas(new Size(pictureBox_editModel.Width, pictureBox_editModel.Height), pictureBox_editModel.CreateGraphics());
        }

        private void InitializeFacade()
        {
            facade = new Facade();
        }

        private void InitializeListModels()
        {
            // Initialize group
            ListViewGroup listViewGroup = new ListViewGroup();
            listViewGroup.Header = "Модели";

            // Initialize items
            foreach (Model model in mainCanvas.Models())
            {
                ListViewItem item = new ListViewItem(model.name, listViewGroup);
                item.ImageIndex = ModelImageIndex(model.type);
                listView_models.Items.Add(item);
            }

            // Add items
            listView_models.Groups.Add(listViewGroup);
            listView_models.Refresh();
        }
        private int ModelImageIndex(Modeltype modelType)
        {
            switch (modelType)
            {
                case Modeltype.Cube:
                    return 0;
                case Modeltype.DirectPrism:
                    return 1;
                case Modeltype.InclinedPrism:
                    return 2;
                case Modeltype.Pyramid:
                    return 3;
                case Modeltype.TruncatedPyramid:
                    return 4;
                case Modeltype.Icosahedron:
                    return 5;
                default:
                    return 5;
            }
        }

        #endregion

        #region Model information

        private void listView_models_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (listView_models.SelectedItems.Count != 0)
            {
                currentModelIndex = listView_models.SelectedItems[0].Index;
                currentModel = new Model(mainCanvas.Model(currentModelIndex));

                UpdateInformationTable(currentModel);

                sceneCommand = new DeleteModelsCommand(ref selfCanvas);
                facade._execute(sceneCommand);

                sceneCommand = new AddModelCommand(ref selfCanvas, ref currentModel);
                facade._execute(sceneCommand);

                transformationCommand = new CenteringCommand(ref selfCanvas, currentModel, selfCanvas.Center(), selfCanvas.Size());
                facade._execute(transformationCommand);

                drawCommand = new RefreshCommand(ref selfCanvas);
                facade._execute(drawCommand);
            }
        }

        private void UpdateInformationTable(Model model)
        {
            UpdateInformationTableSizes(model);
            UpdateInformationTableColor(model);
            UpdateInfromationTableMaterial(model);
            UpdateInformationTableCharacteristics(model);
        }

        private void UpdateInformationTableSizes(Model model)
        {
            // length
            numericUpDown_length.Value = model.length != -1 ? (decimal)model.length : (decimal)0;
            numericUpDown_length.Enabled = model.length != -1;

            // width
            numericUpDown_width.Value = model.width != -1 ? (decimal)model.width : (decimal)0;
            numericUpDown_width.Enabled = model.width != -1;

            // height
            numericUpDown_height.Value = model.height != -1 ? (decimal)model.height : (decimal)0;
            numericUpDown_height.Enabled = model.height != -1;

            // angle
            numericUpDown_angle.Value = model.angle != -1 ? (decimal)model.angle : (decimal)0;
            numericUpDown_angle.Enabled = model.angle != -1;

            // radius
            numericUpDown_radius.Value = model.radius != -1 ? (decimal)model.radius : (decimal)0;
            numericUpDown_radius.Enabled = model.radius != -1;
        }

        private void UpdateInformationTableColor(Model model)
        {
            // color
            if (model.color == Color.Empty)
            {
                button_color.BackColor = model.color;
                button_color.Text = "<без цвета>";
            }
            else
            {
                button_color.BackColor = model.color;
                button_color.Text = "";
            }
        }
        
        private void UpdateInfromationTableMaterial(Model model)
        {
            // material
            switch (model.material.type)
            {
                case MaterialType.None:
                    button_material.BackColor = Color.Empty;
                    button_material.Text = "<без материала>";
                    break;
                case MaterialType.Wood:
                    button_material.BackColor = Color.BurlyWood;
                    button_material.Text = "Дерево";
                    break;
                case MaterialType.Stone:
                    button_material.BackColor = Color.DarkGray;
                    button_material.Text = "Камень";
                    break;
                case MaterialType.Metal:
                    button_material.BackColor = Color.Blue;
                    button_material.Text = "Метал";
                    break;
                default:
                    break;
            }
        }
        
        private void UpdateInformationTableCharacteristics(Model model)
        {
            // type
            switch (model.type)
            {
                case Modeltype.Cube:
                    textBox_modelType.Text = "Куб";
                    break;
                case Modeltype.DirectPrism:
                    textBox_modelType.Text = "Прямая призма";
                    break;
                case Modeltype.InclinedPrism:
                    textBox_modelType.Text = "Наклонная призма";
                    break;
                case Modeltype.Pyramid:
                    textBox_modelType.Text = "Пирамида";
                    break;
                case Modeltype.TruncatedPyramid:
                    textBox_modelType.Text = "Усеченная пирамида";
                    break;
                case Modeltype.Icosahedron:
                    textBox_modelType.Text = "Икосаэдр";
                    break;
                default:
                    break;
            }
        }

        

        #endregion

        private void buttonColor_Click(object sender, EventArgs e)
        {
            contextMenuStrip_buttonColor.Show(Cursor.Position);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            button_color.BackColor = Color.Empty;
            button_color.Text = "<без цвета>";
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            button_color.BackColor = colorDialog1.Color;
            button_color.Text = "";
        }

        private void button_material_Click(object sender, EventArgs e)
        {
            contextMenuStrip_buttonMaterial.Show(Cursor.Position);
        }

        private void toolStripMenuItem_resetMaterial_Click(object sender, EventArgs e)
        {
            button_material.BackColor = Color.Empty;
            button_material.Text = "<без материала>";
        }

        private void Wood_Click(object sender, EventArgs e)
        {
            button_material.BackColor = Color.BurlyWood;
            button_material.Text = Wood.Text;
        }

        private void Stone_Click(object sender, EventArgs e)
        {
            button_material.BackColor = Color.DarkGray;
            button_material.Text = Stone.Text;
        }

        private void Metal_Click(object sender, EventArgs e)
        {
            button_material.BackColor = Color.Blue;
            button_material.Text = Metal.Text;
        }

        

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        
    }
}
