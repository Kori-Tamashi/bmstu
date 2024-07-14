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
        Bitmap image;
        Graphics graphics;
        Scene scene;
        Pen basePen;

        public Form1()
        {
            InitializeComponent();

            graphics = canvas.CreateGraphics();
            scene = new Scene(canvas.Size);

            basePen = new Pen(Color.Black);

            Cube cube = new Cube();
            Pyramid pyramid = new Pyramid();
            scene.AddModel(pyramid);
            
            


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

        }

        private void Cube_button_Click(object sender, EventArgs e)
        {
            scene.Draw(ref graphics, basePen);
        }
    }
}
