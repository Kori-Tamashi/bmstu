using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace code
{
    public class Light
    {
        float intensity;
        Vector3D direction;

        public Light()
        {
            intensity = 1;
            direction = new Vector3D(0, 0, -1);
        }

        public Light(float intensity, Vector3D direction)
        {
            this.intensity = intensity;
            this.direction = direction;
        }

        public Light(Vector3D direction, float intensity = 1)
        {
            this.intensity = intensity;
            this.direction = direction;
        }

        public Vector3D Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public float Intensity
        {
            get { return intensity; }
            set { intensity = value; }
        }
    }
}
