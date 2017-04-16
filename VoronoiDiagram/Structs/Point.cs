using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoronoiDiagram.Structs
{
    public class Point
    {
        //public int id;

        public double x { get; }
        public double y { get; }
        
        public Point(double _x, double _y)
        {
            //id = StaticNums.id++;
            this.x = _x;
            this.y = _y;
        }

        public Point(Point p)
        {
            //id = StaticNums.id++;
            this.x = p.x;
            this.y = p.y;
        }

        public bool AreEqual(Point p)
        {
            return (this.x == p.x && this.y == p.y);
        }
    }
}
