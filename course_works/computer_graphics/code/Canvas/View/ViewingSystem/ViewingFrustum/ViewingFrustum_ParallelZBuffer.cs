using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace code
{
    class ViewingFrustum_ParallelZBuffer : ViewingFrustum_ZBuffer
    {
        public ViewingFrustum_ParallelZBuffer(float view_field_width, float view_field_height, float near_plane_distance, float far_plane_distance,
            Camera camera) : base(view_field_width, view_field_height, near_plane_distance, far_plane_distance, camera) { }

        #region Processing

        protected override void ProcessModels(List<Model> models)
        {
            Parallel.ForEach(models, model =>
            {
                ProcessModel(model);
            });
        }

        protected override void ProcessModel(Model model)
        {
            List<Polygon> visiblePolygons = InvisibleFaceDeletor.ProcessModel(model, camera.Direction);

            Parallel.ForEach(visiblePolygons, polygon =>
            {
                ProcessPolygon(polygon, model.Color);
            });
        }

        protected override void ProcessPolygon(Polygon polygon, Color color)
        {
            Parallel.ForEach(polygon.InsidePoints, point =>
            {
                Point viewPortPoint = ViewPortPoint(point);
                Point3D viewingFrustumPoint = ViewingFrustumPoint(point);
                ProcessPoint(viewingFrustumPoint, viewPortPoint, color);
            });
        }

        protected override void ProcessPoint(Point3D viewingFrustumPoint, Point viewPortPoint, Color color)
        {
            lock (zBufferModels)
            {
                base.ProcessPoint(viewingFrustumPoint, viewPortPoint, color);
            }
        }

        #endregion
    }
}
