using code;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace code
{
    class Scene
    {
        List<Model> models;
        Point3D center;
        Size size;
        
        public Scene(Size size) 
        { 
            models = new List<Model>();
            center = new Point3D(size.Width / 2, size.Height / 2, (size.Width + size.Height) / 4);
            this.size = size;
        }

        public List<Model> Models
        {
            get { return models; }
            set { models = value; }
        }

        public Model Model(int index)
        {
            return models[index];
        }

        public void Model(int index, Model model)
        {
            models[index] = model;
        }

        public Point3D Center
        {
            get { return center; }
            set { center = value; }
        }

        public Size Size
        {
            get { return size; }
            set { size = value; }
        }

        public void Draw(Graphics graphics)
        {
            foreach (Model model in models)
            {
                model.Draw(graphics);
            }
        }

        public void AddModel(Model model)
        {
            models.Add(model);
        }

        public void RemoveModel(Model model)
        {
            models.Remove(model);
        }

        public void RemoveModel(int index)
        {
            models.RemoveAt(index);
        }

        public void Clear()
        {
            models.Clear();
        }

        public void Move(Move move)
        {
            foreach (Model model in models)
            {
                model.Move(move);
            }
        }

        public void Rotate(Rotate rotate)
        {
            foreach (Model model in models)
            {
                model.Rotate(rotate);
            }
        }

        public void Scale(Scale scale)
        {
            foreach (Model model in models)
            {
                model.Scale(scale);
            }
        }

        public void Centering(Centering centering)
        {
            foreach (Model model in models)
            {
                model.Centering(centering);
            }
        }

        public void Centering()
        {
            foreach (Model model in models)
            {
                model.Centering(center, size);
            }
        }
    }
}
