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
        Size size;
        Scene scene;
        Bitmap bitmap;
        Graphics graphics;
        List<Camera> cameras;

        public Canvas(Size size, Graphics g)
        {
            this.size = size;
            scene = new Scene(size);
            bitmap = new Bitmap(size.Width, size.Height);
            graphics = g;
        }

        public Graphics Graphics
        {
            get { return graphics; }
            set { graphics = value; }
        }

        public List<Model> Models
        {
            get { return scene.Models; }
            set { scene.Models = value; }
        }

        
        public Model Model(int index)
        {
            return scene.Model(index);   
        }

        public void Model(int index, Model model)
        {
            scene.Model(index, model);
        }

        public Point3D Center
        {
            get { return scene.Center; }
        }

        public Size Size
        {
            get { return size; }
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
            scene.Draw(graphics);
        }

        public void GraphicsClear()
        {
            graphics.Clear(Color.White);
        }

        public void Clear()
        {
            DeleteModels();
            GraphicsClear();
        }

        public void Refresh()
        {
            GraphicsClear();
            Draw();
        }

        public void Move(Move move)
        {
            scene.Move(move);
        }

        public void Move(Move move, int index)
        {
            scene.Move(move, index);
        }

        public void Rotate(Rotate rotate)
        {
            scene.Rotate(rotate);
        }

        public void Rotate(Rotate rotate, int index)
        {
            scene.Rotate(rotate, index);
        }

        public void Scale(Scale scale)
        {
            scene.Scale(scale);
        }

        public void Scale(Scale scale, int index)
        {
            scene.Scale(scale, index);
        }

        public void Centering(Centering centering)
        {
            scene.Centering(centering);
        }

        public void Centering()
        {
            scene.Centering();
        }

        public void Centering(Model model)
        {
            scene.Centering(new Centering(model, Center, Size));
        }

    }
}
