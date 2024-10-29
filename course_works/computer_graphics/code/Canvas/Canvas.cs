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
        ViewingSystem viewingSystem;

        double lastRender = 0;

        public Canvas(Size viewPortSize, Graphics pictureBoxGraphics)
        { 
            size = viewPortSize;
            graphics = pictureBoxGraphics;

            InitializeScene(viewPortSize);
            InitializeBitmap(viewPortSize);
            InitializeViewingSystem(viewPortSize);
        }

        #region Initialize

        private void InitializeScene(Size viewPortSize)
        {
            scene = new Scene(viewPortSize);
        }

        private void InitializeBitmap(Size viewPortSize)
        {
            bitmap = new Bitmap(viewPortSize.Width, viewPortSize.Height);
        }

        private void InitializeViewingSystem(Size viewPortSize)
        {
            viewingSystem = new ViewingSystem(viewPortSize);
        }

        #endregion

        #region Getters & Setters

        public Graphics Graphics
        {
            get { return graphics; }
            set { graphics = value; }
        }

        public Bitmap Image
        {
            get { return viewingSystem.Image; }
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

        public float Yaw
        {
            get { return viewingSystem.Yaw; }
            set { viewingSystem.Yaw = value; }
        }

        public float Pitch
        {
            get { return viewingSystem.Pitch; }
            set { viewingSystem.Pitch = value; }
        }

        public float LightIntensity
        {
            get { return scene.LightIntensity; }
            set { scene.LightIntensity = value; }
        }

        public Vector3D LightDirection
        {
            get { return scene.LightDirection; }
            set { scene.LightDirection = value; }
        }

        public Point3D LightPosition
        {
            get { return scene.LightPosition; }
            set { scene.LightPosition = value; }
        }

        public double LastRender
        {
            get { return lastRender; }
            set { lastRender = value; }
        }

        #endregion

        #region Scene

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

        public void AddLight(Light light)
        {
            scene.AddLight(light);
        }

        public void RemoveLight(int index)
        {
            scene.RemoveLight(index);
        }

        public void DeleteModels()
        {
            scene.Clear();
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

        public void MoveLight(Move move)
        {
            scene.MoveLight(move);
        }

        #endregion

        #region Drawing

        public void Draw()
        {
            
        }

        public void UpdateImage(ref PictureBox pb)
        {
            pb.Image = Image;
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

        #endregion

        #region ViewingSystem

        public void Render(RenderMode renderMode)
        {
            viewingSystem.Processing(scene, graphics, renderMode);
        }

        public void MoveCamera(Move move)
        {
            viewingSystem.Move(move);
        }

        public void MoveCamera(float dX, float dY, float dZ)
        {
            viewingSystem.Move( new Move(dX, dY, dZ) ); 
        }

        public void MoveCameraForward(float d)
        {
            viewingSystem.MoveForward(d);
        }

        public void MoveCameraBack(float d)
        {
            viewingSystem.MoveBack(d);
        }

        public void MoveCameraRight(float d)
        {
            viewingSystem.MoveRight(d);
        }

        public void MoveCameraUp(float d)
        {
            viewingSystem.MoveUp(d);
        }

        public void MoveCameraLeft(float d)
        {
            viewingSystem.MoveLeft(d);
        }

        public void MoveCameraDown(float d)
        {
            viewingSystem.MoveDown(d);
        }

        public void MoveCameraUpRight(float d)
        {
            viewingSystem.MoveUpRight(d);
        }

        public void MoveCameraUpLeft(float d)
        {
            viewingSystem.MoveUpLeft(d);
        }

        public void MoveCameraDownRight(float d)
        {
            viewingSystem.MoveDownRight(d);
        }

        public void MoveCameraDownLeft(float d)
        {
            viewingSystem.MoveDownLeft(d);
        }

        public void RotateCameraRight(float angle)
        {
            viewingSystem.RotateRight(angle);
        }

        public void RotateCameraLeft(float angle)
        {
            viewingSystem.RotateLeft(angle);
        }

        public void RotateCameraDown(float angle)
        {
            viewingSystem.RotateDown(angle);
        }

        public void RotateCameraUp(float angle)
        {
            viewingSystem.RotateUp(angle);
        }

        #endregion
    }
}
