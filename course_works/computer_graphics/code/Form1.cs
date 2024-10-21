using code;
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
    public partial class Form1 : Form
    {
        Canvas canvas;
        Facade facade;

        public Form1()
        {
            InitializeComponent();
            InitializeCanvas();
            InitializeFacade();
        }

        private void button2_Click(object sender, EventArgs e)
        {


        }

        #region Initialize

        private void InitializeCanvas()
        {
            canvas = new Canvas(new Size(picture.Width, picture.Height), picture.CreateGraphics());
        }

        private void InitializeFacade()
        {
            facade = new Facade();
        }

        #endregion

        #region ModelsButtons

        private void Cube_button_Click(object sender, EventArgs e)
        {
            Model model = new Cube();

            facade._execute(
                new ListViewAddModelCommand(ref listView_modelsMain, ref model),
                new AddModelCommand(ref canvas, ref model),
                new RenderCommand(ref canvas, ref picture, GetCurrentRenderMode())
            );
        }

        private void directPrism_button_Click(object sender, EventArgs e)
        {
            Model model = new DirectPrism();

            facade._execute(
                new ListViewAddModelCommand(ref listView_modelsMain, ref model),
                new AddModelCommand(ref canvas, ref model),
                new RenderCommand(ref canvas, ref picture, GetCurrentRenderMode())
            );
        }

        private void triangularPyramid_button_Click(object sender, EventArgs e)
        {
            Model model = new Pyramid();

            facade._execute(
                new ListViewAddModelCommand(ref listView_modelsMain, ref model),
                new AddModelCommand(ref canvas, ref model),
                new RenderCommand(ref canvas, ref picture, GetCurrentRenderMode())
            );
        }

        private void Icosahedron_button_Click(object sender, EventArgs e)
        {
            Model model = new Icosahedron();

            facade._execute(
                new ListViewAddModelCommand(ref listView_modelsMain, ref model),
                new AddModelCommand(ref canvas, ref model),
                new RenderCommand(ref canvas, ref picture, GetCurrentRenderMode())
            );
        }

        private void inclinedPrism_button_Click(object sender, EventArgs e)
        {
            Model model = new InclinedPrism();

            facade._execute(
                new ListViewAddModelCommand(ref listView_modelsMain, ref model),
                new AddModelCommand(ref canvas, ref model),
                new RenderCommand(ref canvas, ref picture, GetCurrentRenderMode())
            );
        }

        #endregion

        #region ActionsButtons

        private void button_dialogEdit_Click(object sender, EventArgs e)
        {
            facade._execute(new DialogEditShowCommand(ref canvas));
        }

        private void button_Clear_Click(object sender, EventArgs e)
        {
            facade._execute(
                new ClearCommand(ref canvas),
                new ListViewClearCommand(ref listView_modelsMain)
            );
        }

        private void button5_Click(object sender, EventArgs e)
        {
            facade._execute(new ImageUpdateCommand(ref canvas, ref picture));
        }

        #endregion

        #region TransformationsButtons

        private void button_moveModel_Click(object sender, EventArgs e)
        {
            int currentIndex = GetCurrentModelIndex();

            if (currentIndex == -1)
                return;

            Move move = new Move(
                (float)numericUpDown_moveX.Value,
                (float)numericUpDown_moveY.Value,
                (float)numericUpDown_moveZ.Value
            );

            facade._execute(
                new MoveModelCommand(ref canvas, ref move, currentIndex),
                new RenderCommand(ref canvas, ref picture, GetCurrentRenderMode())
            );
        }

        private void button_rotateModel_Click(object sender, EventArgs e)
        {
            int currentIndex = GetCurrentModelIndex();

            if (currentIndex == -1)
                return;

            Rotate rotate = new Rotate(
                (float)numericUpDown_angleOx.Value,
                (float)numericUpDown_angleOy.Value,
                (float)numericUpDown_angleOz.Value
            );

            facade._execute(
                new RotateModelCommand(ref canvas, ref rotate, currentIndex),
                new RenderCommand(ref canvas, ref picture, GetCurrentRenderMode())
            );
        }

        private void button_scaleModel_Click(object sender, EventArgs e)
        {
            int currentIndex = GetCurrentModelIndex();

            if (currentIndex == -1)
                return;

            Scale scale = new Scale(
                (float)numericUpDown_scaleX.Value,
                (float)numericUpDown_scaleY.Value,
                (float)numericUpDown_scaleZ.Value
            );

            facade._execute(
                new ScaleModelCommand(ref canvas, ref scale, currentIndex),
                new RenderCommand(ref canvas, ref picture, GetCurrentRenderMode())
            );
        }

        #endregion

        #region Camera

        private void button_cameraUp_Click(object sender, EventArgs e)
        {
            facade._execute(
                new CameraUpMoveCommand(ref canvas, 15),
                new RenderCommand(ref canvas, ref picture, GetCurrentRenderMode())
            );
        }

        private void button_cameraDown_Click(object sender, EventArgs e)
        {
            facade._execute(
                new CameraDownMoveCommand(ref canvas, 15),
                new RenderCommand(ref canvas, ref picture, GetCurrentRenderMode())
            );
        }

        private void button_cameraRight_Click(object sender, EventArgs e)
        {
            facade._execute(
                new CameraRightMoveCommand(ref canvas, 15),
                new RenderCommand(ref canvas, ref picture, GetCurrentRenderMode())
            );
        }

        private void button_CmeraLeft_Click(object sender, EventArgs e)
        {
            facade._execute(
                new CameraDownMoveCommand(ref canvas, 15),
                new RenderCommand(ref canvas, ref picture, GetCurrentRenderMode())
            );
        }

        private void button_cameraLeft_Click(object sender, EventArgs e)
        {
            facade._execute(
                new CameraLeftMoveCommand(ref canvas, 15),
                new RenderCommand(ref canvas, ref picture, GetCurrentRenderMode())
            );
        }

        private void button_cameraRightUp_Click(object sender, EventArgs e)
        {
            facade._execute(
                new CameraUpRightMoveCommand(ref canvas, 15),
                new RenderCommand(ref canvas, ref picture, GetCurrentRenderMode())
            );
        }

        private void button_cameraRightDown_Click(object sender, EventArgs e)
        {
            facade._execute(
                new CameraDownRightMoveCommand(ref canvas, 15),
                new RenderCommand(ref canvas, ref picture, GetCurrentRenderMode())
            );
        }

        private void button_cameraLeftUp_Click(object sender, EventArgs e)
        {
            facade._execute(
                new CameraUpLeftMoveCommand(ref canvas, 15),
                new RenderCommand(ref canvas, ref picture, GetCurrentRenderMode())
            );
        }

        private void button_cameraLeftDown_Click(object sender, EventArgs e)
        {
            facade._execute(
                new CameraDownLeftMoveCommand(ref canvas, 15),
                new RenderCommand(ref canvas, ref picture, GetCurrentRenderMode())
            );
        }

        private void button_cameraRotateRight_Click(object sender, EventArgs e)
        {
            facade._execute(
                new CameraRightRotateCommand(ref canvas, 10),
                new RenderCommand(ref canvas, ref picture, GetCurrentRenderMode())
            );
        }

        private void button_cameraRotateLeft_Click(object sender, EventArgs e)
        {
            facade._execute(new CameraLeftRotateCommand(ref canvas, 10),
                new RenderCommand(ref canvas, ref picture, GetCurrentRenderMode())
            );
        }

        private void button_cameraRotateDown_Click(object sender, EventArgs e)
        {
            facade._execute(new CameraDownRotateCommand(ref canvas, 10),
                new RenderCommand(ref canvas, ref picture, GetCurrentRenderMode())
            );
        }

        private void button_cameraRotateUp_Click(object sender, EventArgs e)
        {
            facade._execute(new CameraUpRotateCommand(ref canvas, 10),
                new RenderCommand(ref canvas, ref picture, GetCurrentRenderMode())
            );
        }

        #endregion

        #region Functions

        private int GetCurrentModelIndex()
        {
            int currentIndex = -1;

            try
            {
                currentIndex = listView_modelsMain.SelectedItems[0].Index;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                facade._execute(new ErrorMessageShowCommand("Модель не выбрана."));
            }

            return currentIndex;
        }

        private RenderMode GetCurrentRenderMode()
        {
            if (checkedListBox_renderMode.CheckedItems.Count == 0)
            {
                facade._execute(new ErrorMessageShowCommand("Режим вывода изображения не выбран. Установлен режим по умолчанию: \"Каркасное отображение\"."));
                checkedListBox_renderMode.SetItemChecked(3, true);
                return RenderMode.CarcassDisplay;
            }

            if (checkedListBox_renderMode.GetItemChecked(0))
                return RenderMode.Shadows;
            else if (checkedListBox_renderMode.GetItemChecked(1))
                return RenderMode.Shading;
            else if (checkedListBox_renderMode.GetItemChecked(2))
                return RenderMode.RealDisplay;
            else
                return RenderMode.CarcassDisplay;
        }

        #endregion

        #region Other

        private void checkedListBox_renderMode_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue != CheckState.Checked)
                return;

            for (int i = 0; i < checkedListBox_renderMode.Items.Count; i++)
            {
                if (i != e.Index)
                    checkedListBox_renderMode.SetItemChecked(i, false);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        private void toolTip1_Popup_1(object sender, PopupEventArgs e)
        {

        }

        private void flowLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void checkedBox_algorithm_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        #endregion
    }
}
