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
                new Point3D(phi * 100, 100, 0),
                new Point3D(phi * 100, -100, 0),
                new Point3D(-phi * 100, -100, 0),
                new Point3D(-phi * 100, 100, 0),
                new Point3D(0, phi * 100, 100),
                new Point3D(0, -phi * 100, 100),
                new Point3D(0, -phi * 100, -100),
                new Point3D(0, phi * 100, -100),
                new Point3D(100, 0, phi * 100),
                new Point3D(100, 0, -phi * 100),
                new Point3D(-100, 0, -phi * 100),
                new Point3D(-100, 0, phi * 100)
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
            this.radius = radius;
            this.angle = -1;

            ConstructCenter(this.points);
            ConstructEdges(this.points, this.indexes);
        }

        protected override void Update()
        {
            UpdateCenter();
            UpdateRadius();
        }

        private void UpdateRadius()
        {
            radius = (float)Math.Sqrt(
                Math.Pow(points[0].X, 2) +
                Math.Pow(points[0].Y, 2) +
                Math.Pow(points[0].Z, 2)
                );
        }

        public override float Radius
        {
            get { return (float)radius; }
            set { SetRadius(value); radius = value; }
        }

        private void SetRadius(float newRadius)
        {
            float k = newRadius / (float)radius;
            Scale(new Scale(k, k, k));
        }
    }
}
