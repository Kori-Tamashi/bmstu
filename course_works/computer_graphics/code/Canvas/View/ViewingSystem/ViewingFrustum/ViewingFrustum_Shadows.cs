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

        public override void Processing(List<Model> models, Light light)
        {
            ProcessShadows(models, light);
            ProcessModels(models, light);
            ProcessBitmap();
        }

        protected virtual void ProcessShadows(List<Model> models, Light light)
        {
            shadowsCamera = new ViewingFrustum_ParallelZBuffer(
                view_field_width,
                view_field_height,
                near_plane_distance,
                far_plane_distance,
                light.Camera
            );

            shadowsCamera.Processing(models);
        }

        protected override void ProcessPolygon(Polygon polygon, Material material, Color color, Light light)
        {
            float intensity = GetIntensity(polygon, material, light);

            foreach (Point3D point in polygon.InsidePoints)
            {
                Point viewPortPoint = ViewPortPoint(point);
                Point3D viewingFrustumPoint = ViewingFrustumPoint(point);
                ProcessPoint(point, viewingFrustumPoint, viewPortPoint, color, intensity);
            }
        }

        protected virtual void ProcessPoint(Point3D worldPoint, Point3D viewingFrustumPoint, Point viewPortPoint, Color color, float intensity)
        {
            // Умоляю, не трогай эту часть кода...

            if (viewingFrustumPoint.Z < zBufferModels[viewPortPoint.Y, viewPortPoint.X])
            {
                Point lightViewPortPoint = shadowsCamera.ViewPortPoint(worldPoint);
                Point3D lightViewPoint = shadowsCamera.ViewingFrustumPoint(worldPoint);

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
