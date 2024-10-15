using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;

using IntBuffer = code.Matrix<int>;
using ColorBuffer = System.Drawing.Color[][];

namespace code
{
    class ViewingFrustum_ZBuffer : ViewingFrustum
    {
        protected const int minLimitZ = 10000;

        protected Bitmap bitmap;
        protected Size viewPortSize;
        protected IntBuffer zBufferModels;
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

        protected virtual void ProcessPolygon(Polygon polygon, Graphics gr)
        {
            //Polygon clippedPolygon = Clipping.ClipPolygon(planes, polygon);

            foreach (Edge edge in polygon.Edges)
            {
                Point start = ViewPortPoint(edge.start);
                Point end = ViewPortPoint(edge.end);

                gr.DrawLine(new Pen(Color.Black), start, end);
            }
        }
    }
}
