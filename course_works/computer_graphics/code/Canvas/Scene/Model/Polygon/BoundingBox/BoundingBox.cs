using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace code
{
    public class BoundingBox
    {
        public Point3D Min { get; set; }
        public Point3D Max { get; set; }

        public BoundingBox(Point3D min, Point3D max)
        {
            Min = min;
            Max = max;
        }
    }
}
