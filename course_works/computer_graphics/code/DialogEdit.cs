using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace code
{
    partial class DialogEdit : Form
    {
        PictureBox mainPicture;
        Canvas mainCanvas;

        Model currentMainModel;
        int currentModelIndex;

        RenderMode renderMode;
        bool modelInitialized = false;

        public DialogEdit(ref Canvas mainCanvas, ref PictureBox mainPicture, RenderMode renderMode)
        {
            this.renderMode = renderMode;
            this.mainCanvas = mainCanvas;
            this.mainPicture = mainPicture;

            InitializeComponent();
            InitializeListModels();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        #region Initialize

        private void InitializeListModels()
        {
            // Initialize group
            ListViewGroup listViewGroup = new ListViewGroup();
            listViewGroup.Header = "Модели";

            // Initialize items
            foreach (Model model in mainCanvas.Models)
            {
                ListViewItem item = new ListViewItem(model.Name, listViewGroup);
                item.ImageIndex = ModelType.ModelImageIndex(model.Type);
                listView_models.Items.Add(item);
            }

            // Add items
            listView_models.Groups.Add(listViewGroup);
            listView_models.Refresh();
        }

        #endregion

        #region Model information

        private void listView_models_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (listView_models.SelectedItems.Count != 0)
            {
                currentModelIndex = listView_models.SelectedItems[0].Index;
                currentMainModel = mainCanvas.Model(currentModelIndex);

                modelInitialized = false;
                UpdateInformationTable(currentMainModel);
                modelInitialized = true;
            }
        }

        private void MainCanvasRender()
        {
            if (renderMode != RenderMode.CarcassDisplay)
            {
                mainCanvas.Render(renderMode);
                mainCanvas.UpdateImage(ref mainPicture);
                mainPicture.Refresh();
            }
            else
            {
                mainCanvas.GraphicsClear();
                mainCanvas.Render(renderMode);
            }
        }

        private void UpdateInformationTable(Model model)
        {
            UpdateInformationTableSizes(model);
            UpdateInformationTableColor(model);
            UpdateInformationTableMaterial(model);
            UpdateInformationTableName(model);
        }

        private void UpdateInformationTableSizes(Model model)
        {
            // length
            numericUpDown_length.Value = model.Length != -1 ? (decimal)model.Length : (decimal)numericUpDown_length.Minimum;
            numericUpDown_length.Enabled = (model.Type != Modeltype.Model);

            // width
            numericUpDown_width.Value = model.Width != -1 ? (decimal)model.Width : (decimal)model.Length;
            numericUpDown_width.Enabled = model.Width != -1;

            // height
            numericUpDown_height.Value = model.Height != -1 ? (decimal)model.Height : (decimal)numericUpDown_height.Minimum;
            numericUpDown_height.Enabled = model.Height != -1;

            // facesCount
            numericUpDown_facesCount.Value = model.FacesCount;
            numericUpDown_facesCount.Enabled = (model.Type == Modeltype.Model);

            // lowerRadius
            numericUpDown_lowerBaseRadius.Value = (decimal)model.LowerBaseRadius;
            numericUpDown_lowerBaseRadius.Enabled = (model.Type == Modeltype.Model);

            // upperRadius
            numericUpDown_upperBaseRadius.Value = (model.UpperBaseRadius != -1) ? (decimal)model.UpperBaseRadius : numericUpDown_upperBaseRadius.Minimum;
            numericUpDown_upperBaseRadius.Enabled = (model.Type == Modeltype.Model);
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

        private void UpdateInformationTableMaterial(Model model)
        {
            numericUpDown_material_k_d.Value = (decimal)model.Material.k_d;
            numericUpDown_material_k_m.Value = (decimal)model.Material.k_m;
            numericUpDown_material_a.Value = (decimal)model.Material.a;
        }

        private void UpdateInformationTableName(Model model)
        {
            textBox_name.Text = model.Name;
        }

        #endregion

        #region Buttons

        private void buttonColor_Click(object sender, EventArgs e)
        {
            if (currentMainModel == null)
            {
                ErrorMessage errorMessage = new ErrorMessage();
                errorMessage.Show("Модель не выбрана.");
                return;
            }

            contextMenuStrip_buttonColor.Show(System.Windows.Forms.Cursor.Position);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (currentMainModel == null)
            {
                ErrorMessage errorMessage = new ErrorMessage();
                errorMessage.Show("Модель не выбрана.");
            }

            ChangeColorEvent(Color.Empty);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (currentMainModel == null)
            {
                ErrorMessage errorMessage = new ErrorMessage();
                errorMessage.Show("Модель не выбрана.");
                return;
            }

            colorDialog1.ShowDialog();
            ChangeColorEvent(colorDialog1.Color);
        }

        private void ChangeColorEvent(Color newColor)
        {
            if (currentMainModel == null)
            {
                ErrorMessage errorMessage = new ErrorMessage();
                errorMessage.Show("Модель не выбрана.");
                return;
            }

            mainCanvas.Model(currentModelIndex).Color = newColor;

            UpdateInformationTableColor(currentMainModel);

            if (modelInitialized)
                MainCanvasRender();
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
            if (currentMainModel == null)
            {
                ErrorMessage errorMessage = new ErrorMessage();
                errorMessage.Show("Модель не выбрана.");
                return;
            }

            mainCanvas.Model(currentModelIndex).Name = textBox_name.Text;
            listView_models.SelectedItems[0].Text = textBox_name.Text;
        }

        private void numericUpDown_length_ValueChanged(object sender, EventArgs e)
        {
            if (currentMainModel == null)
            {
                ErrorMessage errorMessage = new ErrorMessage();
                errorMessage.Show("Модель не выбрана.");
                return;
            }

            if (currentMainModel.Length == -1)
                return;

            float newLength = (float)numericUpDown_length.Value;
            currentMainModel.Length = newLength;

            if (modelInitialized)
            {
                UpdateInformationTableSizes(currentMainModel);
                MainCanvasRender();
            }
        }

        private void numericUpDown_width_ValueChanged(object sender, EventArgs e)
        {
            if (currentMainModel == null)
            {
                ErrorMessage errorMessage = new ErrorMessage();
                errorMessage.Show("Модель не выбрана.");
                return;
            }

            if (currentMainModel.Width == -1)
                return;

            float newWidth = (float)numericUpDown_width.Value;
            currentMainModel.Width = newWidth;

            if (modelInitialized)
            {
                UpdateInformationTableSizes(currentMainModel);
                MainCanvasRender();
            }
        }

        private void numericUpDown_height_ValueChanged(object sender, EventArgs e)
        {
            if (currentMainModel == null)
            {
                ErrorMessage errorMessage = new ErrorMessage();
                errorMessage.Show("Модель не выбрана.");
                return;
            }

            if (currentMainModel.Height == -1)
                return;

            float newHeight = (float)numericUpDown_height.Value;
            currentMainModel.Height = newHeight;

            if (modelInitialized)
            {
                UpdateInformationTableSizes(currentMainModel);
                MainCanvasRender();
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void numericUpDown_material_k_d_ValueChanged(object sender, EventArgs e)
        {
            if (currentMainModel == null)
            {
                ErrorMessage errorMessage = new ErrorMessage();
                errorMessage.Show("Модель не выбрана.");
                return;
            }

            currentMainModel.Material.k_d = (float)numericUpDown_material_k_d.Value;

            if (modelInitialized)
                MainCanvasRender();
        }

        private void numericUpDown_material_a_ValueChanged(object sender, EventArgs e)
        {
            if (currentMainModel == null)
            {
                ErrorMessage errorMessage = new ErrorMessage();
                errorMessage.Show("Модель не выбрана.");
                return;
            }

            currentMainModel.Material.a = (float)numericUpDown_material_a.Value;

            if (modelInitialized)
                MainCanvasRender();
        }

        private void numericUpDown_material_k_m_ValueChanged(object sender, EventArgs e)
        {
            if (currentMainModel == null)
            {
                ErrorMessage errorMessage = new ErrorMessage();
                errorMessage.Show("Модель не выбрана.");
                return;
            }

            currentMainModel.Material.k_m = (float)numericUpDown_material_k_m.Value;

            if (modelInitialized)
                MainCanvasRender();
        }

        private void numericUpDown_facesCount_ValueChanged(object sender, EventArgs e)
        {
            if (currentMainModel == null)
            {
                ErrorMessage errorMessage = new ErrorMessage();
                errorMessage.Show("Модель не выбрана.");
                return;
            }

            currentMainModel.FacesCount = (int)numericUpDown_facesCount.Value;

            if (modelInitialized)
            {
                UpdateInformationTableSizes(currentMainModel);
                MainCanvasRender();
            }
        }

        private void numericUpDown_upperBaseRadius_ValueChanged(object sender, EventArgs e)
        {
            if (currentMainModel == null)
            {
                ErrorMessage errorMessage = new ErrorMessage();
                errorMessage.Show("Модель не выбрана.");
                return;
            }

            currentMainModel.UpperBaseRadius = (int)numericUpDown_upperBaseRadius.Value;

            if (modelInitialized)
            {
                UpdateInformationTableSizes(currentMainModel);
                MainCanvasRender();
            }
        }

        private void numericUpDown_lowerBaseRadius_ValueChanged(object sender, EventArgs e)
        {
            if (currentMainModel == null)
            {
                ErrorMessage errorMessage = new ErrorMessage();
                errorMessage.Show("Модель не выбрана.");
                return;
            }

            currentMainModel.LowerBaseRadius = (int)numericUpDown_lowerBaseRadius.Value;

            if (modelInitialized)
            {
                UpdateInformationTableSizes(currentMainModel);
                MainCanvasRender();
            }
        }
    }
}
