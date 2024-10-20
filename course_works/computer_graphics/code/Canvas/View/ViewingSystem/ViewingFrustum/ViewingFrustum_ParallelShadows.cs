using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace code
{
    class ViewingFrustum_ParallelShadows : ViewingFrustum_Shadows
    {
        public ViewingFrustum_ParallelShadows(float view_field_width, float view_field_height, float near_plane_distance, float far_plane_distance,
            Camera camera) : base(view_field_width, view_field_height, near_plane_distance, far_plane_distance, camera) { }

        protected override void ProcessModels(List<Model> models, Light light)
        {
            Parallel.ForEach(models, model =>
            {
                ProcessModel(model, light);
            });
        }

        protected override void ProcessModel(Model model, Light light)
        {
            List<Polygon> clippedPolygons = InvisibleFaceDeletor.ProcessModel(model, camera.Direction);

            Parallel.ForEach(clippedPolygons, polygon =>
            {
                ProcessPolygon(polygon, model.Material, model.Color, light);
            });
        }

        protected override void ProcessPolygon(Polygon polygon, Material material, Color color, Light light)
        {
            float intensity = GetIntensity(polygon, material, light);

            Parallel.ForEach(polygon.InsidePoints, point =>
            {
                ProcessPoint(point, color, intensity);
            });
        }

    }
}
