using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoronoiDiagram.Structs
{
    public class Center
    {
        public int id;

        public Point point;  // location

        public List<Center> neighbours;
        public List<Edge> borders;
        public List<Corner> corners;

        public Center(Point p)
        {
            id = StaticNums.id++;
            this.point = new Point(p);
            this.neighbours = new List<Center>();
            this.borders = new List<Edge>();
            this.corners = new List<Corner>();
        }

        public Center(Center c)
        {
            id = StaticNums.id++;
            this.point = new Point(c.point);
            this.neighbours = c.neighbours ?? new List<Center>(c.neighbours);
            this.borders = c.borders ?? new List<Edge>(c.borders);
            this.corners = c.corners ?? new List<Corner>(c.corners);
        }
    }
}
