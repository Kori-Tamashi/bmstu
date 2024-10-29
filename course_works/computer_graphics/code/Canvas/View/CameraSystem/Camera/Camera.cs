using code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using Windows.Devices.Geolocation;

namespace code
{
    public class Camera
    {
        float yaw = 0;
        float pitch = 0;

        Point3D position;
        Vector3D direction;
        Vector3D right;
        Vector3D up;

        public Camera(Vector3D direction, Point3D position)
        {
            this.direction = direction;
            this.position = position;

            InitializeVectors();
            InitializeYawPitch(direction);
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

        public void InitializeYawPitch(Vector3D direction)
        {
            yaw = (float)(Math.Atan2(direction.Z, direction.X) * (180.0 / Math.PI));

            if (direction.Length > 0)
            {
                pitch = (float)(Math.Asin(direction.Y / direction.Length) * (180.0 / Math.PI));
            }
            else
            {
                pitch = 0; 
            }

            yaw = NormalizeYaw(yaw);
            pitch = NormalizePitch(pitch);
        }

        #endregion

        #region Getters & Setters

        public Point3D Position
        {
            get { return position; }
            set { position = value; }
        }

        public Vector3D Direction
        {
            get { return direction; }
            set { SetDirection(value); }
        }

        private void SetDirection(Vector3D direction)
        {
            this.direction = direction;
            InitializeYawPitch(direction);
        }

        public Vector3D Right
        {
            get { return right; }
        }

        public Vector3D Up
        {
            get { return up; }
        }

        public float Yaw
        {
            get { return yaw; }
            set { SetYaw(value); }
        }

        public float Pitch
        {
            get { return pitch; }
            set { SetPitch(value); }
        }

        #endregion

        #region Movement

        public void Move(Move move)
        {
            code.Move.Transform(move, position);
        }

        public void MoveForward(float distance)
        {
            // Вычисляем новое положение, добавляя направление к текущей позиции
            Point3D newPosition = new Point3D(
                position.X + distance * direction.X,
                position.Y + distance * direction.Y,
                position.Z + distance * direction.Z
            );

            // Перемещаем камеру в новое положение
            Move(new Move(
                newPosition.X - position.X,
                newPosition.Y - position.Y,
                newPosition.Z - position.Z
            ));
        }

        public void MoveBack(float distance)
        {
            MoveForward(-distance);
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

        public void RotateRight(float angle)
        {
            float radians = angle * (float)(Math.PI / 180.0);

            float cos = (float)Math.Cos(radians);
            float sin = (float)Math.Sin(radians);

            Vector3D newDirection = new Vector3D(
                direction.X* cos + up.X * sin,
                direction.Y* cos + up.Y * sin,
                direction.Z* cos + up.Z * sin
            );

            right = Vector3D.CrossProduct(newDirection, new Vector3D(0, 1, 0));
            up = Vector3D.CrossProduct(newDirection, right);

            newDirection.Normalize();
            right.Normalize();
            up.Normalize();

            direction = newDirection;
        }

        public void RotateLeft(float angle)
        {
            RotateRight(-angle);
        }

        public void RotateDown(float angle)
        {
            float radians = angle * (float)(Math.PI / 180.0);

            float cos = (float)Math.Cos(radians);
            float sin = (float)Math.Sin(radians);

            Vector3D newDirection = new Vector3D(
                direction.X * cos + up.X * sin,
                direction.Y * cos + up.Y * sin,
                direction.Z * cos + up.Z * sin
            );

            up = Vector3D.CrossProduct(newDirection, right);
            right = Vector3D.CrossProduct(up, newDirection);

            newDirection.Normalize();
            right.Normalize();
            up.Normalize();

            direction = newDirection;
        }

        public void RotateUp(float angle)
        {
            RotateDown(-angle);
        }

        public void SetYaw(float newYaw)
        {
            yaw = NormalizeYaw(newYaw); 
            UpdateDirection();
        }

        public void SetPitch(float newPitch)
        {
            pitch = NormalizePitch(newPitch); 
            UpdateDirection();
        }

        private void UpdateDirection()
        {
            // Обновляем направление камеры на основе текущих yaw и pitch
            float radiansYaw = yaw * (float)(Math.PI / 180.0);
            float radiansPitch = pitch * (float)(Math.PI / 180.0);

            // Вычисляем новое направление
            Vector3D newDirection = new Vector3D(
                (float)( Math.Cos(radiansYaw) * Math.Cos(radiansPitch) ),
                (float)  Math.Sin(radiansPitch),
                (float)( Math.Sin(radiansYaw) * Math.Cos(radiansPitch) )
            );

            direction = newDirection;
            right = Vector3D.CrossProduct(direction, new Vector3D(0, 1, 0));
            up = Vector3D.CrossProduct(right, direction);

            direction.Normalize();
            right.Normalize();
            up.Normalize();
        }

        private float NormalizeYaw(float angle)
        {
            if (angle > 180) return angle - 360;
            if (angle < -180) return angle + 360;
            return angle;
        }

        private float NormalizePitch(float angle)
        {
            if (angle > 90) return 90;
            if (angle < -90) return -90;
            return angle;
        }


        #endregion
    }
}
