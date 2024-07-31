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

        float scalek = (float)0.5;

        public Centering(Model model, Point3D dstCenter, Size dstSize)
        {
            float diffX = dstCenter.X - model.center.X;
            float diffY = dstCenter.Y - model.center.Y;
            float diffZ = dstCenter.Z - model.center.Z;

            float dstWidth = dstSize.Width;
            float dstHeight = dstSize.Height;
            float dstLength = (dstWidth + dstHeight) / 2;

            float lengthK = dstLength / model.length;
            float widthK = model.width != -1 ? dstWidth / model.width : lengthK;
            float heightK = model.height != -1 ? dstHeight / model.height : lengthK;

            lengthK *= scalek;
            widthK *= scalek;
            heightK *= scalek;

            move = new Move(diffX, diffY, diffZ);
            scale = new Scale(lengthK, widthK, heightK);
        }
    }
}
