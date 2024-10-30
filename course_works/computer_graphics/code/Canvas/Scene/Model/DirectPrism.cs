using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;


namespace code
{
    class DirectPrism : Model
    {
        public DirectPrism() 
        {
            points = new List<Point3D> {
                new Point3D(0, 0, 0), // 0
                new Point3D(0, 0, 100), // 1
                new Point3D(100, 0, 100), // 2
                new Point3D(100, 0, 0), // 3
                new Point3D(0, 200, 0), // 4
                new Point3D(0, 200, 100), // 5
                new Point3D(100, 200, 100), // 6
                new Point3D(100, 200, 0), // 7
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

            name = "Прямая призма";
            type = Modeltype.DirectPrism;
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

        public DirectPrism(Model other)
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
                new Polygon(points[0], points[1], points[5], points[4]),
                new Polygon(points[3], points[2], points[6], points[7]),
                new Polygon(points[0], points[1], points[2], points[3]),
                new Polygon(points[1], points[5], points[6], points[2]),
                new Polygon(points[5], points[4], points[7], points[6]),
                new Polygon(points[4], points[0], points[3], points[7])
            };
        }

        protected override void Update()
        {
            UpdateCenter();
            UpdateLength();
            UpdateWidth();
            UpdateHeight();
            UpdatePolygons();
            UpdateLowerBaseRadius();
            UpdateUpperBaseRadius();
        }

        protected override void UpdateLength()
        {
            length = (float)Math.Sqrt(
                Math.Pow(points[2].X - points[1].X, 2) +
                Math.Pow(points[2].Y - points[1].Y, 2) +
                Math.Pow(points[2].Z - points[1].Z, 2)
                );
        }

        protected virtual void UpdateWidth()
        {
            width = (float)Math.Sqrt(
                Math.Pow(points[1].X - points[0].X, 2) +
                Math.Pow(points[1].Y - points[0].Y, 2) +
                Math.Pow(points[1].Z - points[0].Z, 2)
                );
        }

        protected override void UpdateHeight()
        {
            height = (float)Math.Sqrt(
                Math.Pow(points[4].X - points[0].X, 2) +
                Math.Pow(points[4].Y - points[0].Y, 2) +
                Math.Pow(points[4].Z - points[0].Z, 2)
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

        public override float Width
        {
            get { return width; }
            set { SetWidth(value); Update(); width = value; }
        }

        public override float Height
        {
            get { return height; }
            set { SetHeight(value); Update(); height = value; }
        }

        public override int FacesCount
        {
            get { return facesCount; }
            set {  }
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

        protected virtual void SetLength(float newLength)
        {
            float k = newLength / length;
            Scale scale = new Scale(k, k, k);

            code.Scale.Transform(scale, points[3], points[0]);
            code.Scale.Transform(scale, points[2], points[1]);
            code.Scale.Transform(scale, points[7], points[4]);
            code.Scale.Transform(scale, points[6], points[5]);
        }

        protected virtual void SetWidth(float newWidth)
        {
            float k = newWidth / width;
            Scale scale = new Scale(k, k, k);

            code.Scale.Transform(scale, points[0], points[1]);
            code.Scale.Transform(scale, points[3], points[2]);
            code.Scale.Transform(scale, points[4], points[5]);
            code.Scale.Transform(scale, points[7], points[6]);
        }

        protected override void SetHeight(float newHeight)
        {
            float k = newHeight / height;
            Scale scale = new Scale(k, k, k);

            for (int i = 0; i < 4; i++)
            {
                code.Scale.Transform(scale, points[i], points[i + 4]);
            }
        }

        public override Model Copy()
        {
            return new DirectPrism(this);
        }

        protected override Matrix<float> _Matrix()
        {
            Matrix<float> matrix = base._Matrix();

            for (int i = 0; i < 4; i++)
                matrix[i, 0] *= -1;

            return -1 * matrix;
        }
    }
}
