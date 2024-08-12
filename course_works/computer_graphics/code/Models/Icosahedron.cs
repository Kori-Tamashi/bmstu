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

            this.points = new List<Point3D> {
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

            this.indexes = new List<int> {
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
                9, 7,
                7, 10,
                10, 3,
                3, 2,
                2, 11,
                11, 5,

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
            
            this.type = Modeltype.Icosahedron;
            this.color = Color.Empty;
            this.length = -1;
            this.height = -1;
            this.width = -1;
            this.radius = -1;
            this.angle = -1;

            ConstructCenter(this.points);
            ConstructEdges(this.points, this.indexes);
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

        protected override void Update()
        {
            UpdateCenter();
            UpdateRadius();
            UpdateLength();
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

        public override float Length
        {
            get { return length; }
            set { SetLength(value); length = value; }
        }

        public override float Radius
        {
            get { return radius; }
            set { SetRadius(value); radius = value; }
        }

        private void SetLength(float newLength)
        {
            float k = newLength / length;
            Scale(new Scale(k, k, k));
        }

        private void SetRadius(float newRadius)
        {
            float k = newRadius / radius;
            Scale(new Scale(k, k, k));
        }

        public override Model Copy()
        {
            return new Icosahedron(this);
        }
    }
}
