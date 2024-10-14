using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace code
{
    public class Vector4D
    {
        public float X;
        public float Y;
        public float Z;
        public float W;

        public Vector4D(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public Vector4D(Point3D point)
        {
            X = point.X;
            Y = point.Y;
            Z = point.Z;
            W = 1;
        }

        public Vector4D(Polygon p)
        {
            X = p.A;
            Y = p.B;
            Z = p.C;
            W = p.D;
        }

        public Vector4D(Point3D start, Point3D end)
        {
            X = end.X - start.X;
            Y = end.Y - start.Y;
            Z = end.Z - start.Z;
            W = 0;
        }

        public float Length
        {
            get { return GetLength(); }
        }

        public static float DotProduct(Vector4D v1, Vector4D v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z + v1.W * v2.W;
        }

        public float GetLength()
        {
            return (float)Math.Sqrt(X * X + Y * Y + Z * Z + W * W);
        }

        public void Normalize()
        {
            float length = Length;
            X /= length;
            Y /= length;
            Z /= length;
            W /= length;
        }

        public Vector4D NormalizedCopy()
        {
            float length = Length;
            return new Vector4D(X / length, Y / length, Z / length, W / length);
        }

        public bool IsNull()
        {
            return Math.Abs(X) < 1e-5 && Math.Abs(Y) < 1e-5 && Math.Abs(Z) < 1e-5 && Math.Abs(W) < 1e-5;
        }

        public static Vector4D operator -(Vector4D v1, Vector4D v2)
        {
            return new Vector4D(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z, v1.W - v2.W);
        }

        public static Vector4D operator +(Vector4D v1, Vector4D v2)
        {
            return new Vector4D(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z, v1.W + v2.W);
        }

        public static Vector4D operator *(Vector4D v1, float scalar)
        {
            return new Vector4D(v1.X * scalar, v1.Y * scalar, v1.Z * scalar, v1.W * scalar);
        }

        public static Vector4D operator *(float scalar, Vector4D v1)
        {
            return new Vector4D(v1.X * scalar, v1.Y * scalar, v1.Z * scalar, v1.W * scalar);
        }

        public float this[int i]
        {
            get
            {
                if (i < 0 || i > 3)
                    throw new ArgumentOutOfRangeException();

                switch (i)
                {
                    case 0:
                        return X;
                    case 1:
                        return Y;
                    case 2:
                        return Z;
                    case 3:
                        return W;
                    default:
                        return 0;
                }
            }
            set
            {
                if (i < 0 || i > 3)
                    throw new ArgumentOutOfRangeException();

                switch (i)
                {
                    case 0:
                        X = value;
                        break;
                    case 1:
                        Y = value;
                        break;
                    case 2:
                        Z = value;
                        break;
                    case 3:
                        W = value;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
