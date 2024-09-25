using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using IntBuffer = code.Matrix<int>;
using ColorBuffer = System.Drawing.Color[][];

namespace code
{
    class SolidShading : CanvasProcessor
    {
        const int minLimitZ = -10000;

        Light light;
        Vector3D supervisor;

        Bitmap bitmap;
        IntBuffer zBuffer;
        ColorBuffer colorBuffer;

        public SolidShading(Size size, List<Model> models)
        {
            light = new Light();
            supervisor = new Vector3D(0, 0, -1).NormalizedCopy();

            InitializeZbuffer(size);
            InitializeBitmap(size);
            InitializeColorBuffer(size);
            Processing(models);
        }

        private void InitializeZbuffer(Size size)
        {
            zBuffer = new IntBuffer(size.Height, size.Width, minLimitZ);
        }

        private void InitializeBitmap(Size size)
        {
            bitmap = new Bitmap(size.Width, size.Height);
        }

        private void InitializeColorBuffer(Size size)
        {
            colorBuffer = new Color[size.Height][];
            for (int i = 0; i < size.Height; i++)
            {
                colorBuffer[i] = new Color[size.Width];
                for (int j = 0; j < bitmap.Width; j++)
                {
                    colorBuffer[i][j] = Color.White;
                }
            }
        }

        public Bitmap Image
        {
            get { return bitmap; }
        }

        public void Processing(List<Model> models)
        {
            ProcessModels(models);
            ProcessBitmap();
        }

        private void ProcessBitmap()
        {
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    bitmap.SetPixel(x, y, colorBuffer[y][x]);
                }
            }
        }

        private void ProcessModels(List<Model> models)
        {
            foreach (Model model in models)
            {
                ProcessModel(model);
            }
        }

        private void ProcessModel(Model model)
        {
            foreach (Polygon polygon in model.Polygons)
            {
                ProcessPolygon(polygon, model.Color);
            }
        }

        private void ProcessPolygon(Polygon polygon, Color modelColor)
        {
            float intensity = GetIntensity(polygon);

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

        private float GetIntensity(Polygon polygon)
        {
            Vector3D normal = polygon.Normal().NormalizedCopy();

            float R_z = 2 * normal.Z * normal.Z - 1;
            float R_x = 2 * normal.Z * normal.X;
            float R_y = 2 * normal.Z * normal.Y;
            Vector3D reflection = new Vector3D(R_x, R_y, R_z);

            float LNAngle = Vector3D.Angle(light, normal);
            float SRAngle = Vector3D.Angle(supervisor, reflection);

            Metal metal = new Metal();

            return Math.Abs(
                metal.I_a * metal.k_a + (light.Intensity / (0 + metal.K)) * (
                    metal.k_d * Vector3D.DotProduct(light, normal) +
                    metal.k_s * (float)Math.Pow(
                        Vector3D.DotProduct(reflection, supervisor), metal.n)
                    )
                );
        }

        private void ProcessPoint(int x, int y, int z, float intensity, Color modelColor)
        {
            if (z > zBuffer[y, x])
            {
                zBuffer[y, x] = z;
                colorBuffer[y][x] = (modelColor == Color.Empty) ?
                    Color.FromArgb(
                    (int)(Color.Black.A * intensity),
                    (int)(Color.Black.R * intensity),
                    (int)(Color.Black.G * intensity),
                    (int)(Color.Black.B * intensity))
                    :
                    Color.FromArgb(
                    (int)(modelColor.A * intensity),
                    (int)(modelColor.R * intensity),
                    (int)(modelColor.G * intensity),
                    (int)(modelColor.B * intensity)
                );
            }
        }
    }
}
