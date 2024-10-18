using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace code
{
    class InvisibleFaceDeletor
    {
        static public List<Polygon> ProcessModel(Model model, Vector3D supervisor)
        {
            Matrix<float> modelMatrix = model.Matrix;
            Matrix<float> supervisorMatrix = new Matrix<float>(1, 4, 0);
            supervisorMatrix[0, 0] = supervisor.X;
            supervisorMatrix[0, 1] = supervisor.Y;
            supervisorMatrix[0, 2] = supervisor.Z;

            Matrix<float> sMultiplication = supervisorMatrix * modelMatrix;

            List<Polygon> visiblePolygons = new List<Polygon>();
            for (int i = 0; i < model.Polygons.Count; i++)
            {
                if (sMultiplication[0, i] < 0)
                    visiblePolygons.Add(model.Polygons[i]);
            }

            return visiblePolygons;
        }

        static public List<Polygon> ProcessModel(Model model)
        {
            return ProcessModel(model, new Vector3D(0, 0, -1));
        }
    }
}
