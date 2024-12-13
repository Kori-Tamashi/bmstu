using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace code
{
    class ViewingFrustum_PhongShading : ViewingFrustum_ParallelZBuffer
    {
        public ViewingFrustum_PhongShading(float view_field_width, float view_field_height, float near_plane_distance, float far_plane_distance,
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

            foreach (Polygon polygon in model.Polygons)
            {
                ProcessPolygon(polygon, model.Material, model.Color, light);
            }
        }

        
        protected void ProcessPolygon(Polygon polygon, Material material, Color color, Light light)
        {
            Vector3D polygonNormal = polygon.Normal();

            foreach (Point3D point in polygon.InsidePoints)
            {
                ProcessPoint(point, color, material, polygonNormal, light);
            }
        }

        protected void ProcessPoint(Point3D worldPoint, Color color, Material material, Vector3D polygonNormal, Light light)
        {
            Point3D viewingFrustumPoint = ViewingFrustumPoint(worldPoint);
            Point viewPortPoint = ViewPortPointByViewingFrustumPoint(viewingFrustumPoint);

            if (viewingFrustumPoint.Z > zBufferModels[viewPortPoint.Y, viewPortPoint.X])
            {
                float intensity = GetIntensity(worldPoint, polygonNormal, material, light);
                zBufferModels[viewPortPoint.Y, viewPortPoint.X] = viewingFrustumPoint.Z;
                colorBufferModels[viewPortPoint.Y][viewPortPoint.X] = (color == Color.Empty) ? _Color(Color.Black, intensity) : _Color(color, intensity);
            }
        }

        protected Color _Color(Color color, float intensity)
        {
            return Color.FromArgb(
                (int)Math.Clamp(color.A * intensity, 0, 255), // Альфа
                (int)Math.Clamp(color.R * intensity, 0, 255), // Красный
                (int)Math.Clamp(color.G * intensity, 0, 255), // Зеленый
                (int)Math.Clamp(color.B * intensity, 0, 255)  // Синий
            );
        }

        protected float GetIntensity(Point3D point, Vector3D polygonNormal, Material material, Light light)
        {
            Vector3D N = polygonNormal; // вектор нормали к поверхности

            Vector3D L = light.Direction;     // вектор направления света
            Vector3D R = 2 * Vector3D.DotProduct(N, L) * N - L;  // вектор отраженного луча
            Vector3D V = new Vector3D(point, light.Position);   // вектор от точки до зрителя

            N.Normalize();
            L.Normalize();
            R.Normalize();
            V.Normalize();

            float I_o = light.Intensity; // интенсивность источника света
            float I_p = I_o / 10;        // интенсивность рассеянного освещения
            float K_p = 0.9f;            // коэффициент рассеянного освещения   
            float K_d = material.k_d;    // коэффициент диффузного освещения
            float K_m = material.k_m;    // коэффициент зеркального освещения
            float a = material.a;        // коэффициент блеска

            float ambient = (I_p * K_p);
            float diffuse = (float)(I_o * K_d * Vector3D.DotProduct(L, N));
            float specular = (float)(I_o * K_m * Math.Pow(Vector3D.DotProduct(R, V), a));

            float intensity = ambient + diffuse + specular;

            return Math.Clamp(intensity, 0, 1);
        }
    }
}
