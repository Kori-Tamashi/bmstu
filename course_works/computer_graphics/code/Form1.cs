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

        DrawCommand drawCommand;
        SceneCommand sceneCommand;

        public Form1()
        {
            InitializeComponent();
            InitializeCanvas();

            facade = new Facade();

            Model model = new Pyramid();
            sceneCommand = new AddModelCmd(ref canvas, ref model);
            facade._execute(sceneCommand);
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
    }
}
