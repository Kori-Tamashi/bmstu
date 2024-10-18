using code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace code
{
    public class Camera
    {
        Point3D position;
        Vector3D direction;
        Vector3D right;
        Vector3D up;

        public Camera(Vector3D direction, Point3D position)
        {
            this.direction = direction;
            this.position = position;

            InitializeVectors();
        }

        #region Initialize

        private void InitializeVectors()
        {
            Vector3D top = new Vector3D(0, 1, 0);

            right = Vector3D.CrossProduct(direction, top);
            up = Vector3D.CrossProduct(direction, right);

            direction.Normalize();
            right.Normalize();
            up.Normalize();
        }

        #endregion

        #region Getters & Setters

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

        #endregion

        #region Movement

        public void Move(Move move)
        {
            code.Move.Transform(move, position);
        }

        public void MoveRight(float d)
        {
            Point3D newPosition = new Point3D(
                position.X + d * right.X, 
                position.Y + d * right.Y, 
                position.Z + d * right.Z
            );

            Move(new Move(
                newPosition.X - position.X, 
                newPosition.Y - position.Y, 
                newPosition.Z - position.Z
                )
            );
        }

        public void MoveLeft(float d)
        {
            MoveRight(-d);
        }

        public void MoveUp(float d)
        {
            Point3D newPosition = new Point3D(
                position.X + d * up.X, 
                position.Y + d * up.Y, 
                position.Z + d * up.Z
            );

            Move(new Move(
                newPosition.X - position.X, 
                newPosition.Y - position.Y, 
                newPosition.Z - position.Z
                )
            );
        }

        public void MoveDown(float d)
        {
            MoveUp(-d);
        }

        public void MoveUpRight(float d)
        {
            MoveUp(d);
            MoveRight(d);
        }

        public void MoveUpLeft(float d)
        {
            MoveUp(d);
            MoveLeft(d);
        }

        public void MoveDownRight(float d)
        {
            MoveDown(d);
            MoveRight(d);
        }

        public void MoveDownLeft(float d)
        {
            MoveDown(d);
            MoveLeft(d);
        }

        public void Rotate(Rotate rotate)
        {
            code.Rotate.Transform(rotate, position);
        }

        #endregion
    }
}
