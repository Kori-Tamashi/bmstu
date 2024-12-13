using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using System.Windows.Forms.DataVisualization.Charting;

using IntBuffer = code.Matrix<int>;
using FloatBuffer = code.Matrix<float>;
using ColorBuffer = System.Drawing.Color[][];
using System.Drawing.Imaging;
using System.Numerics;


namespace code
{
    class ZBufferShadows : SolidShading
    {
        Bitmap bitmapShadows;
        FloatBuffer angles;
        IntBuffer zBufferShadows;
        protected ColorBuffer colorBufferShadows;

        public ZBufferShadows(Size size, List<Model> models, List<Light> lights, Vector3D supervisor) : base(size, lights, supervisor)
        {
            InitializeColorBufferShadows(size);
            InitializeZbufferShadows(size);
            InitializeAngles(lights, supervisor);
            Processing(models);

            //WriteToCsv(zBufferModels.Matrix_, "zBufferModel.csv");
            //WriteToCsv(zBufferShadows.Matrix_, "zBufferShadows.csv");
        }

        #region Initialize

        protected override void InitializeBitmap(Size size)
        {
            bitmap = new Bitmap(size.Width, size.Height);
            bitmapShadows = new Bitmap(size.Width, size.Height);
        }

        private void InitializeZbufferShadows(Size size)
        {
            zBufferShadows = new IntBuffer(size.Height, size.Width, minLimitZ);
        }

        private void InitializeAngles(List<Light> lights, Vector3D supervisor)
        {
            angles = new FloatBuffer(lights.Count, 3);

            for (int i = 0; i < lights.Count; i++)
            {
                angles[i, 0] = Vector3D.Angle(lights[i].Direction.YZProjection(), supervisor.YZProjection());
                angles[i, 1] = Vector3D.Angle(lights[i].Direction.XZProjection(), supervisor.XZProjection());
                angles[i, 2] = 0;

                if (lights[i].Direction.X > 0)
                    angles[i, 1] *= -1;
                if (lights[i].Direction.Y > 0)
                    angles[i, 0] *= -1;
            }
        }

        protected virtual void InitializeColorBufferShadows(Size size)
        {
            colorBufferShadows = new Color[size.Height][];

            // Инициализация каждого ряда в отдельном потоке
            Parallel.For(0, size.Height, i =>
            {
                colorBufferShadows[i] = new Color[size.Width];

                // Заполнение ряда цветом
                for (int j = 0; j < size.Width; j++)
                {
                    colorBufferShadows[i][j] = Color.White;
                }
            });
        }

        #endregion

        public Bitmap ShadowImage
        {
            get { return bitmapShadows; }
        }

        #region Processing

        protected override void Processing(List<Model> models)
        {
            ProcessModelsShadows(models);
            ProcessModels(models);
            ProcessBitmap(bitmap);
            ProcessBitmapShadows(bitmapShadows);
        }

        protected virtual void ProcessBitmapShadows(Bitmap bitmap)
        {
            BitmapData data = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb
                );

            unsafe
            {
                byte* ptr = (byte*)data.Scan0;

                for (int y = 0; y < bitmap.Height; y++)
                {
                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        int offset = (y * data.Stride) + (x * 4);

                        ptr[offset] = colorBufferShadows[y][x].B;
                        ptr[offset + 1] = colorBufferShadows[y][x].G;
                        ptr[offset + 2] = colorBufferShadows[y][x].R;
                        ptr[offset + 3] = colorBufferShadows[y][x].A;
                    }
                }
            }

            bitmap.UnlockBits(data);
        }

        protected virtual void ProcessModelsShadows(List<Model> models)
        {
            foreach (Model model in models)
            {
                for (int i = 0; i < lights.Count; i++)
                {
                    ProcessModelShadows(model, i);
                }
            }
        }

        protected virtual void ProcessModelShadows(Model model, int lightIndex)
        {
            Model rotatedModel = GetRotatedModel(model, lightIndex);

            foreach (Polygon polygon in rotatedModel.Polygons)
            {
                ProcessPolygonShadows(polygon, model.Color, lightIndex);
            }
        }

        protected virtual void ProcessPolygonShadows(Polygon polygon, Color color, int lightIndex)
        {
            for (int y = 0; y < zBufferShadows.Rows; y++)
            {
                for (int x = 0; x < zBufferShadows.Columns; x++)
                {
                    int z = (int)polygon.Z(x, y);
                    if (ShadowOnImage(x, y))
                        ProcessPointShadows(x, y, z, color);
                }
            }
        }

        protected virtual void ProcessPointShadows(Point3D point, Color color)
        {
            ProcessPointShadows((int)point.X, (int)point.Y, (int)point.Z, color);
        }

        protected virtual void ProcessPointShadows(int x, int y, int z, Color color)
        {
            if (z > zBufferShadows[y, x])
            {
                zBufferShadows[y, x] = z;
                colorBufferShadows[y][x] = color == Color.Empty ? Color.Black : color;
            }
        }

        protected override void ProcessModel(Model model)
        {
            foreach (Polygon polygon in model.Polygons)
            {
                for (int i = 0; i < lights.Count; i++)
                {
                    ProcessPolygon(polygon, model.Material, model.Color, i);
                }
            }
        }

        protected virtual void ProcessPolygon(Polygon polygon, Material material, Color modelColor, int lightIndex)
        {
            float intensity = GetIntensity(polygon, material);

            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    int z = (int)polygon.Z(x, y);
                    if (polygon.IsInside(x, y, z))
                        ProcessPoint(x, y, z, intensity, modelColor, lightIndex);
                }
            }
        }

        protected virtual void ProcessPoint(int x, int y, int z, float intensity, Color color, int lightIndex)
        {
            if (z > zBufferModels[y, x])
            {
                ProcessShadow(x, y, z, intensity, color, lightIndex);
            }
        }

        protected virtual void ProcessShadow(int originalX, int originalY, int originalZ, float intensity, Color color, int lightIndex)
        {
            Point3D rotatedPoint = GetRotatedPoint(originalX, originalY, originalZ, lightIndex);

            int rotatedX = (int)rotatedPoint.X;
            int rotatedY = (int)rotatedPoint.Y;
            int rotatedZ = (int)rotatedPoint.Z;

            if (ShadowOnImage(rotatedX, rotatedY))
            {
                if (rotatedZ > zBufferShadows[rotatedY, rotatedX])
                {
                    zBufferModels[originalY, originalX] = originalZ;
                    colorBufferModels[originalY][originalX] = (color == Color.Empty) ? _Color(Color.Black, intensity) : _Color(color, intensity);
                }
                else
                {
                    zBufferModels[originalY, originalX] = rotatedZ;
                    colorBufferModels[originalY][originalX] = (color == Color.Empty) ? _ColorMix(Color.Black, Color.Gray, 0.4f) : _ColorMix(Color.Black, color, 0.4f);
                }
            }
            else
            {
                zBufferModels[originalY, originalX] = originalZ;
                colorBufferModels[originalY][originalX] = (color == Color.Empty) ? _Color(Color.Black, intensity) : _Color(color, intensity);
            }
        }

        protected virtual Model GetRotatedModel(Model model, int lightIndex)
        {
            Model rotatedModel = model.Copy();

            for (int i = 0; i < rotatedModel.Points.Count; i++)
            {
                rotatedModel.Points[i] = GetRotatedPoint(rotatedModel.Points[i], lightIndex);
            }

            rotatedModel.Update_();

            return rotatedModel;
        }

        protected virtual Point3D GetRotatedPoint(Point3D point, int lightIndex)
        {
            Point3D res = new Point3D(point.X, point.Y, point.Z);
            Rotate rotate = new Rotate(angles[lightIndex, 0], angles[lightIndex, 1], 0);
            Rotate.Transform(rotate, res, new Point3D(lights[lightIndex].Position.X, lights[lightIndex].Position.Y, lights[lightIndex].Position.Z));
            return res;
        }

        protected virtual Point3D GetRotatedPoint(float x, float y, float z, int lightIndex)
        {
            return GetRotatedPoint(new Point3D(x, y, z), lightIndex);
        }

        private bool ShadowOnImage(Point3D point)
        {
            return ShadowOnImage((int)point.X, (int)point.Y);
        }

        private bool ShadowOnImage(int x, int y)
        {
            return (x >= 0) && (y >= 0) && (x < zBufferShadows.Columns) && (y < zBufferShadows.Rows);
        }

        public static Color _ColorMix(Color a, Color b, float aPers)
        {
            aPers = Math.Min(aPers, 1);

            int red = (int)(a.R * aPers + b.R * (1 - aPers));
            int green = (int)(a.G * aPers + b.G * (1 - aPers));
            int blue = (int)(a.B * aPers + b.B * (1 - aPers));

            return Color.FromArgb(red, green, blue);
        }

        public static void WriteToCsv(List<List<int>> array, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                for (int i = 0; i < array.Count; i++)
                {
                    for (int j = 0; j < array[i].Count; j++)
                    {
                        writer.Write(array[i][j].ToString());
                        if (j < array[i].Count - 1)
                        {
                            writer.Write(",");
                        }
                    }
                    writer.WriteLine();
                }
            }
        }

        #endregion


        private void WinterpumaAddShadows()
        {
            for (int y = 0; y < zBufferModels.Rows; y++)
            {
                for (int x = 0; x < zBufferModels.Columns; x++)
                {
                    int z = zBufferModels[y, x];

                    if (z != minLimitZ)
                    {
                        Point3D point = new Point3D(x, y, z);
                        Rotate rotate = new Rotate(angles[0, 0], angles[0, 1], angles[0, 2]);
                        Rotate.Transform(rotate, point);

                        int rotatedX = (int)point.X;
                        int rotatedY = (int)point.Y;
                        int rotatedZ = (int)point.Z;

                        Color color = Image.GetPixel(x, y);
                        if (!ShadowOnImage(rotatedY, rotatedX))
                        {
                            Image.SetPixel(x, y, color);
                            continue;
                        }

                        if (zBufferShadows[rotatedY, rotatedX] > rotatedZ + 5)
                        {
                            Image.SetPixel(x, y, _ColorMix(Color.Black, color, 0.4f));
                        }
                        else
                        {
                            Image.SetPixel(x, y, color);
                        }
                    }
                }
            }
        }
    }
}
