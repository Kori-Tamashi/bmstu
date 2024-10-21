using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace code
{
    class ViewingSystem
    {
        protected const int maxCameras = 6;
        protected const int farDistance = 400;
        protected const int nearDistance = 100;

        protected Vector3D startCameraDirection = new Vector3D(0, 0, -1);
        protected Point3D startCameraPosition = new Point3D(85, -85, 300);


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
            return new Camera(startCameraDirection, startCameraPosition);
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
