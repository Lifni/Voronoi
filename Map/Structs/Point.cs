using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map.Structs
{
    public class Point
    {
        public double x { get; }
        public double y { get; }

        public Point(double _x, double _y)
        {
            this.x = _x;
            this.y = _y;
        }

        public bool AreEqual(Point p)
        {
            return (this.x == p.x && this.y == p.y);
        }
    }
}
