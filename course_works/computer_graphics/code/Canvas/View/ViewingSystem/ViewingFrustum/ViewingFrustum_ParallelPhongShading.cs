﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace code
{
    class ViewingFrustum_ParallelPhongShading : ViewingFrustum_PhongShading
    {
        public ViewingFrustum_ParallelPhongShading(float view_field_width, float view_field_height, float near_plane_distance, float far_plane_distance,
            Camera camera) : base(view_field_width, view_field_height, near_plane_distance, far_plane_distance, camera) { }

        public new void ProcessingShading(Scene scene)
        {
            Processing(scene);
        }

        public new void ProcessingShading(List<Model> models, Light light)
        {
            Processing(models, light);
        }

        public new void ProcessingShading(Model model, Light light)
        {
            Processing(model, light);
        }

        public new void Processing(Scene scene)
        {
            ClearView();
            ProcessModels(scene.Models, scene.CurrentLight);
            ProcessBitmap();
        }

        public new void Processing(List<Model> models, Light light)
        {
            ClearView();
            ProcessModels(models, light);
            ProcessBitmap();
        }

        public new void Processing(Model model, Light light)
        {
            ClearView();
            ProcessModel(model, light);
            ProcessBitmap();
        }

        protected new void ProcessModels(List<Model> models, Light light)
        {
            Parallel.ForEach(models, model =>
            {
                ProcessModel(model, light);
            });
        }

        protected new void ProcessModel(Model model, Light light)
        {
            List<Polygon> clippedPolygons = InvisibleFaceDeletor.ProcessModel(model, camera);

            Parallel.ForEach(model.Polygons, polygon =>
            {
                ProcessPolygon(polygon, model.Material, model.Color, light);
            });
        }

        protected new void ProcessPolygon(Polygon polygon, Material material, Color color, Light light)
        {
            Vector3D polygonNormal = polygon.Normal();

            Parallel.ForEach(polygon.InsidePoints, point =>
            {
                ProcessPoint(point, color, material, polygonNormal, light);
            });
        }
    }
}
