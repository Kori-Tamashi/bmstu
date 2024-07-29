using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace code
{
    class Centering
    {
        public Move move;
        public Scale scale;

        float scaleK = (float) 0.75; 

        public Centering(Point3D srcCenter, Point3D dstCenter, Size dstSize)
        {
            float diffX = dstCenter.X - srcCenter.X;
            float diffY = dstCenter.Y - srcCenter.Y;
            float diffZ = dstCenter.Z - srcCenter.Z;

            float dstWidth = dstSize.Width;
            float dstHeight = dstSize.Height;
            float dstLength = (dstWidth + dstHeight) / 2;

            move = new Move(diffX, diffY, diffZ);
            scale = new Scale(1, 1, 1);
        }
    }
}
