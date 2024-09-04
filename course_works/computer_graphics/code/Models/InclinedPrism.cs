using code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace code
{
    class InclinedPrism : Model
    {
        public InclinedPrism() 
        {
            points = new List<Point3D> {
                new Point3D(100, 0, 0), // 0
                new Point3D(100, 0, 100), // 1
                new Point3D(200, 0, 100), // 2
                new Point3D(200, 0, 0), // 3
                new Point3D(0, 200, 0), // 4
                new Point3D(0, 200, 100), // 5
                new Point3D(100, 200, 100), // 6
                new Point3D(100, 200, 0), // 7
            };

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

            name = "Наклонная призма";
            type = Modeltype.InclinedPrism;
            color = Color.Empty;
            length = -1;
            width = -1;
            height = -1;
            angle = -1;
            radius = -1;

            ConstructCenter(points);
            ConstructEdges(points, indexes);
            Update();
        }

        public InclinedPrism(Model other)
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
            UpdateWidth();
            UpdateHeight();
            UpdateAngle();
        }

        private void UpdateLength()
        {
            length = (float)Math.Sqrt(
                Math.Pow(points[2].X - points[1].X, 2) +
                Math.Pow(points[2].Y - points[1].Y, 2) +
                Math.Pow(points[2].Z - points[1].Z, 2)
                );
        }

        private void UpdateWidth()
        {
            width = (float)Math.Sqrt(
                Math.Pow(points[1].X - points[0].X, 2) +
                Math.Pow(points[1].Y - points[0].Y, 2) +
                Math.Pow(points[1].Z - points[0].Z, 2)
                );
        }

        private void UpdateHeight()
        {
            Vector3D baseVector1 = new Vector3D(points[4], points[5]);
            Vector3D baseVector2 = new Vector3D(points[4], points[7]);
            Vector3D baseNormal = Vector3D.CrossProduct(baseVector1, baseVector2);
            baseNormal.Normalize();

            Vector3D side = new Vector3D(points[4], points[0]);
            height = Vector3D.DotProduct(baseNormal, side);
            height = Math.Abs(height);
        }

        private void UpdateAngle()
        {
            double hypotenuse = Math.Sqrt(
                Math.Pow(points[4].X - points[0].X, 2) +
                Math.Pow(points[4].Y - points[0].Y, 2) +
                Math.Pow(points[4].Z - points[0].Z, 2)
            );

            double angleCos = height / hypotenuse;
            double angleRadians = Math.Acos(angleCos);
            double angleDegrees = angleRadians * 180 / Math.PI;
            angle = Math.Min(90 - (float)angleDegrees, 90);
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

        public override float Angle
        {
            get { return angle; }
            set { SetAngle(value); Update(); angle = value; }
        }

        private void SetLength(float newLength)
        {
            float k = newLength / length;
            Scale scale = new Scale(k, k, k);

            code.Scale.Transform(scale, points[3], points[0]);
            code.Scale.Transform(scale, points[2], points[1]);
            code.Scale.Transform(scale, points[7], points[4]);
            code.Scale.Transform(scale, points[6], points[5]);
        }

        private void SetWidth(float newWidth)
        {
            float k = newWidth / width;
            Scale scale = new Scale(k, k, k);

            code.Scale.Transform(scale, points[0], points[1]);
            code.Scale.Transform(scale, points[3], points[2]);
            code.Scale.Transform(scale, points[4], points[5]);
            code.Scale.Transform(scale, points[7], points[6]);
        }

        private void SetHeight(float newHeight)
        {
            float k = newHeight / height;
            Scale scale = new Scale(k, k, k);

            for (int i = 0; i < 4; i++)
            {
                code.Scale.Transform(scale, points[i], points[i + 4]);
            }
        }

        private void SetAngle(float newAngle)
        {
            float k = newAngle / (int)angle;
            Scale scale1 = new Scale(k, k, k);
            Scale scale2 = new Scale(1 / k, 1 / k, 1 / k);

            code.Scale.Transform(scale1, points[0], points[3]);
            code.Scale.Transform(scale1, points[1], points[2]);
            code.Scale.Transform(scale2, points[3], points[0]);
            code.Scale.Transform(scale2, points[2], points[1]);

            //TODO
        }

        public override Model Copy()
        {
            return new InclinedPrism(this);
        }
    }
}
