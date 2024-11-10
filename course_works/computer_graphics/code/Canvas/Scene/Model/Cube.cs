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
            points = new List<Point3D> {
                new Point3D(0, 0, 0), // 0
                new Point3D(100, 0, 0), // 1
                new Point3D(100, 100, 0), // 2
                new Point3D(0, 100, 0), // 3
                new Point3D(0, 0, 100), // 4
                new Point3D(100, 0, 100), // 5
                new Point3D(100, 100, 100), // 6
                new Point3D(0, 100, 100)  // 7
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

            name = "Куб";
            type = Modeltype.Cube;
            color = Color.Empty;
            material = new Material();

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
            ConstructPolygons(points);
        }

        protected override void ConstructPolygons(List<Point3D> points)
        {
            polygons = new List<Polygon> {
                new Polygon(points[0], points[1], points[5], points[4]), // top
                new Polygon(points[3], points[2], points[6], points[7]), // bottom
                new Polygon(points[0], points[1], points[2], points[3]), // back
                new Polygon(points[1], points[5], points[6], points[2]), // right
                new Polygon(points[5], points[4], points[7], points[6]), // front
                new Polygon(points[4], points[0], points[3], points[7])  // left
            };
        }

        protected override void Update()
        {
            UpdateCenter();
            UpdateLength();
            UpdatePolygons();
            UpdateLowerBaseRadius();
            UpdateUpperBaseRadius();
        }

        protected override void UpdateLength()
        {
            length = (float) Math.Sqrt(
                Math.Pow(points[1].X - points[0].X, 2) + 
                Math.Pow(points[1].Y - points[0].Y, 2) + 
                Math.Pow(points[1].Z - points[0].Z, 2)
                );
        }

        private void UpdatePolygons()
        {
            ConstructPolygons(points);
        }

        public override float Length
        {
            get { return length; }
            set { SetLength(value); Update(); }
        }

        public override int FacesCount
        {
            get { return facesCount; }
            set {  }
        }

        public override float Height
        {
            get { return height; }
            set { }
        }

        public override float UpperBaseRadius
        {
            get { return upperBaseRadius; }
            set { }
        }

        public override float LowerBaseRadius
        {
            get { return lowerBaseRadius; }
            set { }
        }

        private void SetLength(float newLength)
        {
            float k = newLength / length;
            Scale scale = new Scale(k, k, k);

            foreach (Point3D point in points)
            {
                code.Scale.Transform(scale, point, center);
            }

            length = newLength;
        }

        public override Model Copy()
        {
            return new Cube(this);
        }

        protected override Matrix<float> _Matrix()
        {
            Matrix<float> matrix = base._Matrix();

            for (int i = 0; i < 4; i++)
                matrix[i, 0] *= -1;

            return matrix;
        }
    }
}
