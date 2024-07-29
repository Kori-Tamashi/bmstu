using code.Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace code
{
    class Model
    {
        protected Point3D center;
        protected List<Point3D> points;
        protected List<int> indexes;
        protected List<Edge> edges;

        public String type = "Model";
        public String name = "name";

        #region Constructors
        protected Model() 
        {
            center = new Point3D();
            points = new List<Point3D>();
            indexes = new List<int>();
            edges = new List<Edge>();
        }

        protected Model(List<Point3D> points, List<int> indexes)
        {
            points.AddRange(points);
            indexes.AddRange(indexes);
            ConstructCenter(points);
            ConstructEdges(points, indexes);
        }

        protected Model(List<Edge> edges)
        {
            edges.AddRange(edges);
            ConstructPoints(edges);
            ConstructIndexes(edges);
            ConstructCenter(points);
        }

        protected void ConstructCenter(List<Point3D> points)
        {
            center = new Point3D();

            foreach (Point3D point in points)
            {
                center.X += point.X;
                center.Y += point.Y;
                center.Z += point.Z;
            }

            center.X /= points.Count;
            center.Y /= points.Count;
            center.Z /= points.Count;
        }

        protected void ConstructPoints(List<Edge> edges)
        {
            foreach (Edge edge in edges)
            {
                points.Add(edge.start);
                points.Add(edge.end);
            }
        }

        protected void ConstructIndexes(List<Edge> edges)
        {
            int count = 0;
            foreach (Edge edge in edges)
            {
                indexes.Add(count++);
                indexes.Add(count++);
            }
        }

        protected void ConstructEdges(List<Point3D> points, List<int> indexes)
        {
            for (int i = 0; i < indexes.Count; i += 2)
            {
                Edge edge = new Edge(points[indexes[i]], points[indexes[i + 1]]);
                edges.Add(edge);
            }
        }
        #endregion

        public void Draw(Graphics graphics, Pen pen)
        {
            foreach (Edge edge in edges)
            {
                graphics.DrawLine(
                    pen,
                    (float)edge.start.X,
                    (float)edge.start.Y,
                    (float)edge.end.X,
                    (float)edge.end.Y
                );
            }
        }

        public void Move(Move move)
        {
            Transform(move, this.center);
        }

        public void Rotate(Rotate rotate)
        {
            Transform(rotate, this.center);
        }

        public void Rotate(Rotate rotate, Point3D center)
        {
            Transform(rotate, center);
        }

        public void Scale(Scale scale)
        {
            Transform(scale, this.center);
        }

        public void Scale(Scale scale, Point3D center)
        {
            Transform(scale, center);
        }

        protected void UpdateCenter()
        {
            ConstructCenter(this.points);
        }

        private void Transform(Transformation transformation, Point3D center)
        {
            UpdateCenter();
            MovePointsToOrigin(center);
            TransformPoints(transformation);
            MovePointsToCenter(center);
        }

        private void Transform(TransformationMatrix matrix, Point3D center)
        {
            UpdateCenter();
            MovePointsToOrigin(center);
            TransformPoints(matrix);
            MovePointsToCenter(center);
        }

        private void MovePointsToOrigin(Point3D center)
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
                    {diff.X,      diff.Y,      diff.Z,      1}
                }
            );

            TransformPoints(matrix);
            UpdateCenter();
        }

        private void MovePointsToCenter(Point3D center)
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
                    {diff.X,      diff.Y,      diff.Z,      1}
                }
            );

            TransformPoints(matrix);
            UpdateCenter();
        }

        private void TransformPoints(Transformation transformation)
        {
            foreach (Point3D point in points)
            {
                Matrix cur_location = new Matrix(point);
                Matrix new_location = cur_location * transformation.matrix;

                point.X = new_location[0, 0];
                point.Y = new_location[0, 1];
                point.Z = new_location[0, 2];
            }
        }

        private void TransformPoints(TransformationMatrix matrix)
        {
            foreach(Point3D point in points)
            {
                Matrix cur_location = new Matrix(point);
                Matrix new_location = cur_location * matrix;

                point.X = new_location[0, 0];
                point.Y = new_location[0, 1];
                point.Z = new_location[0, 2];
            }
        }
    }
}
