using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;


namespace code
{
    class Pyramid : Model
    {
        public Pyramid() 
        {
            this.points = new List<Point3D> {
                new Point3D(0, 0, 0),                   // 0
                new Point3D((float)50, (float)Math.Sqrt(3) / 2 * 100, 0),  // 1
                new Point3D(100, 0, 0),                   // 2
                new Point3D((float)50, (float)Math.Sqrt(3) / 6 * 100, 100)   // 3
            };

            this.indexes = new List<int>() {
                0, 1,
                1, 2,
                2, 0,
                0, 3,
                1, 3,
                2, 3
            };

            ConstructCenter(this.points);
            ConstructEdges(this.points, this.indexes);
        }
    }
}
