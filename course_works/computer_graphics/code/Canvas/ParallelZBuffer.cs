using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;

using IntBuffer = code.Matrix<int>;
using ColorBuffer = System.Drawing.Color[][];
using System.Drawing.Imaging;

namespace code
{
    class ParallelZBuffer : CanvasProcessor
    {
        const int minLimitZ = -10000;

        Bitmap bitmap;
        IntBuffer zBuffer;
        ColorBuffer colorBuffer;

        public ParallelZBuffer(Bitmap bitmap, List<Model> models)
        {
            InitializeZbuffer(bitmap);
            InitializeBitmap(bitmap);
            InitializeColorBuffer(bitmap);
            ParallelProcessing(models);
        }

        public ParallelZBuffer(Size size, List<Model> models)
        {
            InitializeZbuffer(size);
            InitializeBitmap(size);
            InitializeColorBuffer(size);
            ParallelProcessing(models);
        }

        public Bitmap Image
        {
            get { return bitmap; }
        }

        #region Initialize

        private void InitializeZbuffer(Bitmap bitmap)
        {
            zBuffer = new IntBuffer(bitmap.Height, bitmap.Width);

            for (int i = 0; i < zBuffer.Rows; i++)
            {
                for (int j = 0; j < zBuffer.Columns; j++)
                {
                    zBuffer[i, j] = minLimitZ;
                }
            }
        }

        private void InitializeZbuffer(Size size)
        {
            zBuffer = new IntBuffer(size.Height, size.Width);

            for (int i = 0; i < zBuffer.Rows; i++)
            {
                for (int j = 0; j < zBuffer.Columns; j++)
                {
                    zBuffer[i, j] = minLimitZ;
                }
            }
        }

        private void InitializeBitmap(Bitmap originalBitmap)
        {
            bitmap = new Bitmap(bitmap);
        }

        private void InitializeBitmap(Size size)
        {
            bitmap = new Bitmap(size.Width, size.Height);
        }

        private void InitializeColorBuffer(Bitmap bitmap)
        {
            colorBuffer = new Color[bitmap.Height][];
            for (int i = 0; i < bitmap.Height; i++)
            {
                colorBuffer[i] = new Color[bitmap.Width];
                for (int j = 0; j < bitmap.Width; j++)
                {
                    colorBuffer[i][j] = Color.White;
                }
            }
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

        #endregion

        #region Processing

        private void ParallelProcessing(List<Model> models)
        {
            ParallelProcessModels(models);
            ParallelProcessBitmap();
        }

        private void ParallelProcessBitmap()
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
                        int offset = (y * data.Stride) +(x * 4);

                        ptr[offset] = colorBuffer[y][x].B;
                        ptr[offset + 1] = colorBuffer[y][x].G;
                        ptr[offset + 2] = colorBuffer[y][x].R;
                        ptr[offset + 3] = colorBuffer[y][x].A;
                    }
                }
            }

            bitmap.UnlockBits(data);
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
            // Параллельная обработка каждого полигона модели
            Parallel.ForEach(model.Polygons, polygon =>
            {
                for (int y = 0; y < zBuffer.Rows; y++)
                {
                    for (int x = 0; x < zBuffer.Columns; x++)
                    {
                        int z = (int)polygon.Z(x, y);
                        if (polygon.IsInside(x, y, z))
                            ParallelProcessPoint(x, y, z, model.Color);
                    }
                }
            });
        }

        private void ParallelProcessPoint(int x, int y, int z, Color color)
        {
            lock (zBuffer)
            {
                if (z > zBuffer[y, x])
                {
                    zBuffer[y, x] = z;
                    colorBuffer[y][x] = color == Color.Empty ? Color.Black : color;
                }
            }
        }

        #endregion
    }
}
