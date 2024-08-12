﻿using code.Objects;
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

        public override float Length
        {
            get { return length; }
            set { SetLength(value); length = value; }
        }

        public override float Height
        {
            get { return height; }
            set { SetHeight(value); height = value; }
        }

        //private void SetLength(float newLength)
        //{
        //    Point3D baseCenter = new Point3D(
        //        (points[0].X + points[1].X + points[2].X) / 3,
        //        (points[0].Y + points[1].Y + points[2].Y) / 3,
        //        (points[0].Z + points[1].Z + points[2].Z) / 3
        //    );

        //    List<Vector3D> baseVectors = new List<Vector3D> {
        //        new Vector3D(baseCenter, points[0]),
        //        new Vector3D(baseCenter, points[1]),
        //        new Vector3D(baseCenter, points[2]),
        //    };

        //    foreach (Vector3D vector in baseVectors)
        //    {
        //        vector.Normalize();
        //    }

        //    for (int i = 0; i < baseVectors.Count; i++)
        //    {
        //        points[i].X = baseCenter.X + baseVectors[i].X * newLength / 2;
        //        points[i].Y = baseCenter.Y + baseVectors[i].Y * newLength / 2;
        //        points[i].Z = baseCenter.Z + baseVectors[i].Z * newLength / 2;
        //    }

        //    Update();
        //}

        private void SetLength(float newLength)
        {
            float k = newLength / length;
            Scale(new Scale(k, 1, k));
        }

        //private void SetHeight(float newHeight)
        //{
        //    Point3D baseCenter = new Point3D(
        //        (points[0].X + points[1].X + points[2].X) / 3,
        //        (points[0].Y + points[1].Y + points[2].Y) / 3,
        //        (points[0].Z + points[1].Z + points[2].Z) / 3
        //    );

        //    List<Vector3D> baseVectors = new List<Vector3D> {
        //        new Vector3D(points[0], points[1]),
        //        new Vector3D(points[0], points[2])
        //    };

        //    Vector3D baseNormal = Vector3D.CrossProduct(baseVectors[0], baseVectors[1]);
        //    baseNormal.Normalize();

        //    points[3].X = baseCenter.X - baseNormal.X * newHeight;
        //    points[3].Y = baseCenter.Y - baseNormal.Y * newHeight;
        //    points[3].Z = baseCenter.Z - baseNormal.Z * newHeight;

        //    Update();
        //}

        private void SetHeight(float newHeight)
        {
            float k = newHeight / height;
            Scale(new Scale(1, k, 1));
        }

        public override Model Copy()
        {
            return new Pyramid(this);
        }
    }
}
