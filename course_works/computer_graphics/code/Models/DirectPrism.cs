﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace code
{
    class DirectPrism : Model
    {
        public DirectPrism() 
        {
            this.points = new List<Point3D> {
                new Point3D(0, 0, 0), // 0
                new Point3D(1, 0, 0), // 1
                new Point3D(1, 1, 0), // 2
                new Point3D(0, 1, 0), // 3
                new Point3D(0, 0, 2), // 4
                new Point3D(1, 0, 2), // 5
                new Point3D(1, 1, 2), // 6
                new Point3D(0, 1, 2)  // 7
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

            ConstructEdges(this.points, this.indexes);
        }
    }
}