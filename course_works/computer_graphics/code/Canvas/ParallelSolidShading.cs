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
    class ParallelSolidShading : CanvasProcessor
    {
        const int minLimitZ = -10000;

        Light light;
        Vector3D supervisor;

        Bitmap bitmap;
        IntBuffer zBuffer;
        ColorBuffer colorBuffer;

        public ParallelSolidShading(Size size, List<Model> models)
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
            ParallelProcessModels(models);
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

        private void ParallelProcessModels(List<Model> models)
        {
            Parallel.ForEach(models, model =>
            {
                ParallelProcessModel(model);
            });
        }

        private void ParallelProcessModel(Model model)
        {
            List<Polygon> visiblePolygons = InvisibleFaceDeletor.ProcessModel(model);

            // Параллельная обработка каждого полигона модели
            Parallel.ForEach(visiblePolygons, polygon =>
            {
                ParallelProcessPolygon(polygon, model.Material, model.Color);
            });
        }

        private void ParallelProcessPolygon(Polygon polygon, Material modelMaterial, Color modelColor)
        {
            float intensity = GetIntensity(polygon, modelMaterial);

            for (int y = 0; y < zBuffer.Rows; y++)
            {
                for (int x = 0; x < zBuffer.Columns; x++)
                {
                    int z = (int)polygon.Z(x, y);
                    if (polygon.IsInside(x, y, z))
                        ParallelProcessPoint(x, y, z, intensity, modelColor);
                }
            }
        }

        private float GetIntensity(Polygon polygon, Material material)
        {
            Vector3D normal = polygon.Normal().NormalizedCopy();

            float R_z = 2 * normal.Z * normal.Z - 1;
            float R_x = 2 * normal.Z * normal.X;
            float R_y = 2 * normal.Z * normal.Y;
            Vector3D reflection = new Vector3D(R_x, R_y, R_z);

            return Math.Abs(
                material.I_a * material.k_a + (light.Intensity / (material.d + material.K)) * (
                    material.k_d * Vector3D.DotProduct(light, normal) +
                    material.k_s * (float)Math.Pow(
                        Vector3D.DotProduct(reflection, supervisor), material.n)
                    )
                );
        }

        private void ParallelProcessPoint(int x, int y, int z, float intensity, Color modelColor)
        {
            lock (zBuffer)
            {
                if (z > zBuffer[y, x])
                {
                    zBuffer[y, x] = z;
                    colorBuffer[y][x] = (modelColor == Color.Empty) ?
                        Color.FromArgb(
                        (int)(Math.Min(Color.Black.A * intensity, 255)),
                        (int)(Math.Min(Color.Black.R * intensity, 255)),
                        (int)(Math.Min(Color.Black.G * intensity, 255)),
                        (int)(Math.Min(Color.Black.B * intensity, 255)))
                        :
                        Color.FromArgb(
                        (int)(Math.Min(modelColor.A * intensity, 255)),
                        (int)(Math.Min(modelColor.R * intensity, 255)),
                        (int)(Math.Min(modelColor.G * intensity, 255)),
                        (int)(Math.Min(modelColor.B * intensity, 255))
                    );
                }
            }
        }
    }
}
