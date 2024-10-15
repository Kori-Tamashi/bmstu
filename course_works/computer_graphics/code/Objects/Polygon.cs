using System;
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
        List<Point3D> points;
        List<Edge> edges;

        public Polygon(List<Point3D> points)
        {
            if (points.Count < 3)
                throw new ArgumentException("Точек для инициализации плоскости должно быть минимум 3.");

            this.points = points;
            ConstructCoefficients_NormalMethod(points);
            ConstructEdges(points);
        }

        public Polygon(params Point3D[] points)
        {
            if (points.Length < 3)
                throw new ArgumentException("Точек для инициализации плоскости должно быть минимум 3.");

            this.points = new List<Point3D>(points);
            ConstructCoefficients_NormalMethod(this.points);
            ConstructEdges(this.points);
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

            ConstructEdges(points);
        }

        #region Initialize

        private void ConstructCoefficients_NewellMethod(List<Point3D> points)
        {
            // Theory - D. Rogers page 254

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
            get { return GetInsidePoints(); }
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
            Polygon p = new Polygon(Normal(), points);
            return p;
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

        public List<Point3D> GetInsidePoints()
        {
            List<Point3D> pointsInside = new List<Point3D>();
            BoundingBox bb = GetBoundingBox();

            int diffX = (int)( bb.Max.X - bb.Min.X );
            int diffY = (int)( bb.Max.Y - bb.Min.Y );
            int diffZ = (int)( bb.Max.Z - bb.Min.Z ) ;

            if (diffY != 0)
            {
                if (diffX != 0)
                {
                    for (int y = (int)bb.Min.Y; y <= bb.Max.Y; y++)
                    {
                        for (int x = (int)bb.Min.X; x <= bb.Max.X; x++)
                        {
                            float z = Z(x, y);
                            if (IsInside(x, y, z))
                                pointsInside.Add(new Point3D(x, y, z));
                        }
                    }
                }
                else
                {
                    for (int y = (int)bb.Min.Y; y <= bb.Max.Y; y++)
                    {
                        for (int z = (int)bb.Min.Z; z <= bb.Max.Z; z++)
                        {
                            float x = X(y, z);
                            if (IsInside(x, y, z))
                                pointsInside.Add(new Point3D(x, y, z));
                        }
                    }
                }
                
            }
            else
            {
                for (int x = (int)bb.Min.X; x <= bb.Max.X; x++)
                {
                    for (int z = (int)bb.Min.Z; z <= bb.Max.Z; z++)
                    {
                        float y = Y(x, z);
                        if (IsInside(x, y, z))
                            pointsInside.Add(new Point3D(x, y, z));
                    }
                }
            }

            return pointsInside;
        }

        public BoundingBox GetBoundingBox()
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

        #endregion
    }
}
