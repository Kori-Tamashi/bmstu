using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace code
{
    public class Transformation
    {
        public TransformationMatrix matrix;

        public Transformation()
        {
            matrix = new TransformationMatrix();
        }

        public Transformation(TransformationMatrix matrix)
        {
            this.matrix = matrix;
        }

        public void ToIdentity()
        {
            for (int i = 0; i < matrix.Rows; i++)
            {
                matrix[i, i] = 1;
            }
        }

        static public void Transform(Transformation transformation, Point3D point)
        {
            TransformPoints(transformation, point);
        }

        static public void Transform(TransformationMatrix matrix, Point3D point)
        {
            TransformPoints(matrix, point);
        }

        static public void Transform(Transformation transformation, List<Point3D> points)
        {
            TransformPoints(transformation, points);
        }

        static public void Transform(TransformationMatrix matrix, List<Point3D> points)
        {
            TransformPoints(matrix, points);
        }

        static public void Transform(Transformation transformation, Point3D point, Point3D center)
        {
            MovePointsToOrigin(center, point);
            TransformPoints(transformation, point);
            MovePointsToCenter(center, point);
        }

        static public void Transform(TransformationMatrix matrix, Point3D point, Point3D center)
        {
            MovePointsToOrigin(center, point);
            TransformPoints(matrix, point);
            MovePointsToCenter(center, point);
        }


        static public void Transform(Transformation transformation, List<Point3D> points, Point3D center)
        {
            MovePointsToOrigin(center, points);
            TransformPoints(transformation, points);
            MovePointsToCenter(center, points);
        }

        static public void Transform(TransformationMatrix matrix, List<Point3D> points, Point3D center)
        {
            MovePointsToOrigin(center, points);
            TransformPoints(matrix, points);
            MovePointsToCenter(center, points);
        }

        static private void MovePointsToOrigin(Point3D center, Point3D point)
        {
            Point3D diff = new Point3D(
                0 - center.X,
                0 - center.Y,
                0 - center.Z
            );

            TransformationMatrix matrix = new TransformationMatrix(new float[4, 4] {
                    {1,       0,       0,       0},
                    {0,       1,       0,       0},
                    {0,       0,       1,       0},
                    {diff.X,  diff.Y,  diff.Z,  1}
                }
            );

            TransformPoints(matrix, point);
        }

        static private void MovePointsToOrigin(Point3D center, List<Point3D> points)
        {
            Point3D diff = new Point3D(
                0 - center.X,
                0 - center.Y,
                0 - center.Z
            );

            TransformationMatrix matrix = new TransformationMatrix(new float[4, 4] {
                    {1,       0,       0,       0},
                    {0,       1,       0,       0},
                    {0,       0,       1,       0},
                    {diff.X,  diff.Y,  diff.Z,  1}
                }
            );

            TransformPoints(matrix, points);
        }

        static private void MovePointsToCenter(Point3D center, Point3D point)
        {
            Point3D diff = new Point3D(
                center.X - 0,
                center.Y - 0,
                center.Z - 0
            );

            TransformationMatrix matrix = new TransformationMatrix(new float[4, 4] {
                    {1,       0,       0,       0},
                    {0,       1,       0,       0},
                    {0,       0,       1,       0},
                    {diff.X,  diff.Y,  diff.Z,  1}
                }
            );

            TransformPoints(matrix, point);
        }

        static private void MovePointsToCenter(Point3D center, List<Point3D> points)
        {
            Point3D diff = new Point3D(
                center.X - 0,
                center.Y - 0,
                center.Z - 0
            );

            TransformationMatrix matrix = new TransformationMatrix(new float[4, 4] {
                    {1,       0,       0,       0},
                    {0,       1,       0,       0},
                    {0,       0,       1,       0},
                    {diff.X,  diff.Y,  diff.Z,  1}
                }
            );

            TransformPoints(matrix, points);
        }

        static private void TransformPoints(Transformation transformation, Point3D point)
        {
            Matrix<float> cur_location = new Matrix<float>(point);
            Matrix<float> new_location = cur_location * transformation.matrix;

            point.X = new_location[0, 0];
            point.Y = new_location[0, 1];
            point.Z = new_location[0, 2];
        }

        static private void TransformPoints(TransformationMatrix matrix, Point3D point)
        {
            Matrix<float> cur_location = new Matrix<float>(point);
            Matrix<float> new_location = cur_location * matrix;

            point.X = new_location[0, 0];
            point.Y = new_location[0, 1];
            point.Z = new_location[0, 2];
        }


        static private void TransformPoints(Transformation transformation, List<Point3D> points)
        {
            foreach (Point3D point in points)
            {
                Matrix<float> cur_location = new Matrix<float>(point);
                Matrix<float> new_location = cur_location * transformation.matrix;

                point.X = new_location[0, 0];
                point.Y = new_location[0, 1];
                point.Z = new_location[0, 2];
            }
        }

        static private void TransformPoints(TransformationMatrix matrix, List<Point3D> points)
        {
            foreach (Point3D point in points)
            {
                Matrix<float> cur_location = new Matrix<float>(point);
                Matrix<float> new_location = cur_location * matrix;

                point.X = new_location[0, 0];
                point.Y = new_location[0, 1];
                point.Z = new_location[0, 2];
            }
        }

        public float this[int row, int col]
        {
            get
            {
                return matrix[row, col];
            }
            set
            {
                matrix[row, col] = value;
            }
        }

        public static Transformation operator *(Transformation a, Transformation b)
        {
            return new Transformation(a.matrix * b.matrix);
        }
    }
}
