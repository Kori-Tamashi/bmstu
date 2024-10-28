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
using WinRT;

namespace code
{
    partial class DialogEdit : Form
    {
        PictureBox mainPicture;
        Canvas mainCanvas;

        Model currentMainModel;
        Model currentSelfModel;
        int currentModelIndex;

        RenderMode renderMode;

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
                currentSelfModel = currentMainModel.Copy();

                UpdateInformationTable(currentMainModel);
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

        #endregion

        #region Buttons

        private void buttonColor_Click(object sender, EventArgs e)
        {
            contextMenuStrip_buttonColor.Show(System.Windows.Forms.Cursor.Position);
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

        private void ChangeColorEvent(Color newColor)
        {
            currentSelfModel.Color = newColor;
            mainCanvas.Model(currentModelIndex).Color = newColor;
            UpdateInformationTableColor(currentSelfModel);

            mainCanvas.Refresh();
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

            UpdateInformationTableSizes(currentMainModel);
            MainCanvasRender();
        }

        private void numericUpDown_width_ValueChanged(object sender, EventArgs e)
        {
            if (currentMainModel.Width == -1)
                return;

            float newWidth = (float)numericUpDown_width.Value;
            float modelWidth = currentMainModel.Width;

            currentSelfModel.Width *= newWidth / modelWidth;
            currentMainModel.Width = newWidth;

            UpdateInformationTableSizes(currentMainModel);
            MainCanvasRender();
        }

        private void numericUpDown_height_ValueChanged(object sender, EventArgs e)
        {
            if (currentMainModel.Height == -1)
                return;

            float newHeight = (float)numericUpDown_height.Value;
            float modelHeight = currentMainModel.Height;

            currentSelfModel.Height *= newHeight / modelHeight;
            currentMainModel.Height = newHeight;

            UpdateInformationTableSizes(currentMainModel);
            MainCanvasRender();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
