using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IntBuffer = code.Matrix<int>;
using ColorBuffer = System.Drawing.Color[][];

namespace code
{
    class ZBuffer : CanvasProcessor
    {
        const int minLimitZ = -10000;

        Bitmap bitmap;
        IntBuffer zBuffer;
        ColorBuffer colorBuffer;

        public ZBuffer(Bitmap bitmap, List<Model> models)
        {
            InitializeZbuffer(bitmap);
            InitializeBitmap(bitmap);
            InitializeColorBuffer(bitmap);
            Processing(models);
        }

        public ZBuffer(Size size, List<Model> models)
        {
            InitializeZbuffer(size);
            InitializeBitmap(size);
            InitializeColorBuffer(size);
            Processing(models);
        }

        public Bitmap Image
        {
            get { return bitmap; }
        }

        #region Initialize

        private void InitializeZbuffer(Bitmap bitmap)
        {
            zBuffer = new IntBuffer(bitmap.Height, bitmap.Width, minLimitZ);
        }

        private void InitializeZbuffer(Size size)
        {
            zBuffer = new IntBuffer(size.Height, size.Width, minLimitZ);
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

        private void Processing(List<Model> models)
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
            for (int y = 0; y < zBuffer.Rows; y++)
            {
                foreach (Polygon polygon in model.Polygons)
                {
                    for (int x = 0; x < zBuffer.Columns; x++)
                    {
                        int z = (int)polygon.Z(x, y);
                        if (polygon.IsInside(x, y, z))
                            ProcessPoint(x, y, z, model.Color);
                    }
                }
            }
        }

        private void ProcessPoint(int x, int y, int z, Color color)
        {
            if (z > zBuffer[y, x])
            {
                zBuffer[y, x] = z;
                colorBuffer[y][x] = color == Color.Empty ? Color.Black : color;
            }
        }

        #endregion
    }
}
