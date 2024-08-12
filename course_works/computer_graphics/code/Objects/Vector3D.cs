using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace code.Objects
{
    public class Vector3D
    {
        public float X;
        public float Y;
        public float Z;

        public Vector3D(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3D(Point3D start, Point3D end)
        {
            X = end.X - start.X;
            Y = end.Y - start.Y;
            Z = end.Z - start.Z;
        }

        public static Vector3D CrossProduct(Vector3D v1, Vector3D v2)
        {
            return new Vector3D(
                v1.Y * v2.Z - v1.Z * v2.Y,
                v1.Z * v2.X - v1.X * v2.Z,
                v1.X * v2.Y - v1.Y * v2.X
            );
        }

        public static float DotProduct(Vector3D v1, Vector3D v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
        }

        public void Normalize()
        {
            float length = (float)Math.Sqrt(X * X + Y * Y + Z * Z);
            X /= length;
            Y /= length;
            Z /= length;
        }

        public static Vector3D operator* (Vector3D v1, float scalar)
        {
            return new Vector3D(v1.X * scalar, v1.Y * scalar, v1.Z * scalar);
        }

        public static Vector3D operator+ (Vector3D v1, Point3D point)
        {
            return new Vector3D(v1.X + point.X, v1.Y + point.Y, v1.Z + point.Z);
        }

        public static Point3D operator+ (Point3D point, Vector3D v1)
        {
            return new Point3D(v1.X + point.X, v1.Y + point.Y, v1.Z + point.Z);
        }
    }
}
