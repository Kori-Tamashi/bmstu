﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace code.Objects
{
    public class Edge
    {
        public Point3D start;
        public Point3D end;

        public Edge(Point3D start, Point3D end)
        {
            this.start = start;
            this.end = end;
        }

        public Edge(Edge other)
        {
            start = new Point3D(other.start.X, other.start.Y, other.end.Z);
            end = new Point3D(other.end.X, other.end.Y, other.end.Z);
        }
    }
}
