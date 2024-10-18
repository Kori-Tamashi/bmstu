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

        float scalek = (float)0.45;

        public Centering(Model model, Point3D dstCenter, Size dstSize)
        {
            float diffX = dstCenter.X - model.Center.X;
            float diffY = dstCenter.Y - model.Center.Y;
            float diffZ = dstCenter.Z - model.Center.Z;

            float dstLength = dstSize.Width;
            float dstHeight = dstSize.Height;
            float dstWidth = (dstLength + dstHeight) / 2;

            float lengthK = dstLength / model.Length;
            float widthK = model.Width != -1 ? dstWidth / model.Width : lengthK;
            float heightK = model.Height != -1 ? dstHeight / model.Height : (model.Radius != -1 ? dstHeight / 2 * model.Radius : lengthK);

            float resultK = Math.Min(Math.Min(lengthK, widthK), heightK) * scalek;

            move = new Move(diffX, diffY, diffZ);
            scale = new Scale(resultK, resultK, resultK);
        }
    }
}
