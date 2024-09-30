using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace code
{
    class SolidShading : ZBuffer
    {
        protected List<Light> lights;
        protected Vector3D supervisor;

        public SolidShading(Size size, List<Model> models, List<Light> lights, Vector3D supervisor) : base(size)
        {
            this.lights = lights;
            this.supervisor = supervisor;
            Processing(models);
        }

        protected override void ProcessModel(Model model)
        {
            List<Polygon> visiblePolygons = InvisibleFaceDeletor.ProcessModel(model);

            foreach (Polygon polygon in visiblePolygons)
            {
                ProcessPolygon(polygon, model.Material, model.Color);
            }
        }

        protected virtual void ProcessPolygon(Polygon polygon, Material material, Color modelColor)
        {
            float intensity = GetIntensity(polygon, material);

            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    int z = (int)polygon.Z(x, y);
                    if (polygon.IsInside(x, y, z))
                        ProcessPoint(x, y, z, intensity, modelColor);
                }
            }
        }

        protected virtual void ProcessPoint(int x, int y, int z, float intensity, Color modelColor)
        {
            if (z > zBufferModels[y, x])
            {
                zBufferModels[y, x] = z;
                colorBufferModels[y][x] = (modelColor == Color.Empty) ? _Color(Color.Black, intensity) : _Color(modelColor, intensity);
            }
        }

        protected Color _Color(Color color, float intensity)
        {
            return Color.FromArgb(
                    (int)(Math.Max(0, Math.Min(color.A * intensity, 255))),
                    (int)(Math.Max(0, Math.Min(color.R * intensity, 255))),
                    (int)(Math.Max(0, Math.Min(color.G * intensity, 255))),
                    (int)(Math.Max(0, Math.Min(color.B * intensity, 255))));
        }

        protected virtual float GetIntensity(Polygon polygon, Material material)
        {
            Vector3D normal = polygon.Normal().NormalizedCopy();

            float R_z = 2 * normal.Z * normal.Z - 1;
            float R_x = 2 * normal.Z * normal.X;
            float R_y = 2 * normal.Z * normal.Y;
            Vector3D reflection = new Vector3D(R_x, R_y, R_z);

            float intensity = material.I_a * material.k_a;

            foreach (Light light in lights)
            {
                intensity += (light.Intensity / (material.d + material.K)) * (
                    material.k_d * Vector3D.DotProduct(light, normal) +
                    material.k_s * (float)Math.Pow(
                        Vector3D.DotProduct(reflection, supervisor), material.n)
                    );
            }

            return Math.Abs(intensity);
        }
    }
}
