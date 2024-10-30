using Azure;
using code;
using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace code
{
    class Model
    {
        protected List<Polygon> polygons;
        protected List<Point3D> points;
        protected List<int> indexes;
        protected List<Edge> edges;

        protected float length;
        protected float width;
        protected float height;
        protected float radius;
        protected float angle;

        protected float upperBaseRadius;
        protected float lowerBaseRadius;
        protected int facesCount;

        protected Color color;
        protected Material material;

        protected Point3D center;
        protected Modeltype type = Modeltype.Model;
        protected String name = "name";

        #region Constructors

        public Model()
        {
            points = new List<Point3D> {
                new Point3D(0, 0, 0), // 0
                new Point3D(100, 0, 0), // 1
                new Point3D(100, 0, 100), // 2
                new Point3D(0, 0, 100), // 3
                new Point3D(0, 100, 0), // 0
                new Point3D(100, 100, 0), // 1
                new Point3D(100, 100, 100), // 2
                new Point3D(0, 100, 100), // 3
            };

            indexes = new List<int> {
                0, 1,
                1, 2,
                2, 3,
                3, 0,

                4, 5,
                5, 6,
                6, 7,
                7, 4,

                0, 4,
                1, 5,
                2, 6,
                3, 7
            };

            name = "Многогранник";
            type = Modeltype.Model;
            material = new Material();
            color = Color.Empty;

            length = -1;
            width = -1;
            height = -1;
            angle = -1;
            radius = -1;

            upperBaseRadius = -1;
            lowerBaseRadius = -1;
            facesCount = 4;

            ConstructCenter(points);
            ConstructEdges(points, indexes);
            ConstructPolygons(points);
            Update();
        }

        public Model(Model other)
        {
            type = other.type;
            name = other.name;
            material = other.material;
            color = other.color;

            length = other.length;
            width = other.width;
            height = other.height;
            radius = other.radius;
            angle = other.angle;
            center = other.center;

            upperBaseRadius = other.upperBaseRadius;
            lowerBaseRadius = other.lowerBaseRadius;
            facesCount = other.facesCount;

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

        protected virtual void ConstructPolygons(List<Point3D> points)
        {
            List<Point3D> lowerBasePoints = new List<Point3D>(facesCount);
            List<Point3D> upperBasePoints = new List<Point3D>(facesCount);

            for (int i = 0; i < facesCount; i++)
            {
                lowerBasePoints.Add(points[i]);
                upperBasePoints.Add(points[i + facesCount]);
            }

            polygons = new List<Polygon>(facesCount + 2);
            polygons.Add(new Polygon(lowerBasePoints));
            polygons.Add(new Polygon(upperBasePoints));

            for (int i = 0; i < facesCount; i++)
            {
                Polygon polygon = new Polygon(points[i], points[(i + 1) % facesCount], points[facesCount + (i + 1) % facesCount], points[facesCount + i]);
                polygons.Add(polygon);
            }
        }

        protected void CopyPoints(Model other)
        {
            points = new List<Point3D>();
            foreach (Point3D point in other.points)
            {
                points.Add(new Point3D(point.X, point.Y, point.Z));
            }
        }

        protected void CopyIndexes(Model other)
        {
            indexes = new List<int>(other.indexes);
        }

        public virtual Model Copy()
        {
            return new Model(this);
        }

        public void Update_()
        {
            Update();
        }

        protected virtual void Update()
        {
            UpdateCenter();
            UpdateHeight();
            UpdateLength();
            UpdateLowerBaseRadius();
            UpdateUpperBaseRadius();
            UpdatePolygons();
            UpdateEdges();
        }

        protected void UpdateCenter()
        {
            ConstructCenter(points);
        }

        private void UpdatePolygons()
        {
            ConstructPolygons(points);
        }

        private void UpdateEdges()
        {
            ConstructEdges(points, indexes);
        }

        protected virtual void UpdateLength()
        {
            length = (float)Math.Sqrt(
                Math.Pow(points[0].X - points[1].X, 2) +
                Math.Pow(points[0].Y - points[1].Y, 2) +
                Math.Pow(points[0].Z - points[1].Z, 2)
                );
        }

        protected virtual void UpdateHeight()
        {
            Point3D lowerBaseCenter = GetLowerBaseCenter();
            Point3D upperBaseCenter = GetUpperBaseCenter();

            height = (float)Math.Sqrt(
                Math.Pow(upperBaseCenter.X - lowerBaseCenter.X, 2) +
                Math.Pow(upperBaseCenter.Y - lowerBaseCenter.Y, 2) +
                Math.Pow(upperBaseCenter.Z - lowerBaseCenter.Z, 2)
                );
        }

        protected virtual void UpdateLowerBaseRadius()
        {
            Point3D lowerBaseCenter = GetLowerBaseCenter();

            lowerBaseRadius = (float)Math.Sqrt(
                Math.Pow(lowerBaseCenter.X - points[0].X, 2) +
                Math.Pow(lowerBaseCenter.Y - points[0].Y, 2) +
                Math.Pow(lowerBaseCenter.Z - points[0].Z, 2)
                );
        }

        protected virtual void UpdateUpperBaseRadius()
        {
            Point3D upperBaseCenter = GetUpperBaseCenter();

            upperBaseRadius = (float)Math.Sqrt(
                Math.Pow(upperBaseCenter.X - points[facesCount].X, 2) +
                Math.Pow(upperBaseCenter.Y - points[facesCount].Y, 2) +
                Math.Pow(upperBaseCenter.Z - points[facesCount].Z, 2)
                );
        }

        #endregion

        #region Getters & Setters

        public List<Point3D> Points 
        {
            get { return points; } 
        }

        public List<Edge> Edges
        {
            get { return edges; }
        }

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
            set { SetHeight(value); Update(); }
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

        public List<Polygon> Polygons
        {
            get { return polygons; }
        }

        public virtual Matrix<float> Matrix
        {
            get { return _Matrix(); }
        }

        public virtual int FacesCount
        {
            get { return facesCount; }
            set { SetFacesCount(value); Update(); }
        }

        public virtual float UpperBaseRadius
        {
            get { return upperBaseRadius; }
            set { SetUpperBaseRadius(value); Update(); }
        }

        public virtual float LowerBaseRadius
        {
            get { return lowerBaseRadius; }
            set { SetLowerBaseRadius(value); Update(); }
        }

        private void SetFacesCount(int newFacesCount)
        {
            // получение центров нижнего и верхнего оснований
            Point3D lowerBaseCenter = GetLowerBaseCenter();
            Point3D upperBaseCenter = GetUpperBaseCenter();
            
            points.Clear();
            indexes.Clear();

            facesCount = newFacesCount;
            float angleStep = 2 * (float)Math.PI / facesCount;

            // получение нижнего основания
            for (int i = 0; i < facesCount; i++)
            {
                float angle = i * angleStep;
                float x = (float)(Math.Cos(angle) * lowerBaseRadius); 
                float z = (float)(Math.Sin(angle) * lowerBaseRadius);

                points.Add(new Point3D(x + lowerBaseCenter.X, lowerBaseCenter.Y, z + lowerBaseCenter.Z));
                indexes.Add(i);
                indexes.Add((i + 1) % facesCount);
            }

            // получение верхнего основания
            for (int i = 0; i < facesCount; i++)
            {
                float angle = i * angleStep;
                float x = (float)(Math.Cos(angle) * upperBaseRadius);
                float z = (float)(Math.Sin(angle) * upperBaseRadius);

                points.Add(new Point3D(x + upperBaseCenter.X, upperBaseCenter.Y, z + upperBaseCenter.Z));
                indexes.Add(facesCount + i);
                indexes.Add(facesCount + (i + 1) % facesCount);
            }

            // получение боковых граней
            for (int i = 0; i < facesCount; i++)
            {
                indexes.Add(i);
                indexes.Add(i + facesCount);
            }
        }

        protected void SetUpperBaseRadius(float newUpperBaseRadius)
        {
            Point3D upperBaseCenter = GetUpperBaseCenter();
            float angleStep = 2 * (float)Math.PI / facesCount;

            for (int i = 0; i < facesCount; i++)
            {
                float angle = i * angleStep;
                float x = (float)(Math.Cos(angle) * newUpperBaseRadius);
                float z = (float)(Math.Sin(angle) * newUpperBaseRadius);

                points.Add(new Point3D(x + upperBaseCenter.X, upperBaseCenter.Y, z + upperBaseCenter.Z));

                points[facesCount + i].X = x + upperBaseCenter.X;
                points[facesCount + i].Y = upperBaseCenter.Y;
                points[facesCount + i].Z = z + upperBaseCenter.Z;
            }

            upperBaseRadius = newUpperBaseRadius;
        }

        protected void SetLowerBaseRadius(float newLowerBaseRadius)
        {
            Point3D lowerBaseCenter = GetLowerBaseCenter();
            float angleStep = 2 * (float)Math.PI / facesCount;

            for (int i = 0; i < facesCount; i++)
            {
                float angle = i * angleStep;
                float x = (float)(Math.Cos(angle) * newLowerBaseRadius);
                float z = (float)(Math.Sin(angle) * newLowerBaseRadius);

                points.Add(new Point3D(x + lowerBaseCenter.X, lowerBaseCenter.Y, z + lowerBaseCenter.Z));

                points[i].X = x + lowerBaseCenter.X;
                points[i].Y = lowerBaseCenter.Y;
                points[i].Z = z + lowerBaseCenter.Z;
            }

            lowerBaseRadius = newLowerBaseRadius;
        }

        protected virtual void SetHeight(float newHeight)
        {
            float heightDifference = newHeight - height;

            for (int i = 0; i < facesCount; i++)
            {
                points[i + facesCount].Y += heightDifference; 
            }

            height = newHeight;
        }

        protected Point3D GetUpperBaseCenter()
        {
            Point3D upperBaseCenter = new Point3D(0, 0, 0);

            for (int i = 0; i < facesCount; i++)
            {
                upperBaseCenter.X += points[i + facesCount].X;
                upperBaseCenter.Y += points[i + facesCount].Y;
                upperBaseCenter.Z += points[i + facesCount].Z;
            }

            upperBaseCenter.X /= facesCount;
            upperBaseCenter.Y /= facesCount;
            upperBaseCenter.Z /= facesCount;

            return upperBaseCenter;
        }

        protected Point3D GetLowerBaseCenter()
        {
            Point3D lowerBaseCenter = new Point3D(0, 0, 0);

            for (int i = 0; i < facesCount; i++)
            {
                lowerBaseCenter.X += points[i].X;
                lowerBaseCenter.Y += points[i].Y;
                lowerBaseCenter.Z += points[i].Z;
            }

            lowerBaseCenter.X /= facesCount;
            lowerBaseCenter.Y /= facesCount;
            lowerBaseCenter.Z /= facesCount;

            return lowerBaseCenter;
        }

        #endregion

        #region Transformation

        public void Move(Move move)
        {
            Transform(move, center);
        }

        public void Rotate(Rotate rotate)
        {
            Transform(rotate, center);
        }

        public void Rotate(Rotate rotate, Point3D center)
        {
            Transform(rotate, center);
        }

        public void Scale(Scale scale)
        {
            Transform(scale, center);
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

        public void Centering(Point3D center, Size size)
        {
            Centering centering = new Centering(this, center, size);
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
                Matrix<float> cur_location = new Matrix<float>(point);
                Matrix<float> new_location = cur_location * transformation.matrix;

                point.X = new_location[0, 0];
                point.Y = new_location[0, 1];
                point.Z = new_location[0, 2];
            }
        }

        private void TransformPoints(TransformationMatrix matrix)
        {
            foreach(Point3D point in points)
            {
                Matrix<float> cur_location = new Matrix<float>(point);
                Matrix<float> new_location = cur_location * matrix;

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

        protected virtual Matrix<float> _Matrix()
        {
            Matrix<float> modelMatrix = new Matrix<float>(4, polygons.Count);
            for (int i = 0; i < polygons.Count; i++)
            {
                modelMatrix[0, i] = polygons[i].A;
                modelMatrix[1, i] = polygons[i].B;
                modelMatrix[2, i] = polygons[i].C;
                modelMatrix[3, i] = polygons[i].D;
            }

            return modelMatrix;
        }
        
    }
}
