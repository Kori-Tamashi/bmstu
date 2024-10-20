using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms.VisualStyles;

namespace code
{
    public class Polygon
    {
        float a, b, c, d;
        List<Edge> edges;
        List<Point3D> points;

        List<Point3D> insidePoints;
        List<Polygon> triangles;

        volatile bool insidePointsInitialized = false;
        volatile bool trianglesInitialized = false;

        public Polygon(List<Point3D> points, bool asyncPart = true)
        {
            if (points.Count < 3)
                throw new ArgumentException("Точек для инициализации плоскости должно быть минимум 3.");

            this.points = points;

            InitializeCoefficients();
            InitializeEdges();
        }

        public Polygon(params Point3D[] points)
        {
            if (points.Length < 3)
                throw new ArgumentException("Точек для инициализации плоскости должно быть минимум 3.");

            this.points = new List<Point3D>(points);

            InitializeCoefficients();
            InitializeEdges();
        }

        public Polygon(Vector3D normal)
        {
            a = normal.X;
            b = normal.Y;
            c = normal.Z;
            d = 0;

            points = new List<Point3D>();
            edges = new List<Edge>();
        }

        public Polygon(Vector3D normal, List<Point3D> points)
        {
            this.points = points;

            a = normal.X;
            b = normal.Y;
            c = normal.Z;
            d = -(a * points[0].X + b * points[0].Y + c * points[0].Z);

            InitializeEdges();
        }

        #region Initialize

        private void InitializeCoefficients()
        {
            ConstructCoefficients_NormalMethod(points);
        }

        private void InitializeEdges()
        {
            ConstructEdges(points);
        }

        private void InitializeInsidePoints(float accuracy = 0.3f)
        {
            BoundingBox bb = GetBoundingBox();

            float increment = accuracy;

            float minX = bb.Min.X;
            float minY = bb.Min.Y;
            float minZ = bb.Min.Z;
            float maxX = bb.Max.X;
            float maxY = bb.Max.Y;
            float maxZ = bb.Max.Z;

            int numPointsX = (int)((maxX - minX) / increment) + 1;
            int numPointsY = (int)((maxY - minY) / increment) + 1;

            ConcurrentBag<Point3D> pointsInsideConcurrent = new ConcurrentBag<Point3D>();

            if (numPointsY > 1)
            {
                if (numPointsX > 1)
                {
                    ProcessPointsInXY(minX, minY, maxX, maxY, increment, pointsInsideConcurrent);
                }
                else
                {
                    ProcessPointsInYZ(minY, minZ, maxY, maxZ, increment, pointsInsideConcurrent);
                }
            }
            else
            {
                ProcessPointsInXZ(minX, minZ, maxX, maxZ, increment, pointsInsideConcurrent);
            }

            insidePoints = pointsInsideConcurrent.ToList();
            insidePointsInitialized = true;
        }

        private void InitializeTriangles()
        {
            triangles = GetTriangles();
            trianglesInitialized = true;
        }

        private void ConstructCoefficients_NewellMethod(List<Point3D> points)
        {
            a = b = c = 0;

            for (int i = 0, j = 1; i < points.Count; i++, j = (i == points.Count - 1) ? 0 : i + 1) 
            {
                a += (points[i].Y - points[j].Y) * (points[i].Z + points[j].Z);
                b += (points[i].Z - points[j].Z) * (points[i].X + points[j].X);
                c += (points[i].X - points[j].X) * (points[i].Y + points[j].Y);
            }

            d = -(a * points[0].X + b * points[0].Y + c * points[0].Z);
        }

        private void ConstructCoefficients_NormalMethod(List<Point3D> points)
        {
            Vector3D vector1 = new Vector3D(points[1], points[0]);
            Vector3D vector2 = new Vector3D(points[1], points[2]);
            Vector3D baseNormal = Vector3D.CrossProduct(vector1, vector2);

            a = baseNormal.X;
            b = baseNormal.Y;
            c = baseNormal.Z;
            d = -(a * points[0].X + b * points[0].Y + c * points[0].Z);
        }

        private void ConstructEdges(List<Point3D> points)
        {
            edges = new List<Edge>();

            for (int i = 0; i < points.Count; i++)
            {
                if (i != points.Count - 1)
                    edges.Add(new Edge(points[i], points[i + 1]));
                else
                    edges.Add(new Edge(points[i], points[0]));
            }
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

        public float A
        {
            get { return a; }
        }

        public float B
        {
            get { return b; }
        }

        public float C
        { 
            get { return c; }
        }

        public float D
        { 
            get { return d; }
        }

        public List<Point3D> InsidePoints
        {
            get
            {
                if (!insidePointsInitialized)
                {
                    InitializeInsidePoints();
                }

                return insidePoints;
            }
        }

        public List<Polygon> Triangles
        {
            get
            {
                if (!trianglesInitialized)
                {
                    InitializeTriangles();
                }

                return triangles;
            }
        }

        public float Square
        {
            get { return GetSquare(); }
        }

        public (Point Min, Point Max) LimitingRectangle
        {
            get { return GetLimitingRectangle(); }
        }

        public BoundingBox BoundingBox
        {
            get { return GetBoundingBox(); }
        }

        #endregion

        #region Methods

        public float X(float y, float z)
        {
            return (a == 0) ? 0 : -(b * y + c * z + d) / a;
        }

        public float Y(float x, float z)
        {
            return (b == 0) ? 0 : -(a * x + c * z + d) / b;
        }

        public float Z(float x, float y)
        {
            return (c == 0) ? 0 : -(a * x + b * y + d) / c;
        }

        public void Turn()
        {
            Vector3D vec = new Vector3D(a, b, c);
            vec.Turn();

            a = vec.X;
            b = vec.Y;
            c = vec.Z;
        }

        public Vector3D Normal()
        {
            Vector3D vector1 = edges[0].ToVector();
            Vector3D vector2 = edges[1].ToVector();
            Vector3D normal = Vector3D.CrossProduct(vector1, vector2);
            normal.Normalize();

            return normal;
        }

        public Polygon NormalizedCopy()
        {
            return new Polygon(Normal(), points);
        }

        public bool IsInside(float x, float y, float z)
        {
            Vector3D polygonNormal = Normal();

            foreach (Edge edge in edges)
            {
                Vector3D vectorEdge = edge.ToVector();
                Vector3D vectorEdgeNormal = Vector3D.CrossProduct(vectorEdge, polygonNormal);
                vectorEdgeNormal.Turn();

                Vector3D vectorToPoint = new Vector3D(edge.start, new Point3D(x, y, z));

                if (Vector3D.DotProduct(vectorEdgeNormal, vectorToPoint) < 0)
                    return false;
            }

            return true;
        }

        public bool IsInsideParallel(float x, float y, float z)
        {
            Vector3D polygonNormal = Normal();
            bool inside = true;

            Parallel.ForEach(edges, edge =>
            {
                Vector3D vectorEdge = edge.ToVector();
                Vector3D vectorEdgeNormal = Vector3D.CrossProduct(vectorEdge, polygonNormal);
                vectorEdgeNormal.Turn();

                Vector3D vectorToPoint = new Vector3D(edge.start, new Point3D(x, y, z));

                if (Vector3D.DotProduct(vectorEdgeNormal, vectorToPoint) < 0)
                {
                    inside = false;
                }
            });

            return inside;
        }

        private float GetSquare()
        {
            float area = 0.0f;

            Point3D basePoint = points[0];

            for (int i = 1; i < points.Count - 1; i++)
            {
                Point3D pointA = points[i];
                Point3D pointB = points[i + 1];

                Vector3D vector1 = new Vector3D(basePoint, pointA);
                Vector3D vector2 = new Vector3D(basePoint, pointB);

                float triangleArea = Vector3D.CrossProduct(vector1, vector2).Length / 2.0f;

                area += triangleArea;
            }

            return area;
        }

        private void ProcessPointsInXY(float minX, float minY, float maxX, float maxY, float increment, ConcurrentBag<Point3D> pointsInside)
        {
            Parallel.For(0, (int)((maxY - minY) / increment) + 1, j =>
            {
                float y = minY + j * increment;

                for (int i = 0; i <= (maxX - minX) / increment; i++)
                {
                    float x = minX + i * increment;
                    float zCoord = Z(x, y);
                    AddPointIfInside(x, y, zCoord, pointsInside);
                }
            });

        }

        private void ProcessPointsInYZ(float minY, float minZ, float maxY, float maxZ, float increment, ConcurrentBag<Point3D> pointsInside)
        {
            Parallel.For(0, (int)((maxZ - minZ) / increment) + 1, k =>
            {
                float z = minZ + k * increment;

                for (int j = 0; j <= (maxY - minY) / increment; j++)
                {
                    float y = minY + j * increment;
                    float xCoord = X(y, z);
                    AddPointIfInside(xCoord, y, z, pointsInside);
                }
            });
        }

        private void ProcessPointsInXZ(float minX, float minZ, float maxX, float maxZ, float increment, ConcurrentBag<Point3D> pointsInside)
        {
            Parallel.For(0, (int)((maxX - minX) / increment) + 1, i =>
            {
                float x = minX + i * increment;

                for (int k = 0; k <= (maxZ - minZ) / increment; k++)
                {
                    float z = minZ + k * increment;
                    float yCoord = Y(x, z);
                    AddPointIfInside(x, yCoord, z, pointsInside);
                }
            });
        }

        private void AddPointIfInside(float x, float y, float z, ConcurrentBag<Point3D> pointsInside)
        {
            if (IsInside(x, y, z))
            {
                pointsInside.Add(new Point3D(x, y, z));
            }
        }

        private List<Polygon> GetTriangles()
        {
            List<Polygon> triangles = new List<Polygon>();

            if (points.Count == 3)
            {
                triangles.Add(this);
            }
            else
            {
                List<Point3D> ftr = new List<Point3D> { points[0], points[1], points[2] };
                List<Point3D> str = new List<Point3D> { points[0], points[2], points[3] };

                triangles.Add(new Polygon(ftr, false));
                triangles.Add(new Polygon(str, false));
            }

            return triangles;
        }

        private (Point Min, Point Max) GetLimitingRectangle()
        {
            BoundingBox bb = GetBoundingBox();
            return (new Point((int)bb.Min.X, (int)bb.Min.Y), 
                    new Point((int)bb.Max.X, (int)bb.Max.Y));
        }

        private BoundingBox GetBoundingBox()
        {
            float minX = float.MaxValue;
            float minY = float.MaxValue;
            float minZ = float.MaxValue;
            float maxX = float.MinValue;
            float maxY = float.MinValue;
            float maxZ = float.MinValue;

            foreach (Point3D point in Points)
            {
                minX = Math.Min(minX, point.X);
                minY = Math.Min(minY, point.Y);
                minZ = Math.Min(minZ, point.Z);
                maxX = Math.Max(maxX, point.X);
                maxY = Math.Max(maxY, point.Y);
                maxZ = Math.Max(maxZ, point.Z);
            }

            return new BoundingBox(new Point3D(minX, minY, minZ), new Point3D(maxX, maxY, maxZ));
        }

        public Point3D Barycentric(int x, int y)
        {
            return Barycentric(new Point(x, y));
        }

        public Point3D Barycentric(Point point)
        {
            if (Points.Count != 3)
            {
                foreach (Polygon p in Triangles)
                {
                    (float alpha, float beta, float gamma) = BarycentricCoeffs(p, point);

                    if (BarycentricIsInside(alpha, beta, gamma))
                        return new Point3D(point.X, point.Y, BarycentricZ(p, alpha, beta, gamma));
                }
            }

            return new Point3D(point.X, point.Y, BarycentricZ(this, point));
        }

        private (float alpha, float beta, float gamma) BarycentricCoeffs(Polygon triangle, Point point)
        {
            Point3D A = triangle.Points[0];
            Point3D B = triangle.Points[1];
            Point3D C = triangle.Points[2];

            float square_ = triangle.Square;
            float square = (A.Y - C.Y - point.Y) * (B.X - C.X) + (B.Y - C.Y) * (C.X - A.X);
            float alpha = ((point.Y - C.Y) * (B.X - C.X) + (B.Y - C.Y) * (C.X - point.X)) / square;
            float beta = ((point.Y - A.Y) * (C.X - A.X) + (C.Y - A.Y) * (A.X - point.X)) / square;
            float gamma = 1 - alpha - beta;

            return (alpha, beta, gamma);
        }

        private float BarycentricZ(Polygon triangle, float alpha, float beta, float gamma)
        {
            Point3D A = triangle.Points[0];
            Point3D B = triangle.Points[1];
            Point3D C = triangle.Points[2];

            A.Z = (A.Z == 0) ? 1e-6f : A.Z;
            B.Z = (B.Z == 0) ? 1e-6f : B.Z;
            C.Z = (C.Z == 0) ? 1e-6f : C.Z;

            float z_ = ((alpha / A.Z) + (beta / B.Z) + (gamma / C.Z));

            return (z_ == 0) ? 1e12f : 1 / z_;
        }

        private float BarycentricZ(Polygon triangle, Point point)
        {
            Point3D A = triangle.Points[0];
            Point3D B = triangle.Points[1];
            Point3D C = triangle.Points[2];
            (float alpha, float beta, float gamma) = BarycentricCoeffs(triangle, point);

            float z_ = ((alpha / A.Z) + (beta / B.Z) + (gamma / C.Z));

            return (z_ == 0) ? 1e6f : 1 / z_;
        }

        public bool BarycentricIsInside(int x, int y)
        {
            return BarycentricIsInside(new Point(x, y));
        }

        public bool BarycentricIsInside(Point point)
        {
            if (Points.Count != 3)
            {
                foreach (Polygon p in Triangles)
                {
                    if (BarycentricIsInside(p, point))
                        return true;
                }

                return false;
            }

            return BarycentricIsInside(this, point);
        }

        private bool BarycentricIsInside(Polygon triangle, Point point)
        {
            (float a, float b, float c) = BarycentricCoeffs(triangle, point);
            return a >= -1e-6f && b >= -1e-6f && c >= -1e-6f;
        }

        private bool BarycentricIsInside(float alpha, float beta, float gamma)
        {
            return alpha >= -1e-6f && beta >= -1e-6f && gamma >= -1e-6f;
        }

        #endregion
    }
}
