using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace code
{
    class ParallelZBuffer : ZBuffer
    {
        public ParallelZBuffer(Size size, List<Model> models) : base(size, models) { }

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

            // Параллельная обработка каждого полигона модели
            Parallel.ForEach(visiblePolygons, polygon =>
            {
                ProcessPolygon(polygon, model.Color);
            });
        }

        protected override void ProcessPolygon(Polygon polygon, Color modelColor)
        {
            // Используем Parallel.For для итерации по строкам
            Parallel.For(0, zBufferModels.Rows, y =>
            {
                for (int x = 0; x < zBufferModels.Columns; x++)
                {
                    int z = (int)polygon.Z(x, y);
                    if (polygon.IsInside(x, y, z))
                        ProcessPoint(x, y, z, modelColor);
                }
            });
        }

        protected override void ProcessPoint(int x, int y, int z, Color color)
        {
            lock (zBufferModels)
            {
                base.ProcessPoint(x, y, z, color);
            }
        }
    }
}
