using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace code
{
    class CameraSystem
    {
        List<Camera> cameras;
        int currentCamera;

        public CameraSystem(Size size)
        {
            cameras = new List<Camera> { };
            currentCamera = 0;

            Vector3D direction = new Vector3D(0, 0, -1);
            Point3D position = new Point3D(size.Width / 2, size.Height / 2, 0);
            cameras.Add(new Camera(direction, position));
        }

        public int MoveStep 
        {
            get { return cameras[currentCamera].MoveStep; }
        }

        public void MoveCamera(Direction direction)
        {
            cameras[currentCamera].Move(direction);
        }

        public void RotateCamera(Rotate rotate)
        {
            cameras[currentCamera].Rotate(rotate);
        }
    }
}
