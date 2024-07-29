using code;
using code;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace code
{

    class Canvas
    {
        Pen pen;
        Size size;
        Bitmap bitmap;
        Graphics graphics;

        public Scene scene;

        public Canvas(Size size, Graphics g)
        {
            this.size = size;
            pen = new Pen(Color.Black);
            scene = new Scene(size);
            bitmap = new Bitmap(size.Width, size.Height);
            graphics = g;
        }

        public List<Model> Models()
        {
            return scene.models;
        }

        public Model Model(int index)
        {
            return scene.models[index];
        }

        public Point3D Center()
        {
            return scene.center;
        }

        public Size Size()
        {
            return size;
        }

        public void AddModel(Model model)
        {
            scene.AddModel(model);
        }

        public void RemoveModel(Model model)
        {
            scene.RemoveModel(model);
        }

        public void RemoveModel(int index)
        {
            scene.RemoveModel(index);
        }

        public void DeleteModels()
        {
            scene.Clear();
        }

        public void Draw()
        {
            scene.Draw(graphics, pen);
        }

        public void Clear()
        {
            graphics.Clear(Color.White);
        }

        public void Refresh()
        {
            Clear();
            Draw();
        }

        public void Move(Move move)
        {
            scene.Move(move);
            Refresh();
        }

        public void Rotate(Rotate rotate)
        {
            scene.Rotate(rotate);
            Refresh();
        }

        public void Scale(Scale scale)
        {
            scene.Scale(scale);
            Refresh();
        }

        public void Centering(Centering centering)
        {
            scene.Centering(centering);
            Refresh();
        }
    }
}
