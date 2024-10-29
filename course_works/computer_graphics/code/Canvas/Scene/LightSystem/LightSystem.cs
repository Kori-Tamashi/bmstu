using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace code
{
    class LightSystem
    {
        const int maxLights = 1;

        Vector3D startLightDirection = new Vector3D(0, 0, 1);
        Point3D startLightPosition = new Point3D(83, 83, -300);

        List<Light> lights;
        int currentLightIndex = 0;

        public LightSystem()
        {
            InitializeLights();
        }

        #region Initialize

        private void InitializeLights()
        {
            lights = new List<Light> {
                StartLight()
            };
        }

        private Light StartLight()
        {
            return new Light(startLightDirection, startLightPosition);
        }

        #endregion

        #region Getters & Setters

        public int CurrentLightIndex
        {
            get { return currentLightIndex; }
            set { currentLightIndex = Math.Clamp(value, 0, lights.Count - 1); }
        }

        public Light CurrentLight
        {
            get { return lights[currentLightIndex]; }
        }

        public int MaxLights
        {
            get { return maxLights; }
        }

        public float LightIntensity
        {
            get { return lights[currentLightIndex].Intensity; }
            set { lights[currentLightIndex].Intensity = value; }
        }

        public Vector3D LightDirection
        {
            get { return lights[currentLightIndex].Direction; }
            set { lights[currentLightIndex].Direction = value; }
        }

        public Point3D LightPosition
        {
            get { return lights[currentLightIndex].Position; }
            set { lights[currentLightIndex].Position = value; }
        }

        #endregion

        #region Methods

        public void AddLight(Light light)
        {
            lights.Add(light);
        }

        public void AddLight(Vector3D direction, Point3D position)
        {
            lights.Add( new Light(direction, position) );
        }

        public void DeleteLight(int index)
        {
            lights.RemoveAt(index);
        }

        public void MoveLight(Move move)
        {
            lights[currentLightIndex].Move(move);
        }

        public void MoveRight(float d)
        {
            lights[currentLightIndex].MoveRight(d);
        }

        public void MoveUp(float d)
        {
            lights[currentLightIndex].MoveUp(d);
        }

        public void MoveLeft(float d)
        {
            lights[currentLightIndex].MoveLeft(d);
        }

        public void MoveDown(float d)
        {
            lights[currentLightIndex].MoveDown(d);
        }

        public void MoveUpRight(float d)
        {
            lights[currentLightIndex].MoveUpRight(d);
        }

        public void MoveUpLeft(float d)
        {
            lights[currentLightIndex].MoveUpLeft(d);
        }

        public void MoveDownRight(float d)
        {
            lights[currentLightIndex].MoveDownRight(d);
        }

        public void MoveDownLeft(float d)
        {
            lights[currentLightIndex].MoveDownLeft(d);
        }

        #endregion
    }
}
