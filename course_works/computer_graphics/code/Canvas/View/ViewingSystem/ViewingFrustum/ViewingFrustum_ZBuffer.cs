using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using System.Windows.Forms.DataVisualization.Charting;

using FloatBuffer = code.Matrix<float>;
using ColorBuffer = System.Drawing.Color[][];
using System.Drawing.Imaging;
using System.Reflection;


namespace code
{
    class ViewingFrustum_ZBuffer : ViewingFrustum
    {
        protected const int minLimitZ = -10000;

        protected Bitmap bitmap;
        protected Size viewPortSize;
        protected FloatBuffer zBufferModels;
        protected ColorBuffer colorBufferModels;

        public ViewingFrustum_ZBuffer(float view_field_width, float view_field_height, float near_plane_distance, float far_plane_distance,
            Camera camera) : base(view_field_width, view_field_height, near_plane_distance, far_plane_distance, camera) 
        {
            InitializeSize(view_field_width, view_field_height);
            InitializeBitmap(viewPortSize);
            InitializeZbufferModels(viewPortSize);
            InitializeColorBufferModels(viewPortSize);
        }

        #region Initialize

        protected virtual void InitializeSize(float view_field_width, float view_field_height)
        {
            viewPortSize = new Size((int)view_field_width, (int)view_field_height);
        }

        protected virtual void InitializeZbufferModels(Size size)
        {
            zBufferModels = new FloatBuffer(size.Height, size.Width, minLimitZ);
        }

        protected virtual void InitializeBitmap(Size size)
        {
            bitmap = new Bitmap(size.Width, size.Height);
        }

        protected virtual void InitializeColorBufferModels(Size size)
        {
            colorBufferModels = new Color[size.Height][];

            for (int i = 0; i < size.Height; i++)
            {
                colorBufferModels[i] = new Color[size.Width];

                // Заполнение ряда цветом
                for (int j = 0; j < size.Width; j++)
                {
                    colorBufferModels[i][j] = Color.White;
                }
            }
        }

        #endregion

        #region Getters & Setters

        public Bitmap Image
        {
            get { return bitmap; }
        }

        public FloatBuffer ZBuffer
        {
            get { return zBufferModels; }
        }

        #endregion

        #region Processing

        public void ProcessingZBuffer(Scene scene)
        {
            Processing(scene);
        }
        public void ProcessingZBuffer(List<Model> models)
        {
            Processing(models);
        }

        public void Processing(Scene scene)
        {
            ClearView();
            ProcessModels(scene.Models);
            ProcessBitmap();
        }

        public void Processing(List<Model> models)
        {
            ClearView();
            ProcessModels(models);
            ProcessBitmap();
        }

        public void Processing(Model model)
        {
            ClearView();
            ProcessModel(model);
            ProcessBitmap();
        }

        protected void ProcessBitmap()
        {
            BitmapData data = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb
            );

            unsafe
            {
                byte* ptr = (byte*)data.Scan0;

                int width = bitmap.Width;
                int height = bitmap.Height;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
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

        protected void ProcessModels(List<Model> models)
        {
            foreach (Model model in models)
            {
                ProcessModel(model);
            }
        }

        protected void ProcessModel(Model model)
        {
            List<Polygon> visiblePolygons = InvisibleFaceDeletor.ProcessModel(model, camera);

            foreach (Polygon polygon in visiblePolygons)
            {
                ProcessPolygon(polygon, model.Color);
            }
        }

        protected void ProcessPolygon(Polygon polygon, Color color)
        {
            foreach (Point3D point in polygon.InsidePoints)
            {
                ProcessPoint(point, color);
            }
        }

        protected void ProcessPoint(Point3D worldPoint, Color color)
        {
            Point3D viewingFrustumPoint = ViewingFrustumPoint(worldPoint);
            Point viewPortPoint = ViewPortPointByViewingFrustumPoint(viewingFrustumPoint);

            if (viewingFrustumPoint.Z > zBufferModels[viewPortPoint.Y, viewPortPoint.X])
            {
                zBufferModels[viewPortPoint.Y, viewPortPoint.X] = viewingFrustumPoint.Z;
                colorBufferModels[viewPortPoint.Y][viewPortPoint.X] = (color == Color.Empty) ? Color.Black : color;
            }
        }

        #endregion

        protected void ClearView()
        {
            ClearBitmap();
            ClearColorBuffer();
            ClearZBufferModels();
        }

        private void ClearColorBuffer()
        {
            for (int i = 0; i < viewPortSize.Height; i++)
            {
                for (int j = 0; j < viewPortSize.Width; j++)
                {
                    colorBufferModels[i][j] = Color.White;
                }
            }
        }

        private void ClearZBufferModels()
        {
            for (int i = 0; i < viewPortSize.Height; i++)
            {
                for (int j = 0; j < viewPortSize.Width; j++)
                {
                    zBufferModels[i, j] = minLimitZ;
                }
            }
        }

        private void ClearBitmap()
        {
            BitmapData data = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb
            );

            unsafe
            {
                byte* ptr = (byte*)data.Scan0;

                int width = bitmap.Width;
                int height = bitmap.Height;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        int offset = (y * data.Stride) + (x * 4);

                        ptr[offset] = Color.White.B;
                        ptr[offset + 1] = Color.White.G;
                        ptr[offset + 2] = Color.White.R;
                        ptr[offset + 3] = Color.White.A;
                    }
                }
            }

            bitmap.UnlockBits(data);
        }
    }
}
