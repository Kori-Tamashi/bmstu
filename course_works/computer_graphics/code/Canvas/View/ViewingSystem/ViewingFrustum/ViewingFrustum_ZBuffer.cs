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


namespace code
{
    class ViewingFrustum_ZBuffer : ViewingFrustum
    {
        protected const int minLimitZ = 10000;

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

        public FloatBuffer ZBuffer
        {
            get { return zBufferModels; }
        }

        #endregion

        #region Processing

        public virtual void Processing(List<Model> models)
        {
            ProcessModels(models);
            ProcessBitmap();
        }

        public virtual void Processing(Model model)
        {
            ProcessModel(model);
            ProcessBitmap();
        }

        protected virtual void ProcessBitmap()
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

                Parallel.For(0, height, y =>
                {
                    for (int x = 0; x < width; x++)
                    {
                        int offset = (y * data.Stride) + (x * 4);

                        ptr[offset] = colorBufferModels[y][x].B;
                        ptr[offset + 1] = colorBufferModels[y][x].G;
                        ptr[offset + 2] = colorBufferModels[y][x].R;
                        ptr[offset + 3] = colorBufferModels[y][x].A;
                    }
                });
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
            List<Polygon> visiblePolygons = InvisibleFaceDeletor.ProcessModel(model, camera.Direction);

            foreach (Polygon polygon in visiblePolygons)
            {
                ProcessPolygon(polygon, model.Color);
            }
        }

        protected virtual void ProcessPolygon(Polygon polygon, Color color)
        {
            foreach (Point3D point in polygon.InsidePoints)
            {
                Point viewPortPoint = ViewPortPoint(point);
                Point3D viewingFrustumPoint = ViewingFrustumPoint(point);
                ProcessPoint(viewingFrustumPoint, viewPortPoint, color);
            }
        }

        protected virtual void ProcessPoint(Point3D viewingFrustumPoint, Point viewPortPoint, Color color)
        {
            if (viewingFrustumPoint.Z < zBufferModels[viewPortPoint.Y, viewPortPoint.X])
            {
                zBufferModels[viewPortPoint.Y, viewPortPoint.X] = viewingFrustumPoint.Z;
                colorBufferModels[viewPortPoint.Y][viewPortPoint.X] = (color == Color.Empty) ? Color.Black : color;
            }
        }

        #endregion
    }
}
