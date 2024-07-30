using code.Objects;
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
            this.points = new List<Point3D> {
                new Point3D(0, (float) Math.Sqrt(6) * 100 / 3, 0),                          // 0
                new Point3D(50, (float) Math.Sqrt(6) * 100 / 3, (float)Math.Sqrt(3) * 50),  // 1
                new Point3D(100, (float) Math.Sqrt(6) * 100 / 3, 0),                        // 2
                new Point3D(50, 0, (float)Math.Sqrt(3) / 3 * 50)                            // 3
            };

            this.indexes = new List<int>() {
                0, 1,
                1, 2,
                2, 0,
                0, 3,
                1, 3,
                2, 3
            };

            this.type = Modeltype.Pyramid;

            this.length = 100;
            this.height = (float)Math.Sqrt(6) * 100 / 3;
            this.width = -1;
            this.radius = -1;
            this.angle = -1;

            ConstructCenter(this.points);
            ConstructEdges(this.points, this.indexes);
        }

        protected override void Update()
        {
            UpdateCenter();
            UpdateLength();
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

        private void UpdateHeight()
        {
            // Находим вектор, перпендикулярный основанию
            Vector3D vector1 = new Vector3D(points[0], points[1]);
            Vector3D vector2 = new Vector3D(points[0], points[2]);
            Vector3D baseNormal = Vector3D.CrossProduct(vector1, vector2);
            baseNormal.Normalize();

            // Находим расстояние от точки 3 до плоскости основания
            Vector3D vector3 = new Vector3D(points[0], points[3]);
            height = Vector3D.DotProduct(vector3, baseNormal);
            height = Math.Abs(height);
        }
    }


}
