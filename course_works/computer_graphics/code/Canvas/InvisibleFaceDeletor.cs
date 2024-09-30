using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace code
{
    class InvisibleFaceDeletor
    {
        static public List<Polygon> ProcessModel(Model model)
        {
            Matrix<float> modelMatrix = model.Matrix;
            Matrix<float> supervisor = new Matrix<float>(1, 4, 0);
            supervisor[0, 2] = -1;

            Matrix<float> sMultiplication = supervisor * modelMatrix;

            List<Polygon> visiblePolygons = new List<Polygon>();
            for (int i = 0; i < model.Polygons.Count; i++)
            {
                if (sMultiplication[0, i] > 0)
                    visiblePolygons.Add(model.Polygons[i]);
            }

            return visiblePolygons;
        }
    }
}
