using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace code
{
    class Pyramid : Model
    {
        public Pyramid() 
        {
            this.points = new List<Point3D> {
                new Point3D(0, 0, 0),                   // 0
                new Point3D(0.5, Math.Sqrt(3) / 2, 0),  // 1
                new Point3D(1, 0, 0),                   // 2
                new Point3D(0.5, Math.Sqrt(3) / 6, 1)   // 3
            };

            this.indexes = new List<int>() {
                0, 1,
                1, 2,
                2, 0,
                0, 3,
                1, 3,
                2, 3
            };

            ConstructEdges(this.points, this.indexes);
        }
    }
}
