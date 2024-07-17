using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace code
{
    class Move : Transformation
    {

        public Move(float dx, float dy, float dz)
        {
            this.matrix = new TransformationMatrix(new float[4, 4] {
                    {1,       0,       0,       0},
                    {0,       1,       0,       0},
                    {0,       0,       1,       0},
                    {dx,      dy,      dz,      1}
                }
            );
        }

        public Move(Point3D point)
        {
            this.matrix = new TransformationMatrix(new float[4, 4] {
                    {1,       0,       0,       0},
                    {0,       1,       0,       0},
                    {0,       0,       1,       0},
                    {point.X, point.Y, point.Z, 1}
                }
            );
        }
    }
}
