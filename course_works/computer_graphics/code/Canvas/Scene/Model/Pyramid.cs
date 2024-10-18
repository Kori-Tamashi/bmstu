using code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;


namespace code
{
    class Pyramid : Model
    {
        public Pyramid() 
        {
            points = new List<Point3D> {
                new Point3D(0, (float) Math.Sqrt(6) * 100 / 3, 0),                          // 0
                new Point3D(50, (float) Math.Sqrt(6) * 100 / 3, (float)Math.Sqrt(3) * 50),  // 1
                new Point3D(100, (float) Math.Sqrt(6) * 100 / 3, 0),                        // 2
                new Point3D(50, 0, (float)Math.Sqrt(3) / 3 * 50)                            // 3
            };

            indexes = new List<int>() {
                0, 1,
                1, 2,
                2, 0,
                0, 3,
                1, 3,
                2, 3
            };

            name = "Пирамида";
            type = Modeltype.Pyramid;
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

        public Pyramid(Model other)
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
                new Polygon(points[0], points[1], points[2]),
                new Polygon(points[0], points[3], points[1]),
                new Polygon(points[1], points[3], points[2]),
                new Polygon(points[2], points[3], points[0]),
            };
        }

        protected override void Update()
        {
            UpdateCenter();
            UpdateLength();
            UpdateHeight();
            UpdatePolygons();
        }

        private void UpdateLength()
        {
            length = (float)Math.Sqrt(
                Math.Pow(points[1].X - points[0].X, 2) +
                Math.Pow(points[1].Y - points[0].Y, 2) +
                Math.Pow(points[1].Z - points[0].Z, 2)
                );
        }

        private void UpdateHeight()
        {
            Vector3D vector1 = new Vector3D(points[0], points[1]);
            Vector3D vector2 = new Vector3D(points[0], points[2]);
            Vector3D baseNormal = Vector3D.CrossProduct(vector1, vector2);
            baseNormal.Normalize();

            Vector3D vector3 = new Vector3D(points[0], points[3]);
            height = Vector3D.DotProduct(vector3, baseNormal);
            height = Math.Abs(height);
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

        public override float Height
        {
            get { return height; }
            set { SetHeight(value); Update(); height = value; }
        }

        private void SetLength(float newLength)
        {
            float k = newLength / length;
            Scale scale = new Scale(k, k, k);

            Point3D baseCenter = new Point3D(
                    (points[0].X + points[1].X + points[2].X) / 3,
                    (points[0].Y + points[1].Y + points[2].Y) / 3,
                    (points[0].Z + points[1].Z + points[2].Z) / 3
                );

            for (int i = 0; i < 3; i++)
            {
                code.Scale.Transform(scale, points[i], baseCenter);
            }
        }

        private void SetHeight(float newHeight)
        {
            float k = newHeight / height;
            Scale scale = new Scale(k, k, k);

            Point3D baseCenter = new Point3D(
                    (points[0].X + points[1].X + points[2].X) / 3,
                    (points[0].Y + points[1].Y + points[2].Y) / 3,
                    (points[0].Z + points[1].Z + points[2].Z) / 3
                );

            code.Scale.Transform(scale, points[3], baseCenter);
        }

        public override Model Copy()
        {
            return new Pyramid(this);
        }
    }
}
