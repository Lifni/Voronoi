using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoronoiDiagram.Structs;

namespace VoronoiDiagram
{
    public class ConvexHull
    {
        public List<Point> Generate(List<Point> points)
        {
            return this.GrahamAndrew(points);
        }

        private List<Point> GrahamAndrew(List<Point> points)
        {
            if (points.Count == 1)
            {
                return points;
            }
            points = points.OrderBy(i => i.x).ThenBy(i => i.y).ToList();
            Point p1 = points[0];
            Point p2 = points[points.Count - 1];
            List<Point> up = new List<Point>();
            List<Point> down = new List<Point>();
            up.Add(p1);
            down.Add(p1);

            for (int i = 1; i < points.Count; i++)
            {
                int fl = this.PositionPoint(p1, points[i], p2);
                if (i == points.Count - 1 || fl == -1)
                {
                    while (up.Count >= 2 && this.PositionPoint(up[up.Count - 2], up[up.Count - 1], points[i]) != -1)
                        up.Remove(up[up.Count - 1]);
                    up.Add(points[i]);
                }
                if (i == points.Count - 1 || fl == 1)
                {
                    while (down.Count >= 2 && this.PositionPoint(down[down.Count - 2], down[down.Count - 1], points[i]) != 1)
                        down.Remove(down[down.Count - 1]);
                    down.Add(points[i]);
                }
            }
            points = new List<Point>(up);
            down.Remove(down[0]);
            down.Reverse();
            down.Remove(down[0]);
            points.AddRange(down);
            return points;
        }

        private int PositionPoint(Point a, Point b, Point c)
        {
            double D = a.x * (b.y - c.y) + b.x * (c.y - a.y) + c.x * (a.y - b.y);
            if (D == 0) return 0;
            return D > 0 ? 1 : -1;
        }
    }
}
