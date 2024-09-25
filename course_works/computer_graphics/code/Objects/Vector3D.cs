using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace code
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

        public float Length
        {
            get { return GetLength(); }
        }

        public static float DotProduct(Vector3D v1, Vector3D v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
        }

        public static float DotProduct(Light light, Vector3D v)
        {
            return DotProduct(light.Direction, v);
        }

        public static float Angle(Vector3D v1, Vector3D v2)
        {
            float angleRadians = (float)Math.Acos( DotProduct(v1, v2) / (v1.Length * v2.Length) );
            return (float)( angleRadians * 180 / Math.PI );
        }

        public static float Angle(Light light, Vector3D v)
        {
            return Angle(light.Direction, v);
        }

        public float GetLength()
        {
            return (float)Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        public void Abs()
        {
            X = Math.Abs(X);
            Y = Math.Abs(Y);
            Z = Math.Abs(Z);
        }
        
        public void Turn()
        {
            X *= -1;
            Y *= -1;
            Z *= -1;
        }

        public bool IsNull()
        {
            return Math.Abs(X) < 1e-5 && Math.Abs(Y) < 1e-5 && Math.Abs(Z) < 1e-5;
        }

        public void Normalize()
        {
            float length = Length;
            X /= length;
            Y /= length;
            Z /= length;
        }

        public void PlaneRotate(bool clockwise = true)
        {
            Functions<float>.Swap(ref X, ref Y);
            Y *= clockwise ? -1 : 1;
            X *= clockwise ? 1 : -1;
        }

        public Vector3D NormalizedCopy()
        {
            float length = Length;
            return new Vector3D(X / length, Y / length, Z / length);
        }

        public Vector3D XYProjection()
        {
            return new Vector3D(X, Y, 0);
        }

        public Vector3D YZProjection()
        {
            return new Vector3D(0, Y, Z);
        }

        public Vector3D XZProjection()
        {
            return new Vector3D(X, 0, Z);
        }

        public float XYAngle()
        {
            return Angle(this, XYProjection());
        }

        public float YZAngle()
        {
            return Angle(this, YZProjection());
        }

        public float XZAngle()
        {
            return Angle(this, XZProjection());
        }

        public float OXAngle()
        {
            return Angle(this, new Vector3D(1, 0, 0));
        }

        public float OYAngle()
        {
            return Angle(this, new Vector3D(0, 1, 0));
        }

        public float OZAngle()
        {
            return Angle(this, new Vector3D(0, 0, 1));
        }

        public static Vector3D operator* (Vector3D v1, float scalar)
        {
            return new Vector3D(v1.X * scalar, v1.Y * scalar, v1.Z * scalar);
        }

        public static Vector3D operator- (Vector3D v1, Vector3D v2)
        {
            return new Vector3D(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        public static Vector3D operator+ (Vector3D v1, Vector3D v2)
        {
            return new Vector3D(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
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
