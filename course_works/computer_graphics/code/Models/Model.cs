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



namespace code.Models
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

    }
}
