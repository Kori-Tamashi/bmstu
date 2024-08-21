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
        DialogEdit dialogEdit;

        Canvas canvas;
        Facade facade;

        DrawsCommand drawCommand;
        SceneCommand sceneCommand;
        TransformationCommand transformationCommand;

        public Form1()
        {
            InitializeComponent();
            InitializeDialogs();
            InitializeCanvas();

            facade = new Facade();

            Model model = new Cube();
            //model.Name = "Кубик";
            //model.Color = Color.Red;
            //sceneCommand = new AddModelCommand(ref canvas, ref model);
            //facade._execute(sceneCommand);

            model = new Pyramid();
            model.Name = "Пирамида";
            sceneCommand = new AddModelCommand(ref canvas, ref model);
            facade._execute(sceneCommand);

            //model = new DirectPrism();
            //model.Name = "Прямая призма";
            //sceneCommand = new AddModelCommand(ref canvas, ref model);
            //facade._execute(sceneCommand);

            //model = new Icosahedron();
            //model.Name = "Икосаэдр";
            //sceneCommand = new AddModelCommand(ref canvas, ref model);
            //facade._execute(sceneCommand);

            model = new InclinedPrism();
            model.Name = "Наклонная призма";
            sceneCommand = new AddModelCommand(ref canvas, ref model);
            facade._execute(sceneCommand);
        }

        private void InitializeDialogs()
        {
            
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
            drawCommand = new ClearCommand(ref canvas);
            facade._execute(drawCommand);
        }

        private void Cube_button_Click(object sender, EventArgs e)
        {
            drawCommand = new DrawCommand(ref canvas);
            facade._execute(drawCommand);

            drawCommand = new RefreshCommand(ref canvas);
            facade._execute(drawCommand);
        }

        private void directPrism_button_Click(object sender, EventArgs e)
        {
            transformationCommand = new MoveCommand(ref canvas, 5, 5, 5);
            facade._execute(transformationCommand);

            transformationCommand = new RotateCommand(ref canvas, 10, 10, 0);
            facade._execute(transformationCommand);

            //transformationCommand = new ScaleCommand(ref canvas, (float)1.8, (float)1.8, (float)1.8);
            //facade._execute(transformationCommand);

            drawCommand = new RefreshCommand(ref canvas);
            facade._execute(drawCommand);
        }

        private void Icosahedron_button_Click(object sender, EventArgs e)
        {
            dialogEdit = new DialogEdit(ref canvas);
            dialogEdit.ShowDialog();
        }
    }
}
