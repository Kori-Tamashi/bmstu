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
        Point3D startCenter = new Point3D(0, 0, 0);

        Size size;
        Point3D center;
        List<Model> models;
        LightSystem lightSystem;
        
        public Scene(Size size) 
        {
            this.size = size;
            
            InitializeModels();
            InitializeCenter();
            InitializeLightSystem();
        }

        #region Initialize

        private void InitializeModels()
        {
            models = new List<Model>();
        }

        private void InitializeCenter()
        {
            center = startCenter;
        }

        private void InitializeLightSystem()
        {
            lightSystem = new LightSystem();
        }

        #endregion

        #region Getters & Setters

        public List<Model> Models
        {
            get { return models; }
            set { models = value; }
        }

        public Light CurrentLight
        {
            get { return lightSystem.CurrentLight; }
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

        public float LightIntensity
        {
            get { return lightSystem.LightIntensity; }
            set { lightSystem.LightIntensity = value; }
        }

        public Vector3D LightDirection
        {
            get { return lightSystem.LightDirection; }
            set { lightSystem.LightDirection = value; }
        }

        public Point3D LightPosition
        {
            get { return lightSystem.LightPosition; }
            set { lightSystem.LightPosition = value; }
        }

        #endregion

        #region Draw

        public void Draw(Graphics graphics)
        {
            foreach (Model model in models)
            {
                model.Draw(graphics);
            }
        }

        #endregion

        #region Models

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

        #endregion

        #region Lights

        public void AddLight(Light light)
        {
            lightSystem.AddLight(light);
        }

        public void RemoveLight(int index)
        {
            lightSystem.DeleteLight(index);
        }

        #endregion

        #region Transformation

        public void Move(Move move)
        {
            foreach (Model model in models)
            {
                model.Move(move);
            }
        }

        public void Move(Move move, int index)
        {
            models[index].Move(move);
        }

        public void Rotate(Rotate rotate)
        {
            foreach (Model model in models)
            {
                model.Rotate(rotate);
            }
        }

        public void Rotate(Rotate rotate, int index)
        {
            models[index].Rotate(rotate);
        }

        public void Scale(Scale scale)
        {
            foreach (Model model in models)
            {
                model.Scale(scale);
            }
        }

        public void Scale(Scale scale, int index)
        {
            models[index].Scale(scale);
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

        public void MoveLight(Move move)
        {
            lightSystem.MoveLight(move);
        }

        #endregion
    }
}
