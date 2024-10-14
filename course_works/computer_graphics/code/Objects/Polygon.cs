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

        public float X(float y, float z)
        {
            return -(b * y + c * z + d) / a;
        }

        public float Y(float x, float z)
        {
            return -(a * x + c * z + d) / b;
        }

        public float Z(float x, float y)
        {
            return -(a * x + b * y + d) / c;
        }

        public Vector3D Normal()
        {
            Vector3D vector1 = edges[0].ToVector();
            Vector3D vector2 = edges[1].ToVector();
            Vector3D normal = Vector3D.CrossProduct(vector1, vector2);
            normal.Normalize();

            return normal;
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
    }
}
