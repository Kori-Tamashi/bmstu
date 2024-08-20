using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinRT;

namespace code
{
    partial class Form2 : Form
    {
        Canvas mainCanvas;
        Canvas selfCanvas;
        Facade facade;

        Model currentMainModel;
        Model currentSelfModel;
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
            foreach (Model model in mainCanvas.Models)
            {
                ListViewItem item = new ListViewItem(model.Name, listViewGroup);
                item.ImageIndex = ModelImageIndex(model.Type);
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

                currentMainModel = mainCanvas.Model(currentModelIndex);
                currentSelfModel = currentMainModel.Copy();

                UpdateInformationTable(currentMainModel);
                UpdateCanvas(currentSelfModel);
            }
        }

        private void UpdateCanvas(Model model)
        {
            selfCanvas.DeleteModels();
            selfCanvas.AddModel(currentSelfModel);
            selfCanvas.Centering(new Centering(currentSelfModel, selfCanvas.Center, selfCanvas.Size));
            selfCanvas.Refresh();
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
            numericUpDown_length.Value = model.Length != -1 ? (decimal)model.Length : (decimal)0;
            numericUpDown_length.Enabled = model.Length != -1;

            // width
            numericUpDown_width.Value = model.Width != -1 ? (decimal)model.Width : (decimal)0;
            numericUpDown_width.Enabled = model.Width != -1;

            // height
            numericUpDown_height.Value = model.Height != -1 ? (decimal)model.Height : (decimal)0;
            numericUpDown_height.Enabled = model.Height != -1;

            // angle
            numericUpDown_angle.Value = model.Angle != -1 ? (decimal)model.Angle : (decimal)0;
            numericUpDown_angle.Enabled = model.Angle != -1;

            // radius
            numericUpDown_radius.Value = model.Radius != -1 ? (decimal)model.Radius : (decimal)0;
            numericUpDown_radius.Enabled = model.Radius != -1;
        }

        private void UpdateInformationTableColor(Model model)
        {
            // color
            if (model.Color == Color.Empty)
            {
                button_color.BackColor = model.Color;
                button_color.Text = "<без цвета>";
            }
            else
            {
                button_color.BackColor = model.Color;
                button_color.Text = "";
            }
        }

        private void UpdateInfromationTableMaterial(Model model)
        {
            // material
            switch (model.MaterialType)
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
            switch (model.Type)
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

            //name
            textBox_name.Text = model.Name;
        }

        #endregion

        #region Buttons

        private void buttonColor_Click(object sender, EventArgs e)
        {
            contextMenuStrip_buttonColor.Show(Cursor.Position);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ChangeColorEvent(Color.Empty);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            ChangeColorEvent(colorDialog1.Color);
        }

        private void button_material_Click(object sender, EventArgs e)
        {
            contextMenuStrip_buttonMaterial.Show(Cursor.Position);
        }

        private void toolStripMenuItem_resetMaterial_Click(object sender, EventArgs e)
        {
            ChangeMaterialEvent(MaterialType.None);
        }

        private void Wood_Click(object sender, EventArgs e)
        {
            ChangeMaterialEvent(MaterialType.Wood);
        }

        private void Stone_Click(object sender, EventArgs e)
        {
            ChangeMaterialEvent(MaterialType.Stone);
        }

        private void Metal_Click(object sender, EventArgs e)
        {
            ChangeMaterialEvent(MaterialType.Metal);
        }

        private void ChangeColorEvent(Color newColor)
        {
            currentSelfModel.Color = newColor;
            mainCanvas.Model(currentModelIndex).Color = newColor;
            UpdateInformationTableColor(currentSelfModel);

            mainCanvas.Refresh();
            selfCanvas.Refresh();
        }

        private void ChangeMaterialEvent(MaterialType newMaterialType)
        {
            currentSelfModel.MaterialType = newMaterialType;
            mainCanvas.Model(currentModelIndex).MaterialType = newMaterialType;
            UpdateInfromationTableMaterial(currentSelfModel);

            mainCanvas.Refresh();
            selfCanvas.Refresh();
        }

        #endregion

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox_modelType_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox_name_TextChanged(object sender, EventArgs e)
        {
            currentSelfModel.Name = textBox_name.Text;
            mainCanvas.Model(currentModelIndex).Name = textBox_name.Text;
            listView_models.SelectedItems[0].Text = textBox_name.Text;
        }

        private void numericUpDown_length_ValueChanged(object sender, EventArgs e)
        {
            if (currentMainModel.Length == -1)
                return;

            float newLength = (float)numericUpDown_length.Value;
            float modelLength = currentMainModel.Length;

            currentSelfModel.Length *= newLength / modelLength;
            currentMainModel.Length = newLength;

            selfCanvas.Centering();
            UpdateInformationTableSizes(currentMainModel);

            mainCanvas.Refresh();
            selfCanvas.Refresh();
        }

        private void numericUpDown_width_ValueChanged(object sender, EventArgs e)
        {
            if (currentMainModel.Width == -1)
                return;

            float newWidth = (float)numericUpDown_width.Value;
            float modelWidth = currentMainModel.Width;

            currentSelfModel.Width *= newWidth / modelWidth;
            currentMainModel.Width = newWidth;

            selfCanvas.Centering();
            UpdateInformationTableSizes(currentMainModel);

            mainCanvas.Refresh();
            selfCanvas.Refresh();
        }

        private void numericUpDown_height_ValueChanged(object sender, EventArgs e)
        {
            if (currentMainModel.Height == -1)
                return;

            float newHeight = (float)numericUpDown_height.Value;
            float modelHeight = currentMainModel.Height;

            currentSelfModel.Height *= newHeight / modelHeight;
            currentMainModel.Height = newHeight;

            selfCanvas.Centering();
            UpdateInformationTableSizes(currentMainModel);

            mainCanvas.Refresh();
            selfCanvas.Refresh();
        }

        private void numericUpDown_radius_ValueChanged(object sender, EventArgs e)
        {
            if (currentMainModel.Radius == -1)
                return;

            float newRadius = (float)numericUpDown_radius.Value;
            float modelRadius = currentMainModel.Radius;

            currentSelfModel.Radius *= newRadius / modelRadius;
            currentMainModel.Radius = newRadius;

            selfCanvas.Centering();
            UpdateInformationTableSizes(currentMainModel);

            mainCanvas.Refresh();
            selfCanvas.Refresh();
        }
    }
}
