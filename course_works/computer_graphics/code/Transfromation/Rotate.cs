using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace code
{ 
    class Rotate : Transformation
    {
        public Rotate(float angleX, float angleY, float angleZ) 
        { 
            angleX *= (float)Math.PI / 180;
            angleY *= (float)Math.PI / 180;
            angleZ *= (float)Math.PI / 180;

            TransformationMatrix matrix_x = new TransformationMatrix(new float[4, 4] {
                    {1, 0,                       0,                        0},
                    {0, (float)Math.Cos(angleX), (float)-Math.Sin(angleX), 0},
                    {0, (float)Math.Sin(angleX), (float) Math.Cos(angleX), 0},
                    {0, 0,                       0,                        1}
                }
            );

            TransformationMatrix matrix_y = new TransformationMatrix(new float[4, 4] {
                    {(float)Math.Cos(angleY),  0, (float)Math.Sin(angleY), 0},
                    {0,                        1, 0,                       0},
                    {(float)-Math.Sin(angleY), 0, (float)Math.Cos(angleY), 0},
                    {0,                        0, 0,                       1}
                }
            );

            TransformationMatrix matrix_z = new TransformationMatrix(new float[4, 4] {
                    {(float)Math.Cos(angleZ),  (float)-Math.Sin(angleZ), 0, 0},
                    {(float)Math.Sin(angleZ),  (float) Math.Cos(angleZ), 0, 0},
                    {0,                        0,                        1, 0},
                    {0,                        0,                        0, 1}
                }
            );

            this.matrix = matrix_x * matrix_y * matrix_z;
        }
    }
}
