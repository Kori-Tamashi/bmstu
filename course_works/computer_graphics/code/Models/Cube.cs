using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;


namespace code
{
    class Cube : Model
    {
        public Cube() 
        {
            this.points = new List<Point3D> { 
                new Point3D(0, 0, 0), // 0
                new Point3D(100, 0, 0), // 1
                new Point3D(100, 100, 0), // 2
                new Point3D(0, 100, 0), // 3
                new Point3D(0, 0, 100), // 4
                new Point3D(100, 0, 100), // 5
                new Point3D(100, 100, 100), // 6
                new Point3D(0, 100, 100)  // 7
            };

            this.indexes = new List<int> { 
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

            this.type = Modeltype.Cube;
            this.color = Color.Empty;
            this.length = 100;
            this.width = -1;
            this.height = -1;
            this.angle = -1;
            this.radius = -1;

            ConstructCenter(this.points);
            ConstructEdges(this.points, this.indexes);

        }

        public Cube(Model other)
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
            UpdateLength();
        }

        private void UpdateLength()
        {
            length = (float) Math.Sqrt(
                Math.Pow(points[1].X - points[0].X, 2) + 
                Math.Pow(points[1].Y - points[0].Y, 2) + 
                Math.Pow(points[1].Z - points[0].Z, 2)
                );
        }

        public override float Length
        {
            get { return length; }
            set { SetLength(value); length = value; }
        }

        private void SetLength(float newLength)
        {
            float k = newLength / length;
            Scale(new Scale(k, k, k));
        }

        public override Model Copy()
        {
            return new Cube(this);
        }
    }
}
