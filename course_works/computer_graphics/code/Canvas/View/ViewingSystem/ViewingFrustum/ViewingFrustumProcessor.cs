using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace code
{
    class ViewingFrustumProcessor : ViewingFrustum_ParallelShadows
    {
        public ViewingFrustumProcessor(float view_field_width, float view_field_height, float near_plane_distance, float far_plane_distance,
            Camera camera) : base(view_field_width, view_field_height, near_plane_distance, far_plane_distance, camera) { }

        public virtual void Processing(Scene scene, Graphics gr, RenderMode renderMode)
        {
            Processing(scene.Models, scene.CurrentLight, gr, renderMode);
        }

        public virtual void Processing(List<Model> models, Light light, Graphics gr, RenderMode renderMode)
        {
            switch (renderMode)
            {
                case RenderMode.Shadows:
                    ProcessingShadows(models, light);
                    break;
                case RenderMode.Shading:
                    ProcessingShading(models, light); 
                    break;
                case RenderMode.RealDisplay:
                    ProcessingRealDisplay(models);
                    break;
                case RenderMode.CarcassDisplay:
                    ProcessingCarcassDisplay(models, gr);
                    break;
                default:
                    break;
            }
        }

        public new void ProcessingShadows(Scene scene)
        {
            base.ProcessingShadows(scene);
        }

        public new void ProcessingShading(Scene scene)
        {
            base.ProcessingShading(scene);
        }

        public void ProcessingRealDisplay(Scene scene)
        {
            base.ProcessingZBuffer(scene);
        }

        public void ProcessingCarcassDisplay(Scene scene, Graphics gr)
        {
            base.ProcessingGraphics(scene, gr);
        }

        public new void ProcessingShadows(List<Model> models, Light light)
        {
            base.ProcessingShadows(models, light);
        }

        public new void ProcessingShading(List<Model> models, Light light)
        {
            base.ProcessingShading(models, light);
        }

        public void ProcessingRealDisplay(List<Model> models)
        {
            base.ProcessingZBuffer(models);
        }

        public void ProcessingCarcassDisplay(List<Model> models, Graphics gr)
        {
            base.ProcessingGraphics(models, gr);
        }

    }
}
