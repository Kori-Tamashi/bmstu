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

namespace code
{
    public partial class Form1 : Form
    {
        Canvas canvas;
        Facade facade;

        FormCommand formCommand;
        DrawsCommand drawCommand;
        SceneCommand sceneCommand;
        TransformationCommand transformationCommand;

        public Form1()
        {
            InitializeComponent();
            InitializeCanvas();
            InitializeFacade();
        }

        private void InitializeCanvas()
        {
            canvas = new Canvas(new Size(picture.Width, picture.Height), picture.CreateGraphics());
        }

        private void InitializeFacade()
        {
            facade = new Facade();
        }

        private void Cube_button_Click(object sender, EventArgs e)
        {
            Model model = new Cube();

            formCommand = new ListViewAddModelCommand(ref listView_modelsMain, ref model);
            sceneCommand = new AddModelCommand(ref canvas, ref model);
            drawCommand = new RefreshCommand(ref canvas);

            facade._execute(formCommand);
            facade._execute(sceneCommand);
            facade._execute(drawCommand);
        }

        private void directPrism_button_Click(object sender, EventArgs e)
        {
            Model model = new DirectPrism();

            formCommand = new ListViewAddModelCommand(ref listView_modelsMain, ref model);
            sceneCommand = new AddModelCommand(ref canvas, ref model);
            drawCommand = new RefreshCommand(ref canvas);

            facade._execute(formCommand);
            facade._execute(sceneCommand);
            facade._execute(drawCommand);
        }

        private void triangularPyramid_button_Click(object sender, EventArgs e)
        {
            Model model = new Pyramid();

            formCommand = new ListViewAddModelCommand(ref listView_modelsMain, ref model);
            sceneCommand = new AddModelCommand(ref canvas, ref model);
            drawCommand = new RefreshCommand(ref canvas);

            facade._execute(formCommand);
            facade._execute(sceneCommand);
            facade._execute(drawCommand);
        }

        private void Icosahedron_button_Click(object sender, EventArgs e)
        {
            Model model = new Icosahedron();

            formCommand = new ListViewAddModelCommand(ref listView_modelsMain, ref model);
            sceneCommand = new AddModelCommand(ref canvas, ref model);
            drawCommand = new RefreshCommand(ref canvas);

            facade._execute(formCommand);
            facade._execute(sceneCommand);
            facade._execute(drawCommand);
        }

        private void inclinedPrism_button_Click(object sender, EventArgs e)
        {
            Model model = new InclinedPrism();

            formCommand = new ListViewAddModelCommand(ref listView_modelsMain, ref model);
            sceneCommand = new AddModelCommand(ref canvas, ref model);
            drawCommand = new RefreshCommand(ref canvas);

            facade._execute(formCommand);
            facade._execute(sceneCommand);
            facade._execute(drawCommand);
        }

        private void button_dialogEdit_Click(object sender, EventArgs e)
        {
            formCommand = new DialogEditShowCommand(ref canvas);
            facade._execute(formCommand);
        }

        private void button_Clear_Click(object sender, EventArgs e)
        {
            drawCommand = new ClearCommand(ref canvas);
            formCommand = new ListViewClearCommand(ref listView_modelsMain);

            facade._execute(formCommand);
            facade._execute(drawCommand);
        }

        private void button_moveModel_Click(object sender, EventArgs e)
        {
            int currentIndex = listView_modelsMain.SelectedItems[0].Index;

            Move move = new Move(
                (float)numericUpDown_moveX.Value,
                (float)numericUpDown_moveY.Value,
                (float)numericUpDown_moveZ.Value
            );

            transformationCommand = new MoveModelCommand(ref canvas, ref move, currentIndex);
            drawCommand = new RefreshCommand(ref canvas);

            facade._execute(transformationCommand);
            facade._execute(drawCommand);
        }

        private void button_rotateModel_Click(object sender, EventArgs e)
        {
            int currentIndex = listView_modelsMain.SelectedItems[0].Index;

            Rotate rotate = new Rotate(
                (float)numericUpDown_angleOx.Value,
                (float)numericUpDown_angleOy.Value,
                (float)numericUpDown_angleOz.Value
            );

            transformationCommand = new RotateModelCommand(ref canvas, ref rotate, currentIndex);
            drawCommand = new RefreshCommand(ref canvas);

            facade._execute(transformationCommand);
            facade._execute(drawCommand);
        }

        private void button_scaleModel_Click(object sender, EventArgs e)
        {
            int currentIndex = listView_modelsMain.SelectedItems[0].Index;

            Scale scale = new Scale(
                (float)numericUpDown_scaleX.Value,
                (float)numericUpDown_scaleY.Value,
                (float)numericUpDown_scaleZ.Value
            );

            transformationCommand = new ScaleModelCommand(ref canvas, ref scale, currentIndex);
            drawCommand = new RefreshCommand(ref canvas);

            facade._execute(transformationCommand);
            facade._execute(drawCommand);
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

        private void button5_Click(object sender, EventArgs e)
        {
            ZBuffer zBuffer = new ZBuffer(canvas.Size, canvas.Models);
            picture.Image = zBuffer.Image;
            picture.Refresh();

            //canvas.Graphics = picture.CreateGraphics();

            //drawCommand = new DrawsCommand(ref canvas);
            //facade._execute(drawCommand);

            //drawCommand = new RefreshCommand(ref canvas);
            //facade._execute(drawCommand);
        }
    }
}
