using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoronoiDiagram;
using VoronoiDiagram.Structs;

namespace VoronoiDiagramTests
{
    [TestClass]
    public class ConvexHullTests
    {
        private List<Point> points;

        [TestMethod]
        public void JustTest()
        {
            points = new List<Point>();
            /*points.Add(new Point(0, 0));
            points.Add(new Point(10, 0));
            points.Add(new Point(0, 10));
            points.Add(new Point(10, 10));*/
            /*points.Add(new Point(0, 0));
            points.Add(new Point(1, 0));
            points.Add(new Point(2, 0));
            points.Add(new Point(3, 0));
            points.Add(new Point(4, 0));
            points.Add(new Point(0, 1));
            points.Add(new Point(1, 1));
            points.Add(new Point(2, 1));
            points.Add(new Point(3, 1));
            points.Add(new Point(4, 1));
            points.Add(new Point(0, 2));
            points.Add(new Point(1, 2));
            points.Add(new Point(2, 2));
            points.Add(new Point(3, 2));
            points.Add(new Point(4, 2));
            points.Add(new Point(0, 3));
            points.Add(new Point(1, 3));
            points.Add(new Point(2, 3));
            points.Add(new Point(3, 3));
            points.Add(new Point(4, 3));
            points.Add(new Point(0, 4));
            points.Add(new Point(1, 4));
            points.Add(new Point(2, 4));
            points.Add(new Point(3, 4));
            points.Add(new Point(4, 4));
            */

            points.Add(new Point(20, 100));
            points.Add(new Point(25, 80));
            points.Add(new Point(25, 70));
            points.Add(new Point(35, 70));

            List<Point> ch2 = new List<Point>();
            points.Add(new Point(40, 300));
            points.Add(new Point(50, 10));
            points.Add(new Point(40, 30));
            points.Add(new Point(35, 70));
            /*points.Add(new Point(0, 1));
            points.Add(new Point(4, 1));
            points.Add(new Point(0, 2));
            points.Add(new Point(4, 2));
            points.Add(new Point(0, 3));
            points.Add(new Point(4, 3));
            points.Add(new Point(0, 4));
            points.Add(new Point(1, 4));
            points.Add(new Point(2, 4));
            points.Add(new Point(3, 4));
            points.Add(new Point(4, 4));*/

            Tuple<Point, Point> up = new Tuple<Point, Point>(points[0], ch2[0]);
            Voronoi vor = new Voronoi();
            //vor.Generate(points);
            Point p = points[0];
            points = points.OrderBy(i => -Math.Atan2(i.y - p.y, i.x - p.x)).
                                ThenByDescending(i=>Math.Sqrt((i.x-p.x)*(i.x-p.x)+(i.y-p.y)*(i.y-p.y))).ToList();
            Point left = new Point(0, 0);
            Point center = new Point(4, 0);
            Point right = new Point(6, -2);

            Point v1 = new Point(left.x - center.x,left.y - center.y);
            Point v2 = new Point(right.x - center.x, right.y - center.y);
            double dot = v1.x * v2.x + v1.y * v2.y;
            double det = v1.x * v2.y - v1.y * v2.x;
            double angle = -Math.Atan2(det, dot)*180/Math.PI;
            //double angle =  (v1.x*v2.x + v1.y*v2.y) / (Math.Sqrt(v1.x*v1.x+v1.y*v1.y)+Math.Sqrt(v2.x*v2.x+v2.y*v2.y));
            double t = Math.Atan2(0, 0);
            Assert.AreEqual(1, 1);
        }

    }
}
