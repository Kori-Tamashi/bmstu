using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace code
{
    public class Light
    {
        float intensity;
        Vector3D direction;
        Point3D position;

        public Light()
        {
            intensity = 5;
            direction = new Vector3D(0, 0, -1);
            position = new Point3D(0, 0, 200);
        }

        public Light(float intensity, Vector3D direction, Point3D position)
        {
            this.intensity = intensity;
            this.direction = direction;
            this.position = position;
        }

        public Light(Vector3D direction, Point3D position, float intensity = 5)
        {
            this.intensity = intensity;
            this.direction = direction;
            this.position = position;
        }

        public Vector3D Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public Point3D Position
        {
            get { return position; }
            set { position = value; }
        }

        public float Intensity
        {
            get { return intensity; }
            set { intensity = value; }
        }
    }
}
