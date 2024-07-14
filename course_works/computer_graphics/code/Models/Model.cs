using code.Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

using Pen = System.Drawing.Pen;

namespace code
{
    class Model
    {
        protected List<Point3D> points;
        protected List<int> indexes;
        protected List<Edge> edges;

        protected Model() 
        { 
            points = new List<Point3D>();
            indexes = new List<int>();
            edges = new List<Edge>();
        }

        protected Model(List<Point3D> points, List<int> indexes)
        {
            points.AddRange(points);
            indexes.AddRange(indexes);
            ConstructEdges(points, indexes);
        }

        protected Model(List<Edge> edges)
        {
            edges.AddRange(edges);
            ConstructPoints(edges);
            ConstructIndexes(edges);
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

        #region Draw
        public void Draw(ref Graphics graphics, Pen pen)
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
        #endregion

        #region Add-Remove
        protected void Add(Point3D point)
        {
            AddPoint(point);
        }

        protected void Add(List<Point3D> points)
        {
            AddPoints(points);
        }

        protected void Add(List<int> indexes)
        {
            AddIndexes(indexes);
        }

        protected void Add(Edge edge)
        {
            AddEdge(edge);
        }

        protected void Add(List<Edge> edges)
        {
            AddEdges(edges);
        }

        private void AddPoints(List<Point3D> points)
        {
            this.points.AddRange(points);
        }

        private void AddPoint(Point3D point)
        {
            points.Add(point);
        }

        private void AddPoints(List<Edge> edges)
        {
            foreach (Edge edge in edges)
            {
                points.Add(edge.start);
                points.Add(edge.end);
            }
        }

        private void AddIndexes(List<int> indexes)
        {
            this.indexes.AddRange(indexes);
            AddEdges(indexes);
        }

        private void AddIndexes(List<Edge> edges)
        {
            int count = indexes.Count;
            foreach (Edge edge in edges)
            {
                indexes.Add(count++);
                indexes.Add(count++);
            }
        }

        private void AddEdge(Edge edge)
        {
            edges.Add(edge);
        }

        private void AddEdges(List<int> indexes)
        {
            for (int i = 0; i < indexes.Count; i += 2)
            {
                Edge edge = new Edge(points[indexes[i]], points[indexes[i + 1]]);
                edges.Add(edge);
            }
        }

        private void AddEdges(List<Edge> edges)
        {
            this.edges.AddRange(edges);
            AddPoints(edges);
            AddIndexes(edges);
        }
        #endregion
    }
}
