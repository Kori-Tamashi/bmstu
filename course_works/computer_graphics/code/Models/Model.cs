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
        protected List<Point3D> points;
        protected List<int> indexes;
        protected List<Edge> edges;

        protected float length;
        protected float width;
        protected float height;
        protected float radius;
        protected float angle;

        protected Color color;
        protected Material material;

        protected Point3D center;
        protected Modeltype type = Modeltype.Model;
        protected String name = "name";

        #region Constructors

        public Model()
        {
            center = new Point3D();
            points = new List<Point3D>();
            indexes = new List<int>();
            edges = new List<Edge>();

            length = 0;
            width = 0;
            height = 0;
            radius = 0;
            angle = 0;

            color = Color.Empty;
            type = Modeltype.Model;
            material = new Material();
        }

        public Model(Model other)
        {
            type = other.type;
            name = other.name;
            center = other.center;
            length = other.length;
            width = other.width;
            height = other.height;
            radius = other.radius;
            angle = other.angle;
            color = other.color;
            material = other.material;

            CopyPoints(other);
            CopyIndexes(other);
            ConstructEdges(points, indexes);
        }

        public Model(List<Point3D> points, List<int> indexes)
        {
            color = Color.Empty;
            type = Modeltype.Model;
            material = new Material();

            points.AddRange(points);
            indexes.AddRange(indexes);
            ConstructCenter(points);
            ConstructEdges(points, indexes);
            Update();
        }

        public Model(List<Edge> edges)
        {
            color = Color.Empty;
            type = Modeltype.Model;
            material = new Material();

            edges.AddRange(edges);
            ConstructPoints(edges);
            ConstructIndexes(edges);
            ConstructCenter(points);
            Update();
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
            points = new List<Point3D>();
            foreach (Edge edge in edges)
            {
                points.Add(edge.start);
                points.Add(edge.end);
            }
        }

        protected void ConstructIndexes(List<Edge> edges)
        {
            int count = 0;
            indexes = new List<int>();
            foreach (Edge edge in edges)
            {
                indexes.Add(count++);
                indexes.Add(count++);
            }
        }

        protected void ConstructEdges(List<Point3D> points, List<int> indexes)
        {
            edges = new List<Edge>();
            for (int i = 0; i < indexes.Count; i += 2)
            {
                Edge edge = new Edge(points[indexes[i]], points[indexes[i + 1]]);
                edges.Add(edge);
            }
        }

        private void CopyPoints(Model other)
        {
            points = new List<Point3D>();
            foreach (Point3D point in other.points)
            {
                points.Add(new Point3D(point.X, point.Y, point.Z));
            }
        }

        private void CopyIndexes(Model other)
        {
            indexes = new List<int>(other.indexes);
        }

        protected virtual void Update()
        {

        }

        protected void UpdateCenter()
        {
            ConstructCenter(this.points);
        }

        #endregion

        #region Getters & Setters

        public virtual float Length
        {
            get { return length; }
            set { length = value; }
        }

        public virtual float Width
        {
            get { return width; }
            set { width = value; }
        }

        public virtual float Height
        {
            get { return height; }
            set { height = value; }
        }

        public virtual float Radius
        {
            get { return radius; }
            set { radius = value; }
        }

        public virtual float Angle
        {
            get { return angle; }
            set { angle = value; }
        }

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        public Material Material
        {
            get { return material; }
            set { material = value; }
        }

        public Point3D Center
        {
            get { return center; }
            set { center = value; }
        }

        public Modeltype Type
        {
            get { return type; }
            set { type = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public MaterialType MaterialType
        {
            get { return material.Type; }
            set { material.Type = value; }
        }

        #endregion

        #region Transformation

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

        public void Centering(Centering centering)
        {
            Move(centering.move);
            Scale(centering.scale);
        }

        private void Transform(Transformation transformation, Point3D center)
        {
            UpdateCenter();
            MovePointsToOrigin(center);
            TransformPoints(transformation);
            MovePointsToCenter(center);
            Update();
        }

        private void Transform(TransformationMatrix matrix, Point3D center)
        {
            UpdateCenter();
            MovePointsToOrigin(center);
            TransformPoints(matrix);
            MovePointsToCenter(center);
            Update();
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
                    {diff.X,  diff.Y,  diff.Z,  1}
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
                    {diff.X,  diff.Y,  diff.Z,  1}
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

        #endregion 

        public void Draw(Graphics graphics)
        {
            Pen pen = new Pen(color != Color.Empty ? color : Color.Black);

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
    }
}
