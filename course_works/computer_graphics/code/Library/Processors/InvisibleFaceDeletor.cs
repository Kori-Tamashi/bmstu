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

            List<Polygon> visiblePolygons = new List<Polygon>(model.Polygons.Count);
            for (int i = 0; i < model.Polygons.Count; i++)
            {
                if (sMultiplication[0, i] >= 0)
                    visiblePolygons.Add(model.Polygons[i]);
            }

            return visiblePolygons;
        }

        static public List<Polygon> ProcessModel(Model model)
        {
            return ProcessModel(model, new Vector3D(0, 0, -1));
        }

        static public List<Polygon> ProcessModel(Model model, Camera camera)
        {
            Vector3D supervisor = new Vector3D(camera.Direction);
            supervisor.X = (supervisor.X == 0) ? camera.Position.X : supervisor.X;
            supervisor.Y = (supervisor.Y != 0) ? -camera.Position.Y : supervisor.Y;
            supervisor.Z = (supervisor.Z == 0) ? -camera.Position.Z : supervisor.Z;
            supervisor.Normalize();

            List<Polygon> cameraPolygons = ProcessModel(model, camera.Direction);
            List<Polygon> supervisorPolygons = ProcessModel(model, supervisor);
            return cameraPolygons.Concat(supervisorPolygons).Distinct().ToList();
        }
    }
}
