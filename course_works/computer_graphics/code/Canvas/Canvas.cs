﻿using code;
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

        #endregion

        #region Drawing

        public void Draw()
        {
            viewingSystem.Processing(scene, graphics);
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

        public void MoveCamera(Move move)
        {
            viewingSystem.Move(move);
        }

        public void MoveCamera(float dX, float dY, float dZ)
        {
            viewingSystem.Move( new Move(dX, dY, dZ) ); 
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

        public void MoveUpRight(float d)
        {
            viewingSystem.MoveUpRight(d);
        }

        public void MoveUpLeft(float d)
        {
            viewingSystem.MoveUpLeft(d);
        }

        public void MoveDownRight(float d)
        {
            viewingSystem.MoveDownRight(d);
        }

        public void MoveDownLeft(float d)
        {
            viewingSystem.MoveDownLeft(d);
        }

        #endregion
    }
}
