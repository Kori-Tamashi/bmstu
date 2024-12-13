using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace code
{
    class ViewingFrustum_SolidShading : ViewingFrustum_ParallelZBuffer
    {
        public ViewingFrustum_SolidShading(float view_field_width, float view_field_height, float near_plane_distance, float far_plane_distance,
            Camera camera) : base(view_field_width, view_field_height, near_plane_distance, far_plane_distance, camera) { }

        public void ProcessingShading(Scene scene)
        {
            Processing(scene);
        }

        public void ProcessingShading(List<Model> models, Light light)
        {
            Processing(models, light);
        }

        public void ProcessingShading(Model model, Light light)
        {
            Processing(model, light);
        }

        public new void Processing(Scene scene)
        {
            ClearView();
            ProcessModels(scene.Models, scene.CurrentLight);
            ProcessBitmap();
        }

        public void Processing(List<Model> models, Light light)
        {
            ClearView();
            ProcessModels(models, light);
            ProcessBitmap();
        }

        public void Processing(Model model, Light light)
        {
            ClearView();
            ProcessModel(model, light);
            ProcessBitmap();
        }

        protected void ProcessModels(List<Model> models, Light light)
        {
            foreach (Model model in models)
            {
                ProcessModel(model, light);
            }
        }

        protected void ProcessModel(Model model, Light light)
        {
            List<Polygon> clippedPolygons = InvisibleFaceDeletor.ProcessModel(model, camera);

            foreach (Polygon polygon in clippedPolygons)
            {
                ProcessPolygon(polygon, model.Material, model.Color, light);
            }
        }

        protected void ProcessPolygon(Polygon polygon, Material material, Color color, Light light)
        {
            float intensity = GetIntensity(polygon, material, light);

            foreach (Point3D point in polygon.InsidePoints)
            {
                ProcessPoint(point, color, intensity);
            }
        }

        protected void ProcessPoint(Point3D worldPoint, Color color, float intensity)
        {
            Point3D viewingFrustumPoint = ViewingFrustumPoint(worldPoint);
            Point viewPortPoint = ViewPortPointByViewingFrustumPoint(viewingFrustumPoint);

            if (viewingFrustumPoint.Z > zBufferModels[viewPortPoint.Y, viewPortPoint.X])
            {
                zBufferModels[viewPortPoint.Y, viewPortPoint.X] = viewingFrustumPoint.Z;
                colorBufferModels[viewPortPoint.Y][viewPortPoint.X] = (color == Color.Empty) ? _Color(Color.Black, intensity) : _Color(color, intensity);
            }
        }

        protected Color _Color(Color color, float intensity)
        {
            return Color.FromArgb(
                    (int)(Math.Max(0, Math.Min(color.A * intensity, 255))),
                    (int)(Math.Max(0, Math.Min(color.R * intensity, 255))),
                    (int)(Math.Max(0, Math.Min(color.G * intensity, 255))),
                    (int)(Math.Max(0, Math.Min(color.B * intensity, 255))));
        }

        protected float GetIntensity(Polygon polygon, Material material, Light light)
        {
            Vector3D normal = polygon.Normal().NormalizedCopy();

            float R_z = 2 * normal.Z * normal.Z - 1;
            float R_x = 2 * normal.Z * normal.X;
            float R_y = 2 * normal.Z * normal.Y;
            Vector3D reflection = new Vector3D(R_x, R_y, R_z);

            float intensity = material.I_a * material.k_a + (light.Intensity / (material.d + material.K)) * (
                    material.k_d * Vector3D.DotProduct(light, normal) +
                    material.k_s * (float)Math.Pow(
                        Vector3D.DotProduct(reflection, camera.Direction), material.n)
                    );

            return Math.Abs(intensity);
        }
    }
}
