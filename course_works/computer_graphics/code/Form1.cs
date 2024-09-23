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
using static System.Windows.Forms.AxHost;

namespace code
{
    public partial class Form1 : Form
    {
        Canvas canvas;
        Facade facade;
        CameraSystem cameraSystem;

        public Form1()
        {
            InitializeComponent();
            InitializeCanvas();
            InitializeFacade();
            InitializeCameras();
        }

        private void InitializeCanvas()
        {
            canvas = new Canvas(new Size(picture.Width, picture.Height), picture.CreateGraphics());
        }

        private void InitializeFacade()
        {
            facade = new Facade();
        }

        private void InitializeCameras()
        {
            cameraSystem = new CameraSystem(canvas.Size);
        }

        private void Cube_button_Click(object sender, EventArgs e)
        {
            Model model = new Cube();

            facade._execute(
                new ListViewAddModelCommand(ref listView_modelsMain, ref model),
                new AddModelCommand(ref canvas, ref model),
                new RefreshCommand(ref canvas)
            );
        }

        private void directPrism_button_Click(object sender, EventArgs e)
        {
            Model model = new DirectPrism();

            facade._execute(
                new ListViewAddModelCommand(ref listView_modelsMain, ref model),
                new AddModelCommand(ref canvas, ref model),
                new RefreshCommand(ref canvas)
            );
        }

        private void triangularPyramid_button_Click(object sender, EventArgs e)
        {
            Model model = new Pyramid();

            facade._execute(
                new ListViewAddModelCommand(ref listView_modelsMain, ref model),
                new AddModelCommand(ref canvas, ref model),
                new RefreshCommand(ref canvas)
            );
        }

        private void Icosahedron_button_Click(object sender, EventArgs e)
        {
            Model model = new Icosahedron();

            facade._execute(
                new ListViewAddModelCommand(ref listView_modelsMain, ref model),
                new AddModelCommand(ref canvas, ref model),
                new RefreshCommand(ref canvas)
            );
        }

        private void inclinedPrism_button_Click(object sender, EventArgs e)
        {
            Model model = new InclinedPrism();

            facade._execute(
                new ListViewAddModelCommand(ref listView_modelsMain, ref model),
                new AddModelCommand(ref canvas, ref model),
                new RefreshCommand(ref canvas)
            );
        }

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

        private void button_moveModel_Click(object sender, EventArgs e)
        {
            int currentIndex = listView_modelsMain.SelectedItems[0].Index;

            Move move = new Move(
                (float)numericUpDown_moveX.Value,
                (float)numericUpDown_moveY.Value,
                (float)numericUpDown_moveZ.Value
            );

            facade._execute(
                new MoveModelCommand(ref canvas, ref move, currentIndex),
                new RefreshCommand(ref canvas)
            );
        }

        private void button_rotateModel_Click(object sender, EventArgs e)
        {
            int currentIndex = listView_modelsMain.SelectedItems[0].Index;

            Rotate rotate = new Rotate(
                (float)numericUpDown_angleOx.Value,
                (float)numericUpDown_angleOy.Value,
                (float)numericUpDown_angleOz.Value
            );

            facade._execute(
                new RotateModelCommand(ref canvas, ref rotate, currentIndex),
                new RefreshCommand(ref canvas)
            );
        }

        private void button_scaleModel_Click(object sender, EventArgs e)
        {
            int currentIndex = listView_modelsMain.SelectedItems[0].Index;

            Scale scale = new Scale(
                (float)numericUpDown_scaleX.Value,
                (float)numericUpDown_scaleY.Value,
                (float)numericUpDown_scaleZ.Value
            );

            facade._execute(
                new ScaleModelCommand(ref canvas, ref scale, currentIndex),
                new RefreshCommand(ref canvas)
            );
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

        private async void button5_Click(object sender, EventArgs e)
        {
            facade._execute(new ParallelZBufferProcessCommand(ref canvas, ref picture));
        }

        private void button_cameraUp_Click(object sender, EventArgs e)
        {
            facade._execute(
                new CameraUpMoveCommand(ref canvas, ref cameraSystem),
                new RefreshCommand(ref canvas)
            );
        }

        private void button_cameraDown_Click(object sender, EventArgs e)
        {
            facade._execute(
                new CameraDownMoveCommand(ref canvas, ref cameraSystem),
                new RefreshCommand(ref canvas)
            );
        }

        private void button_cameraRight_Click(object sender, EventArgs e)
        {
            facade._execute(
                new CameraRightMoveCommand(ref canvas, ref cameraSystem),
                new RefreshCommand(ref canvas)
            );
        }

        private void button_CmeraLeft_Click(object sender, EventArgs e)
        {
            facade._execute(
                new CameraDownMoveCommand(ref canvas, ref cameraSystem),
                new RefreshCommand(ref canvas)
            );
        }

        private void button_cameraLeft_Click(object sender, EventArgs e)
        {
            facade._execute(
                new CameraLeftMoveCommand(ref canvas, ref cameraSystem),
                new RefreshCommand(ref canvas)
            );
        }

        private void button_cameraRightUp_Click(object sender, EventArgs e)
        {
            facade._execute(
                new CameraUpRightMoveCommand(ref canvas, ref cameraSystem),
                new RefreshCommand(ref canvas)
            );
        }

        private void button_cameraRightDown_Click(object sender, EventArgs e)
        {
            facade._execute(
                new CameraDownRightMoveCommand(ref canvas, ref cameraSystem),
                new RefreshCommand(ref canvas)
            );
        }

        private void button_cameraLeftUp_Click(object sender, EventArgs e)
        {
            facade._execute(
                new CameraUpLeftMoveCommand(ref canvas, ref cameraSystem),
                new RefreshCommand(ref canvas)
            );
        }

        private void button_cameraLeftDown_Click(object sender, EventArgs e)
        {
            facade._execute(
                new CameraDownLeftMoveCommand(ref canvas, ref cameraSystem),
                new RefreshCommand(ref canvas)
            );
        }
    }
}
