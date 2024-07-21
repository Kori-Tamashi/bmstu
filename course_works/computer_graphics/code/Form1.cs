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
        Form2 dialogEdit;

        Canvas canvas;
        Facade facade;

        DrawCommand drawCommand;
        SceneCommand sceneCommand;
        TransformationCommand transformationCommand;

        public Form1()
        {
            InitializeComponent();
            InitializeDialogs();
            InitializeCanvas();

            facade = new Facade();

            Model model = new Cube();
            sceneCommand = new AddModelCmd(ref canvas, ref model);
            facade._execute(sceneCommand);
        }

        private void InitializeDialogs()
        {
            dialogEdit = new Form2();
        }

        private void InitializeCanvas()
        {
            canvas = new Canvas(new Size(picture.Width, picture.Height), picture.CreateGraphics());
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

        private void triangularPyramid_button_Click(object sender, EventArgs e)
        {
            drawCommand = new ClearCmd(ref canvas);
            facade._execute(drawCommand);
        }

        private void Cube_button_Click(object sender, EventArgs e)
        {
            drawCommand = new DrawCmd(ref canvas);
            facade._execute(drawCommand);
        }

        private void directPrism_button_Click(object sender, EventArgs e)
        {

            //transformationCommand = new MoveCommand(ref canvas, 5, 5, 5);
           // facade._execute(transformationCommand);

            transformationCommand = new RotateCommand(ref canvas, 0, 0, 5);
            facade._execute(transformationCommand);

            //transformationCommand = new ScaleCommand(ref canvas, (float)1.01, (float)1.01, (float)1.01);
           // facade._execute(transformationCommand);

        }

        private void Icosahedron_button_Click(object sender, EventArgs e)
        {
            dialogEdit.ShowDialog();
        }
    }
}
