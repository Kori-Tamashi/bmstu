using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace code
{
    public class Scale : Transformation
    {
        public Scale(float kx, float ky, float kz) 
        {
            if (kx == 0 || ky == 0 || kz == 0)
                throw new ArgumentException();

            matrix = new TransformationMatrix(new float[4, 4] {
                    {kx,      0,       0,      0},
                    {0,       ky,      0,      0},
                    {0,       0,       kz,     0},
                    {0,       0,       0,      1}
                }
            );
        }

        static public void Transform(Scale scale, Point3D point)
        {
            Transformation.Transform(scale, point);
        }

        static public void Transform(Scale scale, Point3D point, Point3D center)
        {
            Transformation.Transform(scale, point, center);
        }
    }
}
