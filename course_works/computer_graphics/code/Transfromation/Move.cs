using code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace code
{
    class Move : Transformation
    {

        public Move(float dx, float dy, float dz)
        {
            matrix = new TransformationMatrix(new float[4, 4] {
                    {1,       0,       0,       0},
                    {0,       1,       0,       0},
                    {0,       0,       1,       0},
                    {dx,      dy,      dz,      1}
                }
            );
        }

        public Move(Point3D point)
        {
            matrix = new TransformationMatrix(new float[4, 4] {
                    {1,       0,       0,       0},
                    {0,       1,       0,       0},
                    {0,       0,       1,       0},
                    {point.X, point.Y, point.Z, 1}
                }
            );
        }

        public Move(Vector3D vector)
        {
            matrix = new TransformationMatrix(new float[4, 4] {
                    {1,        0,        0,        0},
                    {0,        1,        0,        0},
                    {0,        0,        1,        0},
                    {vector.X, vector.Y, vector.Z, 1}
                }
            );
        }

        static public void Transform(Move move, Point3D point)
        {
            Transformation.Transform(move, point);
        }

        static public void Transform(Move move, Point3D point, Point3D center)
        {
            Transformation.Transform(move, point, center);
        }
    }
}
