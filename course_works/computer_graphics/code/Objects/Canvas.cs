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

        public Canvas(Size size, Graphics g)
        {
            this.size = size;
            scene = new Scene(size);
            bitmap = new Bitmap(size.Width, size.Height);
            graphics = g;
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
            set { scene.Center = value; }
        }

        public Size Size
        {
            get { return size; }
            set { size = value; }
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
        }

        public void Rotate(Rotate rotate)
        {
            scene.Rotate(rotate);
        }

        public void Scale(Scale scale)
        {
            scene.Scale(scale);
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
