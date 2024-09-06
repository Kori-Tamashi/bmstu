using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace code
{
    class Icosahedron : Model
    {
        public Icosahedron()
        {
            // Золотое сечение
            float phi = (float)(1 + Math.Sqrt(5)) / 2;

            points = new List<Point3D> {
                new Point3D(phi * 100, 100, 0), // 0
                new Point3D(phi * 100, -100, 0), // 1
                new Point3D(-phi * 100, -100, 0), // 2
                new Point3D(-phi * 100, 100, 0), // 3
                new Point3D(0, phi * 100, 100), // 4
                new Point3D(0, -phi * 100, 100), // 5
                new Point3D(0, -phi * 100, -100), // 6
                new Point3D(0, phi * 100, -100), // 7
                new Point3D(100, 0, phi * 100), // 8
                new Point3D(100, 0, -phi * 100), // 9
                new Point3D(-100, 0, -phi * 100), // 10
                new Point3D(-100, 0, phi * 100) // 11
            };

            indexes = new List<int> {
                1, 9,
                9, 10,
                10, 2,
                2, 5,
                5, 1,

                6, 1,
                6, 9,
                6, 10,
                6, 2,
                6, 5,

                8, 0,
                0, 7,
                7, 3,
                3, 11,
                11, 8,

                4, 8,
                4, 0,
                4, 7,
                4, 3,
                4, 11,

                1, 8,
                8, 5,
                5, 11,
                11, 2,
                2, 3,
                3, 10,
                10, 7,
                7, 9,
                9, 0,
                0, 1
            };

            name = "Икосаэдр";
            type = Modeltype.Icosahedron;
            color = Color.Empty;
            length = -1;
            height = -1;
            width = -1;
            radius = -1;
            angle = -1;

            ConstructCenter(points);
            ConstructEdges(points, indexes);
            ConstructPolygons(points);
            Update();
        }

        public Icosahedron(Model other)
        {
            type = other.Type;
            name = other.Name;
            center = other.Center;
            length = other.Length;
            width = other.Width;
            height = other.Height;
            radius = other.Radius;
            angle = other.Angle;
            color = other.Color;
            material = other.Material;

            CopyPoints(other);
            CopyIndexes(other);
            ConstructEdges(points, indexes);
        }

        protected override void ConstructPolygons(List<Point3D> points)
        {
            polygons = new List<Polygon> {
                new Polygon(points[1], points[6], points[9]),
                new Polygon(points[9], points[6], points[10]),
                new Polygon(points[10], points[6], points[2]),
                new Polygon(points[2], points[6], points[5]),
                new Polygon(points[5], points[6], points[1]),

                new Polygon(points[8], points[4], points[0]),
                new Polygon(points[0], points[4], points[7]),
                new Polygon(points[7], points[4], points[3]),
                new Polygon(points[3], points[4], points[11]),
                new Polygon(points[11], points[4], points[8]),

                new Polygon(points[1], points[8], points[5]),
                new Polygon(points[8], points[5], points[11]),
                new Polygon(points[5], points[11], points[2]),
                new Polygon(points[11], points[2], points[3]),
                new Polygon(points[2], points[3], points[10]),
                new Polygon(points[3], points[10], points[7]),
                new Polygon(points[10], points[7], points[9]),
                new Polygon(points[7], points[9], points[0]),
                new Polygon(points[9], points[0], points[1]),
                new Polygon(points[0], points[1], points[8]),
            };
        }

        protected override void Update()
        {
            UpdateCenter();
            UpdateRadius();
            UpdateLength();
            UpdatePolygons();
        }

        private void UpdateRadius()
        {
            radius = (float)Math.Sqrt(
                Math.Pow(points[4].X - points[6].X, 2) +
                Math.Pow(points[4].Y - points[6].Y, 2) +
                Math.Pow(points[4].Z - points[6].Z, 2)
                ) / 2;
        }

        private void UpdateLength()
        {
            length = (float)Math.Sqrt(
                Math.Pow(points[1].X - points[9].X, 2) +
                Math.Pow(points[1].Y - points[9].Y, 2) +
                Math.Pow(points[1].Z - points[9].Z, 2)
                );
        }

        private void UpdatePolygons()
        {
            ConstructPolygons(points);
        }

        public override float Length
        {
            get { return length; }
            set { SetLength(value); Update(); length = value; }
        }

        public override float Radius
        {
            get { return radius; }
            set { SetRadius(value); Update(); radius = value; }
        }

        private void SetLength(float newLength)
        {
            float k = newLength / length;
            Scale scale = new Scale(k, k, k);

            foreach (Point3D point in points)
            {
                code.Scale.Transform(scale, point, center);
            }
        }

        private void SetRadius(float newRadius)
        {
            float k = newRadius / radius;
            Scale scale = new Scale(k, k, k);

            foreach (Point3D point in points)
            {
                code.Scale.Transform(scale, point, center);
            }
        }

        public override Model Copy()
        {
            return new Icosahedron(this);
        }
    }
}
