using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace code
{
    class ParallelSolidShading : SolidShading
    {
        public ParallelSolidShading(Size size, List<Model> models, List<Light> lights, Vector3D supervisor) : base(size, models, lights, supervisor) { }

        protected override void ProcessModels(List<Model> models)
        {
            Parallel.ForEach(models, model =>
            {
                ProcessModel(model);
            });
        }

        protected override void ProcessModel(Model model)
        {
            List<Polygon> visiblePolygons = InvisibleFaceDeletor.ProcessModel(model);

            Parallel.ForEach(visiblePolygons, polygon =>
            {
                ProcessPolygon(polygon, model.Material, model.Color);
            });
        }

        protected override void ProcessPolygon(Polygon polygon, Material modelMaterial, Color modelColor)
        {
            float intensity = GetIntensity(polygon, modelMaterial);

            Parallel.For(0, zBufferModels.Rows, y =>
            {
                for (int x = 0; x < zBufferModels.Columns; x++)
                {
                    int z = (int)polygon.Z(x, y);
                    if (polygon.IsInside(x, y, z))
                        ProcessPoint(x, y, z, intensity, modelColor);
                }
            });
        }

        protected override void ProcessPoint(int x, int y, int z, float intensity, Color modelColor)
        {
            lock (zBufferModels)
            {
                base.ProcessPoint(x, y, z, intensity, modelColor);
            }
        }
    }
}
