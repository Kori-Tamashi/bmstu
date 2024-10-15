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
        protected List<ViewingFrustum> cameras;

        public ViewingSystem(Size viewPortSize)
        {
            this.viewPortSize = viewPortSize;
            InitializeCameras(viewPortSize);
        }

        #region Initialize

        private void InitializeCameras(Size viewPortSize)
        {
            cameras = new List<ViewingFrustum> {
                StartViewingFrustum(viewPortSize)
            };
        }

        private ViewingFrustum StartViewingFrustum(Size viewPortSize)
        {
            return new ViewingFrustum(
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

        public void Processing(Scene scene, Graphics gr)
        {
            Processing(scene.Models, gr);
        }

        public void Processing(List<Model> models, Graphics gr)
        {
            foreach (Model model in models)
            {
                Processing(model, gr);
            }
        }

        public void Processing(Model model, Graphics gr)
        {
            cameras[currentCameraIndex].ProcessModel(model, gr);
        }

        public void AddCamera(Vector3D direction, Point3D position)
        {
            cameras.Add(
                new ViewingFrustum(
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

        #endregion
    }
}
