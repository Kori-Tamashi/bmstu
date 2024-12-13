using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using Windows.Devices.Input;

namespace code
{
    class ViewingSystem
    {
        protected const int maxCameras = 1;
        protected const int farDistance = 400;
        protected const int nearDistance = 100;

        protected Vector3D startCameraDirection = new Vector3D(0, 0, 1);
        protected Point3D startCameraPosition = new Point3D(85, -85, -300);

        protected Size viewPortSize;
        protected int currentCameraIndex = 0;
        protected List<ViewingFrustumProcessor> cameras;

        public ViewingSystem(Size viewPortSize)
        {
            this.viewPortSize = viewPortSize;
            InitializeCameras(viewPortSize);
        }

        #region Initialize

        private void InitializeCameras(Size viewPortSize)
        {
            cameras = new List<ViewingFrustumProcessor> {
                StartViewingFrustum(viewPortSize)
            };
        }

        private ViewingFrustumProcessor StartViewingFrustum(Size viewPortSize)
        {
            return new ViewingFrustumProcessor(
                    viewPortSize.Width,
                    viewPortSize.Height,
                    nearDistance,
                    farDistance,
                    StartCamera()
                    );
        }

        private Camera StartCamera()
        {
            Camera camera = new Camera(startCameraDirection, startCameraPosition);
            return camera;
        }

        #endregion

        #region Getters & Setters

        public Bitmap Image
        {
            get { return cameras[currentCameraIndex].Image; }
        }

        public int CurrentCameraIndex
        {
            get { return currentCameraIndex; }
            set { currentCameraIndex = Math.Clamp(value, 0, cameras.Count - 1); }
        }

        public int MaxCameras
        {
            get { return maxCameras; }
        }

        public int NearDistance
        {
            get { return nearDistance; }
        }

        public int FarDistance
        {
            get { return farDistance; }
        }

        public float Yaw
        {
            get { return cameras[currentCameraIndex].Yaw; }
            set { cameras[currentCameraIndex].Yaw = value; }
        }

        public float Pitch
        {
            get { return cameras[currentCameraIndex].Pitch; }
            set { cameras[currentCameraIndex].Pitch = value; }
        }

        #endregion

        #region Methods

        public void Processing(Scene scene, Graphics gr, RenderMode renderMode)
        {
            cameras[currentCameraIndex].Processing(scene, gr, renderMode);
        }

        public void Processing(Scene scene, Graphics gr)
        {
            cameras[currentCameraIndex].Processing(scene, gr);
        }

        public void Processing(Scene scene)
        {
            cameras[currentCameraIndex].Processing(scene);
        }

        public void Processing(List<Model> models, Graphics gr)
        {
            cameras[currentCameraIndex].Processing(models, gr);
        }

        public void Processing(List<Model> models)
        {
            cameras[currentCameraIndex].Processing(models);
        }

        public void Processing(Model model, Graphics gr)
        {
            cameras[currentCameraIndex].Processing(model, gr);
        }

        public void Processing(Model model)
        {
            cameras[currentCameraIndex].Processing(model);
        }

        public void DrawCoordinateSystem(Graphics gr)
        {
            float penWidth = 7;
            float axisLength = 90;
            
            Point center = new Point(110, viewPortSize.Height - 50);
            Rotate rotate = new Rotate(cameras[currentCameraIndex].Pitch, cameras[currentCameraIndex].Yaw, 0);

            Vector3D Z = new Vector3D(axisLength, 0, 0);
            Vector3D Y = new Vector3D(0, axisLength, 0);
            Vector3D X = new Vector3D(0, 0, -axisLength);

            X.Rotate(rotate);
            Y.Rotate(rotate);
            Z.Rotate(rotate);

            Point3D xEnd = new Point3D(center.X + (int)X.X, center.Y - (int)X.Y, X.Z);
            Point3D yEnd = new Point3D(center.X + (int)Y.X, center.Y - (int)Y.Y, Y.Z);
            Point3D zEnd = new Point3D(center.X - (int)Z.X, center.Y - (int)Z.Y, Z.Z);

            Point xEndp = new Point((int)xEnd.X, (int)xEnd.Y);
            Point yEndp = new Point((int)yEnd.X, (int)yEnd.Y);
            Point zEndp = new Point((int)zEnd.X, (int)zEnd.Y);

            Pen xPen = new Pen(Color.Red, penWidth);
            Pen yPen = new Pen(Color.Green, penWidth);
            Pen zPen = new Pen(Color.Blue, penWidth);

            gr.DrawLine(xPen, center, xEndp);
            gr.DrawLine(yPen, center, yEndp);
            gr.DrawLine(zPen, center, zEndp);

            Font labelFont = new Font("Microsoft Sans Serif", 14);

            if (xEnd.Z > yEnd.Z)
            {
                if (xEnd.Z > zEnd.Z)
                {
                    gr.DrawLine(xPen, center, xEndp);
                    gr.DrawString("X", labelFont, Brushes.Red, xEndp);

                    if (yEnd.Z > zEnd.Z)
                    {
                        gr.DrawLine(yPen, center, yEndp);
                        gr.DrawString("Y", labelFont, Brushes.Green, yEndp);
                        gr.DrawLine(zPen, center, zEndp);
                        gr.DrawString("Z", labelFont, Brushes.Blue, zEndp);
                    }
                    else
                    {
                        gr.DrawLine(zPen, center, zEndp);
                        gr.DrawString("Z", labelFont, Brushes.Blue, zEndp);
                        gr.DrawLine(yPen, center, yEndp);
                        gr.DrawString("Y", labelFont, Brushes.Green, yEndp);
                    } 
                }
                else
                {
                    gr.DrawLine(zPen, center, zEndp);
                    gr.DrawString("Z", labelFont, Brushes.Blue, zEndp);
                    gr.DrawLine(xPen, center, xEndp);
                    gr.DrawString("X", labelFont, Brushes.Red, xEndp);
                    gr.DrawLine(yPen, center, yEndp);
                    gr.DrawString("Y", labelFont, Brushes.Green, yEndp);
                }
            }
            else
            {
                if (yEnd.Z > zEnd.Z)
                {
                    gr.DrawLine(yPen, center, yEndp);
                    gr.DrawString("Y", labelFont, Brushes.Green, yEndp);

                    if (xEnd.Z > zEnd.Z)
                    {
                        gr.DrawLine(xPen, center, xEndp);
                        gr.DrawString("X", labelFont, Brushes.Red, xEndp);
                        gr.DrawLine(zPen, center, zEndp);
                        gr.DrawString("Z", labelFont, Brushes.Blue, zEndp);
                    }
                    else
                    {
                        gr.DrawLine(zPen, center, zEndp);
                        gr.DrawString("Z", labelFont, Brushes.Blue, zEndp);
                        gr.DrawLine(xPen, center, xEndp);
                        gr.DrawString("X", labelFont, Brushes.Red, xEndp);
                    }
                }
                else
                {
                    gr.DrawLine(zPen, center, zEndp);
                    gr.DrawString("Z", labelFont, Brushes.Blue, zEndp);
                    gr.DrawLine(yPen, center, yEndp);
                    gr.DrawString("Y", labelFont, Brushes.Green, yEndp);
                    gr.DrawLine(xPen, center, xEndp);
                    gr.DrawString("X", labelFont, Brushes.Red, xEndp);
                }
            }

        }

        public void AddCamera(Vector3D direction, Point3D position)
        {
            cameras.Add(
                new ViewingFrustumProcessor(
                    viewPortSize.Width,
                    viewPortSize.Height,
                    nearDistance,
                    farDistance,
                    new Camera(direction, position)
                )
            );
        }

        public void DeleteCamera(int index)
        {
            cameras.RemoveAt(index);
        }

        public void Move(Move move)
        {
            cameras[currentCameraIndex].Move(move);
        }

        public void MoveForward(float d)
        {
            cameras[currentCameraIndex].MoveForward(d);
        }

        public void MoveBack(float d)
        {
            cameras[currentCameraIndex].MoveBack(d);
        }

        public void MoveRight(float d)
        {
            cameras[currentCameraIndex].MoveRight(d);
        }

        public void MoveUp(float d)
        {
            cameras[currentCameraIndex].MoveUp(d);
        }

        public void MoveLeft(float d)
        {
            cameras[currentCameraIndex].MoveLeft(d);
        }

        public void MoveDown(float d)
        {
            cameras[currentCameraIndex].MoveDown(d);
        }

        public void MoveUpRight(float d)
        {
            cameras[currentCameraIndex].MoveUpRight(d);
        }

        public void MoveUpLeft(float d)
        {
            cameras[currentCameraIndex].MoveUpLeft(d);
        }

        public void MoveDownRight(float d)
        {
            cameras[currentCameraIndex].MoveDownRight(d);
        }

        public void MoveDownLeft(float d)
        {
            cameras[currentCameraIndex].MoveDownLeft(d);
        }

        public void RotateRight(float angle)
        {
            cameras[currentCameraIndex].RotateRight(angle);
        }

        public void RotateLeft(float angle)
        {
            cameras[currentCameraIndex].RotateLeft(angle);
        }

        public void RotateDown(float angle)
        {
            cameras[currentCameraIndex].RotateDown(angle);
        }

        public void RotateUp(float angle)
        {
            cameras[currentCameraIndex].RotateUp(angle);
        }

        #endregion
    }
}
