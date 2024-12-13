using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace code
{
    public class Light
    {
        float intensity;
        Camera camera;

        float startIntensity = 5;
        Vector3D startCameraDirection = new Vector3D(0, 0, -1);
        Point3D startCameraPosition = new Point3D(85, -85, 300);

        public Light()
        {
            intensity = startIntensity;
            InitializeCamera(startCameraDirection, startCameraPosition);
        }

        public Light(float intensity, Vector3D direction, Point3D position)
        {
            this.intensity = intensity;
            InitializeCamera(direction, position);
        }

        public Light(Vector3D direction, Point3D position, float intensity = 5)
        {
            this.intensity = intensity;
            InitializeCamera(direction, position);
        }

        private void InitializeCamera(Vector3D direction, Point3D position)
        {
            camera = new Camera(direction, position);
        }

        #region Getters & Setters

        public Camera Camera
        {
            get { return camera; }
        }

        public Vector3D Direction
        {
            get { return camera.Direction; }
            set { camera.Direction = value; }
        }

        public Point3D Position
        {
            get { return camera.Position; }
            set { camera.Position = value; }
        }

        public float Intensity
        {
            get { return intensity; }
            set { intensity = value; }
        }

        #endregion

        #region Movement

        public void Move(Move move)
        {
            camera.Move(move);
        }

        public void MoveRight(float d)
        {
            camera.MoveRight(d);
        }

        public void MoveLeft(float d)
        {
            camera.MoveLeft(d);
        }

        public void MoveUp(float d)
        {
            camera.MoveUp(d);
        }

        public void MoveDown(float d)
        {
            camera.MoveDown(d);
        }

        public void MoveUpRight(float d)
        {
            camera.MoveUpRight(d);
        }

        public void MoveUpLeft(float d)
        {
            camera.MoveUpLeft(d);
        }

        public void MoveDownRight(float d)
        {
            camera.MoveDownRight(d);
        }

        public void MoveDownLeft(float d)
        {
            camera.MoveDownLeft(d);
        }

        #endregion
    }
}
