using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace code
{
    class Clipping
    {
        static public Polygon ClipPolygon(List<Polygon> planes, Polygon polygon)
        {
            Polygon result = polygon;

            foreach (Polygon plane in planes)
            {
                result = ClipPolygonByPlane(plane, result);
            }

            return result;
        }

        static private Polygon ClipPolygonByPlane(Polygon plane, Polygon polygon)
        {
            int first = polygon.Points.Count - 1, second;
            List<Point3D> resultPoints = new List<Point3D>(polygon.Points.Count);

            for (int i = 0; i < polygon.Points.Count; i++)
            {
                second = i;

                Point3D start = polygon.Points[first];
                Point3D end = polygon.Points[second];

                if (PointIsVisible(plane, end))
                {
                    if (PointIsVisible(plane, start))
                    {
                        resultPoints.Add(end);
                    }
                    else
                    {
                        resultPoints.Add(Intersection(plane, start, end));
                        resultPoints.Add(end);
                    }
                }
                else
                {
                    if (PointIsVisible(plane, start))
                    {
                        resultPoints.Add(Intersection(plane, start, end));
                    }
                }

                first = second;
            }

            return new Polygon(resultPoints);
        }

        static private bool PointIsVisible(Polygon plane, Point3D point)
        {
            Vector4D vecPlane = new Vector4D(plane);
            Vector4D vecPoint = new Vector4D(point);

            return Vector4D.DotProduct(vecPlane, vecPoint) > -1e-12f;
        }

        static private Point3D Intersection(Polygon plane, Point3D start, Point3D end)
        {
            Vector4D vecPlane = new Vector4D(plane);
            Vector4D vecStart = new Vector4D(start);
            Vector4D vecEnd = new Vector4D(end);

            float dStart = Vector4D.DotProduct(vecPlane, vecStart);
            float dEnd = Vector4D.DotProduct(vecPlane, vecEnd);
            float t = (float)( dStart / (dStart - dEnd) );

            Vector4D p = vecStart + (vecEnd - vecStart) * t;

            return new Point3D(p.X, p.Y, p.Z);
        }
    }
}
