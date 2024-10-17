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

/*
 * Light system
 * Shadows
 * Camera rotate
 * Dialog edit clear
 */

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
            Model cube = new Cube();
            ViewingFrustum_ParallelZBuffer vf = new ViewingFrustum_ParallelZBuffer(
                picture.Width, 
                picture.Height, 
                100, 400, 
                new Camera(new Vector3D(0, 0, -1), 
                new Point3D(85, -85, 200)));

            Graphics gr = picture.CreateGraphics();

            cube.Rotate(new Rotate(90, 85, 0));

            vf.Processing(cube);
            Action updateImage = () => picture.Image = vf.Image;
            picture.Invoke(updateImage);

            //vf.ProcessModel(cube, gr);
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
            //facade._execute(new ParallelSolidShadingProcessCommand(ref canvas, ref picture));

            List<Light> lights = new List<Light> { new Light(new Vector3D(-0.707f, 0, -0.707f), new Point3D(canvas.Size.Width / 2, canvas.Size.Height / 2, 150)) };
            ZBufferShadows zBuffer = new ZBufferShadows(canvas.Size, canvas.Models, lights, new Vector3D(0, 0, -1));
            Action updateImage = () => picture.Image = zBuffer.Image;
            picture.Invoke(updateImage);
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
                new RefreshCommand(ref canvas)
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
                new RefreshCommand(ref canvas)
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
                new RefreshCommand(ref canvas)
            );
        }

        #endregion

        #region Camera

        private void button_cameraUp_Click(object sender, EventArgs e)
        {
            facade._execute(
                new CameraUpMoveCommand(ref canvas, 5),
                new RefreshCommand(ref canvas)
            );
        }

        private void button_cameraDown_Click(object sender, EventArgs e)
        {
            facade._execute(
                new CameraDownMoveCommand(ref canvas, 5),
                new RefreshCommand(ref canvas)
            );
        }

        private void button_cameraRight_Click(object sender, EventArgs e)
        {
            facade._execute(
                new CameraRightMoveCommand(ref canvas, 5),
                new RefreshCommand(ref canvas)
            );
        }

        private void button_CmeraLeft_Click(object sender, EventArgs e)
        {
            facade._execute(
                new CameraDownMoveCommand(ref canvas, 5),
                new RefreshCommand(ref canvas)
            );
        }

        private void button_cameraLeft_Click(object sender, EventArgs e)
        {
            facade._execute(
                new CameraLeftMoveCommand(ref canvas, 5),
                new RefreshCommand(ref canvas)
            );
        }

        private void button_cameraRightUp_Click(object sender, EventArgs e)
        {
            facade._execute(
                new CameraUpRightMoveCommand(ref canvas, 5),
                new RefreshCommand(ref canvas)
            );
        }

        private void button_cameraRightDown_Click(object sender, EventArgs e)
        {
            facade._execute(
                new CameraDownRightMoveCommand(ref canvas, 5),
                new RefreshCommand(ref canvas)
            );
        }

        private void button_cameraLeftUp_Click(object sender, EventArgs e)
        {
            facade._execute(
                new CameraUpLeftMoveCommand(ref canvas, 5),
                new RefreshCommand(ref canvas)
            );
        }

        private void button_cameraLeftDown_Click(object sender, EventArgs e)
        {
            facade._execute(
                new CameraDownLeftMoveCommand(ref canvas, 5),
                new RefreshCommand(ref canvas)
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

        #endregion

        #region Other

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

        #endregion

        
    }
}
