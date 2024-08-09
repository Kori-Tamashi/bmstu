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
            this.points = new List<Point3D> {
                new Point3D(0, 0, 0), // 0
                new Point3D(0, 0, 100), // 1
                new Point3D(100, 0, 100), // 2
                new Point3D(100, 0, 0), // 3
                new Point3D(0, 200, 0), // 4
                new Point3D(0, 200, 100), // 5
                new Point3D(100, 200, 100), // 6
                new Point3D(100, 200, 0), // 7
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

            this.type = Modeltype.DirectPrism;
            this.color = Color.Empty;
            this.length = 100;
            this.width = 100;
            this.height = 200;
            this.radius = -1;
            this.angle = -1;

            ConstructCenter(this.points);
            ConstructEdges(this.points, this.indexes);
        }

        protected override void Update()
        {
            UpdateCenter();
            UpdateLength();
            UpdateWidth();
            UpdateHeight();
        }

        private void UpdateLength()
        {
            length = (float)Math.Sqrt(
                Math.Pow(points[1].X - points[0].X, 2) +
                Math.Pow(points[1].Y - points[0].Y, 2) +
                Math.Pow(points[1].Z - points[0].Z, 2)
                );
        }

        private void UpdateWidth()
        {
            width = (float)Math.Sqrt(
                Math.Pow(points[2].X - points[1].X, 2) +
                Math.Pow(points[2].Y - points[1].Y, 2) +
                Math.Pow(points[2].Z - points[1].Z, 2)
                );
        }

        private void UpdateHeight()
        {
            height = (float)Math.Sqrt(
                Math.Pow(points[4].X - points[0].X, 2) +
                Math.Pow(points[4].Y - points[0].Y, 2) +
                Math.Pow(points[4].Z - points[0].Z, 2)
                );
        }

        public override float Length
        {
            get { return length; }
            set { SetLength(value); length = value; }
        }

        public override float Width
        {
            get { return width; }
            set { SetWidth(value); width = value; }
        }

        public override float Height
        {
            get { return height; }
            set { SetHeight(value); height = value; }
        }

        private void SetLength(float newLength)
        {
            float k = newLength / length;
            Scale(new Scale(1, 1, k));
        }

        private void SetWidth(float newWidth)
        {
            float k = newWidth / width;
            Scale(new Scale(k, 1, 1));
        }

        private void SetHeight(float newHeight)
        {
            float k = newHeight / height;
            Scale(new code.Scale(1, k, 1));
        }
    }
}
