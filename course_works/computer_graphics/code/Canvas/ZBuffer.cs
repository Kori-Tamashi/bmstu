using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Reflection;

using IntBuffer = code.Matrix<int>;
using ColorBuffer = System.Drawing.Color[][];


namespace code
{
    class ZBuffer : CanvasProcessor
    {
        protected const int minLimitZ = -10000;

        protected Bitmap bitmap;
        protected IntBuffer zBufferModels;
        protected ColorBuffer colorBufferModels;
        
        public ZBuffer(Size size)
        {
            InitializeBitmap(size);
            InitializeZbufferModels(size);
            InitializeColorBufferModels(size);
        }

        public ZBuffer(Size size, List<Model> models)
        {
            InitializeBitmap(size);
            InitializeZbufferModels(size);
            InitializeColorBufferModels(size);
            Processing(models);
        }

        #region Initialize

        protected virtual void InitializeZbufferModels(Size size)
        {
            zBufferModels = new IntBuffer(size.Height, size.Width, minLimitZ);
        }

        protected virtual void InitializeBitmap(Size size)
        {
            bitmap = new Bitmap(size.Width, size.Height);
        }

        protected virtual void InitializeColorBufferModels(Size size)
        {
            colorBufferModels = new Color[size.Height][];

            // Инициализация каждого ряда в отдельном потоке
            Parallel.For(0, size.Height, i =>
            {
                colorBufferModels[i] = new Color[size.Width];

                // Заполнение ряда цветом
                for (int j = 0; j < size.Width; j++)
                {
                    colorBufferModels[i][j] = Color.White;
                }
            });
        }

        #endregion

        #region Getters & Setters

        public Bitmap Image
        {
            get { return bitmap; }
        }

        #endregion

        #region Processing

        protected virtual void Processing(List<Model> models)
        {
            ProcessModels(models);
            ProcessBitmap(bitmap);
        }

        protected virtual void ProcessBitmap(Bitmap bitmap)
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

                        ptr[offset] = colorBufferModels[y][x].B;
                        ptr[offset + 1] = colorBufferModels[y][x].G;
                        ptr[offset + 2] = colorBufferModels[y][x].R;
                        ptr[offset + 3] = colorBufferModels[y][x].A;
                    }
                }
            }

            bitmap.UnlockBits(data);
        }

        protected virtual void ProcessModels(List<Model> models)
        {
            foreach (Model model in models)
            {
                ProcessModel(model);
            }
        }

        protected virtual void ProcessModel(Model model)
        {
            List<Polygon> visiblePolygons = InvisibleFaceDeletor.ProcessModel(model);

            foreach (Polygon polygon in visiblePolygons)
            {
                ProcessPolygon(polygon, model.Color);
            }
        }

        protected virtual void ProcessPolygon(Polygon polygon, Color modelColor)
        {
            for (int y = 0; y < zBufferModels.Rows; y++)
            {
                for (int x = 0; x < zBufferModels.Columns; x++)
                {
                    int z = (int)polygon.Z(x, y);
                    if (polygon.IsInside(x, y, z))
                        ProcessPoint(x, y, z, modelColor);
                }
            }
        }

        protected virtual void ProcessPoint(int x, int y, int z, Color color)
        {
            if (z > zBufferModels[y, x])
            {
                zBufferModels[y, x] = z;
                colorBufferModels[y][x] = color == Color.Empty ? Color.Black : color;
            }
        }

        #endregion
    }
}
