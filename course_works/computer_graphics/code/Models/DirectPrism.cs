using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;


namespace code
{
    class DirectPrism : Model
    {
        public DirectPrism() 
        {
            this.points = new List<Point3D> {
                new Point3D(0, 0, 0), // 0
                new Point3D(100, 0, 0), // 1
                new Point3D(100, 100, 0), // 2
                new Point3D(0, 100, 0), // 3
                new Point3D(0, 0, 200), // 4
                new Point3D(100, 0, 200), // 5
                new Point3D(100, 100, 200), // 6
                new Point3D(0, 100, 200)  // 7
            };

            this.indexes = new List<int> {
                0, 1,
                1, 2,
                2, 3,
                3, 0,
                4, 5,
                5, 6,
                6, 7,
                7, 4,
                0, 4,
                1, 5,
                2, 6,
                3, 7
            };

            this.type = "Direct prism";

            ConstructCenter(this.points);
            ConstructEdges(this.points, this.indexes);
        }
    }
}
