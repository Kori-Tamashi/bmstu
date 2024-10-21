using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace code
{
    class ViewingFrustum_Shadows : ViewingFrustum_ParallelSolidShading
    {
        ViewingFrustum_ParallelZBuffer shadowsCamera;

        public ViewingFrustum_Shadows(float view_field_width, float view_field_height, float near_plane_distance, float far_plane_distance,
            Camera camera) : base(view_field_width, view_field_height, near_plane_distance, far_plane_distance, camera) { }

        public void ProcessingShadows(Scene scene)
        {
            Processing(scene);
        }

        public void ProcessingShadows(List<Model> models, Light light)
        {
            Processing(models, light);
        }

        public new void Processing(Scene scene)
        {
            ClearView();
            ProcessShadows(scene.Models, scene.CurrentLight);
            ProcessModels(scene.Models, scene.CurrentLight);
            ProcessBitmap();
        }

        public new void Processing(List<Model> models, Light light)
        {
            ClearView();
            ProcessShadows(models, light);
            ProcessModels(models, light);
            ProcessBitmap();
        }

        protected void ProcessShadows(List<Model> models, Light light)
        {
            if (shadowsCamera == null)
            {
                shadowsCamera = new ViewingFrustum_ParallelZBuffer(
                    view_field_width,
                    view_field_height,
                    near_plane_distance,
                    far_plane_distance,
                    light.Camera
                );
            }
            else
            {
                shadowsCamera.Camera = light.Camera;
            }

            shadowsCamera.Processing(models);
        }

        protected new void ProcessPolygon(Polygon polygon, Material material, Color color, Light light)
        {
            float intensity = GetIntensity(polygon, material, light);

            Parallel.ForEach(polygon.InsidePoints, point =>
            {
                ProcessPoint(point, color, intensity);
            });
        }

        protected new void ProcessPoint(Point3D worldPoint, Color color, float intensity)
        {
            // DON'T. TOUCH. THIS.

            Point3D viewingFrustumPoint = ViewingFrustumPoint(worldPoint);
            Point viewPortPoint = ViewPortPointByViewingFrustumPoint(viewingFrustumPoint);

            if (viewingFrustumPoint.Z < zBufferModels[viewPortPoint.Y, viewPortPoint.X])
            {
                Point3D lightViewPoint = shadowsCamera.ViewingFrustumPoint(worldPoint);
                Point lightViewPortPoint = shadowsCamera.ViewPortPointByViewingFrustumPoint(lightViewPoint);
                
                if (lightViewPoint.Z > shadowsCamera.ZBuffer[lightViewPortPoint.Y, lightViewPortPoint.X])
                {
                    zBufferModels[viewPortPoint.Y, viewPortPoint.X] = lightViewPoint.Z;
                    colorBufferModels[viewPortPoint.Y][viewPortPoint.X] = (color == Color.Empty) ? _ColorMix(Color.Black, Color.Gray, 0.4f) : _ColorMix(Color.Black, color, 0.4f);
                }
                else
                {
                    zBufferModels[viewPortPoint.Y, viewPortPoint.X] = viewingFrustumPoint.Z;
                    colorBufferModels[viewPortPoint.Y][viewPortPoint.X] = (color == Color.Empty) ? _Color(Color.Black, intensity) : _Color(color, intensity);
                }
            }
        }

        public static Color _ColorMix(Color a, Color b, float aPers)
        {
            aPers = Math.Min(aPers, 1);

            int red = (int)(a.R * aPers + b.R * (1 - aPers));
            int green = (int)(a.G * aPers + b.G * (1 - aPers));
            int blue = (int)(a.B * aPers + b.B * (1 - aPers));

            return Color.FromArgb(red, green, blue);
        }
    }
}
