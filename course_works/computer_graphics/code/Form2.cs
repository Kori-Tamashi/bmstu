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

        #endregion

        private void listView_models_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (listView_models.SelectedItems.Count != 0)
            {
                currentModelIndex = listView_models.SelectedItems[0].Index;
                currentModel = new Model(mainCanvas.Model(currentModelIndex));

                sceneCommand = new DeleteModelsCommand(ref selfCanvas);
                facade._execute(sceneCommand);

                sceneCommand = new AddModelCommand(ref selfCanvas, ref currentModel);
                facade._execute(sceneCommand);

                transformationCommand = new RotateCommand(ref selfCanvas, 15, 15, 0);
                facade._execute(transformationCommand);

                transformationCommand = new CenteringCommand(ref selfCanvas, currentModel.center, selfCanvas.Center(), selfCanvas.Size());
                facade._execute(transformationCommand);

                drawCommand = new RefreshCommand(ref selfCanvas);
                facade._execute(drawCommand);

                UpdateModelData(currentModel);
            }
        }

        private void UpdateModelData(Model model)
        {
            switch (model.type)
            {
                case Modeltype.Cube:
                    UpdateCubeData(model);
                    break;
                case Modeltype.Pyramid:
                    UpdatePyramidData(model);
                    break;
                default:
                    return;
            }
        }

        private void UpdateCubeData(Model cube)
        {
            numericUpDown_radius.Enabled = false;
            numericUpDown_width.Enabled = false;
            numericUpDown_height.Enabled = false;
            numericUpDown_angle.Enabled = false;

            numericUpDown_length.Value = (decimal) cube.length;
            numericUpDown_width.Value = (decimal) cube.width;
            numericUpDown_height.Value = (decimal) cube.height;
            numericUpDown_angle.Value = (decimal) cube.angle;

        }

        private void UpdatePyramidData(Model pyramid) 
        {
            numericUpDown_radius.Enabled = false;
            numericUpDown_width.Enabled = false;
            numericUpDown_angle.Enabled = false;

            numericUpDown_length.Value = (decimal) pyramid.length;
            numericUpDown_height.Value = (decimal) pyramid.height;
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

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void numericUpDown_angle_ValueChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown_width_ValueChanged(object sender, EventArgs e)
        {

        }

        
    }
}
