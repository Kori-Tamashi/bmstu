using code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace code
{
    class Camera
    {
        Point3D position;
        Vector3D direction;
        Vector3D right;
        Vector3D up;

        const int moveStep = 15;

        public Camera(Vector3D direction, Point3D position)
        {
            this.direction = direction;
            this.position = position;

            InitializeVectors();
        }

        private void InitializeVectors()
        {
            Vector3D top = new Vector3D(0, 1, 0);

            right = Vector3D.CrossProduct(direction, top);
            up = Vector3D.CrossProduct(direction, right);

            direction.Normalize();
            right.Normalize();
            up.Normalize();
        }

        public int MoveStep
        {
            get { return moveStep; }
        }

        public Point3D Position
        {
            get { return position; }
        }

        public Vector3D Direction
        {
            get { return direction; }
        }

        public Vector3D Right
        {
            get { return right; }
        }

        public Vector3D Up
        {
            get { return up; }
        }

        public void Move(Move move)
        {
            code.Move.Transform(move, position);
        }

        public void Rotate(Rotate rotate)
        {
            code.Rotate.Transform(rotate, position);
        }
    }
}
