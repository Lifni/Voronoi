using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoronoiDiagram.Structs
{
    public class Edge
    {
        public int id;

        public Center d0, d1;  // Delaunay edge
        public Center v0, v1;  // Voronoi edge
        public Point midpoint;  // halfway between v0,v1
        public Tuple<Point, Point> borders;

        public Edge(Center c1, Center c2)
        {
            id = StaticNums.id++;
            
            this.v0 = c1;
            this.v1 = c2;
            
            this.midpoint = new Point(Math.Round((c1.point.x + c2.point.x) / 2, StaticNums.round),
                                        Math.Round((c1.point.y + c2.point.y) / 2, StaticNums.round));
            if(c1.point.x == c2.point.x)
            {
                this.borders = new Tuple<Point, Point>(new Point(-StaticNums.limX, this.midpoint.y),
                                                        new Point(StaticNums.limX, this.midpoint.y));
                return;
            }
            if(c1.point.y == c2.point.y)
            {
                this.borders = new Tuple<Point, Point>(new Point(this.midpoint.x, -StaticNums.limY),
                                                        new Point(this.midpoint.x, StaticNums.limY));
                return;
            }
            double k = (c2.point.y - c1.point.y) / (c2.point.x - c1.point.x);
            Point start, end;
            double x = Math.Round(k * StaticNums.limY + this.midpoint.x + k * this.midpoint.y, StaticNums.round);
            if (Math.Abs(x) < StaticNums.limX)
            {
                start = new Point(Math.Round(k * StaticNums.limY + this.midpoint.x + k * this.midpoint.y, StaticNums.round),
                                        -StaticNums.limY);
                end = new Point(Math.Round(-k * StaticNums.limY + this.midpoint.x + k * this.midpoint.y, StaticNums.round),
                                        StaticNums.limY);
            }
            else
            {
                start = new Point(-StaticNums.limX,
                                Math.Round(1 / k * StaticNums.limX + 1 / k * this.midpoint.x + this.midpoint.y));
                end = new Point(StaticNums.limX,
                                Math.Round(- 1 / k * StaticNums.limX + 1 / k * this.midpoint.x + this.midpoint.y));
            }
            if(start.x > end.x)
            {
                Point temp = new Point(start);
                start = end;
                end = temp;
            }
            this.borders = new Tuple<Point, Point>(start, end);
        }

        public Edge(Center c1, Center c2, Tuple<Point, Point> borders)
        {
            id = StaticNums.id++;

            this.v0 = c1;
            this.v1 = c2;
            this.borders = borders;
            this.midpoint = new Point((borders.Item1.x + borders.Item2.x) / 2, (borders.Item1.y + borders.Item2.y) / 2);
        }
    }
}
